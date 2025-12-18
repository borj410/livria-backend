using LivriaBackend.commerce.Domain.Model.Commands;
using LivriaBackend.commerce.Domain.Model.Entities;
using System.Threading.Tasks;

namespace LivriaBackend.commerce.Domain.Model.Services
{
    public interface ICartItemCommandService
    {
        Task<CartItem> Handle(CreateCartItemCommand command);
        Task<CartItem> Handle(UpdateCartItemQuantityCommand command);
        Task<bool> Handle(RemoveCartItemCommand command); 
    }
}