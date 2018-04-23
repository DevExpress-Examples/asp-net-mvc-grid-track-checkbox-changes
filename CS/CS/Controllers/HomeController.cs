using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using CS.Models;

namespace CS.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            if(Session["TypedListModel"] == null)
                Session["TypedListModel"] = InMemoryModel.GetTypedListModel();

            return View(Session["TypedListModel"]);
        }

        public ActionResult TypedListDataBindingPartial() {
            ViewData["items"] = GetSerializedObject(Request["items"]);

            return PartialView(Session["TypedListModel"]);
        }

        private Dictionary<string, bool> GetSerializedObject(string inputString) {
            if(string.IsNullOrEmpty(inputString))
                return null;

            Dictionary<string, bool> items = null;

            try {
                items = new JavaScriptSerializer().Deserialize<Dictionary<string, bool>>(inputString);
            }
            catch {
                ViewData["ErrorMessage"] = "Invalid Input JSON String";
            }

            return items;
        }
    }
}