using System;
using System.Threading.Tasks;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;
using Bazingo_Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bazingo_Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        private IProductRepository _products;
        private ICategoryRepository _categories;
        private IOrderRepository _orders;
        private ICartRepository _carts;
        private IWishlistRepository _wishlists;
        private IProductReviewRepository _productReviews;
        private IComplaintRepository _complaints;
        private ICurrencyRepository _currencies;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IProductRepository Products => _products ??= new ProductRepository(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public IOrderRepository Orders => _orders ??= new OrderRepository(_context);
        public ICartRepository Carts => _carts ??= new CartRepository(_context);
        public IWishlistRepository Wishlists => _wishlists ??= new WishlistRepository(_context);
        public IProductReviewRepository ProductReviews => _productReviews ??= new ProductReviewRepository(_context);
        public IComplaintRepository Complaints => _complaints ??= new ComplaintRepository(_context);
        public ICurrencyRepository Currencies => _currencies ??= new CurrencyRepository(_context);

        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts
                throw new ApplicationException("A concurrency error occurred while saving changes.", ex);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update errors
                throw new ApplicationException("An error occurred while saving changes to the database.", ex);
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is currently in progress.");
            }

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction is currently in progress.");
            }

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }
}
