using Microsoft.EntityFrameworkCore;
using VesselNavigationAPI.Models.Db;

namespace VesselNavigationAPI.DB
{
    /// <summary>
    /// VesselNavigationAPI Db context.
    /// </summary>
    public class VesselNavigationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselNavigationDbContext"/> class.
        /// </summary>
        /// <param name="options">DB context options.</param>
        public VesselNavigationDbContext(DbContextOptions<VesselNavigationDbContext> options)
            : base(options) { }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        public virtual DbSet<Vessel> Vessel { get; set; }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        public virtual DbSet<VesselPosition> Position { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vessel>(entity =>
            {
                entity.HasKey(e => e.VesselId);
                entity.Property(e => e.Name);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.UseXminAsConcurrencyToken();
            });

            modelBuilder.Entity<VesselPosition>(entity =>
            {
                entity.HasKey(e => e.VesselPositionId);
                entity.Property(e => e.X);
                entity.Property(e => e.Y);
                entity.Property(e => e.TimeStamp);

                entity.HasOne<Vessel>()
                    .WithMany()
                    .HasForeignKey(e => e.VesselId);

                entity.UseXminAsConcurrencyToken();
            });
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
