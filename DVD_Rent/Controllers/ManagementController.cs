using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DVD_Rent.Models;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using System.Collections;
using System.IO;

namespace DVD_Rent.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        public ActionResult Customer(ManagementModels.Customer<ManagementModels.Customer> model)
        {
            if(Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetCustomer");
                //HTTP GET
                var responseTask = client.GetAsync("GetCustomer");
                responseTask.Wait();
                
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    DataTable dtCust = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                    model.t_cust = ListCustomer(dtCust);
                }
            }
            return View(model);
        }

        public ActionResult Admin(ManagementModels.Admin<ManagementModels.Admin> model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetAdmin");
                //HTTP GET
                var responseTask = client.GetAsync("GetAdmin");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    DataTable dt_admin = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                    model.t_admin = ListAdmin(dt_admin);
                }
            }
            return View(model);
        }

        public ActionResult Movie(ManagementModels.Movie<ManagementModels.Movie> model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetMovie");
                //HTTP GET
                var responseTask = client.GetAsync("GetMovie");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    DataTable dt_movie = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                    model.t_movie = ListMovie(dt_movie);
                }
            }
            return View(model);
        }

        public ActionResult AddAdmin(ManagementModels.AddAdmin<ManagementModels.AddAdmin> model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }

            DataTable tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/Users/LoadDDLstore");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLstore");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }

            model.listDDLStore = PopulateStoreDataTable(tableModel);

            tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/Users/LoadDDLposition");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLposition");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }
            model.listDDLPosition = PopulatepositionDataTable(tableModel);
            return View(model);
        }
        
        public ActionResult AddAdminPost(ManagementModels.AddAdmin<ManagementModels.AddAdmin> model, HttpPostedFileBase file_img)
        {
            if (file_img != null)
            {
                string pic = System.IO.Path.GetFileName(file_img.FileName);
                string path = System.IO.Path.Combine(("E:\\Data User\\rardi\\Documents\\Visual Studio 2015\\Projects\\DVDRent_api\\DVDRent_api\\Assets\\img\\employee"), pic);
                // file is uploaded
                file_img.SaveAs(path);
                model.picture_URL = "http://localhost:26403/Assets/img/employee/" + pic;
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file_img.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/Users/AddEmployee");
                
                var postJob = client.PostAsJsonAsync<ManagementModels.AddAdmin<ManagementModels.AddAdmin>>("AddEmployee", model);
                postJob.Wait();

                var postResult = postJob.Result;
                var result = postResult.Content.ReadAsStringAsync().Result;

                if (postResult.IsSuccessStatusCode)
                {
                    return RedirectToAction("../Management/Admin");
                }

                ModelState.AddModelError(string.Empty, "Server occured errors. Please check with admin!");
            }
            return View(model);
        }

        public ActionResult AddModifyMovie(ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie> model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }

            DataTable tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/LoadDDLagerating");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLagerating");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }
            model.listDDLAgeRating = PopulateAgeRatingDataTable(tableModel);

            tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/LoadGenre");
                //HTTP GET
                var responseTask = client.GetAsync("LoadGenre");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }

            model.listGenre = ListGenre(tableModel);
            List<string> AllgenreMovie = new List<string>();
            for (int i = 0; i < model.listGenre.Count; i++)
            {
                AllgenreMovie.Add(model.listGenre[i].GenreID.ToString());
            }
            Session["ListGenre"] = AllgenreMovie.ToArray();
            //string[] check = Session["ListGenre"] as string[];

            string id = Request.QueryString["mov_id"];
            if (id != null)
            {
                Session["mov_id"] = id;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetDetailMovie?id=" + id + "");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetDetailMovie?id=" + id + "");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        DataTable dtCust = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                        model.Title = dtCust.Rows[0]["Title"].ToString();
                        model.Description = dtCust.Rows[0]["Description"].ToString();
                        model.ReleaseYear = dtCust.Rows[0]["ReleaseYear"].ToString();
                        model.Duration = dtCust.Rows[0]["Duration"].ToString();
                        model.AgeRating = dtCust.Rows[0]["AgeRatingID"].ToString();
                        model.PictureURL = dtCust.Rows[0]["PictureURL"].ToString();
                        model.TrailerURL = dtCust.Rows[0]["TrailerURL"].ToString();

                        List<string> genreMovie = new List<string>();
                        for (int i = 0; i < dtCust.Rows.Count; i++)
                        {
                            genreMovie.Add(dtCust.Rows[i]["GenreID"].ToString());
                        }
                        model.Genre = genreMovie.ToArray();
                        Session["selectedGenreLoad"] = model.Genre;

                        TempData["detailMovie"] = model;
                        return View(model);
                    }
                }
            }
            return View(model);
        }

        public ActionResult AddModifyMoviePost(ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie> model, string[] checkboxesGenre, HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(("E:\\Data User\\rardi\\Documents\\Visual Studio 2015\\Projects\\DVDRent_api\\DVDRent_api\\Assets\\img\\cover"), pic);
                // file is uploaded
                file.SaveAs(path);
                model.PictureURL = "http://localhost:26403/Assets/img/cover/" + pic;
                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }

            string id;
            if (Session["mov_id"] != null)
            {
                id = Session["mov_id"].ToString();

                string[] AllgenreMovie = Session["ListGenre"] as string[];
                string[] SelectedgenreLoad = Session["selectedGenreLoad"] as string[];

                model.TambahGenre = checkboxesGenre.Except(SelectedgenreLoad).ToArray();
                model.KurangGenre = SelectedgenreLoad.Except(checkboxesGenre).ToArray();

                using (var client = new HttpClient())
                {
                    model.Genre = checkboxesGenre;
                    client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/UpdateMovie?mov_id="+ id +"");

                    var postJob = client.PutAsJsonAsync<ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie>>("UpdateMovie?mov_id=" + id + "", model);
                    postJob.Wait();

                    var postResult = postJob.Result;
                    var result = postResult.Content.ReadAsStringAsync().Result;

                    if (postResult.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Berhasil mengupdate movie!";
                        Session["ListGenre"] = null;
                        Session["selectedGenreLoad"] = null;
                        Session["mov_id"] = null;

                        return RedirectToAction("../Management/Movie");
                    }

                    ModelState.AddModelError(string.Empty, "Server occured errors. Please check with admin!");
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    model.Genre = checkboxesGenre;
                    client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/AddMovie");

                    var postJob = client.PostAsJsonAsync<ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie>>("AddMovie", model);
                    postJob.Wait();

                    var postResult = postJob.Result;
                    var result = postResult.Content.ReadAsStringAsync().Result;

                    if (postResult.IsSuccessStatusCode)
                    {
                        TempData["msg"] = "Berhasil menambahkan movie!";
                        Session["ListGenre"] = null;
                        Session["selectedGenreLoad"] = null;
                        Session["mov_id"] = null;

                        return RedirectToAction("../Management/Movie");
                    }

                    ModelState.AddModelError(string.Empty, "Server occured errors. Please check with admin!");
                }
            }
            return View(model);
        }

        public ActionResult AddModifyInventory(ManagementModels.AddModifyInventory<ManagementModels.AddModifyInventory> model)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("../Login/Index");
            }

            DataTable tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/LoadDDLmovie");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLmovie");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }
            model.listDDLMovie = PopulateDDLMovie(tableModel);

            tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/Users/LoadDDLstore");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLstore");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }

            model.listDDLStore = PopulateDDLStore(tableModel);

            tableModel = new DataTable();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/LoadDDLstatusMovie");
                //HTTP GET
                var responseTask = client.GetAsync("LoadDDLstatusMovie");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    tableModel = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                }
            }

            model.listDDLStatus = PopulateDDLMovieStatus(tableModel);
            
            string id = Request.QueryString["mov_id"];
            if (id != null)
            {
                Session["mov_id"] = id;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetDetailMovie?id=" + id + "");
                    //HTTP GET
                    var responseTask = client.GetAsync("GetDetailMovie?id=" + id + "");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsStringAsync().Result;
                        DataTable dtCust = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                        //model.Title = dtCust.Rows[0]["Title"].ToString();
                        //model.Description = dtCust.Rows[0]["Description"].ToString();
                        //model.ReleaseYear = dtCust.Rows[0]["ReleaseYear"].ToString();
                        //model.Duration = dtCust.Rows[0]["Duration"].ToString();
                        //model.AgeRating = dtCust.Rows[0]["AgeRatingID"].ToString();
                        //model.PictureURL = dtCust.Rows[0]["PictureURL"].ToString();
                        //model.TrailerURL = dtCust.Rows[0]["TrailerURL"].ToString();

                        List<string> genreMovie = new List<string>();
                        for (int i = 0; i < dtCust.Rows.Count; i++)
                        {
                            genreMovie.Add(dtCust.Rows[i]["GenreID"].ToString());
                        }
                        //model.Genre = genreMovie.ToArray();
                        //Session["selectedGenreLoad"] = model.Genre;

                        TempData["detailMovie"] = model;
                        return View(model);
                    }
                }
            }
            return View(model);
        }


        private static List<ManagementModels.AddModifyMovie> PopulateAgeRatingDataTable(DataTable dt)
        {
            List<ManagementModels.AddModifyMovie> model = new List<ManagementModels.AddModifyMovie>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddModifyMovie
                {
                    AgeRating = dr["Rating"].ToString(),
                    AgeRatingID = dr["ID"].ToString(),
                });
            }
            return model;
        }

        private static List<ManagementModels.AddAdmin> PopulateStoreDataTable(DataTable dt)
        {
            List<ManagementModels.AddAdmin> model = new List<ManagementModels.AddAdmin>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddAdmin
                {
                    store = dr["StoreName"].ToString(),
                    storeID = dr["ID"].ToString(),
                });
            }
            return model;
        }

        private static List<ManagementModels.AddModifyInventory> PopulateDDLStore(DataTable dt)
        {
            List<ManagementModels.AddModifyInventory> model = new List<ManagementModels.AddModifyInventory>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddModifyInventory
                {
                    Store = dr["StoreName"].ToString(),
                    StoreID = dr["ID"].ToString(),
                });
            }
            return model;
        }


        private static List<ManagementModels.AddAdmin> PopulatepositionDataTable(DataTable dt)
        {
            List<ManagementModels.AddAdmin> model = new List<ManagementModels.AddAdmin>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddAdmin
                {
                    position = dr["position"].ToString(),
                    positionID = dr["ID"].ToString()
                });
            }
            return model;
        }

        private static List<ManagementModels.AddModifyInventory> PopulateDDLMovie(DataTable dt)
        {
            List<ManagementModels.AddModifyInventory> model = new List<ManagementModels.AddModifyInventory>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddModifyInventory
                {
                    Movie = dr["Title"].ToString() + " - " + dr["ReleaseYear"].ToString(),
                    MovieID = dr["ID"].ToString(),
                });
            }
            return model;
        }

        private static List<ManagementModels.AddModifyInventory> PopulateDDLMovieStatus(DataTable dt)
        {
            List<ManagementModels.AddModifyInventory> model = new List<ManagementModels.AddModifyInventory>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddModifyInventory
                {
                    StatusMovie = dr["Status"].ToString(),
                    StatusMovieID = dr["ID"].ToString(),
                });
            }
            return model;
        }

        public static List<ManagementModels.AddModifyMovie> ListGenre(DataTable dt)
        {
            List<ManagementModels.AddModifyMovie> model = new List<ManagementModels.AddModifyMovie>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.AddModifyMovie
                {
                    Genre = dr["Genre"].ToString(),
                    GenreID = dr["ID"].ToString()
                });
            }
            return model;
        }

        public static List<ManagementModels.Customer> ListCustomer(DataTable dt)
        {
            List<ManagementModels.Customer> model = new List<ManagementModels.Customer>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.Customer
                {
                    id = Convert.ToInt32(dr["id"]),
                    firstname = dr["firstname"].ToString(),
                    lastname = dr["lastname"].ToString(),
                    email = dr["email"].ToString(),
                    address = dr["address"].ToString(),
                    onrent = dr["onrent"].ToString(),
                    endedrent = dr["returndate"].ToString(),
                    active = Convert.ToBoolean(dr["active"])
                });
            }
            return model;
        }

        public static List<ManagementModels.Admin> ListAdmin(DataTable dt)
        {
            List<ManagementModels.Admin> model = new List<ManagementModels.Admin>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.Admin
                {
                    id = Convert.ToInt32(dr["id"]),
                    user_id = Convert.ToInt32(dr["UserID"]),
                    address = dr["address"].ToString() + " " + dr["address2"].ToString(),
                    store = dr["storename"].ToString(),
                    email = dr["Email"].ToString(),
                    name = dr["firstname"].ToString() + " " + dr["lastname"].ToString(),
                    position = dr["position"].ToString(),
                    active = Convert.ToBoolean(dr["active"]),
                    createdDate = Convert.ToDateTime(dr["createdDate"].ToString()).ToString("dd MMM yyyy").ToString(),
                    updatedDate = Convert.ToDateTime(dr["updatedDate"].ToString()).ToString("dd MMM yyyy").ToString(),
                    pictureURL = dr["pictureurl"].ToString()
                });
            }
            return model;
        }

        public static List<ManagementModels.Movie> ListMovie(DataTable dt)
        {
            List<ManagementModels.Movie> model = new List<ManagementModels.Movie>();
            DataTable tableModel = dt;
            foreach (DataRow dr in tableModel.Rows)
            {
                model.Add(new ManagementModels.Movie
                {
                    id = Convert.ToInt32(dr["id"]),
                    Title = dr["Title"].ToString(),
                    Description = dr["Description"].ToString(),
                    ReleaseYear = dr["ReleaseYear"].ToString(),
                    Duration = dr["Duration"].ToString() + " minutes",
                    AgeRating = dr["Rating"].ToString(),
                    UpdatedDate = dr["UpdatedDate"].ToString(),
                    PictureURL = dr["PictureURL"].ToString(),
                    TrailerURL = dr["TrailerURL"].ToString()
                });
            }
            return model;
        }

        public JsonResult ChangeStatusCust(int id, bool status)
        {
            using (var client = new HttpClient())
            {
                string statusString = status.ToString();
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/UpdateStatusCustomer?id="+ id +"&status="+ statusString + "");

                //HTTP PUT
                var responseTask = client.PutAsync("UpdateStatusCustomer?id=" + id + "&status=" + statusString + "", null);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { success = "reload"}, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = "no_reload" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChangeStatusAdmin(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/DeleteAdmin?id=" + id);

                //HTTP PUT
                var responseTask = client.DeleteAsync("DeleteAdmin?id=" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { success = "reload" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = "no_reload" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditMovie(int mov_id)
        {
            ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie> model = new ManagementModels.AddModifyMovie<ManagementModels.AddModifyMovie>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/DVDRent/GetDetailMovie?id="+ mov_id +"");
                //HTTP GET
                var responseTask = client.GetAsync("GetDetailMovie");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;
                    DataTable dtCust = (DataTable)JsonConvert.DeserializeObject(readTask, (typeof(DataTable)));
                    model.Title = dtCust.Rows[0]["Title"].ToString();
                    model.Description = dtCust.Rows[0]["Description"].ToString();
                    model.ReleaseYear = dtCust.Rows[0]["ReleaseYear"].ToString();
                    model.Duration = dtCust.Rows[0]["Duration"].ToString();
                    model.AgeRating = dtCust.Rows[0]["AgeRating"].ToString();
                    model.PictureURL = dtCust.Rows[0]["PictureURL"].ToString();
                    model.TrailerURL = dtCust.Rows[0]["TrailerURL"].ToString();
                    
                    TempData["detailMovie"] = model;
                    return Json(new { success = "reload" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = "no_reload" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}