using System;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Collections.Generic;
using Microsoft.AspNet.Identity;

using TotalModel.Models;
using TotalCore.Repositories.Sessions;

using TotalPortal.Models;
using TotalPortal.APIs.Sessions;
using TotalPortal.ViewModels.Sessions;
using TotalBase.Enums;
using System.Data.Entity.Core.Objects;


namespace TotalPortal.APIs.Sessions
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MenuAPIsController : Controller
    {
        private readonly IModuleRepository moduleRepository;
        private readonly IModuleDetailRepository moduleDetailRepository;

        public MenuAPIsController(IModuleRepository moduleRepository, IModuleDetailRepository moduleDetailRepository)
        {
            this.moduleRepository = moduleRepository;
            this.moduleDetailRepository = moduleDetailRepository;
        }


        public ActionResult TaskMenu(int? moduleID)
        {
            if (moduleID == null)
            {
                moduleID = MenuSession.GetModuleID(this.HttpContext);
            }
            else
            {
                MenuSession.SetModuleID(this.HttpContext, (int)moduleID);
            }


            ViewBag.ModuleDetailID = MenuSession.GetModuleDetailID(this.HttpContext);


            //var moduleDetail = moduleDetailRepository.GetModuleDetailByID((int)moduleID);
            var moduleDetail = moduleDetailRepository.GetAllModuleDetails().ToList().Where(w => (w.ModuleID == moduleID || (w.ModuleID == 2 && moduleID == 0 && MenuSession.GetUserLocked(this.HttpContext) == 0)) && w.InActive == 0).OrderBy(o => o.SerialID);
            return PartialView(moduleDetail);
        }

        [ChildActionOnly]
        //[OutputCache(NoStore = true, Location = OutputCacheLocation.Server, Duration = 100)]
        public ActionResult MainMenu()
        {
            try
            {
                this.VersionValidate();

                string moduleName = MenuSession.GetModuleName(this.HttpContext);
                string taskName = MenuSession.GetTaskName(this.HttpContext);
                string taskController = MenuSession.GetTaskController(this.HttpContext);
                ViewBag.ModuleName = moduleName;
                ViewBag.TaskName = taskName;
                ViewBag.TaskController = taskController;

                ViewBag.GlobalFromDate = HomeSession.GetGlobalFromDate(this.HttpContext);
                ViewBag.GlobalToDate = HomeSession.GetGlobalToDate(this.HttpContext);



                //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name
                var Db = new ApplicationDbContext();

                string aspUserID = User.Identity.GetUserId();
                int userID = Db.Users.Where(w => w.Id == aspUserID).FirstOrDefault().UserID;
                ViewBag.LocationName = this.moduleRepository.GetLocationName(userID);
                //BEGIN: Cho nay: sau nay can phai bo di, vi lam nhu the nay khong hay ho gi ca. Thay vao do, se thua ke tu base controller -> de lay userid, locationid, location official name



                var moduleMaster = moduleRepository.GetAllModules().OrderByDescending(p => p.SerialID);

                MenuSession.SetUserLocked(this.HttpContext, 0);
                return PartialView(moduleMaster);
            }
            catch (Exception e)
            {
                ViewBag.LocationName = "[USER LOCKED]";
                ViewBag.ExceptionMessage = e.Message;

                MenuSession.SetUserLocked(this.HttpContext, 1);
                return PartialView(new List<Module>());
            }
        }


        [HttpPost]
        public JsonResult SetModuleDetailID(int moduleDetailID)
        {
            try
            {
                MenuSession.SetModuleDetailID(this.HttpContext, moduleDetailID);     
                return Json(new { SetResult = "Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { SetResult = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SetTask(int? taskID, string taskName, string taskController)
        {
            if (taskID == null)
            {
                return Json(new { Success = 0 });
            }

            int moduleID = MenuSession.GetModuleID(this.HttpContext);
            Module module = moduleRepository.GetModuleByID((int)moduleID);

            MenuSession.SetModuleName(this.HttpContext, module.Description);

            MenuSession.SetTaskID(this.HttpContext, (int)taskID);
            MenuSession.SetTaskName(this.HttpContext, taskName);
            MenuSession.SetTaskController(this.HttpContext, taskController);

            return Json(new { Success = 1 });
        }

        private void VersionValidate()
        {
            if (MenuSession.GetFreshSession(this.HttpContext) == null || MenuSession.GetFreshSession(this.HttpContext) == "Restore")
            {
                foreach (GlobalEnums.FillingLine fillingLine in Enum.GetValues(typeof(GlobalEnums.FillingLine)))
                {
                    this.moduleRepository.ExecuteStoreCommand("UPDATE Configs SET VersionID = " + GlobalEnums.ConfigVersionID((int)fillingLine) + " WHERE ConfigID = " + (int)fillingLine + " AND VersionID < " + GlobalEnums.ConfigVersionID((int)fillingLine), new ObjectParameter[] { });
                }

                if (!this.moduleRepository.VersionValidate(false)) //JUST CHECK ONLY VersionID. THEN CALL AutoUpdates RIGHT BELOW TO UPDATE StoredID IF NEEDED
                    throw new Exception("This web application must be updated. Please contact your administrators.");

                if (!this.moduleRepository.AutoUpdates(MenuSession.GetFreshSession(this.HttpContext) == "Restore"))
                    throw new Exception("This web application must be updated. Please contact your administrators.");
            }
            MenuSession.SetFreshSession(this.HttpContext, "CLOSED");
        }

    }
}