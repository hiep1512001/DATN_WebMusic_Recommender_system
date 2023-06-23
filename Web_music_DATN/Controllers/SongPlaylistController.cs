using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class SongPlaylistController : Controller
    {
        private static List<CustomSong> lst = new List<CustomSong>();
        private static int idPlaylist = 0;

        // GET: SongPlaylist
        public ActionResult Index()
        {
            lst.Clear();
            if (idPlaylist == 0)
            {
                idPlaylist = Convert.ToInt32(TempData["id"]);
            }
            else
            {
                if (TempData["id"] != null)
                {
                    idPlaylist = Convert.ToInt32(TempData["id"]);
                }
            }
            Web_musicEntities conn = new Web_musicEntities();
            List<SONGPLAYLIST> listSongPlaylist = conn.SONGPLAYLISTs.ToList();
            List<int> lstIdSong = new List<int>();
            foreach(SONGPLAYLIST x in listSongPlaylist)
            {
                if (x.ID_PLAYLIST == idPlaylist)
                {
                    lstIdSong.Add(x.ID_SONG);
                }
            }
            CustomSong cus = new CustomSong();
            if (lstIdSong.Count() != 0)
            {
                SONG a = conn.SONGs.Find(lstIdSong[0]);

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
            }
            foreach (int b in lstIdSong)
            {
                SONG song = conn.SONGs.Find(b);
                CustomSong custom = new CustomSong();
                custom.id = song.ID_SONG;
                custom.name = song.NAME_SONG;
                custom.image = song.PICTURE_SONG;
                custom.song = song.PATH_SONG;
                if (song.ID_ALBUM != null)
                {
                    custom.idalbum = Convert.ToInt32(song.ID_ALBUM);
                    custom.tenalbum = conn.ALBUMs.Find(song.ID_ALBUM).NAME_ALBUM;
                }
                custom.idcasi = Convert.ToInt32(song.ID_SINGER);
                custom.idtheloai = Convert.ToInt32(song.ID_GENRE);
                custom.tencasi = conn.SINGERs.Find(song.ID_SINGER).NAME_SINGER;
                custom.tentheloai = conn.GENRES.Find(song.ID_GENRE).NAME_GENRE;
                lst.Add(custom);
            }
            return View(cus);
        }
        public JsonResult LoadData()
        {
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddRating(GetValueRating rating)
        {
            Web_musicEntities conn = new Web_musicEntities();
            if (rating.max != null && rating.value != null)
            {
                double giatri = Convert.ToDouble(rating.value / rating.max);
                if (0 <= giatri && giatri <= 0.2)
                {
                    giatri = 1;
                }
                else if (0.2 < giatri && giatri <= 0.4)
                {
                    giatri = 2;
                }
                else if (0.4 < giatri && giatri <= 0.6)
                {
                    giatri = 3;
                }
                else if (0.6 < giatri && giatri <= 0.8)
                {
                    giatri = 4;
                }
                else
                {
                    giatri = 5;
                }
                int id = 0;
                if (rating.idSong == 0)
                {
                    id = lst[0].id;
                }
                else
                {
                    id = rating.idSong;
                }
                // thay bằng giá trị user
                int idUser = Convert.ToInt32(Session["id"]);
                string date = DateTime.Now.ToShortDateString();
                List<RATING> lstRating = conn.RATINGs.ToList();
                RATING kt = null;
                foreach (RATING a in lstRating)
                {
                    if (a.ID_USER == idUser && a.ID_SONG == id)
                    {
                        kt = a;
                        break;
                    }
                }
                if (kt == null)
                {
                    RATING rt = new RATING();
                    rt.ID_SONG = id;
                    rt.ID_USER = idUser;
                    rt.DATE_RATE = Convert.ToDateTime(date);
                    rt.RATING1 = Convert.ToInt32(giatri);
                    if (rt.RATING1 >= 2)
                    {
                        rt.VIEWS = 1;
                    }
                    else
                    {
                        rt.VIEWS = 0;
                    }
                    conn.RATINGs.Add(rt);
                    conn.SaveChanges();
                }
                else
                {
                    using (var db = new Web_musicEntities())
                    {
                        var result = db.RATINGs.SingleOrDefault(b => b.ID_USER == idUser && b.ID_SONG == id);
                        if (result != null)
                        {
                            result.DATE_RATE = Convert.ToDateTime(date);
                            if (Convert.ToInt32(giatri) >= 2)
                            {
                                result.VIEWS = result.VIEWS + 1;
                            }
                            if (kt.RATING1 < giatri)
                            {
                                result.RATING1 = Convert.ToInt32(giatri);
                            }
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { code = 200, msd = date }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 500, msd = "Khoong laasy ddc" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Delete(int ID)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                var result = conn.SONGPLAYLISTs.SingleOrDefault(b => b.ID_PLAYLIST == idPlaylist && b.ID_SONG==ID);
                conn.SONGPLAYLISTs.Remove(result);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công"+ idPlaylist+" " +ID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}