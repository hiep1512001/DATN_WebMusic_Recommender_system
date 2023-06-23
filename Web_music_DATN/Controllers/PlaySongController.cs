using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class PlaySongController : Controller
    {
        private static List<CustomSong> lst = new List<CustomSong>();
        private static string get = "";
        private static int idSong=0;
        private static int idSongFirst = 0;
        // GET: PlaySong

        public ActionResult Index()
        {
            lst.Clear();
            if (get.Equals(""))
            {
                get = TempData["get"].ToString();
            }
            else
            {
                if (TempData["get"] != null)
                {
                    get = TempData["get"].ToString();
                }
            }
            if (idSong == 0)
            {
                idSong = Convert.ToInt32(TempData["id"]);
            }
            else { 
                if (TempData["id"]!=null)
                {
                    idSong = Convert.ToInt32(TempData["id"]);
                }
            }
            Web_musicEntities conn = new Web_musicEntities();
            if (get.Equals("song"))
            {
                SONG a = conn.SONGs.Find(idSong);
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
                //lấy danh sách bài hát gợi ý
                lst.Add(cus);
                Rs_content content = new Rs_content();
                content.getData(idSong);
                content.addColumn();
                content.addRow();
                content.Chuanhoa();
                List<ValueCosin> lst_gia_tri_cosin = content.Cosin();
                foreach (ValueCosin b in lst_gia_tri_cosin)
                {
                    SONG song = conn.SONGs.Find(b.id);
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
                idSongFirst = cus.id;
                return View(cus);
            }
            else if(get.Equals("singer"))
            {
                List<SONG> litsSong = conn.SONGs.ToList();
                foreach(SONG song in litsSong)
                {
                    if (song.ID_SINGER == idSong)
                    {
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
                }
                SONG a = conn.SONGs.Find(lst[0].id);
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
                idSongFirst = cus.id;
                return View(cus);
            }
            else {
                List<SONG> litsSong = conn.SONGs.ToList();
                foreach (SONG song in litsSong)
                {
                    if (song.ID_ALBUM == idSong)
                    {
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
                }
                SONG a = conn.SONGs.Find(lst[0].id);
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
                idSongFirst = cus.id;
                return View(cus);
            }

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
                    id = idSongFirst;
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
        public JsonResult LoadData()
        {
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AddSongList(CustomSongList model)
        {
            try
            {
                Web_musicEntities conn = new Web_musicEntities();
                SONGPLAYLIST a = new SONGPLAYLIST();
                a.ID_PLAYLIST = model.idplaylist;
                a.ID_SONG = model.idsong;
                conn.SONGPLAYLISTs.Add(a);
                conn.SaveChanges();
                return Json(new { msd = "Thêm thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msd = "Bài hát đã có trong danh sách" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}