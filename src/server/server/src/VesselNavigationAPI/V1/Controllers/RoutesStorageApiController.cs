using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VesselNavigationAPI.Attributes;
using VesselNavigationAPI.DB;
using VesselNavigationAPI.Interfaces;
using VesselNavigationAPI.Models.Db;
using VesselNavigationAPI.Models.Dto;

namespace VesselNavigationAPI.V1.Controllers
{
    /// <summary>
    /// Route input controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [V1]
    [ApiRoute]
    [ApiController]
    public class RoutesStorageApiController : Controller
    {
        private readonly ILogger<RoutesStorageApiController> _logger;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly VesselNavigationDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutesStorageApiController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="mapper">mapper.</param>
        /// <param name="validationService">validationService.</param>
        /// <param name="dbContext">dbContext.</param>
        public RoutesStorageApiController(
            ILogger<RoutesStorageApiController> logger,
            IMapper mapper,
            IValidationService validationService,
            VesselNavigationDbContext dbContext)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region GET

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("GetVessels")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselsDataRetrieving()
        {
            try
            {
                return new JsonResult(this._dbContext.Vessel.AsNoTracking());
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselsDataRetrieving)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <param name="vesselId">vesselId.</param>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("GetVesselPositions/{vesselId:Guid}")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselPositionRetrieving(Guid vesselId)
        {
            try
            {
                return new JsonResult(this._dbContext.Position
                    .Where(x => x.VesselId == vesselId).AsNoTracking());
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselsDataRetrieving)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        [Route("GetAllPositions")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselPositionRetrieving()
        {
            try
            {
                return new JsonResult(this._dbContext.Position.AsNoTracking());
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselsDataRetrieving)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        #endregion

        #region POST

        /// <summary>
        /// Feeding routes by json from request body.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [Route("FeedRoutes")]
        [ProducesResponseType(typeof(OkResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult VesselsDataFeeding()
        {
            try
            {
                using var stream = new StreamReader(this.HttpContext.Request.Body);
                var vesselsData = JsonConvert.DeserializeObject<VesselsDataDto>(stream.ReadToEnd());
                if (vesselsData == null || !vesselsData.Vessels.Any())
                {
                    this._logger.LogWarning("Vessels collection is empty");
                    return new OkResult();
                }

                var vessels = vesselsData.Vessels;
                foreach (var vessel in vessels)
                {
                    var result = this._validationService.ValidateVesselData(vessel);
                }

                var list = this._mapper.Map<List<(Vessel, IEnumerable<VesselPosition>)>>(vessels);
                if (list != null)
                {
                    foreach (var (vessel, vesselPosition) in list)
                    {
                        try
                        {
                            this._dbContext.Vessel.Add(vessel);
                            this._dbContext.Position.AddRange(vesselPosition);
                        }
                        catch (Exception ex)
                        {
                            // TODO add explicit data explanation
                            this._logger.LogError($"{nameof(this.VesselsDataFeeding)}: failed to add data to the table. Error message: '{ex.Message}'");
                        }
                    }
                }

                this._dbContext.SaveChanges();
                return new OkResult();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"{nameof(this.VesselsDataFeeding)} failed. Error message: '{ex.Message}'");
                return new BadRequestResult();
            }
        }

        #endregion
    }
}
