using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Testezin.Entidades;

namespace MVC.Controllers
{
    public class Login : Controller
    {
        private readonly HttpClient httpClient;

        public Login()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5058/");
        }

        public async Task<IActionResult> Logar(Usuarios usuarios){
            var resposta = await httpClient.GetAsync("Usuario/Usuarios/login/dd/dd");
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            var usuario = Newtonsoft.Json.JsonConvert.DeserializeObject<Usuarios>(dados);
            return View("Login", usuario);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}