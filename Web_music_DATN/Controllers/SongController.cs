using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class SongController : Controller
    {
        // GET: Song
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
                var song = from s in conn.SONGs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    song = song.Where(b => b.NAME_SONG.ToLower().Contains(tukhoa));
                }
                List<CustomSong> listCustomsong = new List<CustomSong>();
                foreach (SONG a in song)
                {
                    CustomSong cus = new CustomSong();
                    cus.id = a.ID_SONG;
                    cus.name = a.NAME_SONG;
                    cus.image = a.PICTURE_SONG;
                    cus.song = a.PATH_SONG;
                    if (a.ID_ALBUM != null){
                        cus.idalbum = Convert.ToInt32(a.ID_ALBUM);
                        cus.tenalbum = conn.ALBUMs.Find(a.ID_ALBUM).NAME_ALBUM;
                    }
                    else
                    {
                        cus.tenalbum = "none";
                    }
                    cus.idtacgia =Convert.ToInt32(a.ID_COMPOSER);
                    cus.tentacgia = conn.COMPOSERs.Find(a.ID_COMPOSER).NAME_COMPOSER;

                    cus.idcasi =Convert.ToInt32( a.ID_SINGER);                  
                    cus.idtheloai = Convert.ToInt32( a.ID_GENRE);               
                    cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                    cus.tentheloai = conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;

                    listCustomsong.Add(cus);
                }
                if (layhet.Equals("no"))
                {
                    var kq = listCustomsong.Skip((trang - 1) * pagesize).Take(pagesize).ToList();

                    if (listCustomsong.Count() % pagesize == 0)
                    {
                        sotrang = listCustomsong.Count() / pagesize;
                    }
                    else
                    {
                        sotrang = (listCustomsong.Count() / pagesize) + 1;
                    }
                    return Json(new { ds = kq, sotrang = sotrang }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(listCustomsong, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Add(CustomSong model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var song = new SONG();
                var fileImg = model.ImageFile;
                //xử lý file ảnh
                if (fileImg != null)
                {
                    fileImg.SaveAs(Server.MapPath("/Image/" + fileImg.FileName));
                    song.PICTURE_SONG = "Image/" + fileImg.FileName;
                }
                else
                {
                    song.PICTURE_SONG = "Image/cs_default.jpg";
                }
                //xủa lý file nhạc
                var fileSong = model.SongFile;
                if (fileSong != null)
                {
                    fileSong.SaveAs(Server.MapPath("/Song/" + fileSong.FileName));
                    song.PATH_SONG = "Song/" + fileSong.FileName;
                }
                else
                {
                    song.PATH_SONG = "";
                }
                if (model.idalbum != 0)
                {
                    song.ID_ALBUM = model.idalbum;
                }
                song.ID_COMPOSER = model.idtacgia;
                song.ID_GENRE = model.idtheloai;
                song.ID_SINGER = model.idcasi;
                song.NAME_SONG = model.name;
                conn.SONGs.Add(song);
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
            var a = conn.SONGs.Find(ID);

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
            return Json(cus, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomSong model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                string urlImage = "";
                string urlSong = "";
                var fileImage = model.ImageFile;
                if (fileImage != null)
                {
                    fileImage.SaveAs(Server.MapPath("/Image/" + fileImage.FileName));
                    urlImage = "Image/" + fileImage.FileName;
                }
                else
                {
                    urlImage = conn.SONGs.Find(model.id).PICTURE_SONG;
                }
                var fileSong = model.SongFile;
                if (fileSong != null)
                {
                    fileSong.SaveAs(Server.MapPath("/Song/" + fileSong.FileName));
                    urlSong = "Song/" + fileSong.FileName;
                }
                else
                {
                    urlSong = conn.SONGs.Find(model.id).PATH_SONG;
                }
                using (var db = new Web_musicEntities())
                {
                    var result = db.SONGs.SingleOrDefault(b => b.ID_SONG == model.id);
                    if (result != null)
                    {
                        result.NAME_SONG = model.name;
                        result.ID_SINGER = model.idcasi;
                        if (model.idalbum != 0)
                        {
                            result.ID_ALBUM = model.idalbum;
                        }
                        else
                        {
                            result.ID_ALBUM = null;
                        }
                        result.ID_COMPOSER = model.idtacgia;
                        result.ID_GENRE = model.idtheloai;
                        result.PATH_SONG = urlSong;
                        result.PICTURE_SONG = urlImage;
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
                int i = 1;
                List<RATING> lstRaitng = conn.RATINGs.ToList();
                foreach (RATING a in lstRaitng)
                {
                    if (a.ID_SONG == ID)
                    {
                        i = 0;
                        break;
                    }
                }
                List<SONGPLAYLIST> lstSongList = conn.SONGPLAYLISTs.ToList();
                foreach (SONGPLAYLIST a in lstSongList)
                {
                    if (a.ID_SONG == ID)
                    {
                        i = 0;
                        break;
                    }
                }
                if (i == 1)
                {
                    SONG song = conn.SONGs.Find(ID);
                    conn.SONGs.Remove(song);
                    conn.SaveChanges();
                    return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 500, msd = "khong thanh cong"  }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}