﻿using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Test.Mocks
{
  public class MockDbContext : IDb
  {
    public DbSet<Post> Posts { get; set; }


    public void Dispose()
    {
      this.Dispose();
    }
  }
}
