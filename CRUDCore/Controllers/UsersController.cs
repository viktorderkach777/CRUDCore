using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CRUDCore.DAL.Entities;
using CRUDCore.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUDCore.Controllers
{ 
    [Produces("application/json")]
[Route("api/[controller]")]
//[ApiController]
public class UsersController : ControllerBase
{

    private readonly EFContext _context;

    public UsersController(EFContext context)
    {
        _context = context;
    }

    [HttpGet]
    public List<UserItemViewModel> GetUsers()
    {
       Thread.Sleep(2000);  
        var model = new List<UserItemViewModel>
            {
                new UserItemViewModel
                {
                    Id=1, Email="jon@gg.ss", Image="https://cdn.pixabay.com/photo/2017/07/28/23/34/fantasy-picture-2550222_960_720.jpg",
                    Roles = new List<RoleItemViewModel>
                    {
                        new RoleItemViewModel { Id=2, Name="Admin"}
                    }
                },
                new UserItemViewModel
                {
                    Id=2, Email="bombelyk@gg.ss", Image="https://avatars.mds.yandex.net/get-zen_doc/1362253/pub_5c1f1fbcee97de00aacce7d3_5c1f22b738ad6b00a9ecba7f/scale_600",
                    Roles = new List<RoleItemViewModel>
                    {
                        new RoleItemViewModel { Id=2, Name="Admin"},
                        new RoleItemViewModel { Id=3, Name="Meson"}
                    }
                }
            };
        return model;
    }

    // GET: api/<controller>
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //    return new string[] { "value1", "value2" };
    //}

    //// GET api/<controller>/5
    //[HttpGet("{id}")]
    //public string Get(int id)
    //{
    //    return "value";
    //}

    //// POST api/<controller>
    //[HttpPost]
    //public void Post([FromBody]string value)
    //{
    //}

    //// PUT api/<controller>/5
    //[HttpPut("{id}")]
    //public void Put(int id, [FromBody]string value)
    //{
    //}

    //// DELETE api/<controller>/5
    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{
    //}
}
}
