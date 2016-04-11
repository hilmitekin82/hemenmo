using System;
using System.Web;
using System.Web.Mvc;
using hemenmo.Models;
using System.Collections.Generic;

namespace hemenmo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        readonly List<string> urls = new List<string>()
        {
            "landing?cid=1036&ol=1&lp=21&affid=8009&clickid=",
            "landing?cid=1036&ol=1&lp=23&affid=8010&clickid=",
            "landing?cid=1036&affid=8011&clickid="
        };

        public ActionResult Index()
        {
            home model = new home();

            model = home.GetModel();

            return View("Index",model);
        }

        public ActionResult category(string category)
        {
            home model = new home();
            model = home.GetCategory(category);
            return View("category",model);
        }

        public ActionResult gameplay(string gameplay)
        {
            home model = new home();
            model = home.GetGamePlay(gameplay);
            return View("gameplay", model);
        }

        public ActionResult gameplay_likethis(string gameplay)
        {
            home model = new home();
            using (var db = new hemenmoContainer())
            {
                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.game_id = oyun.id;
                    model.game_name_seo = oyun.name_seo;
                }
                
                HttpCookie cookie = Request.Cookies.Get("like");
                if ((cookie != null) && (cookie.Value != ""))
                {
                }
                else
                {
                    db.set_like(model.game_id);
                    cookie = new HttpCookie("like", "1");
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Add(cookie);
                }
            }
            return Redirect("/oyna/" + model.game_name_seo);
        }

        public ActionResult gameplay_dislikethis(string gameplay)
        {
            home model = new home();
            using (var db = new hemenmoContainer())
            {

                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.game_id = oyun.id;
                    model.game_name_seo=oyun.name_seo;

                }
                HttpCookie cookie = Request.Cookies.Get("dislike");
                if ((cookie != null) && (cookie.Value != ""))
                {

                }
                else
                {
                    db.set_dislike(model.game_id);

                    cookie = new HttpCookie("dislike", "1");
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Add(cookie);
                }
               
            }
            return Redirect("/oyna/" + model.game_name_seo);
        }

        public ActionResult gameplay_votethis(string gameplay,int vote)
        {
            home model = new home();
            using (var db = new hemenmoContainer())
            {

                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.game_id = oyun.id;
                    model.game_name_seo = oyun.name_seo;

                }
                HttpCookie cookie = Request.Cookies.Get("votenow");
                if ((cookie != null) && (cookie.Value != ""))
                {
                }
                else
                {
                    db.set_rating(model.game_id, vote);

                    cookie = new HttpCookie("votenow", "1");
                    cookie.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Add(cookie);
                }
            }
            return Redirect("/oyna/" + model.game_name_seo);
        }


        public ActionResult search(string category)
        {
            home model = new home();
            model = home.GetSearch(category);

            return View("search", model);
        }

        public ActionResult list(string type)
        {
            
            home model = new home();
            model = home.GetList(type);
            return View("list", model);
        }

        public ActionResult playnow(string gameplay)
        {
            home model = new home();
            model = home.GetPlaynow(gameplay);
            return View("playnow", model);
        }

        Random rnd = new Random();

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public void mobil_oyun()
        {
            int index = rnd.Next(0, 2);
            Guid transaction_id;
            transaction_id = Guid.NewGuid();
            using (var _db=new hemenmoContainer())
            {
                _db.sp_store_the_click_id(transaction_id.ToString());
            }
            
            Response.Status = "301 Moved Permanently";
            Response.StatusCode = 301;
            Response.AppendHeader(
             "http://gameofmobi.com/"+ urls[index] +transaction_id.ToString(),
             HttpContext.Request.Url.AbsoluteUri
                                           .Replace("://", "://www.")
            );

            Response.Redirect("http://gameofmobi.com/" + urls[index] + transaction_id.ToString());
        }

        public ActionResult siparis()
        {
            string clickid = "";
            int goal = -1;
            if(Request.QueryString["abonelik"] != null)
            {
                clickid = Request.QueryString["abonelik"];
            }

            if (Request.QueryString["goal"] != null)
            {
                goal = int.Parse( Request.QueryString["goal"]);
            }

            if(!String.IsNullOrEmpty(clickid) && goal != -1)
            {
                using (var _db = new hemenmoContainer())
                {
                    _db.sp_update_the_click_id(clickid, goal);
                }
            }

            return View();
        }

        public ActionResult hata404()
        {
            home model = new home();

            model = home.GetModel();

            return View("hata404", model);
        }

        public ActionResult hata500()
        {
            home model = new home();

            model = home.GetModel();

            return View("hata500", model);
        }
    }
}
