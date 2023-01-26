using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using VesselNavigationAPI.Constants;
using VesselNavigationAPI.Models.Helpers;

namespace VesselNavigationAPI.Controllers
{
    /// <summary>
    /// Default controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Controller]
    [Route("/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configuration">The configuration.</param>
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Default action.
        /// </summary>
        /// <returns>Json.</returns>
        [HttpGet]
        [Route("")]
        [Route("/")]
        public IActionResult Home()
        {
            var info = $"{Assembly.GetEntryAssembly().GetName().Name}: " +
                       $"{Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion}";
            return this.Ok(info);
        }

        /// <summary>
        /// Check status.
        /// </summary>
        /// <returns>Status.</returns>
        [HttpGet]
        [Route("/health")]
        public IActionResult Health()
        {
            var status = CustomNames.UnhealthyStatus;
            using (var connection = new NpgsqlConnection(this._configuration.GetConnectionString("PostgresConnection")))
            {
                try
                {
                    connection.Open();
                    if (connection.State.ToString() == "Open")
                    {
                        status = CustomNames.HealthyStatus;
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    this._logger.LogError($"Unable to open connection to Db: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }

            var healthCheck = new HealthCheck();
            healthCheck.Databases.Add(new DataBase(
                CustomNames.VesselNavigationApiDatabase,
                CustomNames.PostgreSql,
                this._configuration.GetConnectionString("PostgresVersion"),
                status));

            this._logger.LogInformation($"{CustomNames.VesselNavigationApiDatabase} status: {status}");
            return this.Json(healthCheck);
        }
    }
}
