using System.Collections.Generic;
using System.Threading.Tasks;
using Bazingo_Core.Entities;
using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.Payment;

namespace Bazingo_Core.Interfaces
{
    public interface IEscrowRepository
    {
        Task<EscrowTransaction> GetEscrowByIdAsync(int id);
        Task<IEnumerable<EscrowTransaction>> GetAllEscrowsAsync();
        Task<IEnumerable<EscrowTransaction>> GetEscrowsByBuyerIdAsync(string buyerId);
        Task<IEnumerable<EscrowTransaction>> GetEscrowsBySellerIdAsync(string sellerId);
        Task<IEnumerable<EscrowTransaction>> GetEscrowsByOrderIdAsync(int orderId);
        Task<IEnumerable<EscrowTransaction>> GetEscrowsByStatusAsync(EscrowStatus status);
        Task AddEscrowAsync(EscrowTransaction escrow);
        Task UpdateEscrowAsync(EscrowTransaction escrow);
        Task DeleteEscrowAsync(int id);
    }
}
