using System;

namespace Correo
{
    public class Envio
    {
        public Cliente Cliente { get; private set; }
        public DateTime Fecha { get; private set; }
        public int NroEnvio { get; private set; }
        public double Peso { get; private set; }
        public double Ancho { get; private set; }
        public double Alto { get; private set; }
        public double Profundidad { get; private set; }
        public Direccion DireccionOrigen { get; private set; }
        public Direccion DireccionEntrega { get; private set; }
        public double Importe { get; private set; }
        public string Estado { get; private set; }

        public Envio(Cliente cliente, DateTime fecha, int nroEnvio, double peso, double ancho, double alto, double profundidad, Direccion origen, Direccion entrega)
        {
            ValidarMedidas(peso, ancho, alto, profundidad);
            double volumen = ancho * alto * profundidad;

            this.Cliente = cliente;
            this.Fecha = fecha;
            this.NroEnvio = nroEnvio;
            this.Peso = peso;
            this.Ancho = ancho;
            this.Alto = alto;
            this.Profundidad = profundidad;
            this.DireccionOrigen = origen;
            this.DireccionEntrega = entrega;
            this.Estado = "a retirar";
            this.Importe = CalcularImporte(peso, volumen);
        }

        // Validación con lanzamiento de excepciones propias (medidas en cm, peso en Kg)
        public static void ValidarMedidas(double peso, double ancho, double alto, double profundidad)
        {
            if (peso <= 0 || ancho <= 0 || alto <= 0 || profundidad <= 0)
            {
                throw new MedidaInvalidaException("El peso y las medidas deben ser mayores a 0.");
            }

            if (peso > 5)
            {
                throw new PesoMaximoException("El peso supera el máximo permitido de 5Kg.");
            }

            double volumen = ancho * alto * profundidad;
            if (volumen > 3000)
            {
                throw new VolumenMaximoException("El volumen supera el máximo permitido de 3000cm3.");
            }
        }

        // El volumen llega en cm3; la tarifa se calcula por dm3 (1 dm3 = 1000 cm3)
        private double CalcularImporte(double peso, double volumen)
        {
            double volumenDm3 = volumen / 1000;
            double basico = 1000;
            double adicionalPeso = (peso < 1) ? (10000 * peso) : 10000;
            double adicionalVolumen = (volumenDm3 < 1) ? (10000 * volumenDm3) : 10000;

            return basico + adicionalPeso + adicionalVolumen;
        }

        public void ActualizarEstado(string nuevoEstado)
        {
            string estado = nuevoEstado.Trim().ToLower();

            if (estado != "a retirar" && estado != "en proceso" && estado != "entregado")
            {
                throw new EstadoInvalidoException("El estado debe ser: a retirar, en proceso o entregado.");
            }

            this.Estado = estado;
        }

        public void Mostrar()
        {
            Console.WriteLine("Envío Nro: {0} | Fecha: {1} | Estado: {2} | Importe: ${3}", NroEnvio, Fecha.ToString("dd/MM/yyyy"), Estado, Importe);
        }
    }

    public class PesoMaximoException : Exception
    {
        public PesoMaximoException(string mensaje) : base(mensaje) { }
    }

    public class VolumenMaximoException : Exception
    {
        public VolumenMaximoException(string mensaje) : base(mensaje) { }
    }

    public class MedidaInvalidaException : Exception
    {
        public MedidaInvalidaException(string mensaje) : base(mensaje) { }
    }

    public class EstadoInvalidoException : Exception
    {
        public EstadoInvalidoException(string mensaje) : base(mensaje) { }
    }
}
