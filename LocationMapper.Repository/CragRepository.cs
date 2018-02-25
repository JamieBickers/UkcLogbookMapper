﻿using LocationMapper.Entities;
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
            return cragContext.crag.FirstOrDefault(crag => crag.ukccragid == ukcCragId);
        }

        public void AddCrag(Crag crag)
        {
            cragContext.crag.Add(crag);
        }
    }
}