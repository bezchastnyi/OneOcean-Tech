using System;
using Microsoft.AspNetCore.Mvc;

namespace VesselNavigationAPI.Attributes
{
    /// <summary>
    /// Class V1Attribute.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ApiVersionAttribute" />
    [AttributeUsage(AttributeTargets.Class)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "versioning")]
    public class V1Attribute : ApiVersionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="V1Attribute"/> class.
        /// </summary>
        public V1Attribute()
            : base(new ApiVersion(1, 0))
        { }
    }
}
