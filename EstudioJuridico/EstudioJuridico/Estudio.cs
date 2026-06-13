using System;
using System.IO; 
using System.Collections.Generic; 

namespace EstudioJuridico
{
    public class Estudio
    {
        private Dictionary<int, Abogado> abogados;
        private List<Expediente> expedientes;
        
        private string pathAbogados = "abogados.txt";
        private string pathExpedientes = "expedientes.txt";

        public Estudio()
        {
            this.abogados = new Dictionary<int, Abogado>();
            this.expedientes = new List<Expediente>();
            CargarDatos();
        }

        private void CargarDatos()
        {
            if (File.Exists(pathAbogados))
            {
                using (StreamReader sr = new StreamReader(pathAbogados))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] partes = linea.Split(';');
                        Abogado a = new Abogado(int.Parse(partes[0]), partes[1], partes[2], partes[3], int.Parse(partes[4]));
                        abogados.Add(a.Dni, a);
                    }
                }
            }

            if (File.Exists(pathExpedientes))
            {
                using (StreamReader sr = new StreamReader(pathExpedientes))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] partes = linea.Split(';');
                        int dniAbogado = int.Parse(partes[5]);
                        if (abogados.ContainsKey(dniAbogado))
                        {
                            Expediente exp = new Expediente(int.Parse(partes[0]), partes[1], partes[2], partes[3], DateTime.Parse(partes[4]), abogados[dniAbogado]);
                            expedientes.Add(exp);
                        }
                    }
                }
            }
        }

        public void GuardarDatos()
        {
            using (StreamWriter sw = new StreamWriter(pathAbogados))
            {
                foreach (var par in abogados)
                {
                    Abogado a = par.Value;
                    sw.WriteLine(a.Dni + ";" + a.Nombre + ";" + a.Apellido + ";" + a.Especialidad + ";" + a.CantExpedientes);
                }
            }

            using (StreamWriter sw = new StreamWriter(pathExpedientes))
            {
                foreach (Expediente exp in expedientes)
                {
                    sw.WriteLine(exp.Numero + ";" + exp.Titular + ";" + exp.TipoTramite + ";" + exp.Estado + ";" + exp.FechaPresentacion.ToString("dd/MM/yyyy") + ";" + exp.AbogadoCargo.Dni);
                }
            }
        }

        public void AgregarAbogado(Abogado a) { abogados.Add(a.Dni, a); }

        public void EliminarAbogado(int dni)
        {
            if (abogados.ContainsKey(dni)) abogados.Remove(dni);
            else Console.WriteLine("Error: DNI no encontrado.");
        }

        public void ListarAbogados()
        {
            foreach (var par in abogados)
            {
                Abogado a = par.Value;
                Console.WriteLine("DNI: {0} | {1}, {2} | Especialidad: {3} | Exp: {4}", a.Dni, a.Apellido, a.Nombre, a.Especialidad, a.CantExpedientes);
            }
        }

        public void ListarExpedientes()
        {
            foreach (Expediente exp in expedientes)
            {
                Console.WriteLine("Nro: {0} | Titular: {1} | Abogado: {2}", exp.Numero, exp.Titular, exp.AbogadoCargo.Apellido);
            }
        }

        public void AgregarExpediente(int num, string tit, string tipo, string est, DateTime fecha, int dniAbogado)
        {
            if (abogados.ContainsKey(dniAbogado))
            {
                abogados[dniAbogado].AsignarExpediente();
                expedientes.Add(new Expediente(num, tit, tipo, est, fecha, abogados[dniAbogado]));
            }
        }

        public void ModificarEstadoExpediente(int numero, string nuevoEstado)
        {
            foreach (Expediente exp in expedientes)
            {
                if (exp.Numero == numero) exp.Estado = nuevoEstado;
            }
        }

        public void EliminarExpediente(int numero)
        {
            for (int i = 0; i < expedientes.Count; i++)
            {
                if (expedientes[i].Numero == numero)
                {
                    expedientes[i].AbogadoCargo.LiberarExpediente();
                    expedientes.RemoveAt(i);
                    break;
                }
            }
        }

        public void ListarExpedientesAudiencia(int mes)
        {
            foreach (Expediente exp in expedientes)
            {
                if (exp.TipoTramite.ToLower() == "audiencia" && exp.FechaPresentacion.Month == mes)
                {
                    Console.WriteLine("Nro: {0} | Titular: {1} | Abogado: {2}", exp.Numero, exp.Titular, exp.AbogadoCargo.Apellido);
                }
            }
        }
    }
}