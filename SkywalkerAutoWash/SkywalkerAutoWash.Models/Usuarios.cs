using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkywalkerAutoWash.Models
{
    public class Usuarios
    {
        
        public int TipoId { get; set; }


      
        public DateTime DataEntrada { get; set; }

        //[Required(ErrorMessage = "Hora é obrigatório")]
        //[Display(Name = "Hora")]
        //[DataType(DataType.DateTime)]
        //public DateTime HoraEntrada { get; set; }

      
        public string NomeCompleto { get; set; }

     
        public string Email { get; set; }

        public string Cpf { get; set; }

       
        public string Endereco { get; set; }

       
        public int Numero { get; set; }

     
        public string Bairro { get; set; }

        
        public string Cidade { get; set; }

       
        public string Uf { get; set; }

        public string Usuario { get; set; }

      
        public string Senha { get; set; }

        
        public string ConfirmaSenha { get; set; }
        public int VeiculoId { get; set; }
        public string Veiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Fabricante { get; set; }
        public string Placa { get; set; }
        public string Cor { get; set; }
        public string Tipo { get; set; }
        public int MeusPontos { get; set; }
        public string Descricao { get; set; }
        public string TipoPagamento { get; set; }
        public double Valores { get; set; }
    }
}
