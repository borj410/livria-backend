using AutoMapper;
using LivriaBackend.users.Domain.Model.Aggregates;
using LivriaBackend.users.Domain.Model.Commands;
using LivriaBackend.users.Interfaces.REST.Resources;

namespace LivriaBackend.users.Interfaces.REST.Transform
{
    /// <summary>
    /// Perfil de mapeo de AutoMapper para el módulo de usuarios.
    /// Define las reglas para transformar objetos entre:
    /// <list type="bullet">
    ///     <item>Recursos de creación (<c>CreateUserClientResource</c>) y comandos correspondientes (<c>CreateUserClientCommand</c>).</item>
    ///     <item>Entidades de agregado de cliente de usuario (<c>UserClient</c>) y recursos de respuesta (<c>UserClientResource</c>).</item>
    ///     <item>Recursos de actualización (<c>UpdateUserClientResource</c>) y comandos correspondientes (<c>UpdateUserClientCommand</c>).</item>
    ///     <item>Entidades de agregado de administrador de usuario (<c>UserAdmin</c>) y recursos de respuesta (<c>UserAdminResource</c>).</item>
    ///     <item>Recursos de actualización (<c>UpdateUserAdminResource</c>) y comandos correspondientes (<c>UpdateUserAdminCommand</c>).</item>
    ///     <item>Entidades base de usuario (<c>User</c>) y recursos genéricos de usuario (<c>UserResource</c>).</item>
    /// </list>
    /// </summary>
    public class UsersMappingProfile : Profile
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsersMappingProfile"/>.
        /// En el constructor, se definen todas las configuraciones de mapeo.
        /// </summary>
        public UsersMappingProfile()
        {
            CreateMap<CreateUserClientResource, CreateUserClientCommand>();
            CreateMap<UserClient, UserClientResource>();
            CreateMap<UpdateUserClientResource, UpdateUserClientCommand>();
            CreateMap<UserAdmin, UserAdminResource>();
            CreateMap<UpdateUserAdminResource, UpdateUserAdminCommand>();
            CreateMap<User, UserResource>();
            CreateMap<RegisterUserClientRequest, RegisterUserClientCompositeCommand>();
        }
    }
}