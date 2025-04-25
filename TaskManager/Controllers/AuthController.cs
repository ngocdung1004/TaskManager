using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Data;
using TaskManager.DTOs;
using TaskManager.Models;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TaskManagerContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthController(TaskManagerContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var token = GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDto>(user);

                return new AuthResponseDto
                {
                    Token = token,
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(CreateUserDto createUserDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Username == createUserDto.Username))
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }

                var user = _mapper.Map<User>(createUserDto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                var userDto = _mapper.Map<UserDto>(user);

                return new AuthResponseDto
                {
                    Token = token,
                    User = userDto
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
} 