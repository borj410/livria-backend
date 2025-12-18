using System;
using System.ComponentModel.DataAnnotations;
using System.Linq; 
using Microsoft.Extensions.Localization;
using LivriaBackend.notifications.Domain.Model.ValueObjects;
using LivriaBackend.notifications.Application.Resources; 

namespace LivriaBackend.notifications.Application.Validation
{
    /// <summary>
    /// Atributo de validación personalizado para verificar que un valor de cadena
    /// es un tipo de notificación válido definido en la enumeración <see cref="ENotificationType"/>.
    /// Este atributo soporta la localización de mensajes de error.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidNotificationTypeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Obtiene o establece el nombre de la clave del recurso de error para la localización.
        /// El valor predeterminado es "InvalidNotificationType".
        /// </summary>
        public string ErrorResourceName { get; set; } = "InvalidNotificationType"; 

        /// <summary>
        /// Obtiene o establece el tipo del recurso que contiene el mensaje de error para la localización.
        /// El valor predeterminado es <see cref="typeof(DataAnnotations)"/>.
        /// </summary>
        public Type ErrorResourceType { get; set; } = typeof(DataAnnotations); 

        /// <summary>
        /// Determina si el valor especificado del objeto es válido.
        /// Este método verifica si el valor de entrada es una cadena que puede ser parseada
        /// en un valor válido de la enumeración <see cref="ENotificationType"/>.
        /// </summary>
        /// <param name="value">El valor a validar.</param>
        /// <param name="validationContext">El contexto en el que se realiza la validación.</param>
        /// <returns>Un <see cref="ValidationResult"/> que indica si la validación fue exitosa.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if (value == null)
            {
                return ValidationResult.Success;
            }
            
            if (value is string typeString)
            {
                
                if (Enum.TryParse(typeof(ENotificationType), typeString, ignoreCase: true, out object result) && Enum.IsDefined(typeof(ENotificationType), result))
                {
                    return ValidationResult.Success; 
                }
                else
                {
                    
                    var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
                    IStringLocalizer localizer = null;

                    if (localizerFactory != null && ErrorResourceType != null)
                    {
                        localizer = localizerFactory.Create(ErrorResourceType);
                    }
                    
                    string errorMessageTemplate = localizer?[ErrorResourceName] ??
                                                  (ErrorMessage ?? "The '{0}' field has an invalid value.");
                    
                    
                    string finalErrorMessage = string.Format(
                        errorMessageTemplate,
                        validationContext.DisplayName ?? validationContext.MemberName
                    );

                    return new ValidationResult(finalErrorMessage, new[] { validationContext.MemberName });
                }
            }
            
            return new ValidationResult(ErrorMessage ?? "The field must be a string.");
        }
    }
}