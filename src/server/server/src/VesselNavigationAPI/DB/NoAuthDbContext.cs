using Microsoft.EntityFrameworkCore;

namespace VesselNavigationAPI.DB
{
    /// <summary>
    /// VesselNavigationAPI Db context.
    /// </summary>
    public class NoAuthDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoAuthDbContext"/> class.
        /// </summary>
        /// <param name="options">DB context options.</param>
        public NoAuthDbContext(DbContextOptions<NoAuthDbContext> options)
            : base(options)
        {
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var s = modelBuilder.Model;
            /*modelBuilder.Entity<int>(entity =>
            {
                entity.HasKey(e => e.AudienceId);

                entity.HasIndex(e => e.AudienceId).IsUnique();
                entity.HasIndex(e => e.BuildingId);

                entity.UseXminAsConcurrencyToken();
            });*/
        }
    }
}

/*
 * DB Entity Framework Migration script
 *
 * dotnet tool install --global dotnet-ef / dotnet tool update --global dotnet-ef
 * dotnet ef migrations add VesselNavigationAPI_Migration
 * dotnet ef database update
 */
