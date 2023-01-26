using System;
using System.Diagnostics.CodeAnalysis;

namespace VesselNavigationAPI.Models.Db
{
    /// <summary>
    /// Vessel position model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VesselPosition
    {
        /// <summary>
        /// Gets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public Guid VesselPositionId { get; init; }

        /// <summary>
        /// Gets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public Guid VesselId { get; init; }

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        /// <value>X.</value>
        public double X { get; init; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        /// <value>Y.</value>
        public double Y { get; init; }

        /// <summary>
        /// Gets the TimeStamp.
        /// </summary>
        /// <value>TimeStamp.</value>
        public DateTime TimeStamp { get; init; }
    }
}
