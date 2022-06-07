using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto.Models
{
	public class Medicamentos
	{
		public string Substancia { get; set; }		
		public string EAN1 { get; set; }
		public string Produto { get; set; }
		public string Apresentacao { get; set; }
		public double VlrPfSemImposto { get; set; }
		public double PMC0 { get; set; }
		public double PMC12 { get; set; }
		public double PMC17 { get; set; }
		public double PMC17ALC { get; set; }
		public double PMC17_5 { get; set; }
		public double PMC17_5ALC { get; set; }
		public double PMC18 { get; set; }
		public double PMC18ALC { get; set; }
		public double PMC20 { get; set; }
		public string ICMS0 { get; set; }
		public string ListaConcCredTrib { get; set; }
		public string Comerc2020 { get; set; }		
	}
}