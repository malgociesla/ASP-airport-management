using AutoMapper;
namespace AirplaneASP.Mapping
{
    public class AutoMapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        private readonly IMapper _mapper;

        public AutoMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }

        public void Map(TSource source, TDestination destination)
        {
            _mapper.Map(source, destination);
        }

    }
}