using System;
using Microsoft.AspNetCore.Mvc;

namespace VesselNavigationAPI.Attributes
{
    /// <summary>
    /// Present attribute for api routing.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RouteAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.
        /// </summary>
        /// <returns>ApiRouteAttribute.</returns>
        public ApiRouteAttribute()
            : base("v{version:apiVersion}")
        {
        }
    }

    /// <summary>
    /// Present attribute for api routing, provide a partial url path, using for custom endpoint url-path.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RouteAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "ApiRouting")]
    public class CustomizableApiRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizableApiRouteAttribute"/> class.
        /// </summary>
        /// <returns>ApiRouteAttribute.</returns>
        public CustomizableApiRouteAttribute()
            : base("v{version:apiVersion}")
        {
        }
    }
}
