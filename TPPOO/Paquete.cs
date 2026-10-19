using System;
using System.Text;

namespace TPPOO
{
	/// <summary>
	/// Paquete a enviar. Clase base: cada tipo de paquete define sus limites,
	/// su forma de calcular el importe y sus reglas de validacion.
	/// Medidas: peso en kg, ancho/alto/profundidad en cm.
	/// </summary>
	public abstract class Paquete
	{
		protected const decimal PRECIO_BASICO = 1000m;
		private const decimal ADICIONAL_TOPE = 10000m;

		public decimal Peso { get; private set; }
		public decimal Ancho { get; private set; }
		public decimal Alto { get; private set; }
		public decimal Profundidad { get; private set; }

		/// <summary>Volumen en decimetros cubicos (1 dm3 = 1000 cm3).</summary>
		public decimal Volumen
		{
			get { return Ancho * Alto * Profundidad / 1000m; }
		}

		public abstract string Tipo { get; }
		public abstract decimal PesoMaximo { get; }     // kg
		public abstract decimal VolumenMaximo { get; }  // dm3

		/// <summary>Importe del paquete segun su tipo.</summary>
		public abstract decimal CalcularImporte();

		protected Paquete(decimal peso, decimal ancho, decimal alto, decimal profundidad)
		{
			if (peso <= 0 || ancho <= 0 || alto <= 0 || profundidad <= 0)
				throw new ArgumentException("Peso y medidas deben ser mayores a cero.");
			Peso = peso;
			Ancho = ancho;
			Alto = alto;
			Profundidad = profundidad;
		}

		/// <summary>
		/// Chequea que el paquete cumpla las condiciones para viajar de origen a entrega.
		/// Lanza una excepcion (EnvioInvalidoException) si no las cumple.
		/// Los tipos de paquete pueden sobrescribirlo para agregar reglas propias.
		/// </summary>
		public virtual void Validar(Direccion origen, Direccion entrega)
		{
			if (Peso > PesoMaximo)
				throw new PesoMaximoExcedidoException(Peso, PesoMaximo);
			if (Volumen > VolumenMaximo)
				throw new VolumenMaximoExcedidoException(Volumen, VolumenMaximo);
		}

		/// <summary>precio = basico + adicionalPeso + adicionalVolumen</summary>
		protected decimal CalcularImporteBase()
		{
			decimal adicionalPeso = Peso < 1m ? ADICIONAL_TOPE * Peso : ADICIONAL_TOPE;
			decimal adicionalVolumen = Volumen < 1m ? ADICIONAL_TOPE * Volumen : ADICIONAL_TOPE;
			return PRECIO_BASICO + adicionalPeso + adicionalVolumen;
		}

		public override string ToString()
		{
			return string.Format("Paquete {0} | Peso: {1:0.###} kg | Medidas: {2:0.##} x {3:0.##} x {4:0.##} cm | Volumen: {5:0.###} dm3",
			                     Tipo, Peso, Ancho, Alto, Profundidad, Volumen);
		}
	}

	/// <summary>Paquete comun: hasta 5 kg y 3 dm3, precio base.</summary>
	public class PaqueteComun : Paquete
	{
		public PaqueteComun(decimal peso, decimal ancho, decimal alto, decimal profundidad)
			: base(peso, ancho, alto, profundidad) { }

		public override string Tipo { get { return "Comun"; } }
		public override decimal PesoMaximo { get { return 5m; } }
		public override decimal VolumenMaximo { get { return 3m; } }

		public override decimal CalcularImporte()
		{
			return CalcularImporteBase();
		}
	}

	/// <summary>Paquete fragil: mismos limites que el comun, con 50% de recargo.</summary>
	public class PaqueteFragil : Paquete
	{
		private const decimal RECARGO = 1.5m;

		public PaqueteFragil(decimal peso, decimal ancho, decimal alto, decimal profundidad)
			: base(peso, ancho, alto, profundidad) { }

		public override string Tipo { get { return "Fragil"; } }
		public override decimal PesoMaximo { get { return 5m; } }
		public override decimal VolumenMaximo { get { return 3m; } }

		public override decimal CalcularImporte()
		{
			return CalcularImporteBase() * RECARGO;
		}
	}

	/// <summary>Paquete pesado: hasta 20 kg y 10 dm3, el doble del precio base.</summary>
	public class PaquetePesado : Paquete
	{
		public PaquetePesado(decimal peso, decimal ancho, decimal alto, decimal profundidad)
			: base(peso, ancho, alto, profundidad) { }

		public override string Tipo { get { return "Pesado"; } }
		public override decimal PesoMaximo { get { return 20m; } }
		public override decimal VolumenMaximo { get { return 10m; } }

		public override decimal CalcularImporte()
		{
			return CalcularImporteBase() * 2m;
		}
	}

	/// <summary>
	/// Paquete internacional: hasta 5 kg y 3 dm3, recargo fijo y el pais de
	/// entrega debe ser distinto al de origen.
	/// </summary>
	public class PaqueteInternacional : Paquete
	{
		private const decimal RECARGO_ADUANA = 5000m;

		public PaqueteInternacional(decimal peso, decimal ancho, decimal alto, decimal profundidad)
			: base(peso, ancho, alto, profundidad) { }

		public override string Tipo { get { return "Internacional"; } }
		public override decimal PesoMaximo { get { return 5m; } }
		public override decimal VolumenMaximo { get { return 3m; } }

		public override decimal CalcularImporte()
		{
			return CalcularImporteBase() + RECARGO_ADUANA;
		}

		public override void Validar(Direccion origen, Direccion entrega)
		{
			base.Validar(origen, entrega);
			if (string.Equals(origen.Pais, entrega.Pais, StringComparison.OrdinalIgnoreCase))
				throw new DestinoInvalidoException("Un paquete internacional debe entregarse en un pais distinto al de origen ("
				                                   + origen.Pais + ").");
		}
	}
}
