using System.Collections.Generic;
using System.Linq;

namespace AirplaneASP.Mapping
{
    public static class IMapperExtension
    {
        public static IEnumerable<TDestination> Map<TSource, TDestination>(this IMapper<TSource, TDestination> mapper, IEnumerable<TSource> source)
        {
            return source.Select(x => mapper.Map(x));
        }
    }
}