using System.Web.Mvc;

namespace EmployeesTimesheet.Controllers
{
    public class BaseController : Controller
    {
        protected BusinessLogic.PanelLogic BLL;
        protected bool IsAdmin;
        public BaseController()
        {
            BLL = new BusinessLogic.PanelLogic();                      
        }

        protected override void Dispose(bool disposing)
        {
            if (BLL != null)
                BLL.Dispose();

            base.Dispose(disposing);
        }
      
    }
}