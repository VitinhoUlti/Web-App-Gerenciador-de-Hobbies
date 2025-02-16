using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;
using Testezin.Entidades;
using Testezin.Servicos;

namespace MVC.Controllers
{
    public class UsuarioMVC : Controller
    {
        private readonly HttpClient httpClient;

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
            var usuario = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
            var resposta = await httpClient.PostAsync("Usuario/Usuarios", usuario);
            resposta.EnsureSuccessStatusCode();

            var dados = await resposta.Content.ReadAsStringAsync();
            ViewBag.Usuario = new {usuarios.Id, usuarios.Nome, usuarios.Senha};
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
            var usuario = JsonConvert.DeserializeObject<UsuarioModel>(dados);
            ViewBag.Usuario = usuario.token;
            return View("AdministrarUsuarios");
        }

        public IActionResult AdministrarUsuarios()
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