using System;

namespace TPPOO
{
	public class Paquete
	{
		public const decimal PESO_MAXIMO = 5m;
		public const decimal VOLUMEN_MAXIMO = 3m;
		public const decimal PRECIO_BASICO = 1000m;
		private const decimal ADICIONAL_TOPE = 10000m;

		public decimal Peso { get; private set; }
		public decimal Ancho { get; private set; }
		public decimal Alto { get; private set; }
		public decimal Profundidad { get; private set; }

		public decimal Volumen
		{
			get { return Ancho * Alto * Profundidad / 1000m; }
		}

		public Paquete(decimal peso, decimal ancho, decimal alto, decimal profundidad)
		{
			if (peso <= 0 || ancho <= 0 || alto <= 0 || profundidad <= 0)
				throw new ArgumentException("Peso y medidas deben ser mayores a cero.");
			Peso = peso;
			Ancho = ancho;
			Alto = alto;
			Profundidad = profundidad;
		}

		public void Validar()
		{
			if (Peso > PESO_MAXIMO)
				throw new PesoMaximoExcedidoException(Peso, PESO_MAXIMO);
			if (Volumen > VOLUMEN_MAXIMO)
				throw new VolumenMaximoExcedidoException(Volumen, VOLUMEN_MAXIMO);
		}

		public decimal CalcularImporte()
		{
			decimal adicionalPeso = Peso < 1m ? ADICIONAL_TOPE * Peso : ADICIONAL_TOPE;
			decimal adicionalVolumen = Volumen < 1m ? ADICIONAL_TOPE * Volumen : ADICIONAL_TOPE;
			return PRECIO_BASICO + adicionalPeso + adicionalVolumen;
		}

		public override string ToString()
		{
			return string.Format("Paquete | Peso: {0:0.###} kg | Medidas: {1:0.##} x {2:0.##} x {3:0.##} cm | Volumen: {4:0.###} dm3",
			                     Peso, Ancho, Alto, Profundidad, Volumen);
		}
	}
}
