using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Mobile.Domain.Entities;
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
            BatchId = req.DeviceClientSessionId,
            RequestedByUserId = req.TechnicianUserId,
            ActionCount = req.Actions.Count,
            CompletedCount = req.Actions.Count,
            Status = "Completed",
            CorrelationId = req.DeviceClientSessionId,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            CompletedAtUtc = DateTimeOffset.UtcNow,
            ResultExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        _dbContext.OfflineBatches.Add(batch);

        foreach (var action in req.Actions)
        {
            _dbContext.OfflineActionResults.Add(new OfflineActionResult
            {
                FirstBatchId = batch.Id,
                RequestedByUserId = req.TechnicianUserId,
                ClientActionId = action.ClientActionId,
                ActionType = action.ActionType,
                PayloadHash = action.PayloadJson,
                Status = "Completed",
                ProcessedAtUtc = DateTimeOffset.UtcNow
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new SyncOfflineBatchResponse(batch.Id, batch.ActionCount, true));
    }
}
