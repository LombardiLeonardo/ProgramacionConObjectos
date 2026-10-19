using System;
using System.Text;

namespace TPPOO
{
	public class Envio
	{
		public int NroEnvio { get; private set; }
		public Cliente Cliente { get; private set; }
		public DateTime Fecha { get; private set; }
		public Paquete Paquete { get; private set; }
		public Direccion DireccionOrigen { get; private set; }
		public Direccion DireccionEntrega { get; private set; }
		public decimal Importe { get; private set; }
		public EstadoEnvio Estado { get; set; }

		public Envio(int nroEnvio, Cliente cliente, DateTime fecha, Paquete paquete,
		             Direccion origen, Direccion entrega)
		{
			paquete.Validar();

			NroEnvio = nroEnvio;
			Cliente = cliente;
			Fecha = fecha;
			Paquete = paquete;
			DireccionOrigen = origen;
			DireccionEntrega = entrega;
			Importe = paquete.CalcularImporte();
			Estado = EstadoEnvio.ARetirar;
		}

		public string EstadoTexto
		{
			get
			{
				switch (Estado)
				{
					case EstadoEnvio.ARetirar: return "A retirar";
					case EstadoEnvio.EnProceso: return "En proceso";
					default: return "Entregado";
				}
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Envio Nro {0} | Fecha: {1:dd/MM/yyyy} | Estado: {2}", NroEnvio, Fecha, EstadoTexto).AppendLine();
			sb.AppendFormat("  Cliente: {0}", Cliente).AppendLine();
			sb.AppendFormat("  {0}", Paquete).AppendLine();
			sb.AppendFormat("  Origen:  {0}", DireccionOrigen).AppendLine();
			sb.AppendFormat("  Entrega: {0}", DireccionEntrega).AppendLine();
			sb.AppendFormat("  Importe: ${0:N2}", Importe);
			return sb.ToString();
		}
	}
}
