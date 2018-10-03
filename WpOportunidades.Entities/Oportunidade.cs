using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpOportunidades.Entities
{
    public class Oportunidade : Base
    {
        [NotMapped]
        public int TipoProfissional { get; set; }
        [NotMapped]
        public int TipoServico { get; set; }
        [NotMapped]
        public string DescProfissional { get; set; }
        [NotMapped]
        public string DescServico { get; set; }
        public DateTime DataOportunidade { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public Endereco Endereco { get; set; }
        public int IdEmpresa { get; set; }
        public OportunidadeStatus OportunidadeStatus { get; set; }
        public int OportunidadeStatusID { get; set; }

        /// <summary>
        /// Usado para a listagem das oportunidades do usuário
        /// </summary>
        [NotMapped]
        public Status OptStatus { get; set; }
    }
}
