using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Auction;
using static Bazingo_Core.DomainLogic.AuctionManager;

namespace Bazingo_Core.Interfaces
{
    public interface IAuctionManager
    {
        Task<AuctionDetailsDTO> GetAuctionByIdAsync(int id);
        Task<AuctionEntity> CreateAuctionAsync(AuctionDetailsDTO auctionDetails);
        Task<bool> PlaceBidAsync(BidEntity bid);
        Task<bool> EndAuctionAsync(int auctionId);
        Task<IEnumerable<AuctionEntity>> GetActiveAuctionsAsync();
        Task<IEnumerable<AuctionEntity>> GetSellerAuctionsAsync(string sellerId);
        Task<IEnumerable<AuctionEntity>> GetBidderAuctionsAsync(string bidderId);
        Task<BidEntity> GetWinningBidAsync(int auctionId);
        bool ValidateAuction(AuctionEntity auction);
    }
}
