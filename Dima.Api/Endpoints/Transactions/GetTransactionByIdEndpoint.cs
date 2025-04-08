using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Transactions: Get By Id")
            .WithSummary("Returns a transaction")
            .WithDescription("Return a transaction")
            .WithOrder(4)
            .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, long id)
    {
        var request = new GetTransactionByIdRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Id = id
        };
    
        var result = await handler.GetByIdAsync(request); 
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}