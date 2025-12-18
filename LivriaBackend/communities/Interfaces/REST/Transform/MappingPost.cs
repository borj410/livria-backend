using AutoMapper;
using LivriaBackend.communities.Domain.Model.Aggregates;
using LivriaBackend.communities.Domain.Model.Commands;
using LivriaBackend.communities.Interfaces.REST.Resources;

namespace LivriaBackend.communities.Interfaces.REST.Transform
{
    public class MappingPost : Profile
    {
        public MappingPost()
        {
            
            CreateMap<CreatePostResource, CreatePostCommand>();

            
            CreateMap<Post, PostResource>();
        }
    }
}