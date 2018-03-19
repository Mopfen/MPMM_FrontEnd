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
            string message = "<table style='width:70%'><tr><td>ID</td><td>Nazwa</td><td>Tresc</td></tr>";
            foreach (var value in lst.listaObiektow.OrderBy(x => x.id))
            {
                message += "<tr><td>" + value.id + "</td>";
                message += "<td>" + value.nazwa + "</td>";
                message += "<td>" + value.tresc + "</td></tr>";
            }
            message += "</table>";
            return message;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
