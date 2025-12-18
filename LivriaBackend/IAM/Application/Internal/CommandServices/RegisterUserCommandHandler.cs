using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LivriaBackend.IAM.Domain.Model.Commands;
using LivriaBackend.IAM.Domain.Model.Aggregates;
using LivriaBackend.IAM.Domain.Repositories;
using LivriaBackend.shared.Domain.Repositories;

namespace LivriaBackend.IAM.Application.Internal.CommandServices
{
    
    public class RegisterUserCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterUserCommandHandler(
            IIdentityRepository identityRepository,
            IUnitOfWork unitOfWork
           )
        {
            _identityRepository = identityRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<int> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var existingIdentity = await _identityRepository.GetByUsernameAsync(command.Username);
            if (existingIdentity != null)
            {
                throw new ApplicationException($"Username '{command.Username}' is already taken.");
            }
            
            var identity = new Identity(
                command.UserId,
                command.Username,
                command.Password
            );
            
            await _identityRepository.AddAsync(identity);
            await _unitOfWork.CompleteAsync();
            
            return identity.Id;
        }
    }
}