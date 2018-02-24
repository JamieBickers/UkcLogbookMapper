using LocationMapper.Models;
using LocationMapper.WebScrapers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationMapper.WebUi.ServiceLogic
{
    class MapPlotter
    {
        private ICragLocator cragLocator;
        private IUkcReader ukcReader;

        public MapPlotter(ICragLocator cragLocator, IUkcReader ukcReader)
        {
            this.cragLocator = cragLocator;
            this.ukcReader = ukcReader;
        }

        public IEnumerable<MapMarkerDto> FindLocationsUserHasClimbed(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
