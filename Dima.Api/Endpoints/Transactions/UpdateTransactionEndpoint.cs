using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id}", HandleAsync)
            .WithName("Transactions: Update")
            .WithSummary("Updates a transaction")
            .WithDescription("Updates a transaction")
            .WithOrder(3)
            .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ClaimsPrincipal user, UpdateTransactionRequest request, ITransactionHandler handler, long id)
    {
        request.UserId = user.Identity?.Name ?? string.Empty;
        request.Id = id;
        
        var result = await handler.UpdateAsync(request); 
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}