﻿using Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models;
using Effort;
using Api.Controllers;

namespace Test.Controllers
{
  [TestClass]
  public class EffortTests
  {
    private IDb db;

    [TestInitialize]
    public void Initialize()
    {
      AutomapperConfig.Init();

      var connection = DbConnectionFactory.CreateTransient();
      db = new Db(connection);
      //EntityConnection connection = EntityConnectionFactory.CreateTransient("name=DefaultConnection");
    }

    [TestMethod]
    public async Task Posts_GetAll()
    {
      Seed(db);
      var controller = new HomeController(db);

      var posts = await controller.Get();

      Assert.IsNotNull(posts);
      Assert.AreEqual(10, posts.Items.Count());
      Assert.AreEqual(posts.Total, 100);
      Assert.AreEqual("Post text 1", posts.Items.First()?.Text);
    }

    private void Seed(IDb db)
    {
      var user = new User
      {
        Id = Guid.NewGuid().ToString(),
        Name = "Test User",
        Email = "a@b.com",
        UserName = "test",
        Status = UserStatus.Active
      };
      db.Users.Add(user);
      db.SaveChanges();

      for (int i = 1; i <= 100; i++)
      {
        db.Posts.Add(new Post
        {
          Id = i,
          Text = "Post text " + i,
          UserId = user.Id,
        });
        db.SaveChanges();
      }
    }
  }
}
