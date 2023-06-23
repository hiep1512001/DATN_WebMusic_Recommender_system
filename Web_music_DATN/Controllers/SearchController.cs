using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }
            if (id != -1 & id != 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public JsonResult Song(String tukhoa)
        {

            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var song = from s in conn.SONGs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    song = song.Where(b => b.NAME_SONG.ToLower().Contains(tukhoa));
                }
                List<CustomSong> listCustomsong = new List<CustomSong>();
                int n = 1;
                foreach (SONG a in song.ToList())
                {
                    if (n == 6)
                    {
                        break;
                    }
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
                    cus.idcasi = Convert.ToInt32(a.ID_SINGER);
                    cus.idtheloai = Convert.ToInt32(a.ID_GENRE);
                    cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                    cus.tentheloai = conn.GENRES.Find(a.ID_GENRE).NAME_GENRE;
                    cus.views = View(a.ID_SONG);
                    listCustomsong.Add(cus);
                    n = n + 1;
                }
                return Json(listCustomsong, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public int View(int id)
        {
            int views = 0;
            Web_musicEntities conn = new Web_musicEntities();
            List<RATING> lstrating = conn.RATINGs.ToList();
            foreach(RATING a in lstrating)
            {
                if (id == a.ID_SONG)
                {
                    views = views + Convert.ToInt32(a.VIEWS);
                }
            }          
            return views;
        }
        public JsonResult Singer(String tukhoa)
        {

            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var singer = from s in conn.SINGERs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    singer = singer.Where(b => b.NAME_SINGER.ToLower().Contains(tukhoa));
                }
                List<CustomSinger> listCustomSinger = new List<CustomSinger>();
                int n = 1;
                foreach (SINGER a in singer.ToList())
                {
                    if (n == 7)
                    {
                        break;
                    }
                    CustomSinger cus = new CustomSinger();
                    cus.id = a.ID_SINGER;
                    cus.name = a.NAME_SINGER;
                    cus.image = a.PICTURE_SINGER;
                    listCustomSinger.Add(cus);
                    n = n + 1;
                }
                List<CustomSinger> list = new List<CustomSinger>();
                List<SONG> song = conn.SONGs.ToList();
                for (int i = 0; i < listCustomSinger.Count; i++)
                {
                    int kt = 0;
                    foreach (SONG a in song)
                    {
                        if (a.ID_SINGER == listCustomSinger[i].id)
                        {
                            kt = 1;
                        }
                    }
                    if (kt == 1)
                    {
                        list.Add(listCustomSinger[i]);
                    }
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Album(String tukhoa)
        {

            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var album = from s in conn.ALBUMs select s;
                if (!String.IsNullOrEmpty(tukhoa))
                {
                    album = album.Where(b => b.NAME_ALBUM.ToLower().Contains(tukhoa));
                }
                List<CustomAlbum> listCustom = new List<CustomAlbum>();
                int n = 1;
                foreach (ALBUM a in album.ToList())
                {
                    if (n == 7)
                    {
                        break;
                    }
                    CustomAlbum cus = new CustomAlbum();
                    cus.id = a.ID_ALBUM;
                    cus.name = a.NAME_ALBUM;
                    cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                    listCustom.Add(cus);
                    n = n + 1;
                }
                List<CustomAlbum> list = new List<CustomAlbum>();
                List<SONG> song = conn.SONGs.ToList();
                for (int i=0; i<listCustom.Count; i++)
                {
                    int kt = 0;
                    foreach(SONG a in song)
                    {
                        if (a.ID_ALBUM == listCustom[i].id)
                        {
                            kt = 1;
                        }
                    }
                    if (kt==1)
                    {
                        list.Add(listCustom[i]);
                    }
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PlaySong(int id)
        {
            TempData["id"] = id;
            TempData["get"] = "song";
            return RedirectToAction("Index","PlaySong");
        }
        public ActionResult PlaySongSinger(int id)
        {
            TempData["id"] = id;
            TempData["get"] = "singer";
            return RedirectToAction("Index", "PlaySong");
        }
        public ActionResult PlaySongAlbum(int id)
        {
            TempData["id"] = id;
            TempData["get"] = "album";
            return RedirectToAction("Index", "PlaySong");
        }
    }
}