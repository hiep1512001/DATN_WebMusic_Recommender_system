using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class SingerController : Controller
    {
        // GET: Singer
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
        public JsonResult LoadData(string tukhoa, int trang, string layhet)
        {  
            try
            {
                int pagesize = 8;
                int sotrang;
                Web_musicEntities conn = new Web_musicEntities();
                var singer = from s in conn.SINGERs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    singer = singer.Where(b => b.NAME_SINGER.ToLower().Contains(tukhoa));
                }
                List<CustomSinger> listCustomSinger = new List<CustomSinger>();
                foreach(SINGER a in singer)
                {
                    CustomSinger cus = new CustomSinger();
                    cus.id = a.ID_SINGER;
                    cus.name = a.NAME_SINGER;
                    cus.image = a.PICTURE_SINGER;
                    listCustomSinger.Add(cus);
                }
                if (layhet.Equals("no"))
                {
                    var kq = listCustomSinger.Skip((trang - 1) * pagesize).Take(pagesize).ToList();

                    if (listCustomSinger.Count() % pagesize == 0)
                    {
                        sotrang = listCustomSinger.Count() / pagesize;
                    }
                    else
                    {
                        sotrang = (listCustomSinger.Count() / pagesize) + 1;
                    }
                    return Json(new { ds = kq, sotrang = sotrang }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(listCustomSinger, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(new { code =500, msd="lấy danh sách không được" +ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(CustomSinger model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var singer = new SINGER();
                var file = model.ImageFile;
                if (file != null)
                    {
                        file.SaveAs(Server.MapPath("/Image/" + file.FileName));
                        singer.PICTURE_SINGER = "Image/" + file.FileName;
                }
                else
                {
                    singer.PICTURE_SINGER = "Image/cs_default.jpg";
                }
                    singer.NAME_SINGER = model.name;
                    conn.SINGERs.Add(singer);
                    conn.SaveChanges();
                    return Json(new { code = 200, msd = "Thành công"}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "Thêm không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetbyID(int ID)
        {
            Web_musicEntities conn = new Web_musicEntities();
            var Employee = conn.SINGERs.Find(ID);
            CustomSinger customSinger = new CustomSinger();
            customSinger.id = Employee.ID_SINGER;
            customSinger.name = Employee.NAME_SINGER;
            customSinger.image = Employee.PICTURE_SINGER;
            return Json(customSinger, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomSinger model)
        {
            try
            {
                
                Web_musicEntities conn = new Web_musicEntities();
                string urlImage = "";
                var file = model.ImageFile;
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("/Image/" + file.FileName));
                    urlImage = "Image/" + file.FileName;
                }
                else
                {
                   urlImage= conn.SINGERs.Find(model.id).PICTURE_SINGER;
                }
                using (var db= new Web_musicEntities()) {
                    var result = db.SINGERs.SingleOrDefault(b => b.ID_SINGER == model.id);
                    if (result != null)
                    {
                        result.NAME_SINGER = model.name;
                        result.PICTURE_SINGER = urlImage;
                        db.SaveChanges();
                    }
                }
                return Json(new { code = 200, msd = "Thành công"}, JsonRequestBehavior.AllowGet);
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
                SINGER singer= conn.SINGERs.Find(ID);
                conn.SINGERs.Remove(singer);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}