using BusinessLogic.BusinessObjects;
using BusinessLogic.DAL;
using System;
using System.Web.Mvc;

namespace EmployeesTimesheet.Controllers
{
    [Authorize]
    public class ProjectsController : BaseController
    {

        #region Project

        #region CRUD Project

        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Create(ProjectNewViewModel vm)
        {
            try
            {

                Project project = new Project()
                {
                    Active = vm.Active,
                    CreatedDate = vm.CreatedDate,
                    Name = vm.Name
                };

                BLL.CreateNewProject(project);
                return RedirectToAction("GetTlProjects");
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Edit(int id)
        {
            return View(BLL.GetProject(id));
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult Edit(int id, ProjectEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (BLL.UpdateProject(id, vm))
                    {
                        return RedirectToAction("GetTlProjects");
                    }
                    ModelState.AddModelError("", "User does not exist");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(vm);
        }
        [Authorize(Roles = "Admin,Team Leader")]
        public JsonResult DeleteProject(int projectId)
        {
            if (BLL.CheckProject(projectId))
                return Json(new { Response = "Project have Timesheets" }, JsonRequestBehavior.AllowGet);

            try
            {
                BLL.DeleteProject(projectId);
                return Json(new { Response = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Response = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Admin,Team Leader")]
        public ActionResult DeletedProjects()
        {
            bool IsAdmin = User.IsInRole("Admin");
            return View(BLL.DeletedProjects(User.Identity.Name, IsAdmin));
        }
        [Authorize(Roles = "Admin,Team Leader")]
        public JsonResult ReturnToActive(int projectId)
        {
            try
            {
                BLL.ReturnProjectToActive(projectId);
                return Json(new { Response = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { Response = "Project does not exist" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [Authorize]
        [HttpGet]
        public ActionResult GetTlProjects()
        {
            bool IsAdmin = User.IsInRole("Admin");
            return View(BLL.GetProjects(User.Identity.Name, IsAdmin));
        }
        public ActionResult ProjectDetails(int id)
        {
            var projectVm = BLL.GetProjectWithTimeSheets(id, User.Identity.Name);
            ViewBag.TotalHours = BLL.CalculateTime(projectVm.TimeSheets);
            return View(projectVm);
        }
        public ActionResult SpentTimeOnProjects()
        {
            bool isAdmin = User.IsInRole("Admin");
            var projects = BLL.SpentTimeOnProjects(BLL.CurrentUserId(User.Identity.Name), isAdmin);
            foreach (var project in projects)
            {
                project.TotalHours = BLL.CalculateTotalTime(project.TimeSheets);
            }
            return View(projects);
        }

        #endregion

        public JsonResult CreateNewTimesheet(TimeSheetCreateVIewModel vm)
        {
            try
            {
                Timesheet timeSheet = new Timesheet()
                {
                    Date = DateTime.Now,
                    StartTime = vm.StartTime,
                    EndTime = vm.EndTime,
                    ProjectId = vm.ProjectId
                };
                BLL.CreateNewTimesheet(timeSheet);
                return Json(new { Response = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Response = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}