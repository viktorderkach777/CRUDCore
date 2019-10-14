using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using CRUDCore.DAL.Entities;
using CRUDCore.ViewModels;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<DbUser> _userManager;
        private readonly RoleManager<DbRole> _roleManager;

        public UsersController(UserManager<DbUser> userManager,
                   RoleManager<DbRole> roleManager, EFContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public List<UserItemViewModel> GetUsers()
        {
            Thread.Sleep(1500);
            var el = _context.Set<DbUser>().FirstOrDefault(s => s.Email == "admin@gmail.com");
            var model = new List<UserItemViewModel>
            {
                new UserItemViewModel
                {
                    Id=1, Email="jon@gg.ss", Image="https://cdn.pixabay.com/photo/2017/07/28/23/34/fantasy-picture-2550222_960_720.jpg", Age=30, Phone="+380957476156", Description="PHP programmer",
                    Roles = new List<RoleItemViewModel>
                    {
                        new RoleItemViewModel { Id=2, Name="Admin"}
                    }
                },
                new UserItemViewModel
                {
                    Id=2, Email="bombelyk@gg.ss", Image="https://avatars.mds.yandex.net/get-zen_doc/1362253/pub_5c1f1fbcee97de00aacce7d3_5c1f22b738ad6b00a9ecba7f/scale_600", Age=40, Phone="+380957476999", Description=".NET programmer",
                    Roles = new List<RoleItemViewModel>
                    {
                        new RoleItemViewModel { Id=2, Name="Admin"},
                        new RoleItemViewModel { Id=3, Name="Meson"}
                    }
                },
                 new UserItemViewModel
                {
                    Id=3, Email="bober@gg.ss", Image="https://cdn1.thr.com/sites/default/files/2019/07/9d2511ab-f495-43d3-977a-774f91421f92.png", Age=28, Phone="+380677476911", Description="Java programmer",
                    Roles = new List<RoleItemViewModel>
                    {
                        new RoleItemViewModel { Id=2, Name="Admin"},
                        new RoleItemViewModel { Id=3, Name="Meson"}
                    }
                }
            };
            return model;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserItemViewModel model)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    DbUser dbUser = new DbUser
                    {
                        Email = model.Email,
                        Age = model.Age,
                        Image = model.Image,
                        Description = model.Description,
                        Phone = model.Phone,
                        UserName = model.Email
                    };
                    var result = await _userManager.CreateAsync(dbUser, "Qwerty1-");
                    if (!result.Succeeded)
                        throw new Exception("Problem create user");
                    foreach (var role in model.Roles)
                    {
                        var roleresult = await _roleManager.CreateAsync(new DbRole
                        {
                            Name = role.Name

                        });
                        if (!roleresult.Succeeded)
                            throw new Exception("Problem create role");
                    }
                    //throw new Exception("my exception crash");
                    scope.Complete();
                    return Ok(dbUser.Id);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


            
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
