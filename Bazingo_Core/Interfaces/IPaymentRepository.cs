using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Payment;
using Bazingo_Core.Enums;
using PaymentStatus = Bazingo_Core.Entities.Payment.PaymentStatus;

namespace Bazingo_Core.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<IReadOnlyList<Payment>> GetPaymentsByOrderAsync(int orderId);
        Task<IReadOnlyList<Payment>> GetPaymentsByUserAsync(string userId);
        Task<Payment> GetPaymentByTransactionIdAsync(string transactionId);
        Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
