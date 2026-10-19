using System;

namespace TPPOO
{
	/// <summary>
	/// Base de las excepciones que indican que un paquete no cumple
	/// las condiciones para generar un envio.
	/// </summary>
	public class EnvioInvalidoException : Exception
	{
		public EnvioInvalidoException(string mensaje) : base(mensaje)
		{
		}
	}

	/// <summary>
	/// Se lanza cuando el peso del envio supera el maximo permitido.
	/// </summary>
	public class PesoMaximoExcedidoException : EnvioInvalidoException
	{
		public PesoMaximoExcedidoException(decimal peso, decimal maximo)
			: base(string.Format("El peso ({0} kg) supera el maximo permitido ({1} kg).", peso, maximo))
		{
		}
	}

	/// <summary>
	/// Se lanza cuando el volumen del envio supera el maximo permitido.
	/// </summary>
	public class VolumenMaximoExcedidoException : EnvioInvalidoException
	{
		public VolumenMaximoExcedidoException(decimal volumen, decimal maximo)
			: base(string.Format("El volumen ({0:0.###} dm3) supera el maximo permitido ({1} dm3).", volumen, maximo))
		{
		}
	}

	/// <summary>
	/// Se lanza cuando el paquete no puede viajar entre las direcciones indicadas.
	/// </summary>
	public class DestinoInvalidoException : EnvioInvalidoException
	{
		public DestinoInvalidoException(string mensaje) : base(mensaje)
		{
		}
	}
}
