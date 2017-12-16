using LocationsMapper.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LocationsMapper.Database
{
    public class CragLocation
    {
        public int ID { get; set; }
        public int UkcCragId { get; set; }
        public string CragName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool ExactLocation { get; set; }
    }
}
