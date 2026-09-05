using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Integration.Domain;
using CIOT.Modules.Integration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Integration.Application;

public record PartnerSourceDto(Guid Id, string SourceCode, string DisplayName, string IntegrationType, bool IsActive);
public record CreatePartnerSourceRequest(string SourceCode, string DisplayName, string IntegrationType = "REST", string? EndpointUrl = null);

public record CreatePartnerSourceCommand(CreatePartnerSourceRequest Request) : ICommand<PartnerSourceDto>;
public record GetPartnerSourcesQuery : IQuery<List<PartnerSourceDto>>;

public class IntegrationHandlers :
    IRequestHandler<CreatePartnerSourceCommand, Result<PartnerSourceDto>>,
    IRequestHandler<GetPartnerSourcesQuery, Result<List<PartnerSourceDto>>>
{
    private readonly IntegrationDbContext _dbContext;

    public IntegrationHandlers(IntegrationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PartnerSourceDto>> Handle(CreatePartnerSourceCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var source = new PartnerSource
        {
            SourceCode = req.SourceCode.ToUpperInvariant(),
            DisplayName = req.DisplayName,
            IntegrationType = req.IntegrationType,
            EndpointUrl = req.EndpointUrl,
            IsActive = true
        };

        _dbContext.PartnerSources.Add(source);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new PartnerSourceDto(source.Id, source.SourceCode, source.DisplayName, source.IntegrationType, source.IsActive));
    }

    public async Task<Result<List<PartnerSourceDto>>> Handle(GetPartnerSourcesQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.PartnerSources.AsNoTracking()
            .OrderBy(p => p.SourceCode)
            .Select(p => new PartnerSourceDto(p.Id, p.SourceCode, p.DisplayName, p.IntegrationType, p.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
