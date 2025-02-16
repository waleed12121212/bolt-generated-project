using System;
using System.Threading.Tasks;
using Bazingo_Core.Entities.Payment;

namespace Bazingo_Core.DomainLogic
{
    public class EscrowManager
    {
        public bool ValidateEscrow(EscrowTransaction escrow)
        {
            if (escrow == null)
                return false;

            if (escrow.Amount <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(escrow.BuyerId) || string.IsNullOrWhiteSpace(escrow.SellerId))
                return false;

            return true;
        }

        public async Task<bool> ReleaseEscrow(EscrowTransaction escrow)
        {
            if (!ValidateEscrow(escrow))
                return false;

            if (escrow.Status != EscrowStatus.Pending)
                return false;

            // Simulate funds transfer to seller
            await Task.Delay(100); // Simulating API call

            escrow.Status = EscrowStatus.Released;
            escrow.ReleasedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<bool> RefundEscrow(EscrowTransaction escrow)
        {
            if (!ValidateEscrow(escrow))
                return false;

            if (escrow.Status != EscrowStatus.Pending)
                return false;

            // Simulate funds return to buyer
            await Task.Delay(100); // Simulating API call

            escrow.Status = EscrowStatus.Refunded;
            escrow.RefundedAt = DateTime.UtcNow;
            return true;
        }

        public bool CanRelease(EscrowTransaction escrow)
        {
            return escrow != null && 
                   escrow.Status == EscrowStatus.Pending && 
                   (DateTime.UtcNow - escrow.CreatedAt).TotalDays <= 30;
        }

        public bool CanRefund(EscrowTransaction escrow)
        {
            return escrow != null && 
                   escrow.Status == EscrowStatus.Pending && 
                   (DateTime.UtcNow - escrow.CreatedAt).TotalDays <= 30;
        }

        public bool CanDispute(EscrowTransaction escrow)
        {
            return escrow != null && 
                   escrow.Status == EscrowStatus.Pending && 
                   (DateTime.UtcNow - escrow.CreatedAt).TotalDays <= 45;
        }
    }
}
