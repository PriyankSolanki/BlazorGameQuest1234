using System.Threading.Tasks;

public interface ITokenService
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string? token);
    Task RemoveTokenAsync();
}