using System;

namespace TPPOO
{
	public class EnvioInvalidoException : Exception
	{
		public EnvioInvalidoException(string mensaje) : base(mensaje)
		{
		}
	}

	public class PesoMaximoExcedidoException : EnvioInvalidoException
	{
		public PesoMaximoExcedidoException(decimal peso, decimal maximo)
			: base(string.Format("El peso ({0} kg) supera el maximo permitido ({1} kg).", peso, maximo))
		{
		}
	}

	public class VolumenMaximoExcedidoException : EnvioInvalidoException
	{
		public VolumenMaximoExcedidoException(decimal volumen, decimal maximo)
			: base(string.Format("El volumen ({0:0.###} dm3) supera el maximo permitido ({1} dm3).", volumen, maximo))
		{
		}
	}

}
