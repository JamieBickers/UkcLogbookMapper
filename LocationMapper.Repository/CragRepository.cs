using LocationMapper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocationMapper.Repository
{
    public class CragRepository : ICragRepository
    {
        private readonly CragContext cragContext;

        public CragRepository(CragContext cragContext)
        {
            this.cragContext = cragContext;
        }

        public Crag GetCrag(int ukcCragId)
        {
            return cragContext.Crag.SingleOrDefault(crag => crag.UkcCragId == ukcCragId);
        }

        public IEnumerable<Crag> GetCragsWithoutLocation()
        {
            return cragContext.Crag.Where(crag => IsNullOrZero(crag.Latitude) || IsNullOrZero(crag.Longitude));
        }

        public void AddCrag(Crag crag)
        {
            cragContext.Crag.Add(crag);
            cragContext.SaveChanges();
        }

        public void AddCrags(IEnumerable<Crag> crags)
        {
            //Eliminate duplicates
            crags = crags.GroupBy(crag => crag.UkcCragId, (key, group) => group.FirstOrDefault());

            foreach (var crag in crags)
            {
                if (!(cragContext.Crag.Any(dbCrag => dbCrag.UkcCragId == crag.UkcCragId)))
                {
                    cragContext.Crag.Add(crag);
                }
            }
            cragContext.SaveChanges();
        }

        public void UpdateCragLocation(int ukcCragid, MapLocation location)
        {
            var crag = cragContext.Crag.SingleOrDefault(dbCrag => dbCrag.UkcCragId == ukcCragid);
            if (crag != null)
            {
                crag.Latitude = location.Latitude;
                crag.Longitude = location.Longitude;
                crag.ExactLocation = true;
            }
            cragContext.SaveChanges();
        }

        public void DeleteAllCrags()
        {
            foreach (var crag in cragContext.Crag)
            {
                cragContext.Remove(crag);
            }
            cragContext.SaveChanges();
        }

        private bool IsNullOrZero(decimal? number) => number == 0 || number == null;
    }
}
