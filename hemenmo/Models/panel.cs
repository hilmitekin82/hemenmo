using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hemenmo.Models
{
    public class panel
    {
        public int oyunsay { get; set; }
        public int katsay { get; set; }
        public int slidersay { get; set; }

        public List<AdminKategoriler> AdminKategoriler { get; set; }
        public List<AdminHomeOyunlar> AdminOyunlar { get; set; }
        public string AdminPageTitle { get; set; }
        public string AdminCategoryName { get; set; }
        public string AdminCategorySeoName { get; set; }
        public string AdminCategoryIcon { get; set; }
        public int AdminCategoryId { get; set; }
        public int AdminCategoryTopMenu { get; set; }
        public int AdminCategoryPanel { get; set; }
        public string AdminCategoryDescription { get; set; }
        public List<AdminSlider> Adminslider { get; set; }
        

        public int Admingame_id { get; set; }
        public string Admingame_name { get; set; }
        public string Admingame_name_seo { get; set; }
        public int Admingame_cat_id { get; set; }
        public string Admingame_gameplay_url { get; set; }
        public string Admingame_smallimage { get; set; }
        public string Admingame_bigimage { get; set; }
        public string Admingame_title { get; set; }
        public string Admingame_description { get; set; }
        public string Admingame_embed_code { get; set; }
        public string Admingame_spot { get; set; }
        public int Admingame_rating { get; set; }
        public int Admingame_play_count { get; set; }
        public int Admingame_likes { get; set; }
        public int Admingame_dislikes { get; set; }
        public int AdminGamerozet { get; set; }
    }

    public class AdminKategoriler
    {
        public string category_name { get; set; }
        public int category_id { get; set; }
        public string category_name_seo { get; set; }
        public int category_top_menu { get; set; }
        public int category_panel_menu { get; set; }
        public string category_icon { get; set; }
    }

    public class AdminSlider
    {
        public string image_name { get; set; }
        public string image_link { get; set; }
        public int image_order { get; set; }
        public int image_active { get; set; }
        public int image_id { get; set; }
    }

    public class AdminHomeOyunlar
    {
        public int game_id { get; set; }
        public string game_name { get; set; }
        public string game_name_seo { get; set; }
        public int rating { get; set; }
        public string smallimage { get; set; }
        public int cat_id { get; set; }
        public int rozet { get; set; }
    }
}