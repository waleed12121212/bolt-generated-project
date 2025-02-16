using Bazingo_Core.Entities.Auction;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bazingo_Infrastructure.Repositories
{
    public class BidRepository : BaseRepository<BidEntity>, IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BidEntity> GetBidByIdAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Bidder)
                .Include(b => b.Auction)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<BidEntity>> GetAllBidsAsync()
        {
            return await _dbSet
                .Include(b => b.Bidder)
                .Include(b => b.Auction)
                .ToListAsync();
        }

        public async Task<IEnumerable<BidEntity>> GetBidsByAuctionIdAsync(int auctionId)
        {
            return await _dbSet
                .Include(b => b.Bidder)
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .ToListAsync();
        }

        public async Task<IEnumerable<BidEntity>> GetBidsByBidderIdAsync(string bidderId)
        {
            return await _dbSet
                .Include(b => b.Auction)
                .Where(b => b.BidderId == bidderId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<BidEntity> GetHighestBidForAuctionAsync(int auctionId)
        {
            return await _dbSet
                .Include(b => b.Bidder)
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task<BidEntity> AddBidAsync(BidEntity bid)
        {
            await AddAsync(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task UpdateBidAsync(BidEntity bid)
        {
            await UpdateAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBidAsync(int id)
        {
            var bid = await GetBidByIdAsync(id);
            if (bid != null)
            {
                await DeleteAsync(bid);
                await _context.SaveChangesAsync();
            }
        }
    }
}
