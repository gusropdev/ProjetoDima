using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/{id}", HandleAsync)
            .WithName("Transaction: Delete")
            .WithSummary("Delete a transaction")
            .WithDescription("Delete a transaction")
            .WithOrder(2)
            .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, long id)
    {
        var request = new DeleteTransactionRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Id = id
        };
        
        var result = await handler.DeleteAsync(request); 
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}