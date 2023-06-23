using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class GenreController : Controller
    {
        // GET: Genre
        public ActionResult Index()
        {
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }
            if (id == 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpGet]
        public JsonResult LoadData()
        {

            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                List<GENRE> lstgenre = conn.GENRES.ToList();
                List<CustomGenre> listCustomgenre = new List<CustomGenre>();
                foreach (GENRE a in lstgenre)
                {
                    CustomGenre cus = new CustomGenre();
                    cus.id = a.ID_GENRE;
                    cus.name = a.NAME_GENRE;
                    listCustomgenre.Add(cus);
                }
                return Json(listCustomgenre, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Add(CustomGenre model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var genre = new GENRE();
                genre.NAME_GENRE = model.name;
                conn.GENRES.Add(genre);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "Thêm không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetbyID(int ID)
        {
            Web_musicEntities conn = new Web_musicEntities();
            var Genre= conn.GENRES.Find(ID);
            CustomGenre customgenre = new CustomGenre();
            customgenre.id = Genre.ID_GENRE;
            customgenre.name = Genre.NAME_GENRE;
            return Json(customgenre, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomGenre model)
        {
            try
            {
                using (var db = new Web_musicEntities())
                {
                    var result = db.GENRES.SingleOrDefault(b => b.ID_GENRE == model.id);
                    if (result != null)
                    {
                        result.NAME_GENRE = model.name;
                        db.SaveChanges();
                    }
                }
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int ID)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                int kt = 1;
                List<USER> lstUser = conn.USERS.ToList();
                foreach(USER a in lstUser)
                {
                    if (a.ID_GENRE == ID)
                    {
                        kt = 0;
                        break;
                    }
                }
                if (kt == 1)
                {
                    GENRE genre = conn.GENRES.Find(ID);
                    conn.GENRES.Remove(genre);
                    conn.SaveChanges();
                    return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 500, msd = "khong thanh cong" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}