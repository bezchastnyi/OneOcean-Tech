using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using VesselNavigationAPI.DB;
using VesselNavigationAPI.Mapping;

namespace VesselNavigationAPI
{
    /// <summary>
    /// VesselNavigationAPI startup.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private static readonly string AssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        private readonly bool _enableSwagger;
        private readonly bool _enableTokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

            this._enableSwagger =
                (this.Configuration["EnableSwagger"]?.Equals("true", StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault();

            this._enableTokens =
                (this.Configuration["Tokens:EnableTokens"]?.Equals("true", StringComparison.InvariantCultureIgnoreCase)).GetValueOrDefault();
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            Console.OutputEncoding = System.Text.Encoding.Default;

            services.AddMvcCore()
                .AddDataAnnotations()
                .AddApiExplorer()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(context.ModelState);
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var pgConnectionString = this.Configuration.GetConnectionString("PostgresConnection");
            var pgVersionString = this.Configuration.GetConnectionString("PostgresVersion");
            services.AddDbServices<NoAuthDbContext>(pgConnectionString, pgVersionString);
            services.AddAutoMapper(typeof(MapperProfile));

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
            })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            if (this._enableSwagger)
            {
                services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                    .AddSwaggerGen();
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="env">Environment.</param>
        /// <param name="apiDescriptionProvider">Api Description Provider.</param>
        public void Configure(
            IApplicationBuilder app,
            ILogger<Startup> logger,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiDescriptionProvider)
        {
            // app.UseHttpsRedirection();
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            if (this._enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in apiDescriptionProvider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }

                    app.Map("/swagger/versions_info", builder => builder.Run(async context =>
                        await context.Response.WriteAsync(
                            string.Join(Environment.NewLine, options.ConfigObject.Urls.Select(
                                descriptor => $"{descriptor.Name} {descriptor.Url}"))).ConfigureAwait(false)));
                });
            }

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
            });

            app.UseResponseCaching();
        }
    }
}
