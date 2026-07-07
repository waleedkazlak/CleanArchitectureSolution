using MediatR;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanSample.Application.Commands.Product;
public class UpdateProductCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
}


public class PermissionsTransformer : IClaimsTransformation
{
    Task<ClaimsPrincipal> IClaimsTransformation.TransformAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }
}