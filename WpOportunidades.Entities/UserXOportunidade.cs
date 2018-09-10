using System;
using System.Collections.Generic;
using System.Text;

namespace WpOportunidades.Entities
{
    public class UserXOportunidade
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdOportunidade { get; set; }
        public Status Status { get; set; }
    }
}
