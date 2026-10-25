using System;
using System.IO;
using System.Collections.Generic;


namespace Correo
{

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Cliente> listaClientes = new List<Cliente>();
            CargarDatos(listaClientes);

            // Retomamos la numeración a partir del mayor envío guardado
            int contadorEnvios = 1;
            foreach (Cliente cliente in listaClientes)
            {
                foreach (Envio pedido in cliente.Envios)
                {
                    if (pedido.NroEnvio >= contadorEnvios)
                    {
                        contadorEnvios = pedido.NroEnvio + 1;
                    }
                }
            }
            int opcion = 0;

            do
            {
                Console.WriteLine("\n--- MENÚ PRINCIPAL ---");
                Console.WriteLine("1. Generar un nuevo envío");
                Console.WriteLine("2. Actualizar el estado de un envío");
                Console.WriteLine("3. Listar envíos pendientes de un cliente");
                Console.WriteLine("4. Listar clientes con envíos entre fechas");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                opcion = int.Parse(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese el DNI del cliente: ");
                        int dni = int.Parse(Console.ReadLine());
                        Cliente clienteActual = null;

                        // Búsqueda de cliente
                        foreach (Cliente cliente in listaClientes)
                        {
                            if (cliente.DNI == dni)
                            {
                                clienteActual = cliente;
                                break;
                            }
                        }

                        // Alta de cliente nuevo
                        if (clienteActual == null)
                        {
                            Console.WriteLine("Cliente nuevo. Ingrese los datos para el alta:");
                            Console.Write("Nombre y apellido: ");
                            string nombre = Console.ReadLine();
                            Console.Write("Cuit: ");
                            string cuit = Console.ReadLine();
                            Console.Write("Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Celular: ");
                            string celular = Console.ReadLine();

                            Console.WriteLine("--- Dirección del Cliente ---");
                            Direccion dirCliente = CargarDireccion();

                            clienteActual = new Cliente(nombre, dni, cuit, dirCliente, email, celular);
                            listaClientes.Add(clienteActual);
                            Console.WriteLine("Cliente dado de alta exitosamente.");
                        }

                        Console.WriteLine("\n--- Datos del Envío ---");
                        Console.Write("Fecha (dd/mm/aaaa): ");
                        DateTime fechaEnvio = DateTime.Parse(Console.ReadLine());
                        Console.Write("Peso (en Kg): ");
                        double peso = double.Parse(Console.ReadLine());
                        Console.Write("Ancho (en cm): ");
                        double ancho = double.Parse(Console.ReadLine());
                        Console.Write("Alto (en cm): ");
                        double alto = double.Parse(Console.ReadLine());
                        Console.Write("Profundidad (en cm): ");
                        double prof = double.Parse(Console.ReadLine());

                        // Verificamos de entrada si el envío es posible, antes de pedir las direcciones
                        try
                        {
                            Envio.ValidarMedidas(peso, ancho, alto, prof);
                            Console.WriteLine("El envío es posible, continúe con las direcciones.");
                        }
                        catch (MedidaInvalidaException ex)
                        {
                            Console.WriteLine("Error: {0}", ex.Message);
                            GuardarDatos(listaClientes);
                            break;
                        }
                        catch (PesoMaximoException ex)
                        {
                            Console.WriteLine("Error: {0}", ex.Message);
                            GuardarDatos(listaClientes);
                            break;
                        }
                        catch (VolumenMaximoException ex)
                        {
                            Console.WriteLine("Error: {0}", ex.Message);
                            GuardarDatos(listaClientes);
                            break;
                        }

                        Console.WriteLine("--- Dirección de Origen ---");
                        Direccion dirOrigen = CargarDireccion();
                        Console.WriteLine("--- Dirección de Entrega ---");
                        Direccion dirEntrega = CargarDireccion();

                        // Las medidas ya fueron validadas, el constructor no debería lanzar excepciones
                        Envio nuevoEnvio = new Envio(clienteActual, fechaEnvio, contadorEnvios, peso, ancho, alto, prof, dirOrigen, dirEntrega);
                        clienteActual.AgregarEnvio(nuevoEnvio);
                        contadorEnvios++;
                        Console.WriteLine("Envío generado exitosamente. Importe calculado: ${0}", nuevoEnvio.Importe);
                        GuardarDatos(listaClientes);
                        break;

                    case 2:
                        Console.Write("Ingrese el DNI del cliente propietario del envío: ");
                        int dniBuscado = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el Nro de Envío: ");
                        int nroBuscado = int.Parse(Console.ReadLine());

                        bool encontrado = false;
                        foreach (Cliente cliente in listaClientes)
                        {
                            if (cliente.DNI == dniBuscado)
                            {
                                foreach (Envio pedido in cliente.Envios)
                                {
                                    if (pedido.NroEnvio == nroBuscado)
                                    {
                                        Console.Write("Ingrese el nuevo estado (a retirar / en proceso / entregado): ");
                                        string estado = Console.ReadLine();
                                        encontrado = true;
                                        try
                                        {
                                            pedido.ActualizarEstado(estado);
                                            Console.WriteLine("Estado actualizado correctamente.");
                                        }
                                        catch (EstadoInvalidoException ex)
                                        {
                                            Console.WriteLine("Error: {0}", ex.Message);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        if (!encontrado)
                        {
                            Console.WriteLine("Envío no encontrado.");
                        }
                        else
                        {
                            GuardarDatos(listaClientes);
                        }
                        break;

                    case 3:
                        Console.Write("Ingrese el DNI del cliente: ");
                        int dniPendientes = int.Parse(Console.ReadLine());

                        foreach (Cliente cliente in listaClientes)
                        {
                            if (cliente.DNI == dniPendientes)
                            {
                                Console.WriteLine("\nEnvíos pendientes de {0}:", cliente.NombreApellido);
                                foreach (Envio pedido in cliente.Envios)
                                {
                                    // Utilizando operadores lógicos para filtrar
                                    if (pedido.Estado == "a retirar" || pedido.Estado == "en proceso")
                                    {
                                        pedido.Mostrar();
                                    }
                                }
                            }
                        }
                        break;

                    case 4:
                        Console.Write("Ingrese fecha inicial (dd/mm/aaaa): ");
                        DateTime fechaInicio = DateTime.Parse(Console.ReadLine());
                        Console.Write("Ingrese fecha final (dd/mm/aaaa): ");
                        DateTime fechaFin = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("\nClientes con envíos entre {0} y {1}:", fechaInicio.ToString("dd/MM/yyyy"), fechaFin.ToString("dd/MM/yyyy"));

                        foreach (Cliente cliente in listaClientes)
                        {
                            bool tieneEnvios = false;
                            foreach (Envio pedido in cliente.Envios)
                            {
                                if (pedido.Fecha >= fechaInicio && pedido.Fecha <= fechaFin)
                                {
                                    if (!tieneEnvios)
                                    {
                                        cliente.MostrarDatos();
                                        tieneEnvios = true;
                                    }
                                    pedido.Mostrar();
                                }
                            }
                        }
                        break;

                    case 5:
                        Console.WriteLine("Saliendo del sistema...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            } while (opcion != 5);
        }

        // Método auxiliar para no repetir código de consola
        public static Direccion CargarDireccion()
        {
            Console.Write("Calle: ");
            string calle = Console.ReadLine();
            Console.Write("Nro: ");
            int nro = int.Parse(Console.ReadLine());
            Console.Write("Piso: ");
            string piso = Console.ReadLine();
            Console.Write("Dpto: ");
            string dpto = Console.ReadLine();
            Console.Write("Código Postal: ");
            string codigoPostal = Console.ReadLine();
            Console.Write("Ciudad: ");
            string ciudad = Console.ReadLine();
            Console.Write("País: ");
            string pais = Console.ReadLine();

            return new Direccion(calle, nro, piso, dpto, codigoPostal, ciudad, pais);
        }

        // Graba clientes y envíos en archivos de texto separados por ";"
        public static void GuardarDatos(List<Cliente> listaClientes)
        {
            using (StreamWriter escritorClientes = new StreamWriter("Clientes.csv"))
            {
                foreach (Cliente cliente in listaClientes)
                {
                    Direccion direccion = cliente.DireccionCliente;
                    escritorClientes.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11}",
                        cliente.DNI, cliente.NombreApellido, cliente.Cuit, cliente.Email, cliente.Celular,
                        direccion.Calle, direccion.Nro, direccion.Piso, direccion.Dpto,
                        direccion.CodigoPostal, direccion.Ciudad, direccion.Pais);
                }
            }

            using (StreamWriter escritorEnvios = new StreamWriter("Envios.csv"))
            {
                foreach (Cliente cliente in listaClientes)
                {
                    foreach (Envio pedido in cliente.Envios)
                    {
                        Direccion origen = pedido.DireccionOrigen;
                        Direccion entrega = pedido.DireccionEntrega;
                        escritorEnvios.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                            cliente.DNI, pedido.NroEnvio, pedido.Fecha.ToString("dd/MM/yyyy"), pedido.Peso,
                            pedido.Ancho, pedido.Alto, pedido.Profundidad, pedido.Estado,
                            origen.Calle, origen.Nro, origen.Piso, origen.Dpto, origen.CodigoPostal, origen.Ciudad, origen.Pais,
                            entrega.Calle, entrega.Nro, entrega.Piso, entrega.Dpto, entrega.CodigoPostal, entrega.Ciudad, entrega.Pais);
                    }
                }
            }
        }

        // Lee los archivos (si existen) y rearma la lista de clientes con sus envíos
        public static void CargarDatos(List<Cliente> listaClientes)
        {
            if (File.Exists("Clientes.csv"))
            {
                using (StreamReader lectorClientes = new StreamReader("Clientes.csv"))
                {
                    string linea;
                    while ((linea = lectorClientes.ReadLine()) != null)
                    {
                        string[] partes = linea.Split(';');
                        Direccion direccion = new Direccion(partes[5], int.Parse(partes[6]), partes[7], partes[8], partes[9], partes[10], partes[11]);
                        Cliente cliente = new Cliente(partes[1], int.Parse(partes[0]), partes[2], direccion, partes[3], partes[4]);
                        listaClientes.Add(cliente);
                    }
                }
            }

            if (File.Exists("Envios.csv"))
            {
                using (StreamReader lectorEnvios = new StreamReader("Envios.csv"))
                {
                    string linea;
                    while ((linea = lectorEnvios.ReadLine()) != null)
                    {
                        string[] partes = linea.Split(';');
                        int dniCliente = int.Parse(partes[0]);
                        Direccion origen = new Direccion(partes[8], int.Parse(partes[9]), partes[10], partes[11], partes[12], partes[13], partes[14]);
                        Direccion entrega = new Direccion(partes[15], int.Parse(partes[16]), partes[17], partes[18], partes[19], partes[20], partes[21]);

                        foreach (Cliente cliente in listaClientes)
                        {
                            if (cliente.DNI == dniCliente)
                            {
                                Envio pedido = new Envio(cliente, DateTime.Parse(partes[2]), int.Parse(partes[1]),
                                    double.Parse(partes[3]), double.Parse(partes[4]), double.Parse(partes[5]), double.Parse(partes[6]),
                                    origen, entrega);
                                pedido.ActualizarEstado(partes[7]);
                                cliente.AgregarEnvio(pedido);
                            }
                        }
                    }
                }
            }
        }
    }
}
