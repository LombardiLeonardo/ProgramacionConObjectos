using System;
using System.Collections.Generic;

namespace TPPOO
{
	public class Correo
	{
		private List<Cliente> clientes = new List<Cliente>();
		private List<Envio> envios = new List<Envio>();
		private int proximoNroEnvio = 1;

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

		public Envio GenerarEnvio(Cliente cliente, DateTime fecha, Paquete paquete,
		                          Direccion origen, Direccion entrega)
		{
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

		public List<Envio> ListarPendientes(string dni)
		{
			List<Envio> resultado = new List<Envio>();
			foreach (Envio e in envios)
				if (e.Cliente.Dni == dni && e.Estado != EstadoEnvio.Entregado)
					resultado.Add(e);
			return resultado;
		}

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
