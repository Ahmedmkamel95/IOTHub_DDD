using CIOT.Modules.CustomerOutlet.Application.Commands;
using CIOT.Modules.CustomerOutlet.Application.Dtos;
using CIOT.Modules.CustomerOutlet.Domain;
using CIOT.Unit.Tests.TestInfrastructure;
using Microsoft.EntityFrameworkCore;
using TUnit.Assertions;
using TUnit.Core;

namespace CIOT.Unit.Tests.CustomerOutlet;

public sealed class CustomerOutletTests
{
    [Test]
    public async Task CreateCustomer_NormalizesCodeAndCountry()
    {
        await using var context = TestDbContextFactory.CreateCustomerOutletContext();
        var result = await new CustomerOutletCommandHandlers(context).Handle(
            new CreateCustomerCommand(new CreateCustomerRequest("cust-001", "Customer", null, "ae", null, null)),
            CancellationToken.None
        );

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value.CustomerCode).IsEqualTo("CUST-001");
        await Assert.That(result.Value.CountryCode).IsEqualTo("AE");
    }

    [Test]
    public async Task CreateCustomer_WhenCodeExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateCustomerOutletContext();
        context.Customers.Add(new Customer { CustomerCode = "CUST-001", CountryCode = "AE" });
        await context.SaveChangesAsync();

        var result = await new CustomerOutletCommandHandlers(context).Handle(
            new CreateCustomerCommand(new CreateCustomerRequest("cust-001", null, null, "AE", null, null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Customer.Duplicate");
    }

    [Test]
    public async Task CreateOutlet_WhenCodeExists_ReturnsConflict()
    {
        await using var context = TestDbContextFactory.CreateCustomerOutletContext();
        context.Outlets.Add(new Outlet { OutletCode = "OUT-001", CountryCode = "AE" });
        await context.SaveChangesAsync();

        var result = await new CustomerOutletCommandHandlers(context).Handle(
            new CreateOutletCommand(new CreateOutletRequest("out-001", null, null, null, null, null, "AE", null, null, null)),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Outlet.Duplicate");
    }

    [Test]
    public async Task AddOutletNote_WhenOutletDoesNotExist_ReturnsNotFound()
    {
        await using var context = TestDbContextFactory.CreateCustomerOutletContext();

        var result = await new CustomerOutletCommandHandlers(context).Handle(
            new AddOutletNoteCommand(Guid.NewGuid(), new AddOutletNoteRequest("Installation note")),
            CancellationToken.None
        );

        await Assert.That(result.IsFailure).IsTrue();
        await Assert.That(result.Error.Code).IsEqualTo("Outlet.NotFound");
    }
}
