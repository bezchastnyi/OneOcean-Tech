using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Spatial.Euclidean;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VesselNavigationAPI.Attributes;
using VesselNavigationAPI.DB;
using VesselNavigationAPI.Models.Db;
using VesselNavigationAPI.V1.Models;

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
        private const double MinutesInHour = 60;

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
                    .OrderBy(x => x.TimeStamp).AsNoTracking().ToList();

                var speeds = new List<double>();
                for (var i = 0; i < vesselPositions.Count - 1; i++)
                {
                    var speed = SpeedBetweenCartesianDots(vesselPositions[i], vesselPositions[i + 1]);
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
                    .OrderBy(x => x.TimeStamp).AsNoTracking().ToList();

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

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("Vessel/CheckVesselsTrackIntersections")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult CheckVesselsTrackIntersection()
        {
            try
            {
                var vesselPositions = this._dbContext.Position
                    .AsNoTracking()
                    .AsEnumerable()
                    .OrderBy(x => x.TimeStamp)
                    .GroupBy(x => x.VesselId)
                    .ToDictionary(x => x.Key, y => y.ToList());

                // Create Lines from max/min points
                var linesToVesselId = (from t in vesselPositions
                    let vesselRoute = t.Value.ToList()
                    select (new Line2D(
                        new Point2D(vesselRoute[0].X, vesselRoute[0].Y),
                        new Point2D(vesselRoute[^1].X, vesselRoute[^1].Y)),
                        t.Key))
                    .ToList();

                // Get all general intersections by this lines pair-by-pair
                var list = new List<(Point2D, Guid, Guid)>();
                for (var i = 0; i < linesToVesselId.Count; i++)
                {
                    for (var j = i; j < linesToVesselId.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        var intersectionPoint = linesToVesselId[i].Item1.IntersectWith(linesToVesselId[j].Item1);
                        if (intersectionPoint != null)
                        {
                            list.Add((intersectionPoint.Value, linesToVesselId[i].Item2, linesToVesselId[j].Item2));
                        }
                    }
                }

                // Check that this intersections include in current routes and they fit in timestamp
                var results = new List<IntersectionData>();
                foreach (var (p, id1, id2) in list)
                {
                    (VesselPosition coord, double distance, double speed) prevPoint1 = default;
                    for (var i = 0; i < vesselPositions[id1].Count - 1; i++)
                    {
                        if (vesselPositions[id1][i].X < p.X && vesselPositions[id1][i + 1].X > p.X ||
                            vesselPositions[id1][i].Y < p.Y && vesselPositions[id1][i + 1].Y > p.Y)
                        {
                            var distance = DistanceBetweenCartesianDots(vesselPositions[id1][i], new VesselPosition
                            {
                                X = p.X,
                                Y = p.Y
                            });
                            var speed = SpeedBetweenCartesianDots(vesselPositions[id1][i], vesselPositions[id1][i + 1]);
                            prevPoint1 = (vesselPositions[id1][i], distance, speed);
                        }
                    }

                    if (prevPoint1 == default)
                    {
                        continue;
                    }

                    (VesselPosition coord, double distance, double speed) prevPoint2 = default;
                    for (var i = 0; i < vesselPositions[id2].Count - 1; i++)
                    {
                        if (vesselPositions[id2][i].X < p.X && vesselPositions[id2][i + 1].X > p.X ||
                            vesselPositions[id2][i].Y < p.Y && vesselPositions[id2][i + 1].Y > p.Y)
                        {
                            var distance = DistanceBetweenCartesianDots(vesselPositions[id2][i], new VesselPosition
                            {
                                X = p.X,
                                Y = p.Y
                            });
                            var speed = SpeedBetweenCartesianDots(vesselPositions[id2][i], vesselPositions[id2][i + 1]);
                            prevPoint2 = (vesselPositions[id2][i], distance, speed);
                        }
                    }

                    if (prevPoint2 == default)
                    {
                        continue;
                    }

                    var time1 = prevPoint1.distance / prevPoint1.speed;
                    var time2 = prevPoint2.distance / prevPoint2.speed;

                    var hours1 = prevPoint1.coord.TimeStamp.Hour + (prevPoint1.coord.TimeStamp.Minute / MinutesInHour) + time1;
                    var hours2 = prevPoint2.coord.TimeStamp.Hour + (prevPoint2.coord.TimeStamp.Minute / MinutesInHour) + time2;

                    if (Math.Abs(hours2 - hours1) <= 1)
                    {
                        results.Add(new IntersectionData
                        {
                            X = p.X,
                            Y = p.Y,
                            VesselId1 = prevPoint1.Item1.VesselId,
                            VesselId2 = prevPoint2.Item1.VesselId
                        });
                    }
                }

                return new JsonResult(results);
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
            var ac = b.X - a.X;
            var bc = b.Y - a.Y;
            return Math.Sqrt((ac * ac) + (bc * bc));
        }

        private static double SpeedBetweenCartesianDots(VesselPosition a, VesselPosition b)
        {
            var distance = DistanceBetweenCartesianDots(a, b);
            var aTime = a.TimeStamp.Hour + (a.TimeStamp.Minute / MinutesInHour);
            var bTime = b.TimeStamp.Hour + (b.TimeStamp.Minute / MinutesInHour);

            return distance / (bTime - aTime);
        }
    }
}
