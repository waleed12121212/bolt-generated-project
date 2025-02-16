using Bazingo_Core.Entities;
using Bazingo_Core.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bazingo_Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<ApiResponse<IReadOnlyList<Currency>>> GetAllCurrenciesAsync();
        Task<ApiResponse<Currency>> GetCurrencyByIdAsync(int id);
        Task<ApiResponse<Currency>> GetDefaultCurrencyAsync();
        Task<ApiResponse<Currency>> UpdateCurrencyAsync(Currency currency);
    }
}
