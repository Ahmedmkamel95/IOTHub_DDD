using CIOT.Common.CQRS;
using CIOT.Common.Results;
using CIOT.Modules.Admin.Domain;
using CIOT.Modules.Admin.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CIOT.Modules.Admin.Application;

public record EquipmentModelDto(Guid Id, string Manufacturer, string Model, string MachineType, bool IsActive);
public record CreateEquipmentModelRequest(string Manufacturer, string Model, string MachineType);

public record CreateEquipmentModelCommand(CreateEquipmentModelRequest Request) : ICommand<EquipmentModelDto>;
public record GetEquipmentModelsQuery : IQuery<List<EquipmentModelDto>>;

public class AdminHandlers :
    IRequestHandler<CreateEquipmentModelCommand, Result<EquipmentModelDto>>,
    IRequestHandler<GetEquipmentModelsQuery, Result<List<EquipmentModelDto>>>
{
    private readonly AdminDbContext _dbContext;

    public AdminHandlers(AdminDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<EquipmentModelDto>> Handle(CreateEquipmentModelCommand command, CancellationToken cancellationToken)
    {
        var req = command.Request;
        var model = new EquipmentModel
        {
            Manufacturer = req.Manufacturer,
            Model = req.Model,
            MachineType = req.MachineType,
            IsActive = true
        };

        _dbContext.EquipmentModels.Add(model);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new EquipmentModelDto(model.Id, model.Manufacturer, model.Model, model.MachineType, model.IsActive));
    }

    public async Task<Result<List<EquipmentModelDto>>> Handle(GetEquipmentModelsQuery request, CancellationToken cancellationToken)
    {
        var list = await _dbContext.EquipmentModels.AsNoTracking()
            .OrderBy(m => m.Manufacturer).ThenBy(m => m.Model)
            .Select(m => new EquipmentModelDto(m.Id, m.Manufacturer, m.Model, m.MachineType, m.IsActive))
            .ToListAsync(cancellationToken);

        return Result.Success(list);
    }
}
