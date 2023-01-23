using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VesselNavigationAPI.Attributes;

namespace VesselNavigationAPI.V1.Controllers
{
    /// <summary>
    /// Audience controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [V1]
    [ApiRoute]
    [ApiController]
    public class AudienceController : Controller
    {
        // private readonly NoAuthDbContext _context;
        private readonly ILogger<AudienceController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudienceController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public AudienceController(ILogger<AudienceController> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Audience by id.
        /// </summary>
        /// <returns>The audience.</returns>
        [HttpGet]
        [Route("Audience")]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public IActionResult Audience()
        {
            return new JsonResult("RESULT");

            // this._logger.LogError($"{nameof(VesselNavigationAPI)} table is empty");
            // return this.NotFound();
        }

        /// <summary>
        /// Audience by id.
        /// </summary>
        /// <returns>The audience.</returns>
        /// <param name="id">Audience ID.</param>
        [HttpGet]
        [Route("Audience/{id:int}")]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public IActionResult Audience(int id)
        {
            return new JsonResult("RESULT");

            // this._logger.LogError($"{nameof(VesselNavigationAPI)} table is empty");
            // return this.NotFound();
        }

        /// <summary>
        /// Audiences by building.
        /// </summary>
        /// <returns>Audiences.</returns>
        /// <param name="id">Building ID.</param>
        [HttpGet]
        [Route("Audience/Building/{id:int}")]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public IActionResult Building(int id)
        {
            return new JsonResult("RESULT");

            // this._logger.LogError($"{nameof(VesselNavigationAPI)} table is empty");
            // return this.NotFound();
        }
    }
}
