using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.CustomerOutlet.Application.Dtos;
using CIOT.Modules.CustomerOutlet.Domain;
using CIOT.Modules.CustomerOutlet.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.CustomerOutlet.Application.Commands;

public record CreateCustomerClusterCommand(CreateCustomerClusterRequest Request) : ICommand<CustomerClusterDto>;
public record CreateCustomerCommand(CreateCustomerRequest Request) : ICommand<CustomerDto>;
public record CreateOutletCommand(CreateOutletRequest Request) : ICommand<OutletDto>;
public record AddOutletNoteCommand(Guid OutletId, AddOutletNoteRequest Request) : ICommand<OutletNoteDto>;

public class CustomerOutletCommandHandlers :
    IRequestHandler<CreateCustomerClusterCommand, Result<CustomerClusterDto>>,
    IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>,
    IRequestHandler<CreateOutletCommand, Result<OutletDto>>,
    IRequestHandler<AddOutletNoteCommand, Result<OutletNoteDto>>
{
    private readonly CustomerOutletDbContext _dbContext;

    public CustomerOutletCommandHandlers(CustomerOutletDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CustomerClusterDto>> Handle(CreateCustomerClusterCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.CustomerClusters.AnyAsync(c => c.ClusterCode.ToUpper() == req.ClusterCode.ToUpper(), cancellationToken);
        if (existing)
        {
            return Result.Failure<CustomerClusterDto>(Error.Conflict("Cluster.Duplicate", $"Customer Cluster '{req.ClusterCode}' already exists."));
        }

        var cluster = new CustomerCluster
        {
            ClusterCode = req.ClusterCode.ToUpperInvariant(),
            ClusterName = req.ClusterName,
            Description = req.Description,
            IsActive = req.IsActive
        };

        _dbContext.CustomerClusters.Add(cluster);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CustomerClusterDto(cluster.Id, cluster.ClusterCode, cluster.ClusterName, cluster.Description, cluster.IsActive));
    }

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Customers.AnyAsync(c => c.CustomerCode.ToUpper() == req.CustomerCode.ToUpper(), cancellationToken);
        if (existing)
        {
            return Result.Failure<CustomerDto>(Error.Conflict("Customer.Duplicate", $"Customer '{req.CustomerCode}' already exists."));
        }

        var customer = new Customer
        {
            CustomerCode = req.CustomerCode.ToUpperInvariant(),
            CustomerName1 = req.CustomerName1,
            CustomerName2 = req.CustomerName2,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            VatNumber = req.VatNumber,
            WholesalerCode = req.WholesalerCode,
            CustomerClusterId = req.CustomerClusterId,
            IsActive = true
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CustomerDto(customer.Id, customer.CustomerCode, customer.CustomerName1, customer.CustomerName2, customer.CountryCode, customer.VatNumber, customer.IsActive, customer.CustomerClusterId));
    }

    public async Task<Result<OutletDto>> Handle(CreateOutletCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var existing = await _dbContext.Outlets.AnyAsync(o => o.OutletCode.ToUpper() == req.OutletCode.ToUpper(), cancellationToken);
        if (existing)
        {
            return Result.Failure<OutletDto>(Error.Conflict("Outlet.Duplicate", $"Outlet '{req.OutletCode}' already exists."));
        }

        var outlet = new Outlet
        {
            OutletCode = req.OutletCode.ToUpperInvariant(),
            CustomerId = req.CustomerId,
            OutletType = req.OutletType,
            AddressLine = req.AddressLine,
            City = req.City,
            PostalCode = req.PostalCode,
            CountryCode = req.CountryCode.ToUpperInvariant(),
            Latitude = req.Latitude,
            Longitude = req.Longitude,
            SalesTerritoryId = req.SalesTerritoryId,
            IsActive = true
        };

        _dbContext.Outlets.Add(outlet);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new OutletDto(outlet.Id, outlet.OutletCode, outlet.CustomerId, outlet.OutletType, outlet.AddressLine, outlet.City, outlet.PostalCode, outlet.CountryCode, outlet.Latitude, outlet.Longitude, outlet.SalesTerritoryId, outlet.IsActive));
    }

    public async Task<Result<OutletNoteDto>> Handle(AddOutletNoteCommand command, CancellationToken cancellationToken)
    {
        var outletExists = await _dbContext.Outlets.AnyAsync(o => o.Id == command.OutletId, cancellationToken);
        if (!outletExists)
        {
            return Result.Failure<OutletNoteDto>(Error.NotFound("Outlet.NotFound", $"Outlet with ID '{command.OutletId}' not found."));
        }

        var note = new OutletNote
        {
            OutletId = command.OutletId,
            RelatedAssetId = command.Request.RelatedAssetId,
            NoteBody = command.Request.NoteBody,
            IsDeleted = false
        };

        _dbContext.OutletNotes.Add(note);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new OutletNoteDto(note.Id, note.OutletId, note.RelatedAssetId, note.NoteBody, note.CreatedAtUtc));
    }
}
