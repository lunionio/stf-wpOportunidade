using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WpOportunidades.Entities
{
    public class UserXOportunidade
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int OportunidadeId { get; set; }
        public Oportunidade Oportunidade { get; set; }
        public Status Status { get; set; }
        public int StatusID { get; set; }

        [NotMapped]
        public string EmailContratante { get; set; }

        [NotMapped]
        public string EmailContratado { get; set; }

        [NotMapped]
        public string NomeContratado { get; set; }
    }
}
