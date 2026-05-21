using RealEstateCrmApi.Domain.Entities;

namespace RealEstateCrmApi.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
