using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VesselNavigationAPI.Attributes;
using VesselNavigationAPI.DB;
using VesselNavigationAPI.Models.Db;

namespace VesselNavigationAPI.V1.Controllers
{
    /// <summary>
    /// Route input controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [V1]
    [ApiRoute]
    [ApiController]
    public class RoutesCalculationApiController : Controller
    {
        private readonly ILogger<RoutesCalculationApiController> _logger;
        private readonly VesselNavigationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutesCalculationApiController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">dbContext.</param>
        public RoutesCalculationApiController(
            ILogger<RoutesCalculationApiController> logger, VesselNavigationDbContext dbContext)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <param name="vesselId">vesselId.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("Vessel/AverageSpeed/{vesselId:Guid}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselAverageSpeed(Guid vesselId)
        {
            try
            {
                var vesselPositions = this._dbContext.Position
                    .Where(x => x.VesselId == vesselId)
                    .OrderBy(x => x.TimeStamp).ToList();

                var speeds = new List<double>();
                for (var i = 0; i < vesselPositions.Count - 1; i++)
                {
                    var a = vesselPositions[i];
                    var b = vesselPositions[i + 1];

                    var distance = DistanceBetweenCartesianDots(a, b);

                    double minutes1 = a.TimeStamp.Minute;
                    double minutes2 = b.TimeStamp.Minute;
                    const double sixty = 60;
                    var aTime = a.TimeStamp.Hour + (minutes1 / sixty);
                    var bTime = b.TimeStamp.Hour + (minutes2 / sixty);

                    var speed = distance / (bTime - aTime);
                    speeds.Add(speed);
                }

                var averageSpeed = speeds.Sum() / speeds.Count;
                return new JsonResult(averageSpeed);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselAverageSpeed)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <param name="vesselId">vesselId.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("Vessel/TotalDistance/{vesselId:Guid}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselTotalDistance(Guid vesselId)
        {
            try
            {
                var vesselPositions = this._dbContext.Position
                    .Where(x => x.VesselId == vesselId)
                    .OrderBy(x => x.TimeStamp).ToList();

                double totalDistance = 0;
                for (var i = 0; i < vesselPositions.Count - 1; i++)
                {
                    totalDistance += DistanceBetweenCartesianDots(vesselPositions[i], vesselPositions[i + 1]);
                }

                return new JsonResult(totalDistance);
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselAverageSpeed)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        // https://byjus.com/maths/distance-between-two-points-formula/#:~:text=The%20formula%20to%20find%20the,coordinate%20plane%20or%20x%2Dy%20plane.
        // https://ru.onlinemschool.com/math/library/analytic_geometry/point_point_length/#:~:text=AB%20%3D%20%E2%88%9AAC2%20%2B%20BC,%D1%82%D0%BE%D1%87%D0%BA%D0%B0%D0%BC%D0%B8%20%D0%B2%20%D0%BF%D1%80%D0%BE%D1%81%D1%82%D1%80%D0%B0%D0%BD%D1%81%D1%82%D0%B2%D0%B5%20%D0%B2%D1%8B%D0%B2%D0%BE%D0%B4%D0%B8%D1%82%D1%81%D1%8F%20%D0%B0%D0%BD%D0%B0%D0%BB%D0%BE%D0%B3%D0%B8%D1%87%D0%BD%D0%BE.
        private static double DistanceBetweenCartesianDots(VesselPosition a, VesselPosition b)
        {
            double ac = b.X - a.X;
            double bc = b.Y - a.Y;
            return Math.Sqrt((ac * ac) + (bc * bc));
        }
    }
}
