using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using LivriaBackend.IAM.Application.Resources;
using LivriaBackend.IAM.Domain.Model.Commands;
using LivriaBackend.IAM.Domain.Repositories;
using LivriaBackend.IAM.Application.Internal.OutboundServices;
using LivriaBackend.shared.Domain.Repositories;
using LivriaBackend.users.Domain.Model.Repositories;

namespace LivriaBackend.IAM.Application.Internal.CommandServices
{
    public class LoginClientCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IUserClientRepository _userClientRepository;

        public LoginClientCommandHandler(
            IIdentityRepository identityRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IUserClientRepository userClientRepository)
        {
            _identityRepository = identityRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userClientRepository = userClientRepository;
        }

        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var identity = await _identityRepository.GetByUsernameAsync(command.Username);

            
            if (identity == null || !identity.VerifyPassword(command.Password))
            {
                return new LoginResponse(0, 0, command.Username, false, "Invalid username or password.");
            }
            
            
            var userClient = await _userClientRepository.GetByIdAsync(identity.UserId);
            if (userClient == null)
            {
                return new LoginResponse(0, 0, command.Username, false, "Invalid username or password for client login.");
            }
            
            await _identityRepository.UpdateAsync(identity);
            await _unitOfWork.CompleteAsync();
            
            var roles = new List<string> { "UserClient" };
            string token = _tokenService.GenerateToken(identity, roles);

            return new LoginResponse(identity.Id, identity.UserId, identity.UserName, true, "Login successful.", token);
        }
    }
}