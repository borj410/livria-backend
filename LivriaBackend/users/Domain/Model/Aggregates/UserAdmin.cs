using System;
using System.Collections.Generic;

namespace LivriaBackend.users.Domain.Model.Aggregates
{
    /// <summary>
    /// Representa un usuario con privilegios administrativos en el sistema.
    /// Hereda de la clase base <see cref="User"/> y añade propiedades específicas de administrador.
    /// </summary>
    public class UserAdmin : User
    {
        /// <summary>
        /// Obtiene un valor que indica si el administrador tiene acceso a las funcionalidades administrativas.
        /// </summary>
        public bool AdminAccess { get; private set; }

        /// <summary>
        /// Obtiene el pin de seguridad asociado a la cuenta del administrador.
        /// </summary>
        public string SecurityPin { get; private set; }

        /// <summary>
        /// Obtiene el capital asignado al administrador. Por defecto es 500.
        /// </summary>
        public decimal Capital { get; private set; }

        /// <summary>
        /// Constructor protegido sin parámetros, típicamente utilizado por ORMs como Entity Framework Core.
        /// </summary>
        protected UserAdmin() : base()
        {
            Capital = 5000m; 
            SecurityPin = string.Empty;
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UserAdmin"/> con las propiedades especificadas.
        /// </summary>
        /// <param name="display">El nombre visible o alias del administrador (FullName).</param>
        /// <param name="username">El nombre de usuario único del administrador.</param>
        /// <param name="email">La dirección de correo electrónico del administrador.</param>
        /// <param name="adminAccess">Indica si el administrador tiene acceso de administrador.</param>
        /// <param name="securityPin">El pin de seguridad del administrador.</param>
        public UserAdmin(string display, string username, string email, bool adminAccess, string securityPin)
            : base(display, username, email)
        {
            AdminAccess = adminAccess;
            SecurityPin = securityPin;
            Capital = 5000m; 
        }

        /// <summary>
        /// Actualiza las propiedades de un administrador de usuario.
        /// Utiliza el método base <see cref="User.UpdateUserProperties"/> para actualizar las propiedades de usuario comunes.
        /// </summary>
        /// <param name="display">El nuevo nombre visible o alias.</param>
        /// <param name="email">La nueva dirección de correo electrónico.</param>
        /// <param name="adminAccess">El nuevo estado de acceso de administrador.</param>
        /// <param name="securityPin">El nuevo pin de seguridad.</param>
        public void Update(string display, string email, bool adminAccess, string securityPin)
        {
            base.UpdateUserProperties(display, email);
            AdminAccess = adminAccess;
            SecurityPin = securityPin;
        }

        /// <summary>
        /// Añade una cantidad al capital del administrador.
        /// </summary>
        /// <param name="amount">La cantidad a añadir. Debe ser no negativa.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se lanza si la cantidad es negativa.</exception>
        public void AddCapital(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to add to capital must be positive.");
            }
            Capital += amount;
        }

        /// <summary>
        /// Reduce una cantidad del capital del administrador.
        /// </summary>
        /// <param name="amount">La cantidad a reducir. Debe ser no negativa.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se lanza si la cantidad es negativa.</exception>
        /// <exception cref="InvalidOperationException">Se lanza si la reducción hace que el capital sea negativo.</exception>
        public void DecreaseCapital(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount to decrease capital must be positive.");
            }
            if (Capital - amount < 0)
            {
                throw new InvalidOperationException("Capital cannot go below zero.");
            }
            Capital -= amount;
        }

        /// <summary>
        /// Establece el capital del administrador a un nuevo valor.
        /// </summary>
        /// <param name="newCapital">El nuevo valor del capital.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se lanza si el nuevo capital es negativo.</exception>
        public void UpdateCapital(decimal newCapital)
        {
            if (newCapital < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newCapital), "Capital cannot be negative.");
            }
            Capital = newCapital;
        }
    }
}