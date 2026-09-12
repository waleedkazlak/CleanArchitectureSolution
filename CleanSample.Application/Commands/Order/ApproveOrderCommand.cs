using MediatR;

namespace CleanSample.Application.Commands.Order;

public record ApproveOrderCommand(long OrderId) : IRequest<bool>;
