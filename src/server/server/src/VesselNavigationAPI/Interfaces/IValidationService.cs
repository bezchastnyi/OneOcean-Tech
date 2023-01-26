using System.ComponentModel.DataAnnotations;
using VesselNavigationAPI.Models.Dto;

namespace VesselNavigationAPI.Interfaces
{
    /// <summary>
    /// Interface of deserialize service.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Json Deserializer.
        /// </summary>
        /// <param name="vesselData">The json url.</param>
        /// <returns>Built model from json.</returns>
        ValidationResult ValidateVesselData(VesselDto vesselData);
    }
}
