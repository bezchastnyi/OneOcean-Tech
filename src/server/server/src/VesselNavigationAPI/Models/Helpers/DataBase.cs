namespace VesselNavigationAPI.Models.Helpers
{
    /// <summary>
    /// Database.
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataBase"/> class.
        /// </summary>
        /// <param name="name">Name of database.</param>
        /// <param name="dbSystem">Database system.</param>
        /// <param name="version">Version.</param>
        /// <param name="status">Status.</param>
        public DataBase(string name, string dbSystem, string version, string status)
        {
            this.Name = name;
            this.DbSystem = dbSystem;
            this.Version = version;
            this.Status = status;
        }

        /// <summary>
        /// Gets the name of database.
        /// </summary>
        /// <value>Name of database.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the database system.
        /// </summary>
        /// <value>Database system.</value>
        public string DbSystem { get; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>Version.</value>
        public string Version { get; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>Status.</value>
        public string Status { get; }
    }
}
