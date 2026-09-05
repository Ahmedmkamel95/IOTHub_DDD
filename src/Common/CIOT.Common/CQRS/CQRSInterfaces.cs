using CIOT.Common.Results;
using MediatR;

namespace CIOT.Common.CQRS;

public interface ICommand : IRequest<Result>;
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
