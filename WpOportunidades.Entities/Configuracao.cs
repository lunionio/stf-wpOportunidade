using System;
using System.Collections.Generic;
using System.Text;

namespace WpOportunidades.Entities
{
    public class Configuracao : Base
    {
        public string Chave { get; set; }
        public string Valor { get; set; }

        public Configuracao()
        {

        }
    }
}
