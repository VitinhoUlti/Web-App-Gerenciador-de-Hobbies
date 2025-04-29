using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Entidades
{
    public class Hobbies
    {
        public int Id { get; set; }
        public int IdDoUsuario { get; set; }
        public string Nome { get; set; }
        public DateOnly Aniversario { get; set; }
        public string Hobbie { get; set; }
        public string Gostos { get; set; }
        public string PossiveisPresentes { get; set; }
    }
}