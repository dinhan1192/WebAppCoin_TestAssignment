using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppCoin_TestAssignment.App_Start;
using WebAppCoin_TestAssignment.Models;

namespace WebAppCoin_TestAssignment.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        //private RoleManager<AppRole> roleManager;
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



        //public AccountsController()
        //{
        //    //var roleStore = new RoleStore<AppRole>(dbContext);
        //    //roleManager = new RoleManager<AppRole>(roleStore);
        //    var userStore = new UserStore<AppUser>(dbContext);
        //    userManager = new UserManager<AppUser>(userStore);
        //}
        public AccountsController()
        {
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        [Authorize(Roles = "Admin")]
        //[Authorize]
        public ActionResult AddUserToRole(string accountId, string roleName)
        {
            UserManager.AddToRole(accountId, roleName);
            return Redirect("/Coins/Index");
        }

        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ProcessRegister(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var account = new AppUser()
                {
                    UserName = user.UserName,
                    BirthDay = DateTime.Now
                };

                IdentityResult result = await UserManager.CreateAsync(account, user.Password);
                Debug.WriteLine("@@@" + result.Succeeded);
                if (result.Succeeded)
                {
                    //Debug.WriteLine("@@@" + account.Id);
                    UserManager.AddToRole(account.Id, "User");
                    var authenticationManager = System.Web.HttpContext.Current
                        .GetOwinContext().Authentication;
                    var userIdentity = UserManager.CreateIdentity(
                        account, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = account.Id, code = code }, protocol: Request.Url.Scheme);
                    //await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + "google.com" + "\">here</a>");
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    //await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"http://google.com.vn\">here</a>");

                    return Redirect("/Coins/Index");
                }
                AddErrors(result);
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult ProcessLogin(string username, string password)
        {
            var user = UserManager.Find(username, password);
            if (user != null)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(
                    user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                return Redirect("/Coins/Index");
            }
            return View("Login");
        }

        public ActionResult Logout()
        {
            var authenticationManager = System.Web.HttpContext.Current
                .GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return View("Login");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}