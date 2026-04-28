using Backend.Core.DTOs;

namespace Backend.Core.Services;

public interface IAuthServices
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}