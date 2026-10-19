using System;
using System.Text;

namespace TPPOO
{
	public class Direccion
	{
		public string Calle { get; private set; }
		public int Numero { get; private set; }
		public string Piso { get; private set; }
		public string Depto { get; private set; }
		public string CodigoPostal { get; private set; }
		public string Ciudad { get; private set; }
		public string Pais { get; private set; }

		public Direccion(string calle, int numero, string piso, string depto,
		                 string codigoPostal, string ciudad, string pais)
		{
			Calle = calle;
			Numero = numero;
			Piso = piso;
			Depto = depto;
			CodigoPostal = codigoPostal;
			Ciudad = ciudad;
			Pais = pais;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Calle).Append(" ").Append(Numero);
			if (!string.IsNullOrEmpty(Piso))
				sb.Append(", Piso ").Append(Piso);
			if (!string.IsNullOrEmpty(Depto))
				sb.Append(", Dpto. ").Append(Depto);
			sb.Append(", (").Append(CodigoPostal).Append(") ");
			sb.Append(Ciudad).Append(", ").Append(Pais);
			return sb.ToString();
		}
	}
}
