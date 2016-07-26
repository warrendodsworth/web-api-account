﻿using Api.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Api.Models.Bind;
using PagedList.EntityFramework;
using Microsoft.AspNet.Identity;
using AutoMapper;
using Api.Models.Dto;

namespace Api.Controllers
{
  [Authorize]
  [RoutePrefix("api/user/posts")]
  public class UserPostsController : ApiController
  {
    private Db _db = new Db();

    [Route("")]
    public async Task<PagedResult<PostDto>> Get(string search = null, int page = 1, int show = 10)
    {
      var userId = User.Identity.GetUserId();
      var q = _db.Posts.Include(n => n.User)
                       .Where(x => x.UserId == userId).AsQueryable();

      if (search != null)
      {
        q = q.Where(x => x.Text.Contains(search));
      }

      var res = await q.OrderByDescending(n => n.CreateDate).ToPagedListAsync(page, show);

      return new PagedResult<PostDto>
      {
        items = res.Select(x => Mapper.Map<PostDto>(x)),
        total = res.TotalItemCount
      };
    }

    [Route("{id:int}", Name = "UserPost")]
    public async Task<IHttpActionResult> Get(int id)
    {
      var post = await _db.Posts.FindAsync(id);
      if (post == null)
      {
        return NotFound();
      }

      return Ok(post);
    }

    [Route("")]
    public async Task<IHttpActionResult> Post(PostBindingModel item)
    {
      item.UserId = User.Identity.GetUserId();

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var post = Mapper.Map<Post>(item);

      _db.Posts.Add(post);
      var created = 1 == await _db.SaveChangesAsync();
      if (created)
        return CreatedAtRoute("UserPost", new { id = post.Id }, Mapper.Map<PostDto>(post));


      return BadRequest(ModelState);
    }

    [Route("{id:int}")]
    public async Task<IHttpActionResult> Put(int id, PostBindingModel item)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest();
      }

      if (id != item.Id)
      {
        return BadRequest();
      }

      var post = await _db.Posts.FindAsync(id);

      Mapper.Map(item, post);

      _db.Entry(post).State = EntityState.Modified;
      await _db.SaveChangesAsync();

      return StatusCode(HttpStatusCode.NoContent);
    }

    [Route("{id:int}")]
    public async Task<IHttpActionResult> Delete(int id)
    {
      var note = await _db.Posts.FindAsync(id);
      if (note == null)
      {
        return NotFound();
      }

      _db.Posts.Remove(note);
      await _db.SaveChangesAsync();

      return Ok();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _db.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
