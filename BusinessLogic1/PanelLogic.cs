using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.BusinessObjects;
using BusinessLogic.DAL;
using BusinessLogic.BusinessObjects.Dropdowns;

namespace BusinessLogic
{
    public class PanelLogic : IDisposable
    {
        protected DAL.TimesheetContext DB;
        public PanelLogic()
        {
            DB = new DAL.TimesheetContext();
        }

        public void Dispose()
        {
            if (DB != null)
                DB.Dispose();
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        public void Register(User user, string password, int roleId)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.Password = password;

            user.Active = true;

            DB.Users.Add(user);
            user.UserRoles.Add(new UserRole() { RoleId = roleId });
            DB.SaveChanges();

        }
        public User Login(string username, string password)
        {
            var user = DB.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
                return null;

            return user;
        }
        public int CurrentUserId(string username)
        {
            return DB.Users.FirstOrDefault(x => x.Username == username).Id;
        }
        public bool IsUsernameExist(string username)
        {
            return DB.Users.Any(x => x.Username == username);
        }
       

        #region Employees
        public List<EmployeeViewModel> GetEmployees(string username, bool isAdmin)
        {
            int id = CurrentUserId(username);
            if (isAdmin)
            {
                return DB.Users.Where(x => x.Id != id && x.Active == true).Select(r => new EmployeeViewModel()
                {
                    Id = r.Id,
                    Username = r.Username,
                    TeamLeader = DB.Users.FirstOrDefault(y => y.Id == r.LeaderId).Username,
                    UserRole = r.UserRoles.FirstOrDefault(x => x.UserId == r.Id).Role.Name
                }).OrderBy(r => r.TeamLeader).ToList();
            }
            return DB.Users.Where(x => x.LeaderId == id && x.Active == true).Select(r => new EmployeeViewModel()
            {
                Id = r.Id,
                Username = r.Username,
            }).ToList();
        }
        public UserEditViewModel GetEmployee(int id)
        {
            var employee = DB.Users.FirstOrDefault(x => x.Id == id && x.Active == true);
            return new UserEditViewModel()
            {
                Id = employee.Id,
                Username = employee.Username,
                LeaderId = employee.LeaderId,
                RoleId = DB.UserRoles.FirstOrDefault(x => x.UserId == employee.Id).RoleId
            };
        }
       
        public void EditEmpoyeeTeamLeader(int id, UserEditViewModel vm)
        {
            if (!CheckUserExistForEdit(id, vm.Username))
            {
                DB.Users.FirstOrDefault(x => x.Id == id && x.Active == true).Username = vm.Username;
                DB.SaveChanges();
            };
        }
        public bool CheckTlHaveEmployees(int employeeId)
        {
            return DB.Users.Any(x => x.LeaderId == employeeId && x.Active == true);
        }
        public bool CheckIsTlOfEmployee(int tlId, int employeeId)
        {
            if (CheckUserExist(employeeId))
            {
                if (DB.Users.FirstOrDefault(x => x.Id == employeeId).LeaderId == tlId)
                    return true;
            }
            return false;
        }
        public bool CheckUserExist(int id)
        {
            if (DB.Users.Any(x => x.Id == id))
                return true;

            return false;

        }
        public bool CheckUserExistForEdit(int id, string username)
        {
            if (CheckUserExist(id))
            {
                return DB.Users.Any(x => x.Username == username && x.Id != id && x.Active == true);
            }
            return true;
        }
        #endregion

        #region Projects
        public void CreateNewProject(Project project)
        {
            DB.Projects.Add(project);
            DB.SaveChanges();
        }
        public List<ProjectViewModel> GetProjects(string username, bool isAdmin)
        {
            if (isAdmin)
            {
                return DB.Projects.Where(x => x.Active).Select(x => new ProjectViewModel
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    ProjectName = x.Name,
                   
                }).ToList();
            }
            int? tlId = DB.Users.FirstOrDefault(x => x.Username == username).LeaderId;
            int id = DB.Users.FirstOrDefault(x => x.Username == username).Id;
            return DB.Projects.Where(x => x.Active).Select(x => new ProjectViewModel
            {
                Id = x.Id,
                CreatedDate = x.CreatedDate,
                ProjectName = x.Name

            }).ToList();
        }
        public ProjectViewModel GetProjectWithTimeSheets(int projectId, string username)
        {
            int? userId = DB.Users.FirstOrDefault(x => x.Username == username).Id;
            Project dbProject = DB.Projects.Include("TimeSheets").FirstOrDefault(x => x.Id == projectId && x.Active);
            ProjectViewModel proj = new ProjectViewModel()
            {
                Id = dbProject.Id,
                CreatedDate = dbProject.CreatedDate,
                ProjectName = dbProject.Name,
                TimeSheets = dbProject.Timesheets.Where(r => r.UserId == userId && r.Active == true).Select(r => new TimeSheetViewModel
                {
                    Details = r.DetailDescription,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    Date = r.Date.ToShortDateString()
                }).ToList()
            };
            proj.TimeSheets = proj.TimeSheets.OrderByDescending(x => x.Date).ToList();


            return proj;
        }

