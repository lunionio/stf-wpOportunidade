﻿using System.Collections.Generic;

namespace WpOportunidades.Model
{
    public class AcaoViewModel
    {
        public int TipoAcao { get; set; }
        public string Tipo { get; set; }
        public string Caminho { get; set; }
        public int idMotorAux { get; set; }
        public List<ParametroViewModel> Parametro { get; set; }
    }
}