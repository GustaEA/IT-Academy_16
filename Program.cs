using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Projeto.Models;

namespace Projeto
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var path = @"C:\Users\gusta\Desktop\Projeto IT-Academy\Projeto\TA_PRECO_MEDICAMENTO.csv";			
			List<Medicamentos> ListaMedicamentos = new List<Medicamentos>();

			CriaLista(path, ListaMedicamentos);			

			string opcaoUsuario = ObterOpcaoUsuario();

			while(opcaoUsuario.ToUpper() != "X")
			{
				switch (opcaoUsuario)
				{
					case "1":
						Medicamentos2020(ListaMedicamentos);
						break;
					case "2":
						BuscaCodigoBarras(ListaMedicamentos);
						break;
					case "3":
						GraficoPercentual(ListaMedicamentos);
						break;
					default:
						System.Console.WriteLine("Digite um valor válido.");
						break;
				}
				opcaoUsuario = ObterOpcaoUsuario();
			}
		}

		private static void CriaLista(string path, List<Medicamentos> ListaMedicamentos)
		{
			using (var reader = new StreamReader(path))
			{
				var linha = reader.ReadLine();
				while (!reader.EndOfStream)
				{
					linha = reader.ReadLine();
					var valor = Regex.Split(linha, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

					Medicamentos medicamento = new Medicamentos
					{
						Substancia = valor[0],
						EAN1 = valor[5],
						Produto = valor[8],
						Apresentacao = valor[9],
						VlrPfSemImposto = ConverteDouble(valor[13]),
						PMC0 = ConverteDouble(valor[23]),
						PMC12 = ConverteDouble(valor[24]),
						PMC17 = ConverteDouble(valor[25]),
						PMC17ALC = ConverteDouble(valor[26]),
						PMC17_5 = ConverteDouble(valor[27]),
						PMC17_5ALC = ConverteDouble(valor[28]),
						PMC18 = ConverteDouble(valor[29]),
						PMC18ALC = ConverteDouble(valor[30]),
						PMC20 = ConverteDouble(valor[31]),
						ICMS0 = valor[35],
						ListaConcCredTrib = valor[37],
						Comerc2020 = valor[38]
					};
					ListaMedicamentos.Add(medicamento);
				}
			}
		}		
						
		private static void GraficoPercentual(List<Medicamentos> ListaMedicamentos)
		{
			System.Console.WriteLine("GRÁFICO DA LISTA DE CONCESSÃO DE CRÉDITO TRIBUTÁRIO (PIS/COFINS) DE 2020.");
			List<Medicamentos> ListCredTrib = ListaMedicamentos.FindAll(m=> m.Comerc2020.Equals("Sim"));
			double negativa = 0;
			double positiva = 0;
			double neutra = 0;

			double PorcNegativa = 0;
			double porcPositiva = 0;
			double porcNeutra = 0;			

			foreach (var item in ListCredTrib)
			{
				if (item.ListaConcCredTrib.Equals("Positiva"))
				{
					positiva++;
				}
				else if (item.ListaConcCredTrib.Equals("Negativa"))
				{
					negativa++;
				}
				else
				{
					neutra++;
				}
			}

			Console.Write("Classficação		");
			Console.Write("Percentual		");
			Console.WriteLine("Gáfico");

			porcPositiva = RetornaPorcentagem(ListCredTrib, positiva, "Positiva");

			PorcNegativa = RetornaPorcentagem(ListCredTrib, negativa, "Negativa");
			
			porcNeutra = RetornaPorcentagem(ListCredTrib, neutra, "Neutra	");
		}
		
		private static void BuscaCodigoBarras(List<Medicamentos> ListaMedicamentos)
		{
			System.Console.WriteLine("Digite o código de barras.");
			string codigoBarras = Console.ReadLine();
			Medicamentos medicamento = ListaMedicamentos.Find(m => m.EAN1.Equals(codigoBarras));
			List<double> ListPMC = new List<double>();
			if(medicamento != null)
			{
				if(medicamento.ICMS0.Equals("Sim"))
				{
					ListPMC.Add(medicamento.PMC0);
				}
				ListPMC.Add(medicamento.PMC12);
				ListPMC.Add(medicamento.PMC17);
				ListPMC.Add(medicamento.PMC17ALC);
				ListPMC.Add(medicamento.PMC17_5);
				ListPMC.Add(medicamento.PMC17_5ALC);
				ListPMC.Add(medicamento.PMC18);
				ListPMC.Add(medicamento.PMC18ALC);
				ListPMC.Add(medicamento.PMC20);
				
				double PMCmin = ListPMC.Min();
				string strPMCmin = String.Format("{0:N2}", PMCmin);
				System.Console.WriteLine($"PMC mínimo é: {strPMCmin}");			
				
				double PMCmax = ListPMC.Max();
				string strPMCmax = String.Format("{0:N2}", PMCmax);
				System.Console.WriteLine($"PMC máximo é: {strPMCmax}");
				
				double PMCdif = PMCmax - PMCmin;				
				string strPMCdif = String.Format("{0:N2}", PMCdif);
				System.Console.WriteLine($"A diferença é de: R${strPMCdif}");
			}
			else
			{
				System.Console.WriteLine("Código de barras inválido :(");
			}						
		}
		
		private static void Medicamentos2020(List<Medicamentos> ListaMedicamentos)
		{
			System.Console.WriteLine("Informe o medicamento desejado!");
			string medicamentoDesejado = Console.ReadLine().ToUpper();

			List<Medicamentos> ListaMedicamentos2020 = ListaMedicamentos.FindAll(m=> m.Substancia.Contains(medicamentoDesejado) && m.Comerc2020.Equals("Sim"));			
			
			if (ListaMedicamentos2020.Count.Equals(0))
			{
				System.Console.WriteLine("Medicamento não encontrado");
			}
			
			foreach (var item in ListaMedicamentos2020)
			{
				System.Console.WriteLine($"Substância: {item.Substancia}, Produto: {item.Produto}, Apresentação: {item.Apresentacao}, Valor PF sem imposto: {item.VlrPfSemImposto}");
			}
		}
					
//=============================[METÓDOS REUTILIZÁVEIS]=============================
		private static string ObterOpcaoUsuario()
		{
			System.Console.WriteLine();
			System.Console.WriteLine("informe a opção desejada: ");
			
			System.Console.WriteLine("1. Buscar produto comercializado em 2020!");
			System.Console.WriteLine("2. Código de barras");
			System.Console.WriteLine("3. Comparativo da LISTA DE CONCESSÃO DE CRÉDITO TRIBUTÁRIO (PIS/COFINS)");
			System.Console.WriteLine("X. Sair");
			
			string opcaoUsuario = Console.ReadLine().ToUpper();
			Console.WriteLine();
			return opcaoUsuario;
		}
		private static double RetornaPorcentagem(List<Medicamentos> ListCredTrib, double perc, string txt)
		{
			double porcentagem = (perc * 100) / (double)ListCredTrib.Count;
			string strPorcentagem = String.Format("{0:N2}", porcentagem);
			string asteriscoPorcentagem = new String('*', (int)porcentagem);
			Console.Write($"{txt}		");
			Console.Write($"{strPorcentagem}%			");
			System.Console.WriteLine(asteriscoPorcentagem);
			return porcentagem;
		}		
		private static double ConverteDouble(string valor)		
		{
			try 
			{
				return Convert.ToDouble(valor);
			}
			catch			
			{
				return 0;
			}
		}
	}
}
