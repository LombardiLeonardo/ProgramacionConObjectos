using System;

namespace EstudioJuridico
{
    class Program
    {
        public static void Main(string[] args)
        {
            Estudio miEstudio = new Estudio();
            string opcion = "";

            while (opcion != "i")
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA ESTUDIO JURÍDICO ===");
                Console.WriteLine("a) Agregar abogado");
                Console.WriteLine("b) Eliminar abogado");
                Console.WriteLine("c) Listado de abogados");
                Console.WriteLine("d) Listado de expedientes");
                Console.WriteLine("e) Agregar un nuevo expediente");
                Console.WriteLine("f) Modificar el estado de un expediente");
                Console.WriteLine("g) Eliminar expediente por numero");
                Console.WriteLine("h) Listado de expedientes ‘audiencia’ por mes");
                Console.WriteLine("i) Salir");
                Console.Write("\nIngrese una opción: ");
                
                opcion = Console.ReadLine().ToLower();

                switch (opcion)
                {
                    case "a":
                        Console.Write("DNI: "); int dni = int.Parse(Console.ReadLine());
                        Console.Write("Nombre: "); string nom = Console.ReadLine();
                        Console.Write("Apellido: "); string ape = Console.ReadLine();
                        Console.Write("Especialidad: "); string esp = Console.ReadLine();
                        miEstudio.AgregarAbogado(new Abogado(dni, nom, ape, esp, 0));
                        break;

                    case "b":
                        Console.Write("DNI del abogado a eliminar: ");
                        miEstudio.EliminarAbogado(int.Parse(Console.ReadLine()));
                        break;

                    case "c":
                        miEstudio.ListarAbogados();
                        break;

                    case "d":
                        miEstudio.ListarExpedientes();
                        break;

                    case "e":
                        try {
                            Console.Write("Número expediente: "); int num = int.Parse(Console.ReadLine());
                            Console.Write("Titular: "); string tit = Console.ReadLine();
                            Console.Write("Tipo trámite: "); string tipo = Console.ReadLine();
                            Console.Write("Estado: "); string est = Console.ReadLine();
                            Console.Write("Fecha (dd/mm/aaaa): "); DateTime fecha = DateTime.Parse(Console.ReadLine());
                            Console.Write("DNI del Abogado: "); int dniAbog = int.Parse(Console.ReadLine());
                            
                            miEstudio.AgregarExpediente(num, tit, tipo, est, fecha, dniAbog);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    case "f":
                        Console.Write("Número expediente: "); int numMod = int.Parse(Console.ReadLine());
                        Console.Write("Nuevo estado: "); string estMod = Console.ReadLine();
                        miEstudio.ModificarEstadoExpediente(numMod, estMod);
                        break;

                    case "g":
                        Console.Write("Número expediente a eliminar: ");
                        miEstudio.EliminarExpediente(int.Parse(Console.ReadLine()));
                        break;

                    case "h":
                        Console.Write("Mes (1-12): ");
                        miEstudio.ListarExpedientesAudiencia(int.Parse(Console.ReadLine()));
                        break;

                    case "i":
                        miEstudio.GuardarDatos();
                        Console.WriteLine("Datos guardados. Saliendo...");
                        break;
                }

                if (opcion != "i")
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey(true);
                }
            }
        }
    }
}