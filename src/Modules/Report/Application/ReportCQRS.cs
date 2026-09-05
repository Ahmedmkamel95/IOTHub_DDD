using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Report.Domain;
using CIOT.Modules.Report.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Report.Application;

public record ReportDefinitionDto(Guid Id, string ReportCode, string DisplayName, string ReportType, bool IsActive);
public record CreateReportDefinitionRequest(string ReportCode, string DisplayName, string ReportType);

public record CreateReportDefinitionCommand(CreateReportDefinitionRequest Request) : ICommand<ReportDefinitionDto>;
public record GetReportDefinitionsQuery : IQuery<List<ReportDefinitionDto>>;

public class ReportHandlers :
    IRequestHandler<CreateReportDefinitionCommand, Result<ReportDefinitionDto>>,
    IRequestHandler<GetReportDefinitionsQuery, Result<List<ReportDefinitionDto>>>
{
    private readonly ReportDbContext _dbContext;

    public ReportHandlers(ReportDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ReportDefinitionDto>> Handle(CreateReportDefinitionCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var r = new ReportDefinition
        {
            ReportCode = req.ReportCode.ToUpperInvariant(),
            DisplayName = req.DisplayName,
            ReportType = req.ReportType,
            IsActive = true
        };

        _dbContext.ReportDefinitions.Add(r);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new ReportDefinitionDto(r.Id, r.ReportCode, r.DisplayName, r.ReportType, r.IsActive));
    }

    public async Task<Result<List<ReportDefinitionDto>>> Handle(GetReportDefinitionsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.ReportDefinitions.AsNoTracking()
            .OrderBy(r => r.ReportCode)
            .Select(r => new ReportDefinitionDto(r.Id, r.ReportCode, r.DisplayName, r.ReportType, r.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
