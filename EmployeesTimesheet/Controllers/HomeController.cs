using System.Web.Mvc;



namespace EmployeesTimesheet.Controllers
{
    public class HomeController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";                    
            return View();
        }
        [Authorize(Roles ="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }
    
    }
}
