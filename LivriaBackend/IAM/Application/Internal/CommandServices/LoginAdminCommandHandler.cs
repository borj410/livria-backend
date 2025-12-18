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
    public class LoginAdminCommandHandler : IRequestHandler<LoginAdminCommand, LoginResponse>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IUserAdminRepository _userAdminRepository;

        public LoginAdminCommandHandler(
            IIdentityRepository identityRepository,
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IUserAdminRepository userAdminRepository)
        {
            _identityRepository = identityRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _userAdminRepository = userAdminRepository;
        }

        public async Task<LoginResponse> Handle(LoginAdminCommand command, CancellationToken cancellationToken)
        {
            var identity = await _identityRepository.GetByUsernameAsync(command.Username);
            
            if (identity == null || !identity.VerifyPassword(command.Password))
            {
                return new LoginResponse(0, 0, command.Username, false, "Invalid username or password.");
            }
            
            var userAdmin = await _userAdminRepository.GetByIdAsync(identity.UserId);
            if (userAdmin == null)
            {
                return new LoginResponse(0, 0, command.Username, false, "Invalid username or password for admin login.");
            }
            
            if (userAdmin.SecurityPin != command.SecurityPin)
            {
                return new LoginResponse(0, 0, command.Username, false, "Invalid security pin.");
            }
            
            await _identityRepository.UpdateAsync(identity);
            await _unitOfWork.CompleteAsync();
            
            var roles = new List<string> { "Admin" };
            string token = _tokenService.GenerateToken(identity, roles);

            return new LoginResponse(identity.Id, identity.UserId, identity.UserName, true, "Login successful.", token);
        }
    }
}