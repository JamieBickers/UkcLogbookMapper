using LocationMapper.Entities;
using LocationMapper.Repository;
using LocationMapper.WebScrapers;
using LocationMapper.WebScrapers.Entities;
using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LocationMapper.DatabaseManager
{
    internal class UkcDatabaseManager
    {
        private IUkcReader ukcReader;
        private ICragRepository cragRepository;
        private ICragLocator cragLocator;
        private static int id = 0;
        private static int ID
        {
            get
            {
                Interlocked.Increment(ref id);
                return id;
            }
        }

        public UkcDatabaseManager()
        {
            var connectionString = "Host=localhost;Username=UkcLogbookMapper;Password=qwerty;Database=Ukc";
            cragRepository = CragRepositoryFactory.GetCragRepository(connectionString);
            ukcReader = new UkcReader();
            cragLocator = new CragLocator();
        }

        public void DeleteAllCragsFromDatabase()
        {
            cragRepository.DeleteAllCrags();
        }

        public void ReadAllCragsFromUkcAndAddThemToDatabase(int startingBatch = 0)
        {
            var crags = new List<UkcCrag>();
            var previousCragsLength = 0;

            for (var i = startingBatch; i < 100; i++)
            {
                var pageHandlers = new Action[1000];
                for (var j = 0; j < 1000; j++)
                {
                    var jCopy = j;
                    pageHandlers[j] = () =>
                    {
                        try
                        {
                            var crag = ukcReader.GetCragData(i * 1000 + jCopy);
                            if (crag.UkcCragId == 23423)
                            {
                                Console.WriteLine(i * 1000 + jCopy);
                            }
                            if (crag != null)
                            {
                                crags.Add(crag);
                            }
                        }
                        catch (AggregateException exception) when (exception.InnerException.GetType() == typeof(HttpRequestException)
                        || exception.InnerExceptions.FirstOrDefault().GetType() == typeof(HttpRequestException))
                        {
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"Unexpected exception with cragId = {i * 1000 + jCopy}: {exception.ToString()}.");
                            throw;
                        }
                    };
                }
                Parallel.Invoke(pageHandlers);

                cragRepository.AddCrags(
                    crags.GetRange(previousCragsLength, crags.Count - previousCragsLength).Select(ConvertUkcCragToDatabaseCrag));

                Console.WriteLine($"Completed {i}th batch and found {crags.Count - previousCragsLength} new crags");

                if (crags.Count == previousCragsLength)
                {
                    Console.WriteLine($"Found dead block at i = {i}");
                    break;
                }
                previousCragsLength = crags.Count;
            }
        }

        public void FindLocationsOfCraggsWithoutALocation()
        {
            var cragsWithoutLocation = cragRepository.GetCragsWithoutLocation().ToArray();
            var length = cragsWithoutLocation.Count();
            Console.WriteLine($"Found {length} crags without a location.");
            var actions = new Action[length];

            var counter = 0;

            for (var i = 0; i < length; i++)
            {
                var iCopy = i;
                actions[i] = () =>
                {
                    var crag = cragsWithoutLocation[iCopy];
                    if (cragLocator.TryFindCrag(crag.CragName, out var location))
                    {
                        lock (cragRepository)
                        {
                            cragRepository.UpdateCragLocation(crag.UkcCragId, location);
                        }
                        counter++;
                    }
                };
            }

            Parallel.Invoke(actions);

            Console.WriteLine($"Updated the locations of {counter}/{length} crags.");
        }

        private Crag ConvertUkcCragToDatabaseCrag(UkcCrag crag)
        {
            return new Crag()
            {
                CragName = crag.CragName,
                UkcCragId = crag.UkcCragId,
                ID = crag.UkcCragId,
                Latitude = crag.Location.Latitude,
                Longitude = crag.Location.Longitude,
                ExactLocation = false
            };
        }
    }
}
