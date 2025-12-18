using System; 
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.commerce.Domain.Model.ValueObjects
{
    /// <summary>
    /// Representa un objeto de valor que contiene los detalles de envío de una orden.
    /// Un <c>record</c> es inmutable y sus propiedades se configuran solo en el momento de la construcción.
    /// </summary>
    public record Shipping
    {
        /// <summary>
        /// Obtiene la dirección de la calle y número para el envío.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Address { get; init; }

        /// <summary>
        /// Obtiene la ciudad de destino del envío. Por defecto es "Lima Metropolitana".
        /// </summary>
        [Required]
        [StringLength(100)]
        public string City { get; init; } 

        /// <summary>
        /// Obtiene el distrito o comuna de destino del envío.
        /// </summary>
        [Required]
        [StringLength(100)]
        public string District { get; init; }

        /// <summary>
        /// Obtiene información adicional o de referencia para el envío (ej. entre calles, color de casa).
        /// </summary>
        [StringLength(500)]
        public string Reference { get; init; } 

        /// <summary>
        /// Constructor privado para uso de frameworks ORM o serialización.
        /// No debe ser utilizado directamente para la creación de instancias de <see cref="Shipping"/>.
        /// </summary>
        private Shipping() 
        {
            Address = string.Empty;
            City = "Lima Metropolitana"; 
            District = string.Empty;
            Reference = string.Empty;
        }

        /// <summary>
        /// Inicializa una nueva instancia del objeto de valor <see cref="Shipping"/> con los detalles especificados.
        /// </summary>
        /// <param name="address">La dirección de la calle y número (no puede ser nula ni vacía).</param>
        /// <param name="city">La ciudad de destino (no puede ser nula ni vacía).</param>
        /// <param name="district">El distrito o comuna (no puede ser nulo ni vacío).</param>
        /// <param name="reference">Información adicional o de referencia.</param>
        /// <exception cref="ArgumentNullException">Se lanza si algún parámetro requerido es nulo o vacío.</exception>
        public Shipping(string address, string city, string district, string reference)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address), "Address cannot be empty.");
            if (string.IsNullOrWhiteSpace(city)) throw new ArgumentNullException(nameof(city), "City cannot be empty.");
            if (string.IsNullOrWhiteSpace(district)) throw new ArgumentNullException(nameof(district), "District cannot be empty.");
            

            Address = address;
            City = city;
            District = district;
            Reference = reference;
        }
    }
}