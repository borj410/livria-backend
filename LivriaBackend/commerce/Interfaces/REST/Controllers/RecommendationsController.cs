using AutoMapper;
using LivriaBackend.commerce.Domain.Model.Aggregates;
using LivriaBackend.commerce.Domain.Model.Queries;
using LivriaBackend.commerce.Domain.Model.Services;
using LivriaBackend.commerce.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using System;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq; 
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace LivriaBackend.commerce.Interfaces.REST.Controllers
{
    /// <summary>
    /// Controlador RESTful para gestionar las operaciones relacionadas con las recomendaciones de libros.
    /// </summary>
    [Authorize(Roles = "UserClient,Admin")]
    [ApiController]
    [Route("api/v1/recommendations")]
    [Produces(MediaTypeNames.Application.Json)]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationQueryService _recommendationQueryService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="RecommendationsController"/>.
        /// </summary>
        /// <param name="recommendationQueryService">El servicio de consulta de recomendaciones.</param>
        /// <param name="mapper">La instancia de AutoMapper para la transformación de objetos.</param>
        public RecommendationsController(IRecommendationQueryService recommendationQueryService, IMapper mapper)
        {
            _recommendationQueryService = recommendationQueryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene los datos de las recomendaciones de libros que le pertenecen a un usuario específico.
        /// </summary>
        /// <param name="userClientId">El identificador único del cliente de usuario.</param>
        /// <returns>
        /// Una acción de resultado HTTP que contiene un <see cref="RecommendationResource"/> si se encuentran recomendaciones (código 200 OK).
        /// Si no se encuentran recomendaciones o el usuario no existe, retorna un <see cref="RecommendationResource"/> vacío (código 200 OK).
        /// Retorna NotFound (404) si hay un error de argumento (por ejemplo, si el UserClientId no es válido).
        /// Retorna StatusCode 500 si ocurre un error inesperado.
        /// </returns>
        [HttpGet("users/{userClientId}")]
        [SwaggerOperation(
            Summary= "Obtener los datos de las recomendaciones que le pertenecen a un usuario en específico.",
            Description= "Te muestra los datos de las recomendaciones por medio del id de un usuario en específico."
        )]
        public async Task<ActionResult<RecommendationResource>> GetUserRecommendations(int userClientId)
        {
            var query = new GetUserRecommendationsQuery(userClientId);
            try
            {
                var recommendation = await _recommendationQueryService.Handle(query);
                if (recommendation == null || !recommendation.RecommendedBooks.Any())
                {
                    
                    return Ok(_mapper.Map<RecommendationResource>(recommendation ?? new Domain.Model.Entities.Recommendation(userClientId, new System.Collections.Generic.List<Book>())));
                }
                var resource = _mapper.Map<RecommendationResource>(recommendation);
                return Ok(resource);
            }
            catch (ArgumentException ex)
            {
                
                return NotFound(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}