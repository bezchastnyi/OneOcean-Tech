using System.Diagnostics.CodeAnalysis;

namespace VesselNavigationAPI.Constants
{
    /// <summary>
    /// Backend constants.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class LogConstants
    {
        /// <summary>
        /// NullOrEmptyErrorMessage.
        /// </summary>
        public const string NullOrEmptyErrorMessage = "{0} must not be null or empty";
    }
}
