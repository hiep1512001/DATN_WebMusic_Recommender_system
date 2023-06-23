using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Kt(CustomUser model)
        {
            Web_musicEntities conn = new Web_musicEntities();
            List<USER> lstUser = conn.USERS.ToList();
            int t = 0;
            foreach(USER a in lstUser)
            {
                if(a.NAME_USER.Equals(model.username) && a.PASSWORD.Equals( model.password))
                {
                    Session["username"] = a.NAME_USER;
                    Session["id"] = a.ID_USER;
                    t = 1;
                    break;
                }
            }
            if (t == 1)
            {
                if(Convert.ToInt32(Session["id"]) == 1)
                {
                    return Json(new { msd = "admin" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { msd = "user" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { msd = "No" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        public JsonResult AddUser(CustomUser model)
        {
            Web_musicEntities conn = new Web_musicEntities();
            List<USER> lstUser = conn.USERS.ToList();
            int t = 0;
            foreach (USER a in lstUser)
            {
                if (a.NAME_USER.Equals(model.username) )
                {
                    t = 1;
                    break;
                }
            }
            if (t == 1)
            {
                return Json(new { msd = "No" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                USER user = new USER();
                user.NAME_USER = model.username.Trim();
                user.PASSWORD = model.password.Trim();
                user.ID_GENRE = model.idtheloai;
                conn.USERS.Add(user);
                conn.SaveChanges();
                List<USER> lst = conn.USERS.ToList();
                foreach(  USER a in lst)
                {
                    if (a.NAME_USER.Equals( model.username))
                    {
                        Session["id"] = a.ID_USER;
                        Session["username"] = a.NAME_USER;
                    }
                }
                return Json(new { msd = "Yes" }, JsonRequestBehavior.AllowGet);              
            }
        }
    }
}