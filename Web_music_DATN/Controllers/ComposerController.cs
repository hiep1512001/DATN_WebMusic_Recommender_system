using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class ComposerController : Controller
    {
        // GET: Composer
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
        public JsonResult LoadData(string tukhoa, int trang, string layhet)
        {

            try
            {
                int pagesize = 8;
                int sotrang;
                Web_musicEntities conn = new Web_musicEntities();
                var composer = from s in conn.COMPOSERs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    composer = composer.Where(b => b.NAME_COMPOSER.ToLower().Contains(tukhoa));
                }
                List<CustomComposer> listCustomcomposer = new List<CustomComposer>();
                foreach (COMPOSER a in composer)
                {
                    CustomComposer cus = new CustomComposer();
                    cus.id = a.ID_COMPOSER;
                    cus.name = a.NAME_COMPOSER;
                    listCustomcomposer.Add(cus);
                }
                if (layhet.Equals("no"))
                {
                    var kq = listCustomcomposer.Skip((trang - 1) * pagesize).Take(pagesize).ToList();

                    if (listCustomcomposer.Count() % pagesize == 0)
                    {
                        sotrang = listCustomcomposer.Count() / pagesize;
                    }
                    else
                    {
                        sotrang = (listCustomcomposer.Count() / pagesize) + 1;
                    }
                    return Json(new { ds = kq, sotrang = sotrang }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(listCustomcomposer, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Add(CustomComposer model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var composer = new COMPOSER();
                composer.NAME_COMPOSER = model.name;
                conn.COMPOSERs.Add(composer);
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
            var composer = conn.COMPOSERs.Find(ID);
            CustomComposer customcomposer = new CustomComposer();
            customcomposer.id = composer.ID_COMPOSER;
            customcomposer.name = composer.NAME_COMPOSER;
            return Json(customcomposer, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomComposer model)
        {
            try
            {
                using (var db = new Web_musicEntities())
                {
                    var result = db.COMPOSERs.SingleOrDefault(b => b.ID_COMPOSER == model.id);
                    if (result != null)
                    {
                        result.NAME_COMPOSER = model.name;
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
                COMPOSER composer = conn.COMPOSERs.Find(ID);
                conn.COMPOSERs.Remove(composer);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}