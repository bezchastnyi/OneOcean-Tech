using System;
using System.Diagnostics.CodeAnalysis;

namespace VesselNavigationAPI.Models.Db
{
    /// <summary>
    /// Vessel model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Vessel
    {
        /// <summary>
        /// Gets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public Guid VesselId { get; init; }

        /// <summary>
        /// Gets the name of Vessel.
        /// </summary>
        /// <value>Name of Vessel.</value>
        public string Name { get; init; }
    }
}
