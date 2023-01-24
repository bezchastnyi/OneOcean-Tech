using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace VesselNavigationAPI.Models.Dto
{
    /// <summary>
    /// VesselsDataDto.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VesselsDataDto
    {
        /// <summary>
        /// Gets or sets the collection of vessel positions.
        /// </summary>
        /// <value>vessel positions.</value>
        [JsonProperty("vessels")]
        public IEnumerable<VesselDto> Vessels { get; set; }
    }
}
