using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_music_DATN.Models;

namespace Web_music_DATN.Controllers
{
    public class FacebookController : Controller
    {
        // GET: Facebook
        public ActionResult Index()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1982344842100042",
                client_secret = "94c7f32860f8af25fc2c047c37116f60",
                redirect_uri = "https://localhost:44367/Facebook/FacebookRedirect",
                scope = "public_profile,email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookRedirect(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1982344842100042",
                client_secret = "94c7f32860f8af25fc2c047c37116f60",
                redirect_uri = "https://localhost:44367/Facebook/FacebookRedirect",
                code = code
            });
            
            fb.AccessToken = result.access_token;
            if (fb.AccessToken != null)
            {
                dynamic me = fb.Get("https://graph.facebook.com/me?fields=email");
                string username = me.email;
                Web_musicEntities conn = new Web_musicEntities();
                List<USER> lstUser = conn.USERS.ToList();
                int t = 0;
                foreach (USER a in lstUser)
                {
                    if (a.NAME_USER.Equals(username))
                    {
                        t = 1;
                        Session["id"] = a.ID_USER;
                        Session["username"] = a.NAME_USER;
                        break;
                    }
                }
                if (t == 1)
                {
                    return RedirectToAction("Index", "HomeUser");
                }
                else
                {
                    USER user = new USER();
                    user.NAME_USER = username.Trim();
                    conn.USERS.Add(user);
                    conn.SaveChanges();
                    List<USER> lst = conn.USERS.ToList();
                    foreach (USER a in lst)
                    {
                        if (a.NAME_USER.Equals(username))
                        {
                            Session["id"] = a.ID_USER;
                            Session["username"] = a.NAME_USER;
                        }
                    }
                    return RedirectToAction("Index", "HomeUser");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}