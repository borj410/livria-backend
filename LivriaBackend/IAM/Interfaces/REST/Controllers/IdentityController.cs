using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using LivriaBackend.IAM.Domain.Model.Commands;
using LivriaBackend.IAM.Application.Resources;
using AutoMapper;
using LivriaBackend.users.Domain.Model.Commands;
using LivriaBackend.users.Interfaces.REST.Resources;
using LivriaBackend.users.Interfaces.REST.Transform;
using System;
using System.Security.Claims;
using LivriaBackend.IAM.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Authorization;

namespace LivriaBackend.IAM.Interfaces.REST.Controllers
{
    [ApiController]
    [Route("api/v1/authentication")]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public IdentityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserClientRequest request)
        {
            var command = new RegisterUserClientCompositeCommand(
                request.Username,
                request.Password,
                request.Display,
                request.Email,
                request.Icon,
                request.Phrase
            );
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest(new { message = "Registration failed." });
            }
            return StatusCode(201, new { message = "Registration successful." });
        }
        
        [HttpPost("sign-in/admin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAdmin([FromBody] LoginAdminCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
            {
                return Unauthorized(new { message = response.Message });
            }
            return Ok(response);
        }
        
        [HttpPost("sign-in/client")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInClient([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
            {
                return Unauthorized(new { message = response.Message });
            }
            return Ok(response);
        }
        
        [Authorize]
        [HttpPut("update-security")]
        public async Task<IActionResult> UpdateSecurity([FromBody] UpdateIdentitySecurityResource resource)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            
            var userId = int.Parse(userIdClaim.Value);
            
            if (string.IsNullOrEmpty(resource.NewUsername) && string.IsNullOrEmpty(resource.NewPassword))
            {
                return BadRequest(new { message = "At least one field (NewUsername or NewPassword) must be provided." });
            }

            var command = new UpdateIdentitySecurityCommand(
                userId,
                resource.CurrentPassword,
                resource.NewUsername,
                resource.NewPassword
            );

            try
            {
                var result = await _mediator.Send(command);
                if (result) return Ok(new { message = "Credentials updated successfully." });
                return BadRequest(new { message = "Could not update credentials." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An internal error occurred." });
            }
        }
    }
}