using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_music_DATN.Models;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Web_music_DATN.Controllers
{
    public class HomeUserController : Controller
    {
        private static List<CustomSong> ListCustomSong = new List<CustomSong>();
        private static List<CustomSong> ListSongTrend = new List<CustomSong>();
        // GET: HomeUser
        public ActionResult Index()
        {
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }         
            if ( id !=-1 & id != 1)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public JsonResult LoadDataSongTrend()
        {
            ListSongTrend.Clear();
            Web_musicEntities conn = new Web_musicEntities();
            List<CustomSong> lstCustom = new List<CustomSong>();
            List<RATING> lstRating = conn.RATINGs.ToList();
            List<TinhLuotTruyCap> lst = new List<TinhLuotTruyCap>();
            int[] kt = new int[lstRating.Count];
            for(int i=0; i<kt.Length; i++)
            {
                kt[i] = 1;
            }
            for(int i=0; i<lstRating.Count(); i++)
            {
                int dem = 1;
                for(int j=i+1; j<lstRating.Count; j++)
                {
                    if (kt[j] == 1)
                    {
                        if (lstRating[i].ID_SONG == lstRating[j].ID_SONG)
                        {
                            dem = dem + 1;
                            kt[j] = 0;
                        }
                    }
                }
                TinhLuotTruyCap cus = new TinhLuotTruyCap();
                cus.id = lstRating[i].ID_SONG;
                cus.dem=dem;
                lst.Add(cus);
            }
            lst.Sort();
            for(int i=lst.Count()-1; i>=0; i--)
            {
                if (lstCustom.Count==6)
                {
                    break;
                }
                SONG song = conn.SONGs.Find(lst[i].id);
                CustomSong cus = new CustomSong();
                cus.id = song.ID_SONG;
                cus.image = song.PICTURE_SONG;
                cus.name = song.NAME_SONG;
                cus.tencasi = conn.SINGERs.Find(song.ID_SINGER).NAME_SINGER;
                lstCustom.Add(cus);
            }
            ListSongTrend = lstCustom;
            return Json(lstCustom, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataSingerTrend()
        {
            List<TinhLuotTruyCap> list = new List<TinhLuotTruyCap>();
            Web_musicEntities conn = new Web_musicEntities();
            List<CustomSong> lstCustom = new List<CustomSong>();

            List<RATING> lstRating = conn.RATINGs.ToList();
            List<TinhLuotTruyCap> lst = new List<TinhLuotTruyCap>();
            int[] kt1 = new int[lstRating.Count];
            for (int i = 0; i < kt1.Length; i++)
            {
                kt1[i] = 1;
            }
            for (int i = 0; i < lstRating.Count(); i++)
            {
                int dem = 1;
                for (int j = i + 1; j < lstRating.Count; j++)
                {
                    if (kt1[j] == 1)
                    {
                        if (lstRating[i].ID_SONG == lstRating[j].ID_SONG)
                        {
                            dem = dem + 1;
                            kt1[j] = 0;
                        }
                    }
                }
                TinhLuotTruyCap cus = new TinhLuotTruyCap();
                cus.id = lstRating[i].ID_SONG;
                cus.dem = dem;
                lst.Add(cus);
            }
            lst.Sort();

            for (int i = lst.Count() - 1; i >= 0; i--)
            {
                SONG song = conn.SONGs.Find(lst[i].id);
                CustomSong cus = new CustomSong();
                cus.id = song.ID_SONG;
                cus.name = song.NAME_SONG;
                cus.idcasi =Convert.ToInt32( song.ID_SINGER);
                cus.image = song.PICTURE_SONG;
                cus.luottruycap = lst[i].dem;
                cus.tencasi = conn.SINGERs.Find(song.ID_SINGER).NAME_SINGER;
                lstCustom.Add(cus);
            }
            int[] kt = new int[lstCustom.Count];
            for (int i = 0; i < kt.Length; i++)
            {
                kt[i] = 1;
            }
            for (int i = 0; i < lstCustom.Count(); i++)
            {
                int dem = lstCustom[i].luottruycap;
                for (int j = i + 1; j < lstCustom.Count; j++)
                {
                    if (kt[j] == 1)
                    {
                        if (lstCustom[i].idcasi == lstCustom[j].idcasi)
                        {
                            dem = dem + lstCustom[j].luottruycap;
                            kt[j] = 0;
                        }
                    }
                }
                TinhLuotTruyCap cus = new TinhLuotTruyCap();
                cus.id = lstCustom[i].idcasi;
                cus.dem = dem;
                list.Add(cus);
            }
            list.Sort();

            List<CustomSinger> lstCustomSinger = new List<CustomSinger>();
            for (int i = list.Count() - 1; i >= 0; i--)
            {
                if (lstCustomSinger.Count == 6)
                {
                    break;
                }
                SINGER singger = conn.SINGERs.Find(list[i].id);
                CustomSinger cus = new CustomSinger();
                cus.id = singger.ID_SINGER;
                cus.name = singger.NAME_SINGER;
                cus.image = singger.PICTURE_SINGER;
                lstCustomSinger.Add(cus);
            }
            return Json(lstCustomSinger, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDataContent()
        {
            
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }
            int idUser = id;
            Web_musicEntities conn = new Web_musicEntities();
            List<RATING> rating1 = conn.RATINGs.ToList();
            List<RATING> rating = new List<RATING>();
            foreach (RATING a in rating1)
            {
                if (a.ID_USER == idUser)
                {
                    rating.Add(a);
                }
            }
            List<CustomSong> lstCustomSong = new List<CustomSong>();
            if (rating.Count() >= 3)
            {
                ListCustomSong.Clear();
                Rs_content content = new Rs_content();
                List<SONG> lst = content.getSongRating(idUser);
                if (lst.Count() > 0)
                {
                    content.getData(lst);
                    content.addColumn();
                    content.addRow();
                    content.Chuanhoa();
                    content.AddValueUserProfile();
                    // gán các bài hát chuaw rating vào matric để đưa ra các bài hát thích nhất
                    content.getData();
                    content.addRow();
                    content.Chuanhoa();
                    List<ValueCosin> lstValue = new List<ValueCosin>();
                    lstValue = content.TinhDoUaThich();

                    foreach (ValueCosin a in lstValue)
                    {
                        if (a.cosin != 0)
                        {
                            SONG song = conn.SONGs.Find(a.id);
                            CustomSong cus = new CustomSong();
                            cus.id = song.ID_SONG;
                            cus.name = song.NAME_SONG;
                            cus.idcasi = Convert.ToInt32(song.ID_SINGER);
                            cus.image = song.PICTURE_SONG;
                            cus.tencasi = conn.SINGERs.Find(song.ID_SINGER).NAME_SINGER;
                            lstCustomSong.Add(cus);
                        }
                    }
                    
                    if (lstCustomSong.Count != 0)
                    {
                        ListCustomSong.Clear();
                        ListCustomSong = lstCustomSong;
                        return Json(new { msg = "yes", ds = lstCustomSong }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                    }                  
                }
                else {
                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                USER user = conn.USERS.Find(idUser);
                var song1 = from s in conn.SONGs select s;
                List<SONG> song = new List<SONG>();
                foreach(SONG a in song1)
                {
                    if (a.ID_GENRE == user.ID_GENRE)
                    {
                        song.Add(a);
                    }
                }
                if (song.Count() != 0)
                {
                    List<TinhLuotTruyCap> lstTruycapSong = new List<TinhLuotTruyCap>();
                    List<RATING> lstRating = conn.RATINGs.ToList();
                    foreach (SONG b in song)
                    {
                        TinhLuotTruyCap tinh = new TinhLuotTruyCap();
                        tinh.id = b.ID_SONG;
                        tinh.dem = 0;
                        foreach (RATING a in lstRating)
                        {
                            if (a.ID_SONG == b.ID_SONG)
                            {

                                tinh.dem = tinh.dem + 1;
                            }
                        }
                        lstTruycapSong.Add(tinh);
                    }
                    lstTruycapSong.Sort();
                    for (int i = lstTruycapSong.Count() - 1; i >= 0; i--)
                    {
                        if (lstCustomSong.Count() == 6)
                        {
                            break;
                        }
                        int kt = 1;
                            foreach(CustomSong a in ListSongTrend)
                        {
                            if(Convert.ToInt32( a.id) ==Convert.ToInt32( lstTruycapSong[i].id))
                            {
                                kt = 0;
                            }
                        }
                        if (kt == 1)
                        {
                            SONG s = conn.SONGs.Find(lstTruycapSong[i].id);
                            CustomSong cus = new CustomSong();
                            cus.id = s.ID_SONG;
                            cus.image = s.PICTURE_SONG;
                            cus.name = s.NAME_SONG;
                            cus.tencasi = conn.SINGERs.Find(s.ID_SINGER).NAME_SINGER;
                            lstCustomSong.Add(cus);
                        }
                    }
                    
                    if (lstCustomSong.Count() > 0)
                    {
                        return Json(new { msg = "yes1", ds = lstCustomSong}, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                }               
            }
            // gia su user id=2
            // tisnh user profile để biết độ trọng tâm của các thuộc tính
        }
        public ActionResult PlaySong(int id)
        {
            TempData["id"] = id;
            TempData["get"] = "song";
            return RedirectToAction("Index", "PlaySong");
        }
        public ActionResult PlaySongSinger(int id)
        {
            TempData["id"] = id;
            TempData["get"] = "singer";
            return RedirectToAction("Index", "PlaySong");
        }
        public JsonResult LoadDataCollab()
        {
            int id = -1;
            if ((Session["id"]) != null)
            {
                id = Convert.ToInt32(Session["id"]);
            }
            int idUser = id;
            Web_musicEntities conn = new Web_musicEntities();
            List<RATING> rating1 = conn.RATINGs.ToList();
            List<RATING> rating = new List<RATING>();
            foreach (RATING a in rating1)
            {
                if (a.ID_USER == idUser)
                {
                    rating.Add(a);
                }
            }
            if (rating.Count() >= 3)
            {
                List<SongRating> lstinput = new List<SongRating>();
                List<SongRating> lstkq = new List<SongRating>();
                List<SONG> lstsong = conn.SONGs.ToList();
                foreach(RATING b in rating)
                {
                    if (b.ID_USER == idUser)
                    {
                        SONG song = conn.SONGs.Find(b.ID_SONG);
                        lstsong.Remove(song);
                    }
                }
                foreach (SONG a in lstsong)
                {
                            SongRating rs = new SongRating();
                            rs.ID_USER = idUser;
                            rs.ID_SONG = a.ID_SONG;
                            lstinput.Add(rs);
                }
                Rs_collab collab = new Rs_collab();
                lstkq = collab.UseCollap(lstinput);
                List<SONG> lstSong = new List<SONG>();
                if (ListCustomSong.Count() > 0)
                {
                    foreach (SongRating a in lstkq)
                    {
                        int kt = 1;
                        foreach (CustomSong b in ListCustomSong)
                        {
                            if (Convert.ToInt32(a.ID_SONG) == Convert.ToInt32(b.id))
                            {
                                kt = 0;

                            }
                        }
                        if (kt == 1)
                        {
                            SONG song = conn.SONGs.Find(a.ID_SONG);
                            lstSong.Add(song);
                        }
                    }
                }
                Rs_content content = new Rs_content();
                List<SONG> lst = content.getSongRating(idUser);
                if (lst.Count() > 0)
                {
                    List<CustomSong> lstCustomSong = new List<CustomSong>();
                    content.getData(lst);
                    content.addColumn();
                    content.addRow();
                    content.Chuanhoa();
                    content.AddValueUserProfile();
                    // gán các bài hát chuaw rating vào matric để đưa ra các bài hát thích nhất
                    content.getData(lstSong);
                    content.addRow();
                    content.Chuanhoa();
                    List<ValueCosin> lstValue = new List<ValueCosin>();
                    lstValue = content.TinhDoUaThich();
                    foreach (ValueCosin a in lstValue)
                    {
                        if (a.cosin != 0)
                        {
                            SONG song = conn.SONGs.Find(a.id);
                            CustomSong cus = new CustomSong();
                            cus.id = song.ID_SONG;
                            cus.name = song.NAME_SONG;
                            cus.idcasi = Convert.ToInt32(song.ID_SINGER);
                            cus.image = song.PICTURE_SONG;
                            cus.tencasi = conn.SINGERs.Find(song.ID_SINGER).NAME_SINGER;
                            lstCustomSong.Add(cus);
                        }
                    }
                    if (lstCustomSong.Count != 0)
                    {
                        return Json(new { msg = "yes", ds = lstCustomSong }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                }

                /*                if (lstkq.Count > 0)
                                {
                                    List<CustomSong> listCustom = new List<CustomSong>();
                                    for (int i = lstkq.Count() - 1; i >= 0; i--)
                                    {
                                        if (listCustom.Count == 6)
                                        {
                                            break;
                                        }
                                        SONG a = conn.SONGs.Find(lstkq[i].ID_SONG);
                                        CustomSong cus = new CustomSong();
                                        cus.id = a.ID_SONG;
                                        cus.name = a.NAME_SONG;
                                        cus.tencasi = conn.SINGERs.Find(a.ID_SINGER).NAME_SINGER;
                                        cus.image = a.PICTURE_SONG;
                                        listCustom.Add(cus);
                                    }
                                    return Json(new { msg = "yes", ds = lstkq }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
                                }*/
            }
            else
            {
                return Json(new {msg="no" }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult Albumtrend()
        {
            Web_musicEntities conn = new Web_musicEntities();
            List<ALBUM> listAlbum = conn.ALBUMs.ToList();
            List<SONG> lstSong = conn.SONGs.ToList();
            List<SONG> lstSongAlbum = new List<SONG>();
            List<TinhLuotTruyCap> lstTruycapAlbum = new List<TinhLuotTruyCap>();
            foreach (ALBUM a in listAlbum)
            {
                TinhLuotTruyCap tinh = new TinhLuotTruyCap();
                tinh.id = a.ID_ALBUM;
                tinh.dem = 0;
                lstTruycapAlbum.Add(tinh);
                foreach (SONG b in lstSong)
                {
                    if(b.ID_ALBUM == a.ID_ALBUM)
                    {
                        lstSongAlbum.Add(b);
                    }
                }
            }
            if (lstSongAlbum.Count != 0)
            {
                List<TinhLuotTruyCap> lstTruycapSong = new List<TinhLuotTruyCap>();
                List<RATING> lstRating = conn.RATINGs.ToList();
                foreach (SONG b in lstSongAlbum)
                {
                    TinhLuotTruyCap tinh = new TinhLuotTruyCap();
                    tinh.id =Convert.ToInt32( b.ID_ALBUM);
                    tinh.dem = 0;
                    foreach(RATING a in lstRating)
                    {
                        if (a.ID_SONG == b.ID_SONG)
                        {
                           
                            tinh.dem = tinh.dem + 1;
                        }
                    }
                    lstTruycapSong.Add(tinh);
                }
                foreach( TinhLuotTruyCap a in lstTruycapAlbum)
                {
                    foreach(TinhLuotTruyCap b in lstTruycapSong)
                    {
                        if (a.id == b.id)
                        {
                            a.dem = a.dem + b.dem;
                        }
                    }
                }
                lstTruycapAlbum.Sort();
                List<CustomAlbum> lstCustom = new List<CustomAlbum>();
                for (int i = lstTruycapAlbum.Count() - 1; i >= 0; i--)
                {
                    if (lstCustom.Count == 6)
                    {
                        break;
                    }
                    if(lstTruycapAlbum[i].dem != 0)
                    {
                        ALBUM album= conn.ALBUMs.Find(lstTruycapAlbum[i].id);
                        CustomAlbum cus = new CustomAlbum();
                        cus.id = album.ID_ALBUM;
                        cus.name = album.NAME_ALBUM;
                        cus.tencasi = conn.SINGERs.Find(album.ID_SINGER).NAME_SINGER;
                        lstCustom.Add(cus);
                    }
                }
                if (lstCustom.Count() != 0)
                {
                    return Json(new { msg = "yes", ds = lstCustom }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { msg = "no" }, JsonRequestBehavior.AllowGet);
        }
    }
}