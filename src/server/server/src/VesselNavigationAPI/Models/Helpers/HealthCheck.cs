using System.Collections.Generic;

namespace VesselNavigationAPI.Models.Helpers
{
    /// <summary>
    /// Health check.
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheck"/> class.
        /// </summary>
        public HealthCheck()
        {
            this.Databases = new List<DataBase>();
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>Database.</value>
        public List<DataBase> Databases { get; }
    }
}
