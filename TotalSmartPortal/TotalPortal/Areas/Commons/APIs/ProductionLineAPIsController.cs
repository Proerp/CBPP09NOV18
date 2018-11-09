using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

using TotalCore.Repositories.Commons;
using TotalModel.Models;
using TotalDTO.Commons;
using TotalPortal.APIs.Sessions;

namespace TotalPortal.Areas.Commons.APIs
{
    //[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class ProductionLineAPIsController : Controller
    {
        private readonly IProductionLineAPIRepository productionLineAPIRepository;

        public ProductionLineAPIsController(IProductionLineAPIRepository productionLineAPIRepository)
        {
            this.productionLineAPIRepository = productionLineAPIRepository;
        }


        public JsonResult GetProductionLineBases(string searchText)
        {
            var result = this.productionLineAPIRepository.GetProductionLineBases(searchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}