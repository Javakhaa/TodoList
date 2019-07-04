using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //if (Session["user"] == null)
            //{
            //    return Content("შესვლა აკრძალურია");
            //}
            //var user = (User)Session["user"];
            //ViewBag.Name = user.FirstName;

            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }


    }
}