using System;
using System.Diagnostics.CodeAnalysis;

namespace VesselNavigationAPI.V1.Models
{
    /// <summary>
    /// Vessel model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IntersectionData
    {
        /// <summary>
        /// Gets or sets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the collection of vessel positions.
        /// </summary>
        /// <value>vessel positions.</value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public Guid VesselId1 { get; set; }

        /// <summary>
        /// Gets or sets the collection of vessel positions.
        /// </summary>
        /// <value>vessel positions.</value>
        public Guid VesselId2 { get; set; }
    }
}
