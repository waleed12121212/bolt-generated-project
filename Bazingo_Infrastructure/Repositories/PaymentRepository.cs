using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bazingo_Core.Entities.Payment;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;

namespace Bazingo_Infrastructure.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Payment> _payments;

        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _payments = context.Set<Payment>();
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByOrderAsync(int orderId)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.OrderId == orderId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentByTransactionIdAsync(string transactionId)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId && !p.IsDeleted);
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.Status == status && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && !p.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Payment> AddAsync(Payment payment)
        {
            payment.PaymentDate = DateTime.UtcNow;
            payment.UpdatedAt = DateTime.UtcNow;
            await _payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public override async Task<Payment> UpdateAsync(Payment payment)
        {
            payment.UpdatedAt = DateTime.UtcNow;
            _context.Entry(payment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return payment;
        }

        public override async Task DeleteAsync(Payment payment)
        {
            payment.IsDeleted = true;
            payment.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(payment);
        }

        public override async Task<bool> AnyAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _payments.AnyAsync(predicate);
        }

        public override async Task<int> CountAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _payments.CountAsync(predicate);
        }

        public override async Task<IReadOnlyList<Payment>> FindAsync(Expression<Func<Payment, bool>> predicate)
        {
            return await _payments.Where(predicate).ToListAsync();
        }

        public override IQueryable<Payment> GetQueryable()
        {
            return _payments.AsQueryable();
        }

        public override async Task<IReadOnlyList<Payment>> GetAllAsync()
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public override async Task<Payment> GetByIdAsync(int id)
        {
            return await _payments
                .Include(p => p.Order)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
    }
}
