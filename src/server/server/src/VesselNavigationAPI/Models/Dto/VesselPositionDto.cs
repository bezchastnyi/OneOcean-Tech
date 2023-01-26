using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace VesselNavigationAPI.Models.Dto
{
    /// <summary>
    /// Vessel position model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VesselPositionDto
    {
        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        /// <value>X.</value>
        [JsonProperty("x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        /// <value>Y.</value>
        [JsonProperty("y")]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the TimeStamp.
        /// </summary>
        /// <value>TimeStamp.</value>
        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
