using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using VesselNavigationAPI.Interfaces;
using VesselNavigationAPI.Models.Dto;

namespace VesselNavigationAPI.Services
{
    /// <summary>
    /// Convert json format of data to model.
    /// </summary>
    public class ValidationService : IValidationService
    {
        private readonly ILogger<ValidationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ValidationService(ILogger<ValidationService> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public ValidationResult ValidateVesselData(VesselDto vesselData)
        {
            return ValidationResult.Success;
        }
    }
}
