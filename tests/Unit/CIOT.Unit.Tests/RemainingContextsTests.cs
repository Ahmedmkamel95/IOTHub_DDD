using CIOT.Modules.Admin.Application;
using CIOT.Modules.Audit.Application;
using CIOT.Modules.Integration.Application;
using CIOT.Modules.LocalAdapter.Application;
using CIOT.Modules.Mobile.Application;
using CIOT.Modules.Report.Application;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.RemainingContexts;

public sealed class RemainingContextsTests
{
    [Test]
    public async Task Admin_CreateAndListEquipmentModels()
    {
        await using var context = TestDbContextFactory.CreateAdminContext();
        var handlers = new AdminHandlers(context);
        var created = await handlers.Handle(
            new CreateEquipmentModelCommand(new CreateEquipmentModelRequest("Acme", "X1", "Coffee")),
            CancellationToken.None
        );
        var listed = await handlers.Handle(new GetEquipmentModelsQuery(), CancellationToken.None);

        await Assert.That(created.IsSuccess).IsTrue();
        await Assert.That(listed.Value).HasSingleItem();
        await Assert.That(await context.EquipmentModels.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task Audit_LogAndFilterEvents()
    {
        await using var context = TestDbContextFactory.CreateAuditContext();
        var handlers = new AuditHandlers(context);
        await handlers.Handle(new LogAuditCommand(null, "Create", "Device", "device-1"), CancellationToken.None);

        var result = await handlers.Handle(new GetAuditEventsQuery("Device", "device-1"), CancellationToken.None);

        await Assert.That(result.Value).HasSingleItem();
        await Assert.That(result.Value[0].Action).IsEqualTo("Create");
    }

    [Test]
    public async Task Integration_CreatePartnerSourceNormalizesCode()
    {
        await using var context = TestDbContextFactory.CreateIntegrationContext();
        var result = await new IntegrationHandlers(context).Handle(
            new CreatePartnerSourceCommand(new CreatePartnerSourceRequest("sap", "SAP")),
            CancellationToken.None
        );

        await Assert.That(result.Value.SourceCode).IsEqualTo("SAP");
        await Assert.That(result.Value.IsActive).IsTrue();
    }

    [Test]
    public async Task LocalAdapter_ApplyDeviceEffectPersistsAppliedStatus()
    {
        await using var context = TestDbContextFactory.CreateLocalAdapterContext();
        var deviceId = Guid.NewGuid();
        var result = await new LocalAdapterHandlers(context).Handle(
            new ApplyDeviceEffectCommand(deviceId, "Refresh", "{}"),
            CancellationToken.None
        );

        await Assert.That(result.Value.DeviceId).IsEqualTo(deviceId);
        await Assert.That(result.Value.Status).IsEqualTo("Applied");
    }

    [Test]
    public async Task Mobile_SyncOfflineBatchProcessesAllActions()
    {
        await using var context = TestDbContextFactory.CreateMobileContext();
        var result = await new MobileHandlers(context).Handle(
            new SyncOfflineBatchCommand(new SyncOfflineBatchRequest(
                Guid.NewGuid(), "session-1",
                [new SyncOfflineActionItem("a1", "Update", "{}"), new SyncOfflineActionItem("a2", "Create", "{}")])),
            CancellationToken.None
        );

        await Assert.That(result.Value.ProcessedCount).IsEqualTo(2);
        await Assert.That(result.Value.Success).IsTrue();
        await Assert.That(await context.OfflineBatches.CountAsync()).IsEqualTo(1);
    }

    [Test]
    public async Task Report_CreateAndListDefinitions()
    {
        await using var context = TestDbContextFactory.CreateReportContext();
        var handlers = new ReportHandlers(context);
        var created = await handlers.Handle(
            new CreateReportDefinitionCommand(new CreateReportDefinitionRequest("ops", "Operations", "Dashboard")),
            CancellationToken.None
        );
        var listed = await handlers.Handle(new GetReportDefinitionsQuery(), CancellationToken.None);

        await Assert.That(created.Value.ReportCode).IsEqualTo("OPS");
        await Assert.That(listed.Value).HasSingleItem();
    }
}
