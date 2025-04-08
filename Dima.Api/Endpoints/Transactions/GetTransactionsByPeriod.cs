using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using Transaction = System.Transactions.Transaction;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionsByPeriod : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", HandleAsync)
            .WithName("Transactions: Get all")
            .WithSummary("Returns all transactions")
            .WithDescription("Returns all transactions")
            .WithOrder(5)
            .Produces<PagedResponse<List<Transaction?>>>();

    public static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        ITransactionHandler handler,
        [FromQuery] DateTime? startDate  = null,
        [FromQuery] DateTime? endDate  = null,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetTransactionsByPeriodRequest
        {
            UserId = user.Identity?.Name ?? string.Empty,
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = startDate,
            EndDate = endDate
        };
        
        var result = await handler.GetByPeriodAsync(request); 
        return result.IsSuccess
            ? Results.Ok(result)
            : Results.BadRequest(result);
    }
}