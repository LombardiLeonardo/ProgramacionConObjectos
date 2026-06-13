using System;

namespace EstudioJuridico
{
    public class Expediente
    {
        private int numero;
        private string titular;
        private string tipoTramite;
        private string estado;
        private DateTime fechaPresentacion;
        private Abogado abogadoCargo; 

        
        public Expediente(int numero, string titular, string tipoTramite, string estado, DateTime fechaPresentacion, Abogado abogadoCargo)
        {
            this.numero = numero;
            this.titular = titular;
            this.tipoTramite = tipoTramite;
            this.estado = estado;
            this.fechaPresentacion = fechaPresentacion;
            this.abogadoCargo = abogadoCargo;
        }

     
        public int Numero { get { return this.numero; } }
        public string Titular { get { return this.titular; } }
        public string TipoTramite { get { return this.tipoTramite; } }
        
    
        public string Estado 
        { 
            get { return this.estado; } 
            set { this.estado = value; }
        }
        
        public DateTime FechaPresentacion { get { return this.fechaPresentacion; } }
        
   
        public Abogado AbogadoCargo { get { return this.abogadoCargo; } }
    }
}