using System;
using System.Collections.Generic;
using System.Text;

namespace WpOportunidades.Entities
{
    public class Email
    {
        public Email(string conteudo, string titulo, string remetente, string destinatario, int idCliente)
        {
            Conteudo = conteudo;
            Titulo = titulo;
            this.remetente = remetente;
            this.destinatario = destinatario;
            this.idCliente = idCliente;
        }

        public string Conteudo { get; set; }
        public string Titulo { get; set; }
        public string remetente { get; set; } //Nomes adaptados ao motor Principal, que trabalha com 'dynamic'
        public string destinatario { get; set; }
        public int idCliente { get; set; }
    }
}
