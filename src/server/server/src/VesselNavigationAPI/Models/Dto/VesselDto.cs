using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace VesselNavigationAPI.Models.Dto
{
    /// <summary>
    /// Vessel model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VesselDto
    {
        /// <summary>
        /// Gets or sets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the collection of vessel positions.
        /// </summary>
        /// <value>vessel positions.</value>
        [JsonProperty("positions")]
        public IEnumerable<VesselPositionDto> VesselPositions { get; set; }
    }
}
