using Bazingo_Core.Entities.Auction;
using Bazingo_Core.Enums;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Infrastructure.Repositories
{
    public class AuctionRepository : BaseRepository<AuctionEntity>, IAuctionRepository
    {
        private readonly ApplicationDbContext _context;

        public AuctionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AuctionEntity> GetAuctionByIdAsync(int id)
        {
            try
            {
                return await _context.Auctions
                    .Include(a => a.Product)
                    .Include(a => a.Seller)
                    .Include(a => a.Winner)
                    .Include(a => a.Bids)
                        .ThenInclude(b => b.Bidder)
                    .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting auction by id: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionEntity>> GetActiveAuctionsAsync()
        {
            try
            {
                return await _context.Auctions
                    .Include(a => a.Product)
                    .Include(a => a.Seller)
                    .Include(a => a.Winner)
                    .Where(a => a.Status == AuctionStatus.Active && !a.IsDeleted)
                    .OrderByDescending(a => a.EndTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting active auctions: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionEntity>> GetSellerAuctionsAsync(string sellerId)
        {
            try
            {
                return await _context.Auctions
                    .Include(a => a.Product)
                    .Include(a => a.Bids)
                    .Where(a => a.SellerId == sellerId && !a.IsDeleted)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting seller auctions: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AuctionEntity>> GetBidderAuctionsAsync(string bidderId)
        {
            try
            {
                return await _context.Auctions
                    .Include(a => a.Product)
                    .Include(a => a.Seller)
                    .Include(a => a.Bids)
                    .Where(a => a.Bids.Any(b => b.BidderId == bidderId) && !a.IsDeleted)
                    .OrderByDescending(a => a.EndTime)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting bidder auctions: {ex.Message}");
            }
        }

        public async Task<bool> PlaceBidAsync(BidEntity bid)
        {
            try
            {
                var auction = await _context.Auctions
                    .Include(a => a.Bids)
                    .FirstOrDefaultAsync(a => a.Id == bid.AuctionId && !a.IsDeleted);

                if (auction == null || auction.Status != AuctionStatus.Active)
                    return false;

                if (bid.Amount <= auction.CurrentPrice)
                    return false;

                // Add the bid
                await _context.Bids.AddAsync(bid);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error placing bid: {ex.Message}");
            }
        }

        public async Task<bool> EndAuctionAsync(int auctionId)
        {
            try
            {
                var auction = await _context.Auctions
                    .Include(a => a.Bids)
                    .FirstOrDefaultAsync(a => a.Id == auctionId && !a.IsDeleted);

                if (auction == null)
                    return false;

                auction.Status = AuctionStatus.Ended;
                
                // Set winner if there are bids
                var winningBid = auction.Bids
                    .OrderByDescending(b => b.Amount)
                    .FirstOrDefault();

                if (winningBid != null)
                {
                    auction.WinnerId = winningBid.BidderId;
                    winningBid.IsWinning = true;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error ending auction: {ex.Message}");
            }
        }
    }
}
