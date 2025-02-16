    using Bazingo_Core.Entities;
    using Bazingo_Core.Interfaces;
    using Bazingo_Infrastructure.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    namespace Bazingo_Infrastructure.Repositories
    {
        public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<CurrencyRepository> _logger;

            public CurrencyRepository(ApplicationDbContext context, ILogger<CurrencyRepository> logger) : base(context)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<Currency>> GetAllAsync()
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.PriceHistories)
                        .Where(c => !c.IsDeleted && c.IsActive)
                        .OrderBy(c => c.Code)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting all currencies");
                    return new List<Currency>();
                }
            }

            public async Task<Currency> GetByIdAsync(int id)
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.PriceHistories)
                        .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting currency with ID: {Id}", id);
                    return null;
                }
            }

            public async Task<Currency> GetByCodeAsync(string code)
            {
                try
                {
                    return await _dbSet
                        .Include(c => c.PriceHistories)
                        .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted && c.IsActive);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting currency by code: {Code}", code);
                    return null;
                }
            }

            public async Task<decimal> GetExchangeRateAsync(string fromCode, string toCode)
            {
                try
                {
                    var fromCurrency = await GetByCodeAsync(fromCode);
                    var toCurrency = await GetByCodeAsync(toCode);

                    if (fromCurrency == null || toCurrency == null)
                    {
                        _logger.LogError("One or both currencies not found: FromCode={FromCode}, ToCode={ToCode}", fromCode, toCode);
                        throw new ArgumentException("One or both currencies not found");
                    }

                    if (!fromCurrency.IsActive || !toCurrency.IsActive)
                    {
                        _logger.LogError("One or both currencies are inactive: FromCode={FromCode}, ToCode={ToCode}", fromCode, toCode);
                        throw new InvalidOperationException("One or both currencies are inactive");
                    }

                    // Convert through base rate (assuming USD is base)
                    return toCurrency.ExchangeRate / fromCurrency.ExchangeRate;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error getting exchange rate: FromCode={FromCode}, ToCode={ToCode}", fromCode, toCode);
                    throw;
                }
            }

            public async Task AddAsync(Currency currency)
            {
                try
                {
                    currency.LastUpdated = DateTime.UtcNow;
                    await base.AddAsync(currency);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding currency: {CurrencyCode}", currency.Code);
                    throw;
                }
            }

            public async Task UpdateAsync(Currency currency)
            {
                try
                {
                    currency.LastUpdated = DateTime.UtcNow;
                    await base.UpdateAsync(currency);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating currency: {CurrencyCode}", currency.Code);
                    throw;
                }
            }

            public async Task DeleteAsync(int id)
            {
                try
                {
                    var currency = await GetByIdAsync(id);
                    if (currency != null)
                    {
                        currency.IsDeleted = true;
                        currency.IsActive = false;
                        currency.LastUpdated = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting currency with ID: {CurrencyId}", id);
                    throw;
                }
            }
        }
    }
