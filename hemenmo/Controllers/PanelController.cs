using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hemenmo.Models;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace hemenmo.Controllers
{
    public class PanelController : Controller
    {
        //
        // GET: /Panel/

        public ActionResult giris()
        {
            try
            {
                if (!String.IsNullOrEmpty(Session["KonsolUser"].ToString()))
                {
                    Response.Redirect("/panel");
                }
            }
            catch (Exception)
            {

            }

            return View();
        }

        [HttpPost]
        public void giris(string usernamefield, string passwordfield)
        {
            using (var userstat = new hemenmoContainer())
            {
                bool durum = false;
                var durumk = userstat.sp_user_login(usernamefield, passwordfield);
                foreach (var item in durumk)
                {
                    durum = item.Value;
                }

                if (durum)
                {
                    Session["KonsolUser"] = usernamefield;
                }
                else
                {
                    Session["KonsolUser"] = null;
                }
            }
            Response.Redirect("/panel");
        }

        public ActionResult Index()
        {
            try
            {
                if (String.IsNullOrEmpty(Session["KonsolUser"].ToString()))
                {
                    Response.Redirect("/panel/giris");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/panel/giris");
            }
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                model.katsay = db.sep_list_categories().Count();
                model.oyunsay = db.sp_list_games().Count();
                model.slidersay = 0;
            }

            return View("Index", model);
        }

        public ActionResult Categories()
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                List<AdminKategoriler> kategoriler = new List<AdminKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    AdminKategoriler kategori = new AdminKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);
                }
                model.AdminKategoriler = kategoriler;
            }
            return View("Categories",model);
        }

        [HttpPost]
        public void Categories(string category, int? top_menu, int? panel, string description)
        {
            string seo_url = GetSeoUrl(category, 50);
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {

                if (top_menu==null)
                {
                    top_menu = 0;
                }

                if (panel == null)
                {
                    panel = 0;
                }

                db.sp_add_categry(category, seo_url, top_menu, panel, "fa-gamepad", description);

            }
            Response.Redirect("/panel/kategori");
        }

        public ActionResult CategoriesEdit(string category)
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                List<AdminKategoriler> kategoriler = new List<AdminKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    AdminKategoriler kategori = new AdminKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);

                    if (item.seo_url == category)
                    {
                        model.AdminCategoryDescription = item.description;
                        model.AdminCategoryIcon = item.icon;
                        model.AdminCategoryName = item.category;
                        model.AdminCategoryId = item.id;
                        model.AdminCategorySeoName = item.seo_url;
                        model.AdminCategoryPanel = item.panel;
                        model.AdminCategoryTopMenu = item.top_menu;
                    }
                }
                model.AdminKategoriler = kategoriler;
            }
            return View("CategoriesEdit", model);
        }

        [HttpPost]
        public void CategoriesEdit(int id,string category, int? top_menu, int? panel, string description)
        {
            string seo_url = GetSeoUrl(category, 50);
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                if (top_menu == null)
                {
                    top_menu = 0;
                }

                if (panel == null)
                {
                    panel = 0;
                }
                db.sp_update_categry(id, category, seo_url, top_menu, panel, "fa-gamepad", description);
            }
            Response.Redirect("/panel/kategori");
        }

        public ActionResult CategoriesDel(int id)
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                db.sp_del_categry(id);
                List<AdminKategoriler> kategoriler = new List<AdminKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    AdminKategoriler kategori = new AdminKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);
                }

                model.AdminKategoriler = kategoriler;
            }
            return View("Categories", model);
        }


        public ActionResult Games()
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                List<AdminHomeOyunlar> oyunlar = new List<AdminHomeOyunlar>();
                foreach (var item in db.sp_list_games())
                {
                    AdminHomeOyunlar oyun = new AdminHomeOyunlar();
                    oyun.game_id = item.id;
                    oyun.game_name = item.name;
                    oyun.game_name_seo = item.name_seo;
                    oyun.rating = item.rating;
                    oyun.smallimage = item.smallimage;
                    oyun.cat_id = item.cat_id;
                    oyun.rozet = (int)item.rozet;
                    oyunlar.Add(oyun);
                }
                model.AdminOyunlar = oyunlar;

                List<AdminKategoriler> kategoriler = new List<AdminKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    AdminKategoriler kategori = new AdminKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);
                }
                model.AdminKategoriler = kategoriler;
            }
            return View("Games", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Games(string name, int cat_id, string gameplay_url, HttpPostedFileBase smallimage, HttpPostedFileBase bigimage, string title, string description, string embed_code, string spot,int rozet)
        {
            string small_resimcik = "";
            if (smallimage != null)
            {
                string types = "image/jpg , image/png , image/gif , image/jpeg";
                string ctype = smallimage.ContentType;
                if (types.Contains(ctype))
                {
                    ViewData["errmesage"] = "Yükleme Başarılı.";
                    string pic = Path.GetFileName(smallimage.FileName);
                    string path = Server.MapPath("/") + "/images/" + pic;
                    smallimage.SaveAs(path);
                    Bitmap gercekresim = new Bitmap(path);
                    double uzunlukOrani = (double)gercekresim.Width / (double)gercekresim.Height;
                    double yukseklikOrani = (double)gercekresim.Height / (double)gercekresim.Width;
                    Size newSize = new Size();

                    if (gercekresim.Width > gercekresim.Height)
                    {
                        newSize.Width = 60;
                        newSize.Height = Convert.ToInt32(60 * yukseklikOrani);
                    }
                    if (gercekresim.Width < gercekresim.Height)
                    {
                        newSize.Width = Convert.ToInt32(60 * uzunlukOrani);
                        newSize.Height = 60;
                    }
                    if (gercekresim.Width == gercekresim.Height)
                    {
                        newSize.Width = 60;
                        newSize.Height = 60;
                    }

                    Bitmap yeniresim = new Bitmap(gercekresim, newSize);
                    small_resimcik = GetSeoUrl(DateTime.Now.ToShortTimeString(), 50) + "_small_" + Path.GetFileName(smallimage.FileName);
                    yeniresim.Save(Server.MapPath("/") + "/games/" + small_resimcik);
                    gercekresim.Dispose();
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                ViewData["errmesage"] = "Dosya Tipi Doğru Değil.";
            }

            string big_resimcik = "";
            if (bigimage != null)
            {
                string types = "image/jpg , image/png , image/gif , image/jpeg";
                string ctype = bigimage.ContentType;
                if (types.Contains(ctype))
                {
                    ViewData["errmesage"] = "Yükleme Başarılı.";
                    string pic = Path.GetFileName(bigimage.FileName);
                    string path = Server.MapPath("/") + "/images/" + pic;
                    bigimage.SaveAs(path);
                    Bitmap gercekresim = new Bitmap(path);
                    double uzunlukOrani = (double)gercekresim.Width / (double)gercekresim.Height;
                    double yukseklikOrani = (double)gercekresim.Height / (double)gercekresim.Width;
                    Size newSize = new Size();

                    if (gercekresim.Width > gercekresim.Height)
                    {
                        newSize.Width = 120;
                        newSize.Height = Convert.ToInt32(120 * yukseklikOrani);
                    }
                    if (gercekresim.Width < gercekresim.Height)
                    {
                        newSize.Width = Convert.ToInt32(120 * uzunlukOrani);
                        newSize.Height = 120;
                    }
                    if (gercekresim.Width == gercekresim.Height)
                    {
                        newSize.Width = 120;
                        newSize.Height = 120;
                    }

                    Bitmap yeniresim = new Bitmap(gercekresim, newSize);
                    big_resimcik = GetSeoUrl(DateTime.Now.ToShortTimeString(), 50) + "_big_" + Path.GetFileName(bigimage.FileName);
                    yeniresim.Save(Server.MapPath("/") + "/games/" + big_resimcik);
                    gercekresim.Dispose();
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                ViewData["errmesage"] = "Dosya Tipi Doğru Değil.";
            }

            string name_seo = GetSeoUrl(name,50);
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                db.sp_insert_game(name, name_seo, cat_id, gameplay_url, small_resimcik, big_resimcik, title, description, embed_code, spot, 0,0,0,0,rozet);
            }
            Response.Redirect("/panel/oyunlar");
        }

        public ActionResult GamesEdit(string gameplay)
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                List<AdminHomeOyunlar> oyunlar = new List<AdminHomeOyunlar>();
                foreach (var item in db.sp_list_games())
                {
                    AdminHomeOyunlar oyun = new AdminHomeOyunlar();
                    oyun.game_id = item.id;
                    oyun.game_name = item.name;
                    oyun.game_name_seo = item.name_seo;
                    oyun.rating = item.rating;
                    oyun.smallimage = item.smallimage;
                    oyun.cat_id = item.cat_id;
                    oyun.rozet = (int)item.rozet;
                    oyunlar.Add(oyun);
                }
                model.AdminOyunlar = oyunlar;

                List<AdminKategoriler> kategoriler = new List<AdminKategoriler>();
                foreach (var item in db.sep_list_categories())
                {
                    AdminKategoriler kategori = new AdminKategoriler();
                    kategori.category_id = item.id;
                    kategori.category_name = item.category;
                    kategori.category_name_seo = item.seo_url;
                    kategori.category_panel_menu = item.panel;
                    kategori.category_top_menu = item.top_menu;
                    kategori.category_icon = item.icon;
                    kategoriler.Add(kategori);
                }
                model.AdminKategoriler = kategoriler;

                foreach (var oyun in db.sp_get_game(gameplay))
                {
                    model.Admingame_id = oyun.id;
                    model.Admingame_name = oyun.name;
                    model.Admingame_name_seo = oyun.name_seo;
                    model.Admingame_cat_id = oyun.cat_id;
                    model.Admingame_gameplay_url = oyun.gameplay_url;
                    model.Admingame_smallimage = oyun.smallimage;
                    model.Admingame_bigimage = oyun.bigimage;
                    model.Admingame_title = oyun.title;
                    model.Admingame_description = oyun.description;
                    model.Admingame_embed_code = oyun.embed_code;
                    model.Admingame_spot = oyun.spot;
                    model.Admingame_rating = oyun.rating;
                    model.Admingame_play_count = oyun.play_count;
                    model.Admingame_likes = oyun.likes;
                    model.Admingame_dislikes = oyun.dislikes;
                    model.AdminGamerozet = (int)oyun.rozet;
                }
            }
            return View("GamesEdit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public void GamesEdit(int? id, string name, int cat_id, string gameplay_url, HttpPostedFileBase smallimage, HttpPostedFileBase bigimage, string title, string description, string embed_code, string spot,int rozet)
        {
            string small_resimcik = "";
            if (smallimage != null)
            {
                string types = "image/jpg , image/png , image/gif , image/jpeg";
                string ctype = smallimage.ContentType;
                if (types.Contains(ctype))
                {
                    ViewData["errmesage"] = "Yükleme Başarılı.";
                    string pic = Path.GetFileName(smallimage.FileName);
                    string path = Server.MapPath("/") + "/images/" + pic;
                    smallimage.SaveAs(path);
                    Bitmap gercekresim = new Bitmap(path);
                    double uzunlukOrani = (double)gercekresim.Width / (double)gercekresim.Height;
                    double yukseklikOrani = (double)gercekresim.Height / (double)gercekresim.Width;
                    Size newSize = new Size();

                    if (gercekresim.Width > gercekresim.Height)
                    {
                        newSize.Width = 60;
                        newSize.Height = Convert.ToInt32(60 * yukseklikOrani);
                    }
                    if (gercekresim.Width < gercekresim.Height)
                    {
                        newSize.Width = Convert.ToInt32(60 * uzunlukOrani);
                        newSize.Height = 60;
                    }
                    if (gercekresim.Width == gercekresim.Height)
                    {
                        newSize.Width = 60;
                        newSize.Height = 60;
                    }

                    Bitmap yeniresim = new Bitmap(gercekresim, newSize);
                    small_resimcik = GetSeoUrl(DateTime.Now.ToShortTimeString(), 50) + "_small_" + Path.GetFileName(smallimage.FileName);
                    yeniresim.Save(Server.MapPath("/") + "/games/" + small_resimcik);
                    gercekresim.Dispose();
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                ViewData["errmesage"] = "Dosya Tipi Doğru Değil.";
            }

            string big_resimcik = "";
            if (bigimage != null)
            {
                string types = "image/jpg , image/png , image/gif , image/jpeg";
                string ctype = bigimage.ContentType;
                if (types.Contains(ctype))
                {
                    ViewData["errmesage"] = "Yükleme Başarılı.";
                    string pic = Path.GetFileName(bigimage.FileName);
                    string path = Server.MapPath("/") + "/images/" + pic;
                    bigimage.SaveAs(path);
                    Bitmap gercekresim = new Bitmap(path);
                    double uzunlukOrani = (double)gercekresim.Width / (double)gercekresim.Height;
                    double yukseklikOrani = (double)gercekresim.Height / (double)gercekresim.Width;
                    Size newSize = new Size();

                    if (gercekresim.Width > gercekresim.Height)
                    {
                        newSize.Width = 120;
                        newSize.Height = Convert.ToInt32(120 * yukseklikOrani);
                    }
                    if (gercekresim.Width < gercekresim.Height)
                    {
                        newSize.Width = Convert.ToInt32(120 * uzunlukOrani);
                        newSize.Height = 120;
                    }
                    if (gercekresim.Width == gercekresim.Height)
                    {
                        newSize.Width = 120;
                        newSize.Height = 120;
                    }

                    Bitmap yeniresim = new Bitmap(gercekresim, newSize);
                    big_resimcik = GetSeoUrl(DateTime.Now.ToShortTimeString(), 50) + "_big_" + Path.GetFileName(bigimage.FileName);
                    yeniresim.Save(Server.MapPath("/") + "/games/" + big_resimcik);
                    gercekresim.Dispose();
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                ViewData["errmesage"] = "Dosya Tipi Doğru Değil.";
            }

            string name_seo = GetSeoUrl(name, 50);
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                db.sp_update_game(id, name, name_seo, cat_id, gameplay_url, small_resimcik, big_resimcik, title, description, embed_code, spot,rozet);
            }
            Response.Redirect("/panel/oyunlar");
        }

        public ActionResult GamesDel(int id)
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                db.sp_delete_game(id);
            }
            return Redirect("/panel/oyunlar");
        }



        public ActionResult Sliders()
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                List<AdminSlider> slidelar = new List<AdminSlider>();
                foreach (var item in db.sp_list_slider())
                {
                    AdminSlider slide = new AdminSlider();
                    slide.image_name = item.image;
                    slide.image_order = item.slide_order;
                    slide.image_id = item.id;
                    slide.image_link = item.link;
                   
                    slidelar.Add(slide);
                }
                model.Adminslider = slidelar;
            }
            return View("Sliders", model);
        }

        [HttpPost]
        public void Sliders(HttpPostedFileBase resim, string link, int order)
        {
            string resimcik = "";
            if (resim != null)
            {
                string types = "image/jpg , image/png , image/gif , image/jpeg";
                string ctype = resim.ContentType;
                if (types.Contains(ctype))
                {
                    ViewData["errmesage"] = "Yükleme Başarılı.";
                    string pic = Path.GetFileName(resim.FileName);
                    string path = Server.MapPath("/") + "/images/" + pic;
                    resim.SaveAs(path);
                    Bitmap gercekresim = new Bitmap(path);
                    double uzunlukOrani = (double)gercekresim.Width / (double)gercekresim.Height;
                    double yukseklikOrani = (double)gercekresim.Height / (double)gercekresim.Width;
                    Size newSize = new Size();

                    if (gercekresim.Width > gercekresim.Height)
                    {
                        newSize.Width = 800;
                        newSize.Height = Convert.ToInt32(800 * yukseklikOrani);
                    }
                    if (gercekresim.Width < gercekresim.Height)
                    {
                        newSize.Width = Convert.ToInt32(450 * uzunlukOrani);
                        newSize.Height = 450;
                    }
                    if (gercekresim.Width == gercekresim.Height)
                    {
                        newSize.Width = 450;
                        newSize.Height = 450;
                    }

                    Bitmap yeniresim = new Bitmap(gercekresim, newSize);
                    resimcik = GetSeoUrl(DateTime.Now.ToShortTimeString(), 50) +"_"+ Path.GetFileName(resim.FileName);
                    yeniresim.Save(Server.MapPath("/") + "/sliders/" + resimcik);
                    gercekresim.Dispose();
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                ViewData["errmesage"] = "Dosya Tipi Doğru Değil.";
            }
            using (var db = new hemenmoContainer())
            {
                db.sp_add_slider(resimcik, order,link, 1);
            }

            Response.Redirect("/panel/slider");
        }

        public ActionResult SlidersDel(int id)
        {
            panel model = new panel();
            using (var db = new hemenmoContainer())
            {
                db.sp_del_slider(id);

            }
            return Redirect("/panel/slider");
        }

        public static string GetSeoUrl(string title, int maxLength)
        {
            title = ChangeTurkishCharacterToEnglish(title.Trim().ToLower());
            // invalid chars, make into spaces
            title = Regex.Replace(title, @"[^a-z0-9\s-]", "");
            // convert multiple spaces/hyphens into one space
            title = Regex.Replace(title, @"[\s-]+", " ").Trim();
            // cut and trim it
            title = title.Substring(0, title.Length <= maxLength ? title.Length : maxLength).Trim();
            // hyphens
            title = Regex.Replace(title, @"\s", "-");

            return title;
        }

        public static string ChangeTurkishCharacterToEnglish(string strText)
        {
            strText = strText.Replace("Ç", "C");
            strText = strText.Replace("ç", "c");
            strText = strText.Replace("Ğ", "G");
            strText = strText.Replace("ğ", "g");
            strText = strText.Replace("İ", "I");
            strText = strText.Replace("ı", "i");
            strText = strText.Replace("Ö", "O");
            strText = strText.Replace("ö", "o");
            strText = strText.Replace("Ş", "S");
            strText = strText.Replace("ş", "s");
            strText = strText.Replace("Ü", "U");
            strText = strText.Replace("ü", "u");
            return strText;
        }

    }
}
