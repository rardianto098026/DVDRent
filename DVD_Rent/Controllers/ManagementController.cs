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
        
        public ActionResult AddAdminPost(ManagementModels.AddAdmin<ManagementModels.AddAdmin> model)
        {
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
            return View(model);
        }

        public ActionResult AddModifyMoviePost(ManagementModels.AddAdmin<ManagementModels.AddAdmin> model)
        {
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
    }
}