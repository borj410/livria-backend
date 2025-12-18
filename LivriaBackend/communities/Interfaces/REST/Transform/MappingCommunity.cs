using AutoMapper;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Interfaces.REST.Resources;

namespace LivriaBackend.communities.Interfaces.REST.Transform
{
    /// <summary>
    /// Perfil de mapeo de AutoMapper para el módulo de comunidades.
    /// Define las reglas de transformación entre recursos REST, comandos y agregados/entidades de dominio.
    /// </summary>
    public class CommunitiesMappingProfile : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CommunitiesMappingProfile"/>.
        /// En este constructor, se configuran todas las relaciones de mapeo para el dominio de comunidades.
        /// </summary>
        public CommunitiesMappingProfile()
        {
            CreateMap<CreateCommunityResource, CreateCommunityCommand>();
            CreateMap<CreatePostResource, CreatePostCommand>();

            
            CreateMap<Community, CommunityResource>();
            CreateMap<Post, PostResource>();
            
            CreateMap<PostReaction, PostReactionResource>();
            
            CreateMap<Comment, CommentResource>();
            
            CreateMap<JoinCommunityResource, JoinCommunityCommand>();
            CreateMap<UserCommunity, UserCommunityResource>();
        }
    }
}