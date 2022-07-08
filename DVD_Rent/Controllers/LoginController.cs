using DVD_Rent.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DVD_Rent.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session.Clear();
            Session.Abandon();
            LoginModels model = new LoginModels();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(LoginModels model)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:26403/api/Users/toLogin");
                model.role = 1;
                var postJob = client.PostAsJsonAsync<LoginModels>("toLogin", model);
                postJob.Wait();

                var postResult = postJob.Result;
                var result = postResult.Content.ReadAsStringAsync().Result;

                if (postResult.IsSuccessStatusCode)
                {
                    Session["username"] = result.ToString().Substring(1, result.Length - 2);
                    //string cek = Session["username"].ToString();
                    return RedirectToAction("../Management/Customer");
                }
                model.status = "failed";

                ModelState.AddModelError(string.Empty, "Server occured errors. Please check with admin!");
            }
            return View(model);
        }
    }
}