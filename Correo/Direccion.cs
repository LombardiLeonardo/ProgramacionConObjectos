using System;

namespace Correo
{
    public class Direccion
    {
        public string Calle { get; private set; }
        public int Nro { get; private set; }
        public string Piso { get; private set; }
        public string Dpto { get; private set; }
        public string CodigoPostal { get; private set; }
        public string Ciudad { get; private set; }
        public string Pais { get; private set; }

        public Direccion(string calle, int nro, string piso, string dpto, string codigoPostal, string ciudad, string pais)
        {
            this.Calle = calle;
            this.Nro = nro;
            this.Piso = piso;
            this.Dpto = dpto;
            this.CodigoPostal = codigoPostal;
            this.Ciudad = ciudad;
            this.Pais = pais;
        }

        public void Mostrar()
        {
            Console.WriteLine("{0} {1}, Piso {2} Dpto {3}, {4}, {5}, {6}", Calle, Nro, Piso, Dpto, CodigoPostal, Ciudad, Pais);
        }
    }
}
