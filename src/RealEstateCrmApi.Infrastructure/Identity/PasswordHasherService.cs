using Microsoft.AspNetCore.Identity;
using RealEstateCrmApi.Application.Common.Interfaces;
using RealEstateCrmApi.Domain.Entities;

namespace RealEstateCrmApi.Infrastructure.Identity;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password) =>
        _passwordHasher.HashPassword(user, password);

    public bool VerifyPassword(User user, string password, string passwordHash) =>
        _passwordHasher.VerifyHashedPassword(user, passwordHash, password)
        == PasswordVerificationResult.Success;
}
