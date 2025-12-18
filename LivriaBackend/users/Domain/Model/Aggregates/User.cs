using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivriaBackend.users.Domain.Model.Aggregates
{
    /// <summary>
    /// Clase abstracta base para todos los tipos de usuarios en el sistema.
    /// Define las propiedades comunes y el comportamiento básico que comparten
    /// tanto los administradores (<see cref="UserAdmin"/>) como los clientes (<see cref="UserClient"/>).
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Obtiene o establece el identificador único del usuario.
        /// Este campo es protegido para permitir su establecimiento por subclases o el ORM.
        /// </summary>
        public int Id { get; protected set; } 

        /// <summary>
        /// Obtiene el nombre visible o alias del usuario.
        /// </summary>
        public string Display { get; private set; }

        /// <summary>
        /// Obtiene el nombre de usuario único para el inicio de sesión.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Obtiene la dirección de correo electrónico del usuario.
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Constructor protegido sin parámetros, típicamente utilizado por ORMs como Entity Framework Core.
        /// </summary>
        protected User() { }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="User"/> con las propiedades básicas.
        /// </summary>
        /// <param name="display">El nombre visible o alias del usuario.</param>
        /// <param name="username">El nombre de usuario único.</param>
        /// <param name="email">La dirección de correo electrónico.</param>
        public User(string display, string username, string email)
        {
            Display = display;
            Username = username;
            Email = email;
        }

        /// <summary>
        /// Constructor protegido para inicializar una instancia de <see cref="User"/> con un ID existente
        /// y las propiedades básicas. Utilizado principalmente para la reconstrucción desde la persistencia.
        /// </summary>
        /// <param name="id">El identificador único del usuario.</param>
        /// <param name="display">El nombre visible o alias del usuario.</param>
        /// <param name="username">El nombre de usuario único.</param>
        /// <param name="email">La dirección de correo electrónico.</param>
        protected User(int id, string display, string username, string email)
            : this(display, username, email) 
        {
            Id = id; 
        }

        /// <summary>
        /// Método protegido para actualizar las propiedades básicas de un usuario.
        /// Este método es utilizado por las clases derivadas para actualizar sus propias propiedades
        /// manteniendo la encapsulación de las propiedades base.
        /// </summary>
        /// <param name="display">El nuevo nombre visible o alias.</param>
        /// <param name="username">El nuevo nombre de usuario.</param>
        /// <param name="email">La nueva dirección de correo electrónico.</param>
        protected void UpdateUserProperties(string display, string email)
        {
            Display = display;
            Email = email;
        }
        
        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) throw new ArgumentException("Username cannot be empty.");
            Username = newUsername; 
        }
    }
}