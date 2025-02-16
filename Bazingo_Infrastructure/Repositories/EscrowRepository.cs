    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Payment;
    using Bazingo_Core.Entities.Shopping;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class EscrowRepository : BaseRepository<EscrowTransaction>, IEscrowRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<EscrowRepository> _logger;

            public EscrowRepository(ApplicationDbContext context, ILogger<EscrowRepository> logger) : base(context)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<EscrowTransaction> GetEscrowByIdAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Order)
                        .Include(e => e.Buyer)
                        .Include(e => e.Seller)
                        .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrow by ID: {EscrowId}", id);
                    return null;
                }
            }

            public async Task<IEnumerable<EscrowTransaction>> GetAllEscrowsAsync()
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Order)
                        .Include(e => e.Buyer)
                        .Include(e => e.Seller)
                        .Where(e => !e.IsDeleted)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all escrows");
                    return new List<EscrowTransaction>();
                }
            }

            public async Task<IEnumerable<EscrowTransaction>> GetEscrowsByBuyerIdAsync(string buyerId)
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Order)
                        .Include(e => e.Seller)
                        .Where(e => e.BuyerId == buyerId && !e.IsDeleted)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrows by buyer ID: {BuyerId}", buyerId);
                    return new List<EscrowTransaction>();
                }
            }

            public async Task<IEnumerable<EscrowTransaction>> GetEscrowsBySellerIdAsync(string sellerId)
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Order)
                        .Include(e => e.Buyer)
                        .Where(e => e.SellerId == sellerId && !e.IsDeleted)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrows by seller ID: {SellerId}", sellerId);
                    return new List<EscrowTransaction>();
                }
            }

            public async Task<IEnumerable<EscrowTransaction>> GetEscrowsByOrderIdAsync(int orderId)
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Buyer)
                        .Include(e => e.Seller)
                        .Where(e => e.OrderId == orderId && !e.IsDeleted)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrows by order ID: {OrderId}", orderId);
                    return new List<EscrowTransaction>();
                }
            }

            public async Task<IEnumerable<EscrowTransaction>> GetEscrowsByStatusAsync(EscrowStatus status)
            {
                try
                {
                    return await _dbSet
                        .Include(e => e.Order)
                        .Include(e => e.Buyer)
                        .Include(e => e.Seller)
                        .Where(e => e.Status == status && !e.IsDeleted)
                        .OrderByDescending(e => e.CreatedAt)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting escrows by status: {Status}", status);
                    return new List<EscrowTransaction>();
                }
            }

            public async Task AddEscrowAsync(EscrowTransaction escrow)
            {
                try
                {
                    escrow.CreatedAt = DateTime.UtcNow;
                    escrow.Status = EscrowStatus.Pending;
                    await AddAsync(escrow);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding escrow for order ID: {OrderId}", escrow.OrderId);
                }
            }

            public async Task UpdateEscrowAsync(EscrowTransaction escrow)
            {
                try
                {
                    // Update timestamps based on status change
                    if (escrow.Status == EscrowStatus.Released && !escrow.ReleasedAt.HasValue)
                    {
                        escrow.ReleasedAt = DateTime.UtcNow;
                    }
                    else if (escrow.Status == EscrowStatus.Refunded && !escrow.RefundedAt.HasValue)
                    {
                        escrow.RefundedAt = DateTime.UtcNow;
                    }

                    await UpdateAsync(escrow);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating escrow with ID: {EscrowId}", escrow.Id);
                }
            }

            public async Task DeleteEscrowAsync(int id)
            {
                try
                {
                    var escrow = await GetEscrowByIdAsync(id);
                    if (escrow != null)
                    {
                        escrow.IsDeleted = true;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting escrow with ID: {EscrowId}", id);
                }
            }
        }
    }
