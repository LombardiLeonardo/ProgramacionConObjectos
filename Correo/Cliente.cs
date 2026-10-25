using System;
using System.Collections.Generic;

namespace Correo
{
    public class Cliente
    {
        private List<Envio> envios;

        public string NombreApellido { get; private set; }
        public int DNI { get; private set; }
        public string Cuit { get; private set; }
        public Direccion DireccionCliente { get; private set; }
        public string Email { get; private set; }
        public string Celular { get; private set; }

        // Solo se puede recorrer; para agregar hay que usar AgregarEnvio
        public IEnumerable<Envio> Envios
        {
            get { return envios; }
        }

        public Cliente(string nombre, int dni, string cuit, Direccion direccion, string email, string celular)
        {
            this.NombreApellido = nombre;
            this.DNI = dni;
            this.Cuit = cuit;
            this.DireccionCliente = direccion;
            this.Email = email;
            this.Celular = celular;
            this.envios = new List<Envio>();
        }

        public void AgregarEnvio(Envio nuevoEnvio)
        {
            envios.Add(nuevoEnvio);
        }

        public void MostrarDatos()
        {
            Console.WriteLine("Cliente: {0} - DNI: {1}", NombreApellido, DNI);
        }
    }
}
