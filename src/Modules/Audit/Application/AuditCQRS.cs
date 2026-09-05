using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Audit.Domain;
using CIOT.Modules.Audit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Audit.Application;

public record AuditEventDto(Guid Id, Guid? UserId, string Action, string EntityType, string EntityId, DateTime CreatedAtUtc);
public record LogAuditCommand(Guid? UserId, string Action, string EntityType, string EntityId, string? ChangesJson = null) : ICommand;
public record GetAuditEventsQuery(string? EntityType = null, string? EntityId = null, int Limit = 50) : IQuery<List<AuditEventDto>>;

public class AuditHandlers :
    IRequestHandler<LogAuditCommand, Result>,
    IRequestHandler<GetAuditEventsQuery, Result<List<AuditEventDto>>>
{
    private readonly AuditDbContext _dbContext;

    public AuditHandlers(AuditDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(LogAuditCommand command, CancellationToken cancellationToken)
    {
        var audit = new AuditEvent
        {
            UserId = command.UserId,
            Action = command.Action,
            EntityType = command.EntityType,
            EntityId = command.EntityId,
            ChangesJson = command.ChangesJson,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.AuditEvents.Add(audit);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<AuditEventDto>>> Handle(GetAuditEventsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.AuditEvents.AsNoTracking().AsQueryable();
        if (!string.IsNullOrEmpty(request.EntityType)) query = query.Where(a => a.EntityType == request.EntityType);
        if (!string.IsNullOrEmpty(request.EntityId)) query = query.Where(a => a.EntityId == request.EntityId);

        var list = await query
            .OrderByDescending(a => a.CreatedAtUtc)
            .Take(request.Limit)
            .Select(a => new AuditEventDto(a.Id, a.UserId, a.Action, a.EntityType, a.EntityId, a.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
