using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult Index()
        {
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }
            if (id==1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public JsonResult LoadData()
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                List<ALBUM> lstalbum = conn.ALBUMs.ToList();
                List<CustomAlbum> listCustomalbum = new List<CustomAlbum>();
                foreach (ALBUM a in lstalbum)
                {
                    CustomAlbum cus = new CustomAlbum();
                    cus.id = a.ID_ALBUM;
                    cus.name = a.NAME_ALBUM;
                    cus.idcasi =Convert.ToInt32(a.ID_SINGER);
                    cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                    listCustomalbum.Add(cus);
                }
                return Json(listCustomalbum, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Add(CustomAlbum model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var album = new ALBUM();
                album.NAME_ALBUM = model.name;
                album.ID_SINGER = model.idcasi;
                conn.ALBUMs.Add(album);
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
            var album = conn.ALBUMs.Find(ID);
            CustomAlbum customalbum = new CustomAlbum();
            customalbum.id = album.ID_ALBUM;
            customalbum.name = album.NAME_ALBUM;
            customalbum.idcasi =Convert.ToInt32( album.ID_SINGER);
            customalbum.tencasi = conn.SINGERs.Find(album.ID_SINGER).NAME_SINGER;

            return Json(customalbum, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomAlbum model)
        {
            try
            {
                using (var db = new Web_musicEntities())
                {
                    var result = db.ALBUMs.SingleOrDefault(b => b.ID_ALBUM == model.id);
                    if (result != null)
                    {
                        result.NAME_ALBUM = model.name;
                        result.ID_SINGER = model.idcasi;
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
                ALBUM album = conn.ALBUMs.Find(ID);
                conn.ALBUMs.Remove(album);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetSong(int id)
        {
            Web_musicEntities conn = new Web_musicEntities();
            var song = from s in conn.SONGs select s;
            List<CustomSong> listCustomsong = new List<CustomSong>();
            song = song.Where(b => b.ID_ALBUM==id);
            foreach (SONG a in song)
            {
                CustomSong cus = new CustomSong();
                cus.id = a.ID_SONG;
                cus.name = a.NAME_SONG;
                cus.image = a.PICTURE_SONG;
                cus.song = a.PATH_SONG;
                if (a.ID_ALBUM != null)
                {
                    cus.idalbum = Convert.ToInt32(a.ID_ALBUM);
                    cus.tenalbum = conn.ALBUMs.Find(a.ID_ALBUM).NAME_ALBUM;
                }
                else
                {
                    cus.tenalbum = "none";
                }
                cus.idtacgia = Convert.ToInt32(a.ID_COMPOSER);
                cus.tentacgia = conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;

                cus.idcasi = Convert.ToInt32(a.ID_SINGER);
                cus.idtheloai = Convert.ToInt32(a.ID_GENRE);
                cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                cus.tentheloai = conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;

                listCustomsong.Add(cus);
            }
            return View(listCustomsong);

        }
    }
}