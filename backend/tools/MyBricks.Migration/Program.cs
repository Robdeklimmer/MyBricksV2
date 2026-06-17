using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.EntityFrameworkCore;
using MyBricks.Infrastructure.Persistence;
using MyBricks.Migration;
using Serilog.Extensions.Logging;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var loggerFactory = new SerilogLoggerFactory(Log.Logger);
var logger = loggerFactory.CreateLogger("Migration");

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .AddCommandLine(args)
    .Build();

var legacyConnString = config["legacy"] ?? "Server=localhost;Port=3306;Database=mybricks_legacy;Uid=root;Pwd=dev;";
var targetConnString = config["target"] ?? "Server=localhost;Port=3306;Database=mybricks;Uid=root;Pwd=dev;";

logger.LogInformation("MyBricks Migration Tool starting…");
logger.LogInformation("Legacy DB : {LegacyDb}", legacyConnString);
logger.LogInformation("Target DB : {TargetDb}", targetConnString);

var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
optionsBuilder.UseMySql(targetConnString, ServerVersion.AutoDetect(targetConnString));
using var dbContext = new ApplicationDbContext(optionsBuilder.Options);

var reader = new LegacyDbReader(legacyConnString, loggerFactory.CreateLogger<LegacyDbReader>());
var legacyData = await reader.ReadAllAsync();

var mapper = new MigrationMapper(loggerFactory.CreateLogger<MigrationMapper>());
var mappedData = mapper.Map(legacyData);

var writer = new NewDbWriter(dbContext, loggerFactory.CreateLogger<NewDbWriter>());
await writer.WriteAsync(mappedData);

logger.LogInformation("Migration complete. Verify row counts before going live.");
Log.CloseAndFlush();
return 0;
