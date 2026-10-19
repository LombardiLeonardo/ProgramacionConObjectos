using System;
using System.Collections.Generic;
using System.Globalization;

namespace TPPOO
{
	class Program
	{
		static Correo correo = new Correo();

		public static void Main(string[] args)
		{
			bool salir = false;
			while (!salir)
			{
				MostrarMenu();
				string opcion = Leer("Opcion: ");
				Console.WriteLine();
				switch (opcion)
				{
					case "1": GenerarEnvio(); break;
					case "2": ActualizarEstado(); break;
					case "3": ListarPendientes(); break;
					case "4": ListarClientesEntreFechas(); break;
					case "0": salir = true; break;
					default: Console.WriteLine("Opcion invalida."); break;
				}
				if (!salir)
				{
					Console.WriteLine();
					Leer("Presione Enter para continuar...");
				}
			}
		}

		static void MostrarMenu()
		{
			Console.Clear();
			Console.WriteLine("=== CORREO ===");
			Console.WriteLine("1. Generar un nuevo envio");
			Console.WriteLine("2. Actualizar el estado de un envio");
			Console.WriteLine("3. Listar envios pendientes de un cliente");
			Console.WriteLine("4. Listar clientes con sus envios entre fechas");
			Console.WriteLine("0. Salir");
			Console.WriteLine();
		}


		static void GenerarEnvio()
		{
			Console.WriteLine("--- Nuevo envio ---");
			string dni = LeerTexto("DNI del cliente: ");
			Cliente cliente = correo.BuscarCliente(dni);
			bool clienteNuevo = (cliente == null);

			if (clienteNuevo)
			{
				Console.WriteLine("Cliente nuevo. Ingrese sus datos:");
				cliente = LeerCliente(dni);
			}
			else
			{
				Console.WriteLine("Cliente existente: " + cliente);
			}

			DateTime fecha = LeerFecha("Fecha del envio (dd/MM/yyyy, Enter = hoy): ", true);
			Paquete paquete = LeerPaquete();

			Direccion origen;
			string usarCliente = Leer("La direccion de origen es la del cliente? (S/N): ").ToUpper();
			if (usarCliente == "S")
				origen = cliente.Direccion;
			else
				origen = LeerDireccion("Direccion de origen");

			Direccion entrega = LeerDireccion("Direccion de entrega");

			try
			{
				Envio envio = correo.GenerarEnvio(cliente, fecha, paquete, origen, entrega);
				Console.WriteLine();
				if (clienteNuevo)
					Console.WriteLine("Cliente dado de alta.");
				Console.WriteLine("Envio generado:");
				Console.WriteLine(envio);
			}
			catch (EnvioInvalidoException ex)
			{
				Console.WriteLine();
				Console.WriteLine("No se genero el envio: " + ex.Message);
			}
		}

		static void ActualizarEstado()
		{
			Console.WriteLine("--- Actualizar estado ---");
			int nro = LeerEntero("Numero de envio: ");
			Envio envio = correo.BuscarEnvio(nro);
			if (envio == null)
			{
				Console.WriteLine("No existe el envio Nro " + nro + ".");
				return;
			}

			Console.WriteLine(envio);
			Console.WriteLine();
			Console.WriteLine("Nuevo estado:");
			Console.WriteLine("1. A retirar");
			Console.WriteLine("2. En proceso");
			Console.WriteLine("3. Entregado");
			EstadoEnvio nuevo = EstadoEnvio.ARetirar;
			bool valido = false;
			while (!valido)
			{
				string op = Leer("Opcion: ");
				valido = true;
				if (op == "1") nuevo = EstadoEnvio.ARetirar;
				else if (op == "2") nuevo = EstadoEnvio.EnProceso;
				else if (op == "3") nuevo = EstadoEnvio.Entregado;
				else { valido = false; Console.WriteLine("Opcion invalida."); }
			}

			correo.ActualizarEstado(nro, nuevo);
			Console.WriteLine("Estado actualizado: " + envio.EstadoTexto);
		}

