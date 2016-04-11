using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hemenmo.Models
{
    public class Game
    {
        public List<PanelKategoriler> Kategoriler { get; set; }
        public List<KategoryOyunlar> Oyunlar { get; set; }
        public string PageTitle { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
        public string CategoryDescription { get; set; }
    }

    public class PanelKategoriler
    {
        public string category_name { get; set; }
        public int category_id { get; set; }
        public string category_name_seo { get; set; }
        public int category_top_menu { get; set; }
        public int category_panel_menu { get; set; }
        public string category_icon { get; set; }
    }

    public class KategoryOyunlar
    {
        public string game_name { get; set; }
        public string game_name_seo { get; set; }
        public int rating { get; set; }
        public string smallimage { get; set; }
    }
}