using System;
using System.Collections.Generic;
using System.Text;

namespace WpOportunidades.Entities
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Status()
        {

        }

        public Status(int id)
        {
            ID = id;
        }
    }
}
