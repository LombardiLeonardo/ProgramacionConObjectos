using System;

namespace TPPOO
{
	public class Cliente
	{
		public string NombreApellido { get; private set; }
		public string Dni { get; private set; }
		public string Cuit { get; private set; }
		public Direccion Direccion { get; private set; }
		public string Email { get; private set; }
		public string Celular { get; private set; }

		public Cliente(string nombreApellido, string dni, string cuit,
		               Direccion direccion, string email, string celular)
		{
			NombreApellido = nombreApellido;
			Dni = dni;
			Cuit = cuit;
			Direccion = direccion;
			Email = email;
			Celular = celular;
		}

		public override string ToString()
		{
			return NombreApellido + " (DNI " + Dni + ", CUIT " + Cuit + ")";
		}
	}
}
