using CIOT.Modules.Asset.Application.Commands;
using CIOT.Modules.Asset.Application.Dtos;
using CIOT.Modules.Asset.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;
using DomainAsset = CIOT.Modules.Asset.Domain.Asset;

namespace CIOT.Unit.Tests.Asset;

public sealed class AssetTests
{
    [Test]
    public async Task RegisterAsset_NormalizesCountryAndPersistsAsset()
    {
        await using var context = TestDbContextFactory.CreateAssetContext();
        var result = await new AssetCommandHandlers(context).Handle(
            new RegisterAssetCommand(new RegisterAssetRequest("SAP-001", "OEM-001", "TECH-001", null, "ae", "Active")),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CountryCode).IsEqualTo("AE");
        await Assert.That(await context.Assets.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task RegisterAsset_WhenSapNumberExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateAssetContext();
        context.Assets.Add(new DomainAsset { SapEquipmentNumber = "SAP-001", CountryCode = "AE" });
        await context.SaveChangesAsync();

        var result = await new AssetCommandHandlers(context).Handle(
            new RegisterAssetCommand(new RegisterAssetRequest("SAP-001", null, null, null, "AE", null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Asset.Duplicate");
    }

    [Test]
    public async Task Asset_AssignToCustomerOutlet_UnassignsPreviousCurrentAssignment()
    {
        var asset = new DomainAsset { Id = Guid.NewGuid() };
        var first = asset.AssignToCustomerOutlet(Guid.NewGuid(), Guid.NewGuid());
        var second = asset.AssignToCustomerOutlet(Guid.NewGuid(), Guid.NewGuid());

        await Assert.That(first.IsCurrent).IsFalse();
        await Assert.That(second.IsCurrent).IsTrue();
        await Assert.That(asset.OutletAssignments).Count().IsEqualTo(2);
    }

    [Test]
    public async Task RegisterAssetValidator_RejectsMissingRequiredFields()
    {
        var result = await new RegisterAssetCommandValidator().ValidateAsync(
            new RegisterAssetCommand(new RegisterAssetRequest(string.Empty, null, null, null, string.Empty, null))
        );

        await Assert.That(result.IsValid).IsFalse();
        await Assert.That(result.Errors).Count().IsEqualTo(2);
    }
}
