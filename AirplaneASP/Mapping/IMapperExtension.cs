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

        public static IEnumerable<TSource> MapBack<TDestination, TSource>(this IMapper<TSource, TDestination> mapper, IEnumerable<TDestination> destination)
        {
            return destination.Select(x => mapper.MapBack(x));
        }
    }
}