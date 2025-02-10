using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Testezin.Entidades;

namespace MVC.Controllers
{
    public class HobbiesMVC : Controller
    {
        private readonly HttpClient httpClient;

        public HobbiesMVC()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5058/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
        }

        public IActionResult CriarHobbies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarHobbies(Hobbies hobbies){
            var hobbie = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(hobbies), Encoding.UTF8, "application/json");
            var resposta = await httpClient.PostAsync("Hobbies?idDoUsuario=1", hobbie);
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            return View("AdministrarHobbies");
        }

        public IActionResult AdministrarHobbies()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}