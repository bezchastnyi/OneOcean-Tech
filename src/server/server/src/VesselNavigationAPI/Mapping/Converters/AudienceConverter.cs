using System;
using System.Text.RegularExpressions;
using AutoMapper;

namespace VesselNavigationAPI.Mapping.Converters
{
    /*/// <summary>
    /// AudienceConverter.
    /// </summary>
    public class AudienceConverter : ITypeConverter<AudienceKhPI, Audience>
    {
        private static readonly Regex Regex1Pattern = new Regex(@"[\d[0-9]{0,4} місць]");
        private static readonly Regex Regex2Pattern = new Regex(@"\d[0-9]{0,4}");

        /// <summary>
        /// Convert model of audience from .
        /// </summary>
        /// <returns>Object of audience of model audience.</returns>
        /// <param name="source">Model of audience KhPI.</param>
        /// <param name="destination">Model of audience.</param>
        /// <param name="context">The context. </param>
        public Audience Convert(AudienceKhPI source, Audience destination, ResolutionContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(source.title))
            {
                return null;
            }

            return new Audience
            {
                AudienceId = source.id,
                AudienceName = ConvertExtensions.FixTitle(source.title),
                NumberOfSeats = SearchNumberOfSeats(source.title),
            };
        }

        private static int? SearchNumberOfSeats(string title)
        {
            var matches = Regex1Pattern.Matches(title);
            if (matches.Count == 0)
            {
                return null;
            }

            foreach (Match match in matches)
            {
                var matches2 = Regex2Pattern.Matches(match.Value);
                foreach (Match match2 in matches2)
                {
                    if (matches2.Count > 0)
                    {
                        return int.Parse(match2.Value);
                    }
                }
            }

            return null;
        }
    }*/
}
