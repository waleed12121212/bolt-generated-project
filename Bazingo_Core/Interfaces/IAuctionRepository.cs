using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Auction;

namespace Bazingo_Core.Interfaces
{
    public interface IAuctionRepository : IBaseRepository<AuctionEntity>
    {
        Task<AuctionEntity> GetAuctionByIdAsync(int id);
        Task<IEnumerable<AuctionEntity>> GetActiveAuctionsAsync();
        Task<IEnumerable<AuctionEntity>> GetSellerAuctionsAsync(string sellerId);
        Task<IEnumerable<AuctionEntity>> GetBidderAuctionsAsync(string bidderId);
        Task<bool> PlaceBidAsync(BidEntity bid);
        Task<bool> EndAuctionAsync(int auctionId);
    }
}
