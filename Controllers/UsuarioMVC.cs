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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;
using Testezin.Entidades;
using Microsoft.AspNetCore.Session;

namespace MVC.Controllers
{
    public class UsuarioMVC : Controller
    {
        private HttpClient httpClient;

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
        public async Task<IActionResult> Cadastrar(Usuarios usuarios)
        {
            var jaExiste = await httpClient.GetAsync("Usuario/Usuarios/nome/9");

            try {
                jaExiste.EnsureSuccessStatusCode();
            }catch {
                ViewBag.Erro = "Esse nome já está cadastrado, tente outro!";

                return View("TelaErro");
            }

            var usuario = new StringContent(JsonConvert.SerializeObject(usuarios), Encoding.UTF8, "application/json");
            var resposta = await httpClient.PostAsync("Usuario/Usuarios", usuario);

            try {
                resposta.EnsureSuccessStatusCode();
            }catch {
                ViewBag.Erro = "Houve um problema no cadastramento dos seus dados, tente novamente!";

                return View("TelaErro");
            }

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<UsuarioModel>(dados);

            HttpContext.Session.SetString("token", dadosusuario.token);
            HttpContext.Session.SetInt32("idusuario", dadosusuario.usuario.Id);

            return View("CriarHobbies");
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logar(Usuarios usuarios)
        {
            var resposta = await httpClient.GetAsync($"Usuario/Usuarios/login/{usuarios.Nome}/{usuarios.Senha}");

            try {
                resposta.EnsureSuccessStatusCode();
            }catch {
                ViewBag.Erro = "Não conseguimos achar seus dados, tente novamente ou se cadastre!";

                return View("TelaErro");
            }

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<UsuarioModel>(dados);

            HttpContext.Session.SetString("token", dadosusuario.token);
            HttpContext.Session.SetInt32("idusuario", dadosusuario.usuario.Id);
            
            return View("CriarHobbies");
        }

        public IActionResult AdministrarUsuarios()
        {
            return View();
        }

        public async Task<IActionResult> AcharUsuarioId(int id)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{HttpContext.Session.GetString("token")}");

            var resposta = await httpClient.GetAsync($"Usuario/Usuarios/id/{id}");

            try {
                resposta.EnsureSuccessStatusCode();
            }catch {
                ViewBag.Erro = "Não conseguimos achar seus dados, tente novamente!";

                return View("TelaErro");
            }

            var dados = await resposta.Content.ReadAsStringAsync();
            var dadosusuario = JsonConvert.DeserializeObject<Usuarios>(dados);

            return View("CriarHobbies");
        }

        public IActionResult CriarHobbies()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarHobbies(Hobbies hobbies)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{HttpContext.Session.GetString("token")}");

            var hobbie = new StringContent(JsonConvert.SerializeObject(hobbies), Encoding.UTF8, "application/json");

            var resposta = await httpClient.PostAsync($"Hobbies?idDoUsuario={HttpContext.Session.GetInt32("idusuario")}", hobbie);

            try {
                resposta.EnsureSuccessStatusCode();

                return View("AdministrarHobbies");
            }catch {
                ViewBag.Erro = "Houve um problema no cadastramento dos seus dados, tente novamente!";

                return View("TelaErro");
            }
        }

        public async Task<IActionResult> AdministrarHobbies()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{HttpContext.Session.GetString("token")}");

            var resposta = await httpClient.GetAsync($"Hobbies/idusuario/{HttpContext.Session.GetInt32("idusuario")}");

            try {
                resposta.EnsureSuccessStatusCode();
            }catch {
                ViewBag.Erro = "Você não pode administrar seus dados se não logar ou cadastrar!";

                return View("TelaErro");
            }

            var dados = await resposta.Content.ReadAsStringAsync();
            List<Hobbies> dadosusuario = JsonConvert.DeserializeObject<List<Hobbies>>(dados);

            ViewBag.Resposta = dadosusuario;

            return View("AdministrarHobbies");
        }

        public IActionResult EditarHobbies(Hobbies hobbie)
        {
            return View();
        }

        public IActionResult DeletarHobbies(int id)
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