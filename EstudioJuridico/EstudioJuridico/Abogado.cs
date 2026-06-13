using System;

namespace EstudioJuridico
{
    public class Abogado
    {
        private int dni;
        private string nombre;
        private string apellido;
        private string especialidad;
        private int cantExpedientes;

        public Abogado(int dni, string nombre, string apellido, string especialidad, int cantExpedientes)
        {
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = apellido;
            this.especialidad = especialidad;
            this.cantExpedientes = cantExpedientes;
        }

      
        public int Dni { get { return this.dni; } }
        public string Nombre { get { return this.nombre; } }
        public string Apellido { get { return this.apellido; } }
        public string Especialidad { get { return this.especialidad; } }
        public int CantExpedientes { get { return this.cantExpedientes; } }

     
        public void AsignarExpediente()
        {
            
            if (this.cantExpedientes >= 6)
            {
                throw new Exception("Error: El abogado " + this.apellido + " ya posee 6 expedientes a su cargo. Debe asignárselo a otro abogado.");
            }
            this.cantExpedientes++;
        }

        public void LiberarExpediente()
        {
    
            if (this.cantExpedientes > 0)
            {
                this.cantExpedientes--;
            }
        }
    }
}