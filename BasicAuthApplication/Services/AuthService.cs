using BasicAuthApplication.Dtos;
using BasicAuthApplication.Interfaces;
using BasicAuthDomain.Entities;
using BasicAuthDomain.Interfaces;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BasicAuthApplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
            {
                throw new Exception("El email ya está registrado");
            }

            if (await _userRepository.ExistsByUsernameAsync(request.Username))
            {
                throw new Exception("El nombre de usuario ya está en uso");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
            };

            var createdUser = await _userRepository.CreateAsync(user);

            var token = _jwtService.GenerateToken(createdUser);

            return new AuthResponseDto
            {
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new Exception("Credenciales inválidas");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Credenciales inválidas");
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token
            };
        }
    }
}
