using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using LivriaBackend.shared.Resources;
using Microsoft.Extensions.Localization;

namespace LivriaBackend.shared.Validation
{
    /// <summary>
    /// Atributo de validación personalizado que verifica si una fecha está dentro
    /// de un rango especificado, desde una fecha mínima hasta la fecha actual (hoy).
    /// </summary>
    /// <remarks>
    /// Este atributo es útil para campos de fecha donde la fecha no puede ser en el futuro
    /// y debe ser posterior o igual a una fecha de inicio determinada.
    /// Soporta la obtención de mensajes de error internacionalizados a través de <see cref="IStringLocalizer"/>.
    /// </remarks>
    public class DateRangeTodayAttribute : ValidationAttribute
    {
        /// <summary>
        /// Obtiene o establece la fecha mínima permitida para la validación.
        /// La fecha debe estar en formato "yyyy-MM-dd". Si no se especifica
        /// o el formato es incorrecto, se usará <see cref="DateTime.MinValue"/>.
        /// </summary>
        public string MinimumDate { get; set; }
        
        /// <summary>
        /// Obtiene o establece el nombre del recurso para el mensaje de error en el archivo de recursos compartidos.
        /// Por defecto es "DateNotInRange".
        /// </summary>
        public string ErrorResourceName { get; set; } = "DateNotInRange";

        /// <summary>
        /// Obtiene o establece el tipo del recurso donde se encuentra el mensaje de error.
        /// Por defecto es <see cref="typeof(SharedResource)"/>.
        /// </summary>
        public Type ErrorResourceType { get; set; } = typeof(SharedResource);
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DateRangeTodayAttribute"/>
        /// con una fecha mínima especificada.
        /// </summary>
        /// <param name="minimumDate">La fecha mínima permitida en formato "yyyy-MM-dd".</param>
        public DateRangeTodayAttribute(string minimumDate)
        {
            MinimumDate = minimumDate;
        }
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DateRangeTodayAttribute"/>
        /// con la fecha mínima por defecto establecida en <see cref="DateTime.MinValue"/>.
        /// </summary>
        public DateRangeTodayAttribute() : this(DateTime.MinValue.ToString("yyyy-MM-dd"))
        {
        }

        /// <summary>
        /// Implementa la lógica de validación para el atributo <see cref="DateRangeTodayAttribute"/>.
        /// </summary>
        /// <param name="value">El valor del campo a validar.</param>
        /// <param name="validationContext">El contexto de validación, que proporciona información sobre el objeto y el miembro que se está validando.</param>
        /// <returns>
        /// Un <see cref="ValidationResult"/> que indica si la validación fue exitosa o falló.
        /// Retorna <see cref="ValidationResult.Success"/> si el valor es nulo o si es una fecha dentro del rango.
        /// Retorna un <see cref="ValidationResult"/> con un mensaje de error si la fecha está fuera de rango
        /// o si el valor no es de tipo <see cref="DateTime"/>.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateValue)
            {
                DateTime parsedMinDate;
                
                if (string.IsNullOrEmpty(MinimumDate) || !DateTime.TryParseExact(MinimumDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedMinDate))
                {
                    parsedMinDate = DateTime.MinValue;
                }
                
                
                DateTime maxDate = DateTime.Today;
                
                
                if (dateValue.Date >= parsedMinDate.Date && dateValue.Date <= maxDate.Date)
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
                                                  (ErrorMessage ?? "The '{0}' field must be a date between {1} and today.");

                    string finalErrorMessage = string.Format(
                        errorMessageTemplate,
                        validationContext.DisplayName ?? validationContext.MemberName, 
                        parsedMinDate.ToShortDateString() 
                    );

                    return new ValidationResult(finalErrorMessage, new[] { validationContext.MemberName });
                }
            }
            
            return new ValidationResult(ErrorMessage ?? "The field must be a valid date type.");
        }
    }
}