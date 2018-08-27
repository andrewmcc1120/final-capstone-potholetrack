using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Web.Models;
using WebApplication.Web.DAL;
using WebApplication.Web.Models.Account;
using WebApplication.Web.Providers.Auth;
using System;

namespace WebApplication.Web.Controllers
{
    public class HomeController : Controller
    {
        //DALs we will need to route requests correctly and display data to the client
        private IUserDAL uDal;
        private IPotholeDAL pDal;
        private readonly IAuthProvider authProvider;

        /// <summary>
        /// Gives this controler access to the user and pothole record DALs and our authorization provider upon instantiation
        /// </summary>
        /// <param name="u"></param>
        /// <param name="p"></param>
        /// <param name="auth"></param>
        public HomeController(IUserDAL u, IPotholeDAL p, IAuthProvider auth)
        {
            this.uDal = u;
            this.pDal = p;
            this.authProvider = auth;
        }

        /// <summary>
        /// Displays the home page with a map showing all of the currently unfixed potholes
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()

        {
            //Get all the reports in the database
            IList<Report> reports = pDal.GetAllReports();
            User currentUser = authProvider.GetCurrentUser();
	        ViewData["loggedIn"] = (currentUser != null);
            var now = DateTime.Now;
            var thisYr = now.Year;
            bool test;
            foreach (var report in reports)
            {
                test = thisYr == report.DateRepaired.Year;
            }
            //If someone is currently logged in
            if (currentUser!=null)
            {
                //Pass their id and list of reports they've submitted to the index page
                ViewData["currentUserID"] = currentUser.Id;
                ViewData["currentReportIDs"] = uDal.GetUserList(currentUser.Id);

            }
            else
            {
                //otherwise, pass in values that are not possible to be matched
                ViewData["currentUserID"] = -1;
	            List<int> temp = new List<int>() {-1};
	            ViewData["currentReportIDs"] = temp;
			}
            return View(reports);
        }

        /// <summary>
        /// Provided a user is registered as an employee, direct them to the Employee Portal
        /// </summary>
        /// <returns></returns>
		[AuthorizationFilter("employee")]
		public IActionResult Employee()
		{
			IList<Report> reports = pDal.GetAllReports();
            User currentUser = authProvider.GetCurrentUser();
            ViewData["loggedIn"] = authProvider.IsLoggedIn;
            ViewData["currentUserID"] = currentUser.Id;

            return View(reports);
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
