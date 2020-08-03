using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicUniversityWeb.Models;
using LogicUniversityWeb.Services;

namespace LogicUniversityWeb.Controllers
{
    [Authorize]
    public class RequisitionController : Controller
    {
        // GET: Requisition
        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult RequisitionList()
        {
            RequisitionService rservice = new RequisitionService();
            List<RequisitionList> requisitions = rservice.GetRequisitionList();
            ViewBag.requisitionList = requisitions;
            return View();
        }

        [Authorize(Roles = "SClerk,SManager,SSupervisor")]
        public ActionResult GetRequisitionDetails(int id)
        {
            RequisitionService rservice = new RequisitionService();
            List<RequisitionDetails> rDetails = rservice.GetRequisitionDetails(id);
            ViewBag.rDetails = rDetails;
            return View();
        }
    }
}
