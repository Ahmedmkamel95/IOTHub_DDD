using CIOT.Modules.Org.Application.Commands;
using CIOT.Modules.Org.Application.Dtos;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.Org;

public sealed class OrgValidatorTests
{
    [Test]
    public async Task CreateCountryValidator_AcceptsValidRequest()
    {
        var result = await new CreateCountryCommandValidator().ValidateAsync(
            new CreateCountryCommand(new CreateCountryRequest("AE", "United Arab Emirates", null))
        );

        await Assert.That(result.IsValid).IsTrue();
    }

    [Test]
    public async Task CreateCountryValidator_RejectsMissingCodeAndName()
    {
        var result = await new CreateCountryCommandValidator().ValidateAsync(
            new CreateCountryCommand(new CreateCountryRequest(string.Empty, string.Empty, null))
        );

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Count().IsEqualTo(2);
    }
}
