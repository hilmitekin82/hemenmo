using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hemenmo.Models;

namespace hemenmo.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public ActionResult category(string category)
        {
            Game model = new Game();
            using (var db = new hemenmoContainer())
            {
                List<PanelKategoriler> kategoriler = new List<PanelKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    PanelKategoriler kategori = new PanelKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);

                    if (item.seo_url==category)
                    {
                        model.CategoryDescription = item.description;
                        model.CategoryIcon = item.icon;
                        model.CategoryName = item.category;
                    }
                }
                model.Kategoriler = kategoriler;

                List<KategoryOyunlar> oyunlar = new List<KategoryOyunlar>();
                foreach (var item in db.sp_list_games())
                {
                    KategoryOyunlar oyun = new KategoryOyunlar();
                    oyun.game_name = item.name;
                    oyun.game_name_seo = item.name_seo;
                    oyun.rating = item.rating;
                    oyun.smallimage = item.smallimage;
                    oyunlar.Add(oyun);
                }
                model.Oyunlar = oyunlar;

                model.PageTitle = "Hemen Mobil Oyna";
            }
            return View();
        }

        public ActionResult gameplay()
        {
            return View();
        }

        public ActionResult search()
        {
            return View();
        }
    }
}