		static void ListarPendientes()
		{
			Console.WriteLine("--- Envios pendientes de un cliente ---");
			string dni = LeerTexto("DNI del cliente: ");
			Cliente cliente = correo.BuscarCliente(dni);
			if (cliente == null)
			{
				Console.WriteLine("No existe un cliente con ese DNI.");
				return;
			}

			List<Envio> pendientes = correo.ListarPendientes(dni);
			Console.WriteLine();
			Console.WriteLine("Cliente: " + cliente);
			if (pendientes.Count == 0)
			{
				Console.WriteLine("No tiene envios pendientes.");
				return;
			}
			foreach (Envio e in pendientes)
			{
				Console.WriteLine();
				Console.WriteLine(e);
			}
		}

		static void ListarClientesEntreFechas()
		{
			Console.WriteLine("--- Clientes con sus envios entre fechas ---");
			DateTime desde = LeerFecha("Desde (dd/MM/yyyy): ", false);
			DateTime hasta = LeerFecha("Hasta (dd/MM/yyyy): ", false);
			if (hasta < desde)
			{
				Console.WriteLine("La fecha 'hasta' no puede ser anterior a 'desde'.");
				return;
			}

			List<KeyValuePair<Cliente, List<Envio>>> lista = correo.ListarClientesConEnvios(desde, hasta);
			if (lista.Count == 0)
			{
				Console.WriteLine("No hay envios en ese rango de fechas.");
				return;
			}
			foreach (KeyValuePair<Cliente, List<Envio>> par in lista)
			{
				Console.WriteLine();
				Console.WriteLine("CLIENTE: " + par.Key);
				foreach (Envio e in par.Value)
				{
					Console.WriteLine();
					Console.WriteLine(e);
				}
			}
		}


		static Cliente LeerCliente(string dni)
		{
			string nombre = LeerTexto("Nombre y apellido: ");
			string cuit = LeerTexto("CUIT: ");
			Direccion dir = LeerDireccion("Direccion del cliente");
			string email = LeerTexto("Email: ");
			string celular = LeerTexto("Celular: ");
			return new Cliente(nombre, dni, cuit, dir, email, celular);
		}

		static Paquete LeerPaquete()
		{
			decimal peso = LeerDecimal("Peso (kg): ");
			decimal ancho = LeerDecimal("Ancho (cm): ");
			decimal alto = LeerDecimal("Alto (cm): ");
			decimal profundidad = LeerDecimal("Profundidad (cm): ");
			return new Paquete(peso, ancho, alto, profundidad);
		}

		static Direccion LeerDireccion(string titulo)
		{
			Console.WriteLine(titulo + ":");
			string calle = LeerTexto("  Calle: ");
			int numero = LeerEntero("  Numero: ");
			string piso = Leer("  Piso (Enter si no tiene): ");
			string depto = Leer("  Dpto. (Enter si no tiene): ");
			string cp = LeerTexto("  Codigo postal: ");
			string ciudad = LeerTexto("  Ciudad: ");
			string pais = LeerTexto("  Pais: ");
			return new Direccion(calle, numero, piso, depto, cp, ciudad, pais);
		}


		static string Leer(string mensaje)
		{
			Console.Write(mensaje);
			string s = Console.ReadLine();
			if (s == null)
				Environment.Exit(0);
			return s.Trim();
		}

		static string LeerTexto(string mensaje)
		{
			while (true)
			{
				string s = Leer(mensaje);
				if (s.Length > 0)
					return s;
				Console.WriteLine("Este dato es obligatorio.");
			}
		}

		static int LeerEntero(string mensaje)
		{
			while (true)
			{
				int n;
				if (int.TryParse(Leer(mensaje), out n) && n > 0)
					return n;
				Console.WriteLine("Ingrese un numero entero mayor a cero.");
			}
		}

		static decimal LeerDecimal(string mensaje)
		{
			while (true)
			{
				string s = Leer(mensaje).Replace(',', '.');
				decimal d;
				if (decimal.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out d) && d > 0)
					return d;
				Console.WriteLine("Ingrese un numero mayor a cero (ej: 2,5).");
			}
		}

		static DateTime LeerFecha(string mensaje, bool enterEsHoy)
		{
			while (true)
			{
				string s = Leer(mensaje);
				if (s.Length == 0 && enterEsHoy)
					return DateTime.Today;
				DateTime f;
				if (DateTime.TryParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out f))
					return f;
				Console.WriteLine("Fecha invalida. Use el formato dd/MM/yyyy.");
			}
		}
	}
}
