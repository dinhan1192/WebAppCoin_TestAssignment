using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.Models;

namespace WebAppCoin_TestAssignment.Controllers
{
    public class AppRolesController : Controller
    {
        private MyDbContext dbContext = new MyDbContext();
        private RoleManager<AppRole> roleManager;

        public AppRolesController()
        {
            var roleStore = new RoleStore<AppRole>(dbContext);
            roleManager = new RoleManager<AppRole>(roleStore);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View("AddRole");
        }

        [Authorize(Roles = "Admin")]
        //[Authorize]
        public ActionResult Store([Bind(Include = "Name")] AppRole role)
        {
            role.CreatedAt = DateTime.Now;
            if (!roleManager.RoleExists(role.Name))
            {
                roleManager.Create(role);
            }
            return Redirect("/Coins/Index");
        }
    }
}