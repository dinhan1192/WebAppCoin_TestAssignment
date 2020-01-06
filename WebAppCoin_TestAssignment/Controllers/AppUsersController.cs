using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.App_Start;
using WebAppCoin_TestAssignment.Models;

namespace WebAppCoin_TestAssignment.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public PartialViewResult AddRolePopup(string Id)
        {
            ViewBag.userIds = Id;
            var roles = DbContext.IdentityRoles.ToList();
            return PartialView("PopupForAddRole", roles);
        }

        //public ActionResult AddUsers2Roles(string[] ids, string[] roleNames)
        //{
        //    foreach (var id in ids)
        //    {
        //        userManager.AddToRoles(id, roleNames);
        //    }
        //    return View();
        //}

        [HttpPost]
        public ActionResult AddUsers2Roles(string Id, string RoleName)
        {
            var arrUserIds = Id.Split(',');
            var arrRoleNames = RoleName.Split(',');
            foreach (var id in arrUserIds)
            {
                UserManager.AddToRoles(id, arrRoleNames);
            }
            return View("Coins/Index");
        }
        // GET: AppUsers
        public ActionResult Index()
        {
            return View(DbContext.Users.ToList());
        }
    }
}