using System;
using System.Collections.Generic;

namespace TPPOO
{
	/// <summary>
	/// La compania de correo: administra clientes y envios.
	/// </summary>
	public class Correo
	{
		private List<Cliente> clientes = new List<Cliente>();
		private List<Envio> envios = new List<Envio>();
		private int proximoNroEnvio = 1; // asegura que el numero de envio sea unico

		public Cliente BuscarCliente(string dni)
		{
			foreach (Cliente c in clientes)
				if (c.Dni == dni)
					return c;
			return null;
		}

		public Envio BuscarEnvio(int nroEnvio)
		{
			foreach (Envio e in envios)
				if (e.NroEnvio == nroEnvio)
					return e;
			return null;
		}

		/// <summary>
		/// Genera un envio. Si el paquete no cumple las condiciones (peso, volumen,
		/// destino) se lanza una excepcion (y no se da de alta nada). Si el cliente es nuevo
		/// (no tiene envios anteriores) se agrega a la lista de clientes.
		/// </summary>
		public Envio GenerarEnvio(Cliente cliente, DateTime fecha, Paquete paquete,
		                          Direccion origen, Direccion entrega)
		{
			// Si el constructor lanza excepcion, el contador y la lista no cambian
			Envio envio = new Envio(proximoNroEnvio, cliente, fecha, paquete, origen, entrega);

			proximoNroEnvio++;
			if (BuscarCliente(cliente.Dni) == null)
				clientes.Add(cliente);
			envios.Add(envio);
			return envio;
		}

		public void ActualizarEstado(int nroEnvio, EstadoEnvio nuevoEstado)
		{
			Envio envio = BuscarEnvio(nroEnvio);
			if (envio == null)
				throw new InvalidOperationException("No existe el envio Nro " + nroEnvio + ".");
			envio.Estado = nuevoEstado;
		}

		/// <summary>Envios del cliente que todavia no fueron entregados.</summary>
		public List<Envio> ListarPendientes(string dni)
		{
			List<Envio> resultado = new List<Envio>();
			foreach (Envio e in envios)
				if (e.Cliente.Dni == dni && e.Estado != EstadoEnvio.Entregado)
					resultado.Add(e);
			return resultado;
		}

		/// <summary>
		/// Cada cliente con sus envios entre dos fechas (inclusive).
		/// Solo incluye clientes que tienen al menos un envio en el rango.
		/// </summary>
		public List<KeyValuePair<Cliente, List<Envio>>> ListarClientesConEnvios(DateTime desde, DateTime hasta)
		{
			List<KeyValuePair<Cliente, List<Envio>>> resultado = new List<KeyValuePair<Cliente, List<Envio>>>();
			foreach (Cliente c in clientes)
			{
				List<Envio> delCliente = new List<Envio>();
				foreach (Envio e in envios)
					if (e.Cliente.Dni == c.Dni && e.Fecha.Date >= desde.Date && e.Fecha.Date <= hasta.Date)
						delCliente.Add(e);
				if (delCliente.Count > 0)
					resultado.Add(new KeyValuePair<Cliente, List<Envio>>(c, delCliente));
			}
			return resultado;
		}
	}
}
