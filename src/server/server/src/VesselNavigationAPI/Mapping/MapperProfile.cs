using System.Collections.Generic;
using AutoMapper;
using VesselNavigationAPI.Mapping.Converters;
using VesselNavigationAPI.Models.Db;
using VesselNavigationAPI.Models.Dto;

namespace VesselNavigationAPI.Mapping
{
    /// <summary>
    /// Building of the profile VesselNavigationAPI model.
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapperProfile"/> class.
        /// </summary>
        public MapperProfile()
        {
            this.CreateMap<VesselDto, (Vessel, IEnumerable<VesselPosition>)>().ConvertUsing<VesselConverter>();
        }
    }
}
