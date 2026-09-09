using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.User;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; set; }
}
