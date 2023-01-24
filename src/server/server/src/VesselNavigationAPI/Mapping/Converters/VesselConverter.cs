using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VesselNavigationAPI.Models.Db;
using VesselNavigationAPI.Models.Dto;

namespace VesselNavigationAPI.Mapping.Converters
{
    /// <summary>
    /// AudienceConverter.
    /// </summary>
    public class VesselConverter : ITypeConverter<VesselDto, (Vessel, IEnumerable<VesselPosition>)>
    {
        /// <summary>
        /// Convert model of audience from KhPI to KIP.
        /// </summary>
        /// <returns>Object of audience of model audience KIP.</returns>
        /// <param name="source">Model of audience KhPI.</param>
        /// <param name="destination">Model of audience KIP.</param>
        /// <param name="context">The context. </param>
        public (Vessel, IEnumerable<VesselPosition>) Convert(VesselDto source, (Vessel, IEnumerable<VesselPosition>) destination, ResolutionContext context)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrEmpty(source.Name))
            {
                return (null, null);
            }

            var vesselId = Guid.NewGuid();
            var vesselPositions = source.VesselPositions.Select(vesselPosition => new VesselPosition
            {
                VesselPositionId = Guid.NewGuid(),
                VesselId = vesselId,
                X = vesselPosition.X,
                Y = vesselPosition.Y,
                TimeStamp = vesselPosition.TimeStamp,
            });

            return (new Vessel
            {
                VesselId = vesselId,
                Name = source.Name,
            }, vesselPositions);
        }
    }
}
