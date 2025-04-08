using System.ComponentModel.DataAnnotations;
using Dima.Core.Enums;

namespace Dima.Core.Requests.Transactions;

public class UpdateTransactionRequest : BaseRequest
{
    public long Id { get; set; }
    
    [Required(ErrorMessage = "The field Title is required")]
    [MaxLength(60, ErrorMessage = "The field Title must have less than 60 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "The field type is required")]
    public ETransactionType Type { get; set; }
    
    [Required(ErrorMessage = "The field amount is required")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "The field Category is required")]
    public long CategoryId { get; set; }
    
    [Required(ErrorMessage = "The field date is required")]
    public DateTime? PaidOrReceivedAt { get; set; }
}