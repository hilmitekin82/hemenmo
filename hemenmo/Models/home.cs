using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hemenmo.Models
{
    public class home
    {

        public List<Kategoriler> Kategoriler { get; set; }
        public List<HomeOyunlar> Oyunlar { get; set; }
        public string PageTitle { get; set; }
        public string CategoryName { get; set; }
        public string CategorySeoName { get; set; }
        public string CategoryIcon { get; set; }
        public int CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public List<Slider> slider { get; set; }

        public int game_id{get;set;}
      public string game_name{get;set;}
      public string game_name_seo{get;set;}
      public int game_cat_id{get;set;}
      public string game_gameplay_url{get;set;}
      public string game_smallimage{get;set;}
      public string game_bigimage{get;set;}
      public string game_title{get;set;}
      public string game_description{get;set;}
      public string game_embed_code{get;set;}
      public string game_spot{get;set;}
      public int game_rating{get;set;}
      public int game_play_count{get;set;}
      public int game_likes { get; set; }
      public int game_dislikes { get; set; }
      public string rozet { get; set; }
      public DateTime game_add_date { get; set; }
        public static home GetModel()
        {
            var model = new home();
            model.Kategoriler = GetKategoriler();
            model.Oyunlar = GetOyunlar();
            model.slider = GetSlider();
            model.PageTitle = "Hemen Mobil Oyna";

            return model;
        }

        public static home GetCategory(string category)
        {
            var model = new home();
            model.Oyunlar = GetOyunlar();
            model.Kategoriler = GetKategoriler();
            using (var db = new hemenmoContainer())
            {
                foreach (var item in db.sep_list_categories().Where(x => x.seo_url == category))
                {
                    model.CategoryDescription = item.description;
                    model.CategoryIcon = item.icon;
                    model.CategoryName = item.category;
                    model.CategoryId = item.id;
                    model.CategorySeoName = item.seo_url;
                }
            }
            model.PageTitle = model.CategoryName + " Kategorisi - Hemen Mobil Oyna";
            return model;
        }

        public static home GetGamePlay(string gameplay)
        {
            var model = new home();
            model.Kategoriler = GetKategoriler();
            model.Oyunlar = GetOyunlar();
            model.slider = GetSlider();
            using (var db = new hemenmoContainer())
            {
                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.game_id = oyun.id;
                    model.game_name = oyun.name;
                    model.game_name_seo = oyun.name_seo;
                    model.game_cat_id = oyun.cat_id;
                    model.game_gameplay_url = oyun.gameplay_url;
                    model.game_smallimage = oyun.smallimage;
                    model.game_bigimage = oyun.bigimage;
                    model.game_title = oyun.title;
                    model.game_description = oyun.description;
                    model.game_embed_code = oyun.embed_code;
                    model.game_spot = oyun.spot;
                    model.game_rating = oyun.rating;
                    model.game_play_count = oyun.play_count;
                    model.game_likes = oyun.likes;
                    model.game_dislikes = oyun.dislikes;

                    model.rozet = rozet_sec((int)oyun.rozet);
                }

                foreach (var item in db.sep_list_categories().Where(x => x.id == model.game_cat_id))
                {
                    model.CategoryDescription = item.description;
                    model.CategoryIcon = item.icon;
                    model.CategoryName = item.category;
                    model.CategoryId = item.id;
                    model.CategorySeoName = item.seo_url;
                }
                model.PageTitle = model.game_name + " Oyunu - Hemen Mobil Oyna";
            }
            return model;
        }

        public static home GetSearch(string category)
        {
            var model = new home();
            model.Oyunlar = GetOyunlar();
            model.Kategoriler = GetKategoriler();
            if (!string.IsNullOrEmpty(category))
            {
                
                using (var db = new hemenmoContainer())
                {
                    foreach (var item in db.sep_list_categories().Where(x => x.seo_url == category))
                    {
                        model.CategoryDescription = item.description;
                        model.CategoryIcon = item.icon;
                        model.CategoryName = item.category;
                        model.CategoryId = item.id;
                        model.CategorySeoName = item.seo_url;
                    }
                }
                model.PageTitle = model.CategoryName + " Kategorisi - Hemen Mobil Oyna";
            }
            else
            {
                model.PageTitle = "Oyun Arama Sayfası - Hemen Mobil Oyna";
            }
            
            return model;
        }

        public static home GetList(string type)
        {
            var model = new home();
            string title = "";
            model.Kategoriler = GetKategoriler();
            model.Oyunlar = GetOyunlar();
            using (var db = new hemenmoContainer())
            {
                List<HomeOyunlar> oyunlar = new List<HomeOyunlar>();
                var game_container = db.sp_list_games().ToList();
                switch (type)
                {
                    case "most_likes":
                        game_container = db.sp_list_games().OrderByDescending(x => x.likes).ToList();
                        title = "En Çok Sevilen Mobil Oyunlar";
                        break;
                    case "most_play":
                        game_container = db.sp_list_games().OrderByDescending(x => x.play_count).ToList();
                        title = "En Çok Oynanan Mobil Oyunlar";
                        break;
                    case "most_new":
                        game_container = db.sp_list_games().OrderByDescending(x => x.add_date).ToList();
                        title = "En Yeni Mobil Oyunlar";
                        break;
                    case "most_rate":
                        game_container = db.sp_list_games().OrderByDescending(x => x.rating).ToList();
                        title = "Öne Çıkan Mobil Oyunlar";
                        break;
                    default:
                        break;
                }
                model.PageTitle = title;
            }
            return model;
        }
        public static List<Kategoriler> GetKategoriler()
        {
            List<Kategoriler> kategoriler = new List<Kategoriler>();
            using (var db = new hemenmoContainer())
            {
                
                foreach (var item in db.sep_list_categories())
                {
                    Kategoriler kategori = new Kategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);
                }
            }

            return kategoriler;
        }
        public static List<HomeOyunlar> GetOyunlar()
        {

            List<HomeOyunlar> oyunlar = new List<HomeOyunlar>();
            using (var db = new hemenmoContainer())
            {

                foreach (var item in db.sp_list_games())
                {
                    HomeOyunlar oyun = new HomeOyunlar();
                    oyun.game_name = item.name;
                    oyun.game_name_seo = item.name_seo;
                    oyun.rating = item.rating;
                    oyun.smallimage = item.smallimage;
                    oyun.cat_id = item.cat_id;
                    
                    oyun.rozet = rozet_sec((int)item.rozet);
                    oyunlar.Add(oyun);
                }
            }

            return oyunlar;
        }
        public static string rozet_sec(int? rozet_id)
        {
            string rozet_addr = "";
            switch (rozet_id)
            {
                case 1: rozet_addr = "/rozetler/new.png"; break;
                case 2: rozet_addr = "/rozetler/turkce.png"; break;
                case 3: rozet_addr = "/rozetler/private.png"; break;
                default:
                    break;
            }
            return rozet_addr;
        }
        public static List<Slider> GetSlider()
        {

            List<Slider> slider = new List<Slider>();
            using (var db = new hemenmoContainer())
            {
                foreach (var item in db.sp_list_slider())
                {
                    Slider slide = new Slider();
                    slide.image_active = item.active;
                    slide.image_name = "/sliders/" + item.image;
                    slide.image_order = item.slide_order;
                    slide.image_link = item.link;
                    slider.Add(slide);
                }
            }

            return slider;
        }

        public static home GetPlaynow(string gameplay)
        {
            var model = new home();
            model.Oyunlar = GetOyunlar();
            model.Kategoriler = GetKategoriler();
            using (var db = new hemenmoContainer())
            {
                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.game_id = oyun.id;
                    model.game_name = oyun.name;
                    model.game_name_seo = oyun.name_seo;
                    model.game_cat_id = oyun.cat_id;
                    model.game_gameplay_url = oyun.gameplay_url;
                    model.game_smallimage = oyun.smallimage;
                    model.game_bigimage = oyun.bigimage;
                    model.game_title = oyun.title;
                    model.game_description = oyun.description;
                    model.game_embed_code = oyun.embed_code;
                    model.game_spot = oyun.spot;
                    model.game_rating = oyun.rating;
                    model.game_play_count = oyun.play_count;
                    model.game_likes = oyun.likes;
                    model.game_dislikes = oyun.dislikes;
                    model.rozet = rozet_sec((int)oyun.rozet);
                }
                db.set_play_count(model.game_id);

                foreach (var item in db.sep_list_categories().Where(x => x.id == model.game_cat_id))
                {
                    model.CategoryDescription = item.description;
                    model.CategoryIcon = item.icon;
                    model.CategoryName = item.category;
                    model.CategoryId = item.id;
                    model.CategorySeoName = item.seo_url;
                }
                model.PageTitle = model.game_name + " Oyunu - Hemen Mobil Oyna";
            }
            return model;
        }
    }

    public class Kategoriler
    {
        public string category_name { get; set; }
        public int category_id { get; set; }
        public string category_name_seo { get; set; }
        public int category_top_menu { get; set; }
        public int category_panel_menu { get; set; }
        public string category_icon { get; set; }
    }

    public class Slider
    {
        public string image_name { get; set; }
        public int image_order { get; set; }
        public int image_active { get; set; }
        public string image_link { get; set; }
    }

    public class HomeOyunlar
    {
        public string game_name { get; set; }
        public string game_name_seo { get; set; }
        public int rating { get; set; }
        public string smallimage { get; set; }
        public int cat_id { get; set; }
        public DateTime add_date { get; set; }
        public string rozet { get; set; }
    }
}