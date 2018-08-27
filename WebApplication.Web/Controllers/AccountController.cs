using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.DAL;
using WebApplication.Web.Models;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;

namespace WebApplication.Web.Controllers
{    
    public class AccountController : Controller
    {
	    private readonly IPotholeDAL dal;
        private readonly IAuthProvider authProvider;

        /// <summary>
        /// When this controller is instantiated, give it access to the DAL for records and our authorization provider
        /// </summary>
        /// <param name="authProvider"></param>
        /// <param name="dal"></param>
        public AccountController(IAuthProvider authProvider, IPotholeDAL dal)
        {
            this.authProvider = authProvider;
	        this.dal = dal;
        }
        
        /// <summary>
        /// Shows the user the log-in View?
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {            
            return View();
        }

        /// <summary>
        /// Handles logging in
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginViewModel)
        {

            User isLoggedIn = authProvider.GetCurrentUser();

            // Ensure the fields were filled out
            if (ModelState.IsValid)
            {
                // Check that they provided correct credentials
                bool validLogin = authProvider.SignIn(loginViewModel.Username, loginViewModel.Password);
                if (validLogin)
                {

                    // Redirect the user where you want them to go after successful login
                     isLoggedIn = authProvider.GetCurrentUser();
                    ViewData["loggedIn"] = (isLoggedIn != null);
                    return RedirectToAction("Index", "Home");
                }

			}
			isLoggedIn = authProvider.GetCurrentUser();
			ViewData["loggedIn"] = (isLoggedIn != null);

			return View("Register");
		}

        /// <summary>
        /// Handles logging-off
        /// </summary>
        /// <returns></returns>
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogOff()
        {
            // Clear user from session
            authProvider.LogOff();

            // Redirect the user where you want them to go after logoff
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows the standard registration page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            User isLoggedIn = authProvider.GetCurrentUser();
            ViewData["loggedIn"] = (isLoggedIn != null);
            return View();
        }

        /// <summary>
        /// Shows the "Employee Registration" page
        /// </summary>
        /// <returns></returns>
		[HttpGet]
        public IActionResult EmployeeRegister()
        {
            User isLoggedIn = authProvider.GetCurrentUser();
            ViewData["loggedIn"] = (isLoggedIn != null);
            return View();
        }

        /// <summary>
        /// Handles registration
        /// </summary>
        /// <param name="registerViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Register them as a new user (and set default role)
                authProvider.Register(registerViewModel.Email,
                                      registerViewModel.UserName,
                                      registerViewModel.Password,
                                      registerViewModel.FirstName,
                                      registerViewModel.LastName,
                                      registerViewModel.PhoneNumber, "user");

                // Redirect the user where you want them to go after registering
                return RedirectToAction("Index", "Home");
            }

            return View(registerViewModel);
        }

        [AuthorizationFilter("user","employee","admin")]
        public IActionResult ViewProfile()
        {
            User isLoggedIn = authProvider.GetCurrentUser();
            Profile profile = new Profile();
            User user = authProvider.GetCurrentUser();
	        profile.User = user;
            profile.Reports = dal.GetReportsByUser(profile.User.Id);
            ViewData["loggedIn"] = (isLoggedIn != null);
            return View(profile);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

                                                //OPTIONAL!!!
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordModel model)
        {
            return View();
        }
    }
}