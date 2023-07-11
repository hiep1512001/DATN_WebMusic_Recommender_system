using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class PlaylistController : Controller
    {
        // GET: Playlist
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
        public JsonResult LoadData()
        {

            try
            {
                int id = -1;
                if ((Session["id"]) != null)
                {
                    id = Convert.ToInt32(Session["id"]);
                }
                List<CustomPlaylist> lstCusPlaylist = new List<CustomPlaylist>();
                Web_musicEntities conn = new Web_musicEntities();
                List<PLAYLIST> lstplaylist = conn.PLAYLISTs.ToList();
                if (lstplaylist.Count!=0)
                {
                    foreach(PLAYLIST a in lstplaylist)
                    {
                        if (a.ID_USER == id)
                        {
                            CustomPlaylist cus = new CustomPlaylist();
                            cus.id = a.ID_PLAYLIST;
                            cus.id_user =Convert.ToInt32( a.ID_USER);
                            cus.name = a.NAME_PLAYLIST;
                            cus.name_user = conn.USERS.Find(a.ID_USER).NAME_USER;
                            cus.sumsong = Sumsong(a.ID_PLAYLIST);
                            lstCusPlaylist.Add(cus);
                        }
                    }
                }
                return Json(lstCusPlaylist, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "lấy danh sách không được" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public int Sumsong(int id)
        {
            int i = 0;
            Web_musicEntities conn = new Web_musicEntities();
            List<SONGPLAYLIST> lst = conn.SONGPLAYLISTs.ToList();
            foreach(SONGPLAYLIST a in lst)
            {
                if (a.ID_PLAYLIST == id)
                {
                    i = i + 1;
                }
            }
            return i;
        }
        public JsonResult Add(CustomPlaylist model)
        {
            try
            {
                int id = -1;
                if ((Session["id"]) != null)
                {
                    id = Convert.ToInt32(Session["id"]);
                }
                Web_musicEntities conn = new Web_musicEntities();
                PLAYLIST playlist = new PLAYLIST();
                playlist.NAME_PLAYLIST = model.name;
                playlist.ID_USER = id;
                conn.PLAYLISTs.Add(playlist);
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
            var a = conn.PLAYLISTs.Find(ID);
            CustomPlaylist cus = new CustomPlaylist();
            cus.id = a.ID_PLAYLIST;
            cus.id_user = Convert.ToInt32(a.ID_USER);
            cus.name = a.NAME_PLAYLIST;
            cus.name_user = conn.USERS.Find(a.ID_USER).NAME_USER;
            cus.sumsong = Sumsong(a.ID_PLAYLIST);
            return Json(cus, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(CustomPlaylist model)
        {
            try
            {
                using (var db = new Web_musicEntities())
                {
                    var result = db.PLAYLISTs.SingleOrDefault(b => b.ID_PLAYLIST == model.id);
                    if (result != null)
                    {
                        result.NAME_PLAYLIST = model.name;
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
                List<SONGPLAYLIST> lst = conn.SONGPLAYLISTs.ToList();
                foreach(SONGPLAYLIST a in lst)
                {
                    if (a.ID_PLAYLIST == ID)
                    {
                        conn.SONGPLAYLISTs.Remove(a);
                        conn.SaveChanges();
                    }
                }
                var playlist = conn.PLAYLISTs.Find(ID);
                conn.PLAYLISTs.Remove(playlist);
                conn.SaveChanges();
                return Json(new { code = 200, msd = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msd = "khong thanh cong" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetIdPlaylist(int id)
        {
            TempData["id"] = id;
            return RedirectToAction("Index", "SongPlaylist");
        }
    }
}