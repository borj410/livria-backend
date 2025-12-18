using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LivriaBackend.IAM.Domain.Model.Commands;
using LivriaBackend.IAM.Domain.Model.Aggregates;
using LivriaBackend.IAM.Domain.Repositories;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Repositories;

namespace LivriaBackend.IAM.Application.Internal.CommandServices
{
    public class UpdateIdentitySecurityCommandHandler
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateIdentitySecurityCommandHandler(
            IIdentityRepository identityRepository,
            IUserClientRepository userClientRepository,
            IUnitOfWork unitOfWork
        )
        {
            _identityRepository = identityRepository;
            _userClientRepository = userClientRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<bool> Handle(UpdateIdentitySecurityCommand request, CancellationToken cancellationToken)
        {
            var identity = await _identityRepository.GetByUserIdAsync(request.UserId);
            var userClient = await _userClientRepository.GetByIdAsync(request.UserId);

            if (identity == null || userClient == null) return false;

            if (!identity.HashedPassword.Matches(request.CurrentPassword))
                throw new ArgumentException("Invalid current password.");
            
            if (!string.IsNullOrEmpty(request.NewUsername))
            {
                var exists = await _identityRepository.ExistsByUsernameAsync(request.NewUsername);
                if (exists) throw new ArgumentException("Username already taken.");

                identity.UpdateUsername(request.NewUsername);
                userClient.UpdateUsername(request.NewUsername);
            }
            
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                identity.UpdatePassword(request.CurrentPassword, request.NewPassword);
            }

            await _identityRepository.UpdateAsync(identity);
            await _userClientRepository.UpdateAsync(userClient);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}