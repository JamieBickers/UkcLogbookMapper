using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Database
{
    public class DbInitialiser
    {
        public static void Initialize(CragLocationContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
