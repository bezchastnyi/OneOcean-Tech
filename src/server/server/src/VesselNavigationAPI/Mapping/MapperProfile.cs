using AutoMapper;

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
            // this.CreateMap<AudienceKhPI, Audience>().ConvertUsing<AudienceConverter>();
        }
    }
}
