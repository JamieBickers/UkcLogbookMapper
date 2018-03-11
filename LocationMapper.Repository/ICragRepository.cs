using LocationMapper.Entities;
using System.Collections.Generic;

namespace LocationMapper.Repository
{
    public interface ICragRepository
    {
        void AddCrag(Crag crag);
        Crag GetCrag(int ukcCragId);
        IEnumerable<Crag> GetCragsWithoutLocation();
        void AddCrags(IEnumerable<Crag> crags);
        void UpdateCragLocation(int ukcCragId, MapLocation location);
        void DeleteAllCrags();
    }
}