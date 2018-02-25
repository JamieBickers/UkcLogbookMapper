using LocationMapper.Entities;

namespace LocationMapper.Repository
{
    public interface ICragRepository
    {
        void AddCrag(Crag crag);
        Crag GetCrag(int ukcCragId);
    }
}