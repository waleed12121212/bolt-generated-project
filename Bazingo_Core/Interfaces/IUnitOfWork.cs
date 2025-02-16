    using System;
    using System.Threading.Tasks;

    namespace Bazingo_Core.Interfaces
    {
        public interface IUnitOfWork : IDisposable
        {
            IProductRepository Products { get; }
            ICategoryRepository Categories { get; }
            IOrderRepository Orders { get; }
            ICartRepository Carts { get; }
            IWishlistRepository Wishlists { get; }
            IProductReviewRepository ProductReviews { get; }
            IComplaintRepository Complaints { get; }
            ICurrencyRepository Currencies { get; }

            Task<int> CompleteAsync();
            Task BeginTransactionAsync();
            Task CommitTransactionAsync();
            Task RollbackTransactionAsync();
        }
    }
