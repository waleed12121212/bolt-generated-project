using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Auction;

namespace Bazingo_Core.Interfaces
{
    public interface IBidRepository : IBaseRepository<BidEntity>
    {
        Task<BidEntity> GetBidByIdAsync(int id);
        Task<IEnumerable<BidEntity>> GetAllBidsAsync();
        Task<IEnumerable<BidEntity>> GetBidsByAuctionIdAsync(int auctionId);
        Task<IEnumerable<BidEntity>> GetBidsByBidderIdAsync(string bidderId);
        Task<BidEntity> GetHighestBidForAuctionAsync(int auctionId);
        Task<BidEntity> AddBidAsync(BidEntity bid);
        Task UpdateBidAsync(BidEntity bid);
        Task DeleteBidAsync(int id);
    }
}
