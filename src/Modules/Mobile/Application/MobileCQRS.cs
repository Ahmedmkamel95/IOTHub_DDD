using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Mobile.Domain;
using CIOT.Modules.Mobile.Infrastructure;
using MediatR;

namespace CIOT.Modules.Mobile.Application;

public record SyncOfflineActionItem(string ClientActionId, string ActionType, string PayloadJson);
public record SyncOfflineBatchRequest(Guid TechnicianUserId, string DeviceClientSessionId, List<SyncOfflineActionItem> Actions);
public record SyncOfflineBatchResponse(Guid BatchId, int ProcessedCount, bool Success);

public record SyncOfflineBatchCommand(SyncOfflineBatchRequest Request) : ICommand<SyncOfflineBatchResponse>;

public class MobileHandlers : IRequestHandler<SyncOfflineBatchCommand, Result<SyncOfflineBatchResponse>>
{
    private readonly MobileDbContext _dbContext;

    public MobileHandlers(MobileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SyncOfflineBatchResponse>> Handle(SyncOfflineBatchCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var batch = new OfflineBatch
        {
            TechnicianUserId = req.TechnicianUserId,
            DeviceClientSessionId = req.DeviceClientSessionId,
            ActionCount = req.Actions.Count,
            Status = "Completed",
            ProcessedAtUtc = DateTime.UtcNow
        };

        foreach (var action in req.Actions)
        {
            batch.Results.Add(new OfflineActionResult
            {
                ClientActionId = action.ClientActionId,
                ActionType = action.ActionType,
                Success = true
            });
        }

        _dbContext.OfflineBatches.Add(batch);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new SyncOfflineBatchResponse(batch.Id, batch.ActionCount, true));
    }
}
