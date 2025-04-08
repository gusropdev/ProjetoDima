using Dima.Api.Data;
using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                Amount = request.Amount,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Title = request.Title,
                Type = request.Type
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
        
            return new Response<Transaction?>(transaction, 201,"Transaction created");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Unable to create transaction");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transaction not found");

            transaction.UserId = request.UserId;
            transaction.CategoryId = request.CategoryId;
            transaction.CreatedAt = DateTime.Now;
            transaction.Amount = request.Amount;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Title = request.Title;
            transaction.Type = request.Type;

            context.Update(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, message: "Transaction updated successfully");

        }
        catch
        {
            return  new Response<Transaction?>(null, 500, "Unable to update transaction");
        }

    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transaction not found");
            
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
            
            return new Response<Transaction?>(transaction, message: "Transaction deleted successfully");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Unable to delete transaction");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);
            
            return transaction is null 
                ? new Response<Transaction?>(null, 404, "Transaction not found") 
                : new Response<Transaction?>(transaction, message: "Transaction found successfully");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Unable to get transaction");

        }
    }
 
    public async Task<PagedResponse<List<Transaction>>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>>(null, 500, "Unable to determine  start or end date");
        }

        try
        {
            var query = context.Transactions
                .AsNoTracking()
                .Where(x => x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate && x.UserId == request.UserId)
                .OrderBy(x => x.CreatedAt);
            
            var transactions = await query
                .Skip((request.PageNumber -1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
            var count = await query.CountAsync();
            
            return new PagedResponse<List<Transaction>>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>>(null, 500, "Unable to get transactions");
        }
    }
}