using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;
using Testezin.Entidades;

namespace MVC.Controllers
{
    public class UsuarioMVC : Controller
    {
        private HttpClient httpClient;
        private string token;

        public UsuarioMVC()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5058/");
        }

        public IActionResult CriarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Usuarios usuarios){
            var usuario = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
            var resposta = await httpClient.PostAsync("Usuario/Usuarios", usuario);
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<UsuarioModel>(dados);

            return View("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logar(Usuarios usuarios){
            var resposta = await httpClient.GetAsync($"Usuario/Usuarios/login/{usuarios.Nome}/{usuarios.Senha}");
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<UsuarioModel>(dados);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVyIiwibmJmIjoxNzQwMzY5MjcxLCJleHAiOjE3NDAzNzY0NzEsImlhdCI6MTc0MDM2OTI3MX0.2yc1OndD8XYEKAMS7vZROI2lzxbRa3S3w13rPqTCkIc");
            
            return View("CriarHobbies");
        }

        public IActionResult AdministrarUsuarios()
        {
            return View();
        }

        public async Task<IActionResult> AcharUsuarioId(int id){
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVyIiwibmJmIjoxNzQwMzY5MjcxLCJleHAiOjE3NDAzNzY0NzEsImlhdCI6MTc0MDM2OTI3MX0.2yc1OndD8XYEKAMS7vZROI2lzxbRa3S3w13rPqTCkIc");
            var resposta = await httpClient.GetAsync($"Usuario/Usuarios/id/5");
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<Usuarios>(dados);
            
            return View("CriarHobbies");
        }

        public IActionResult CriarHobbies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarHobbies(Hobbies hobbies){
            var hobbie = new StringContent(JsonConvert.SerializeObject(hobbies), Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVyIiwibmJmIjoxNzQwMzY5MjcxLCJleHAiOjE3NDAzNzY0NzEsImlhdCI6MTc0MDM2OTI3MX0.2yc1OndD8XYEKAMS7vZROI2lzxbRa3S3w13rPqTCkIc");

            var resposta = await httpClient.PostAsync($"Hobbies?idDoUsuario=1", hobbie);
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