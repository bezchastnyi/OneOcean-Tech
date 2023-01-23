using System;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains DB related extension methods.
    /// </summary>
    public static class DbServiceCollectionsExtensions
    {
        /// <summary>
        /// Adds the database services.
        /// </summary>
        /// <returns>Services.</returns>
        /// <param name="services">The services.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="pgVersionString">The postgres version string. Must contain 2-4 numerics separated by '.'.</param>
        /// <typeparam name="T">The db context.</typeparam>
        /// <exception cref="ArgumentNullException">Services.</exception>
        /// <exception cref="ArgumentException">Connection string must be not null or empty - connectionString.</exception>
        public static IServiceCollection AddDbServices<T>(this IServiceCollection services, string connectionString, string pgVersionString)
            where T : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                // throw new ArgumentException(string.Format(BackendConstants.NullOrEmptyErrorMessage, nameof(connectionString)));
            }

            if (string.IsNullOrEmpty(pgVersionString))
            {
                // throw new ArgumentException(string.Format(BackendConstants.NullOrEmptyErrorMessage, nameof(pgVersionString)));
            }

            var pgVersion = new Version(pgVersionString);
            services.AddDbContextPool<T>(contextOptions =>
            {
                contextOptions.UseNpgsql(connectionString, npgOptions =>
                {
                    npgOptions.SetPostgresVersion(pgVersion);
                    npgOptions.MigrationsAssembly("VesselNavigationAPI")
                        .EnableRetryOnFailure();
                });
            });

            return services;
        }
    }
}