        public ProjectEditViewModel GetProject(int id)
        {

            var projectFromDb = DB.Projects.FirstOrDefault(x => x.Id == id && x.Active);
            return new ProjectEditViewModel()
            {
                Active = projectFromDb.Active,
                Id = projectFromDb.Id,
                Name = projectFromDb.Name,
            };
        }
        public bool UpdateProject(int id, ProjectEditViewModel vm)
        {
            if (CheckProjectExist(id))
            {
                var project = DB.Projects.FirstOrDefault(x => x.Id == id);
                project.Name = vm.Name;
                DB.SaveChanges();
                return true;
            }
            return false;

        }
        public void DeleteProject(int id)
        {
            if (CheckProjectExist(id))
            {
                DB.Projects.FirstOrDefault(x => x.Id == id).Active = false;
                DB.SaveChanges();
            }
        }
        public bool CheckProject(int id)
        {
           
            if (DB.Timesheets.Any(x => x.ProjectId == id))
                return true;

            return false;
        }
        public bool CheckProjectExist(int id)
        {
            if (DB.Projects.Any(x => x.Id == id))
                return true;

            return false;
        }
        public List<ProjectViewModel> DeletedProjects(string username, bool isAdmin)
        {
            if (isAdmin)
            {
                return DB.Projects.Where(x => x.Active == false).Select(r => new ProjectViewModel()
                {
                    CreatedDate = r.CreatedDate,
                    Id = r.Id,
                    ProjectName = r.Name,
                }).ToList();
            }
            int id = CurrentUserId(username);
            return DB.Projects.Where(x => x.Active == false).Select(r => new ProjectViewModel()
            {
                CreatedDate = r.CreatedDate,
                Id = r.Id,
                ProjectName = r.Name
            }).ToList();
        }
        public void ReturnProjectToActive(int projectId)
        {
            if (CheckProjectExist(projectId))
            {
                DB.Projects.FirstOrDefault(x => x.Id == projectId).Active = true;
                DB.SaveChanges();
            }

        }
        #endregion

        #region Timesheets
        public void CreateNewTimesheet(Timesheet model)
        {
            DB.Timesheets.Add(model);
            DB.SaveChanges();
        }
        public List<TimeSheetViewModel> GetTimesheets(int userid)
        {
            return DB.Timesheets.Where(x => x.UserId == userid && x.Active == true).Select(a => new TimeSheetViewModel
            {
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Date = a.Date.ToString(),
                Details = a.DetailDescription,
                Project = a.Project.Name
            }).OrderBy(a => a.Project).ThenByDescending(a => a.Date).ToList();
        }
        public List<ProjectTotalHoursViewModel> SpentTimeOnProjects(int id, bool isAdmin)
        {
            List<Project> dbProjects = new List<Project>();
            if (isAdmin)
                dbProjects = DB.Projects.Include("TimeSheets").ToList();

            return dbProjects
                .Select(x => new ProjectTotalHoursViewModel
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    ProjectName = x.Name,
                    Status = x.Active,
                    TimeSheets = x.Timesheets.Select(r => new Range
                    {
                        Start = r.StartTime,
                        End = r.EndTime,
                    }).ToList()
                }).OrderByDescending(x => x.Status).ThenByDescending(x => x.CreatedDate).ToList();
        }
        public string CalculateTime(List<TimeSheetViewModel> times)
        {
            TimeSpan totalTime = new TimeSpan();
            if (times.Count() == 0)
            {
                return null;
            }
            for (int i = 0; i < times.Count(); i++)
            {
                totalTime = totalTime.Add(times[i].EndTime - times[i].StartTime);
            }
            return totalTime.ToString();
        }
        public string CalculateTotalTime(List<Range> times)
        {
            TimeSpan totalTime = new TimeSpan();
            if (times.Count() == 0)
            {
                return null;
            }
            for (int i = 0; i < times.Count(); i++)
            {
                totalTime = totalTime.Add(times[i].End - times[i].Start);
            }
            return totalTime.ToString();
        }
        #endregion

        public UsersRolesDropdowns GetUsersRolesDropdowns()
        {
            return new UsersRolesDropdowns()
            {
                UsersList = (from role in DB.UserRoles
                             join users in DB.Users
                             on role.UserId equals users.Id
                             where role.RoleId == 2
                             select new Generic()
                             {
                                 id = users.Id,
                                 text = users.Username
                             }).ToList(),
                UsersRoleList = (from t in DB.Roles
                                 where t.Id != 1
                                 orderby t.Name
                                 select new Generic()
                                 {
                                     id = t.Id,
                                     text = t.Name
                                 }).ToList(),
            };
        }
    }
}
