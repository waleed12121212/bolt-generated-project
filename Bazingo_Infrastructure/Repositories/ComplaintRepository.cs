    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bazingo_Core.Entities.Shopping;
    using Bazingo_Core.Entities.Product;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class ComplaintRepository : BaseRepository<Complaint>, IComplaintRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<ComplaintRepository> _logger;

            public ComplaintRepository(ApplicationDbContext context, ILogger<ComplaintRepository> logger) : base(context)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Complaint> GetComplaintByIdAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.User)
                        .Include(c => c.Order)
                        .Include(c => c.Product)
                        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaint by ID: {ComplaintId}", id);
                    return null;
                }
            }

            public async Task<IReadOnlyList<Complaint>> GetComplaintsByOrderAsync(int orderId)
            {
                try
                {
                    var complaints = await _dbSet
                        .Include(c => c.User)
                        .Include(c => c.Product)
                        .Where(c => c.OrderId == orderId && !c.IsDeleted)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();
                    return complaints.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaints by order ID: {OrderId}", orderId);
                    return new List<Complaint>().AsReadOnly();
                }
            }

            public async Task<IReadOnlyList<Complaint>> GetComplaintsByUserAsync(string userId)
            {
                try
                {
                    var complaints = await _dbSet
                        .Include(c => c.Order)
                        .Include(c => c.Product)
                        .Where(c => c.UserId == userId && !c.IsDeleted)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();
                    return complaints.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaints by user ID: {UserId}", userId);
                    return new List<Complaint>().AsReadOnly();
                }
            }

            public async Task<IReadOnlyList<Complaint>> GetComplaintsByProductAsync(int productId)
            {
                try
                {
                    var complaints = await _dbSet
                        .Include(c => c.User)
                        .Include(c => c.Order)
                        .Where(c => c.ProductId == productId && !c.IsDeleted)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();
                    return complaints.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaints by product ID: {ProductId}", productId);
                    return new List<Complaint>().AsReadOnly();
                }
            }

            Task<IReadOnlyList<Complaint>> IComplaintRepository.GetComplaintsByStatusAsync(Bazingo_Core.Enums.ComplaintStatus status)
            {
                try
                {
                    return _dbSet
                        .Include(c => c.User)
                        .Include(c => c.Order)
                        .Include(c => c.Product)
                        .Where(c => c.Status == (Bazingo_Core.Entities.ComplaintStatus)status && !c.IsDeleted)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync()
                        .ContinueWith(t => (IReadOnlyList<Complaint>)t.Result.AsReadOnly());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaints by status: {Status}", status);
                    return Task.FromResult<IReadOnlyList<Complaint>>(new List<Complaint>().AsReadOnly());
                }
            }

            public async Task<Complaint> AddComplaintAsync(Complaint complaint)
            {
                try
                {
                    complaint.CreatedAt = DateTime.UtcNow;
                    complaint.Status = (Bazingo_Core.Entities.ComplaintStatus)Bazingo_Core.Enums.ComplaintStatus.Pending;
                    await AddAsync(complaint);
                    await _context.SaveChangesAsync();
                    return complaint;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding complaint");
                    return null;
                }
            }

            public async Task<IReadOnlyList<Complaint>> GetComplaintsByUserIdAsync(string userId)
            {
                try
                {
                    var complaints = await _dbSet
                        .Include(c => c.Order)
                        .Include(c => c.Product)
                        .Where(c => c.UserId == userId && !c.IsDeleted)
                        .OrderByDescending(c => c.CreatedAt)
                        .ToListAsync();
                    return complaints.AsReadOnly();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting complaints by user ID: {UserId}", userId);
                    return new List<Complaint>().AsReadOnly();
                }
            }

            public async Task UpdateComplaintStatusAsync(int id, string status)
            {
                try
                {
                    var complaint = await GetByIdAsync(id);
                    if (complaint != null && Enum.TryParse<Bazingo_Core.Enums.ComplaintStatus>(status, true, out Bazingo_Core.Enums.ComplaintStatus complaintStatus))
                    {
                        complaint.Status = (Bazingo_Core.Entities.ComplaintStatus)complaintStatus;
                        complaint.LastUpdated = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating complaint status for ID: {ComplaintId}", id);
                }
            }
        }
    }
