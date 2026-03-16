using E_CommerceApi.Models;

namespace E_CommerceApi.Interfaces;

public interface ITokenService
{
   Task <string> GenerateJwtToken(User user);
}