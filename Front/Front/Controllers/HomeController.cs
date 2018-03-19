using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Front.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace Front.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }
        public IActionResult Server()
        {
            ViewData["Message"] = loadData(); ;
            return View();
        }

        private string loadData()
        {
            ListOfValues lst = new ListOfValues();
            lst.listaObiektow = null;
            //List<Value> values = null;
            var client = new HttpClient();
            var task = client.GetAsync("http://mpmmtest.azurewebsites.net/api/values")
              .ContinueWith((taskwithresponse) =>
              {
                  var response = taskwithresponse.Result;
                  var jsonString = response.Content.ReadAsStringAsync();
                  jsonString.Wait();
                  lst = JsonConvert.DeserializeObject<ListOfValues>(jsonString.Result);
              });
            task.Wait();
            string message = "Atrybut | Wartość <br>";
            foreach(var value in lst.listaObiektow)
            {
                message += "ID: " + value.id + "<br>";
                message += "Nazwa: " + value.nazwa + "<br>";
                message += "Tresc: " + value.tresc + "<br>";
            }
            return message;
            /*string page = "http://mpmmtest.azurewebsites.net/api/values/1";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage respone = await client.GetAsync(page))
            using (HttpContent content = respone.Content)
            {
                string data = await content.ReadAsStringAsync();

                if (data != null)
                    message = data;
                loaded = true;
            }*/
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
