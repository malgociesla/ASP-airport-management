namespace AirplaneASP.Mapping
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        void Map(TSource source, TDestination destination);
    }
}
