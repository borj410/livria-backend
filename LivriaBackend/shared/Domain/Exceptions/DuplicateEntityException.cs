using System;

namespace LivriaBackend.shared.Domain.Exceptions
{
    /// <summary>
    /// Excepción lanzada cuando se intenta crear una entidad que ya existe basándose en criterios de unicidad.
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException() { }

        public DuplicateEntityException(string message) : base(message) { }

        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}