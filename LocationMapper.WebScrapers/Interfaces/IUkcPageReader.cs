namespace LocationMapper.WebScrapers.Interfaces
{
    interface IUkcPageReader
    {
        string GetCragPage(int cragId);
        string GetSearchPage(string searchTerm);
        string GetUserLogbookPage(int userId, int pageNumber);
    }
}