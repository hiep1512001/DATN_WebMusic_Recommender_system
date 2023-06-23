using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace Web_music_DATN.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult Index()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "1982344842100042",
                client_secret = "94c7f32860f8af25fc2c047c37116f60",
                redirect_uri = "https://localhost:44367/test/FacebookRedirect",
                scope = "public_profile,email"
            });
            ViewBag.Url = loginUrl;
            return View();
        }
        public ActionResult FacebookRedirect(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1982344842100042",
                client_secret = "94c7f32860f8af25fc2c047c37116f60",
                redirect_uri = "https://localhost:44367/test/FacebookRedirect",
                code = code
            });
            fb.AccessToken = result.access_token;

            dynamic me = fb.Get("https://graph.facebook.com/me?fields=email");
            string name = me.name;
            string email = me.email;
            return RedirectToAction("Index","test");
        }
    }
}