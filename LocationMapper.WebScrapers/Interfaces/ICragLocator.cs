using LocationMapper.Entities;

namespace LocationMapper.WebScrapers.Interfaces
{
    public interface ICragLocator
    {
        bool TryFindCrag(string cragName, out MapLocation location);
        bool TryFindCrag(string countyName, string countryName, out MapLocation location);
    }
}