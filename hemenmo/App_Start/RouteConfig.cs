using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace hemenmo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: "mobil-oyun", url: "mobil-oyunlar", defaults: new { controller = "Home", action = "mobil_oyun" });
            routes.MapRoute(name: "siparis", url: "siparis", defaults: new { controller = "Home", action = "siparis" });
            routes.MapRoute(name: "hata404", url: "hata404", defaults: new { controller = "Home", action = "hata404" });
            routes.MapRoute(name: "hata500", url: "hata500", defaults: new { controller = "Home", action = "hata500" });

            routes.MapRoute(name: "adminpanelgiris", url: "panel/giris", defaults: new { controller = "Panel", action = "giris" });
            routes.MapRoute(name: "adminpanel",url: "panel",defaults: new { controller = "Panel", action = "Index" });
            routes.MapRoute(name: "adminkategoriler", url: "panel/kategori", defaults: new { controller = "Panel", action = "Categories", category = UrlParameter.Optional });
            routes.MapRoute(name: "adminkategoriedit", url: "panel/kategoriedit/{category}", defaults: new { controller = "Panel", action = "CategoriesEdit", category = UrlParameter.Optional });
            routes.MapRoute(name: "adminkategoridel", url: "panel/kategorisil/{id}", defaults: new { controller = "Panel", action = "CategoriesDel" });
            routes.MapRoute(name: "adminslider", url: "panel/slider", defaults: new { controller = "Panel", action = "Sliders"});
            routes.MapRoute(name: "adminsliderdel", url: "panel/slidersil/{id}", defaults: new { controller = "Panel", action = "SlidersDel" });
            routes.MapRoute(name: "adminoyunlar", url: "panel/oyunlar", defaults: new { controller = "Panel", action = "Games", gameplay = UrlParameter.Optional });
            routes.MapRoute(name: "adminoyunedit", url: "panel/oyunedit/{gameplay}", defaults: new { controller = "Panel", action = "GamesEdit", gameplay = UrlParameter.Optional });
            routes.MapRoute(name: "adminoyundel", url: "panel/oyunsil/{id}", defaults: new { controller = "Panel", action = "GamesDel" });


            routes.MapRoute(
    name: "category",
    url: "oyun/{category}",
    defaults: new { controller = "Home", action = "category" }
);

            routes.MapRoute(
                name: "category_ara",
                url: "oyun/ara/{category}",
                defaults: new { controller = "Home", action = "search", category = UrlParameter.Optional }
            );
            
            routes.MapRoute(name: "most_rate", url: "one-cikan-mobil-oyunlar", defaults: new { controller = "Home", action = "list", type = "most_rate" });
            routes.MapRoute(name: "most_play", url: "en-cok-oynanan-mobil-oyunlar", defaults: new { controller = "Home", action = "list", type = "most_play" });
            routes.MapRoute(name: "most_new", url: "en-yeni-mobil-oyunlar", defaults: new { controller = "Home", action = "list", type = "most_new" });
            routes.MapRoute(name: "most_likes", url: "en-cok-sevilen-mobil-oyunlar", defaults: new { controller = "Home", action = "list", type = "most_likes" });

            routes.MapRoute(name: "gameplay", url: "oyna/{gameplay}", defaults: new { controller = "Home", action = "gameplay"} );
            routes.MapRoute(name: "gameplay_like", url: "oyna/{gameplay}/like", defaults: new { controller = "Home", action = "gameplay_likethis" });
            routes.MapRoute(name: "gameplay_dislike", url: "oyna/{gameplay}/dislike", defaults: new { controller = "Home", action = "gameplay_dislikethis" });
            

            routes.MapRoute(
    name: "playnow",
    url: "oyna/{gameplay}/hemenoyna",
    defaults: new { controller = "Home", action = "playnow" }
);
routes.MapRoute(name: "gameplay_vote", url: "oyna/{gameplay}/{vote}", defaults: new { controller = "Home", action = "gameplay_votethis" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}