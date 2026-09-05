using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.LocalAdapter.Domain;
using CIOT.Modules.LocalAdapter.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.LocalAdapter.Application;

public record DeviceProjectionEffectDto(Guid Id, Guid DeviceId, string EffectType, string Status, DateTime AppliedAtUtc);
public record ApplyDeviceEffectCommand(Guid DeviceId, string EffectType, string PayloadJson) : ICommand<DeviceProjectionEffectDto>;
public record GetDeviceEffectsQuery(Guid DeviceId) : IQuery<List<DeviceProjectionEffectDto>>;

public class LocalAdapterHandlers :
    IRequestHandler<ApplyDeviceEffectCommand, Result<DeviceProjectionEffectDto>>,
    IRequestHandler<GetDeviceEffectsQuery, Result<List<DeviceProjectionEffectDto>>>
{
    private readonly LocalAdapterDbContext _dbContext;

    public LocalAdapterHandlers(LocalAdapterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DeviceProjectionEffectDto>> Handle(ApplyDeviceEffectCommand command, CancellationToken cancellationToken)
    {
        var effect = new DeviceProjectionEffect
        {
            DeviceId = command.DeviceId,
            EffectType = command.EffectType,
            EffectPayloadJson = command.PayloadJson,
            AppliedAtUtc = DateTime.UtcNow,
            Status = "Applied"
        };

        _dbContext.DeviceProjectionEffects.Add(effect);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new DeviceProjectionEffectDto(effect.Id, effect.DeviceId, effect.EffectType, effect.Status, effect.AppliedAtUtc));
    }

    public async Task<Result<List<DeviceProjectionEffectDto>>> Handle(GetDeviceEffectsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.DeviceProjectionEffects.AsNoTracking()
            .Where(e => e.DeviceId == request.DeviceId)
            .OrderByDescending(e => e.AppliedAtUtc)
            .Select(e => new DeviceProjectionEffectDto(e.Id, e.DeviceId, e.EffectType, e.Status, e.AppliedAtUtc))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
