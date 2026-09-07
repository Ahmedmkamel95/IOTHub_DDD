using CIOT.Modules.Identity.Application.Commands;
using CIOT.Modules.Identity.Application.Dtos;
using CIOT.Modules.Identity.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Identity;

public sealed class IdentityTests
{
    [Test]
    public async Task CreateUser_NormalizesEmailAndDerivesDisplayName()
    {
        await using var context = TestDbContextFactory.CreateIdentityContext();
        var result = await new CreateUserCommandHandler(context).Handle(
            new CreateUserCommand(new CreateUserRequest(
                "USER@EXAMPLE.COM", null, "Ada", "Lovelace", "Internal", "AE", null)),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.Email).IsEqualTo("user@example.com");
        await Assert.That(result.Value.DisplayName).IsEqualTo("Ada Lovelace");
        await Assert.That(result.Value.Status).IsEqualTo("Invited");
    }

    [Test]
    public async Task CreateUser_WhenEmailExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateIdentityContext();
        context.UserAccounts.Add(new UserAccount { Email = "user@example.com", UserType = "External" });
        await context.SaveChangesAsync();

        var result = await new CreateUserCommandHandler(context).Handle(
            new CreateUserCommand(new CreateUserRequest(
                "USER@example.com", "User", null, null, "External", null, null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("User.DuplicateEmail");
    }

    [Test]
    public async Task CreateUserValidator_RejectsInvalidUserTypeAndEmail()
    {
        var result = await new CreateUserCommandValidator().ValidateAsync(
            new CreateUserCommand(new CreateUserRequest("invalid", null, null, null, "Unknown", null, null))
        );

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Count().IsEqualTo(2);
    }
}
