using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MVC.Models
{
    public record HomeModel(string Nome, string Senha);
}