using Bazingo_Core.Entities.Shopping;
using Bazingo_Core.Interfaces;
using Bazingo_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Infrastructure.Repositories
{
    public class CartRepository : BaseRepository<CartEntity>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<CartEntity> _cartSet;
        private readonly DbSet<CartItemEntity> _cartItemSet;

        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _cartSet = context.Set<CartEntity>();
            _cartItemSet = context.Set<CartItemEntity>();
        }

        public async Task<CartEntity> GetCartByUserIdAsync(string userId)
        {
            return await _cartSet
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);
        }

        public async Task<CartEntity> GetCartWithItemsAsync(string userId)
        {
            var cart = await _cartSet
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                cart = new CartEntity
                {
                    UserId = userId,
                    LastUpdated = DateTime.UtcNow
                };
                await _cartSet.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<CartEntity> AddAsync(CartEntity cart)
        {
            await _cartSet.AddAsync(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task<CartItemEntity> GetCartItemAsync(int cartItemId)
        {
            return await _cartItemSet
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && !ci.IsDeleted);
        }

        public async Task<IEnumerable<CartItemEntity>> GetCartItemsAsync(string userId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null) return new List<CartItemEntity>();

            return await _cartItemSet
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cart.Id && !ci.IsDeleted)
                .ToListAsync();
        }

        public async Task<CartItemEntity> AddCartItemAsync(CartItemEntity cartItem)
        {
            await _cartItemSet.AddAsync(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<bool> UpdateCartItemAsync(CartItemEntity cartItem)
        {
            try
            {
                _cartItemSet.Update(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                var cartItem = await _cartItemSet.FindAsync(cartItemId);
                if (cartItem != null)
                {
                    cartItem.IsDeleted = true;
                    _cartItemSet.Update(cartItem);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            try
            {
                var cart = await GetCartByUserIdAsync(userId);
                if (cart != null)
                {
                    var cartItems = await _cartItemSet
                        .Where(ci => ci.CartId == cart.Id && !ci.IsDeleted)
                        .ToListAsync();

                    foreach (var item in cartItems)
                    {
                        item.IsDeleted = true;
                    }

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
