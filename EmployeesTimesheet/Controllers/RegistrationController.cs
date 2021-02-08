using System;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.BusinessObjects;
using BusinessLogic.DAL;
using System.Web.Security;

namespace EmployeesTimesheet.Controllers
{

    public class RegistrationController : BaseController
    {
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View();

            var userFromDB = BLL.Login(vm.Username, vm.Password);

            if (userFromDB == null)
            {
                ModelState.AddModelError("", "Incorrect entry");
                return View();
            }
            int timeout = vm.RememberMe ? 525600 : 20;
            var ticket = new FormsAuthenticationTicket(vm.Username, vm.RememberMe, timeout);
            string encrypted = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            cookie.Expires = DateTime.Now.AddMinutes(timeout);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Registration");
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Register(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (BLL.IsUsernameExist(viewModel.Username))
                {
                    ModelState.AddModelError("UserNameExist", "Username already exist");
                    return View(viewModel);
                }
                bool IsAdmin = User.IsInRole("Admin");

                if (IsAdmin && (viewModel.RoleId != 2 && !viewModel.TeamLeaderId.HasValue))
                {
                    ModelState.AddModelError("", "User Manager And Role is required for Emplyees");
                    return View(viewModel);
                }
                else if (IsAdmin && (viewModel.RoleId == 2 && viewModel.TeamLeaderId.HasValue))
                {

                    viewModel.TeamLeaderId = null;
                }
                else if (!IsAdmin)
                {
                    viewModel.TeamLeaderId = BLL.CurrentUserId(User.Identity.Name);
                    viewModel.RoleId = 3;
                }

                var userToCreate = AutoMapper.Mapper.Map<User>(viewModel);
                BLL.Register(userToCreate, viewModel.Password, viewModel.RoleId);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

    }
}