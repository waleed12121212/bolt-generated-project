using Bazingo_Core.Interfaces;
using Bazingo_Core.Models.Common;
using Bazingo_Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using Bazingo_Application.Interfaces;

namespace Bazingo_Application.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(IUnitOfWork unitOfWork, ILogger<CurrencyService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<IReadOnlyList<Currency>>> GetAllCurrenciesAsync()
        {
            try
            {
                var currencies = await _unitOfWork.Currencies.GetAllAsync();
                return ApiResponse<IReadOnlyList<Currency>>.CreateSuccess(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all currencies");
                return ApiResponse<IReadOnlyList<Currency>>.CreateError("Error retrieving currencies");
            }
        }

        public async Task<ApiResponse<Currency>> GetCurrencyByIdAsync(int id)
        {
            try
            {
                var currency = await _unitOfWork.Currencies.GetByIdAsync(id);
                if (currency == null)
                    return ApiResponse<Currency>.CreateError("Currency not found");

                return ApiResponse<Currency>.CreateSuccess(currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting currency with ID: {Id}", id);
                return ApiResponse<Currency>.CreateError("Error retrieving currency");
            }
        }

        public async Task<ApiResponse<Currency>> GetDefaultCurrencyAsync()
        {
            try
            {
                var currencies = await _unitOfWork.Currencies.GetAllAsync();
                var defaultCurrency = currencies.FirstOrDefault(c => c.IsDefault);
                
                if (defaultCurrency == null)
                    return ApiResponse<Currency>.CreateError("No default currency found");

                return ApiResponse<Currency>.CreateSuccess(defaultCurrency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting default currency");
                return ApiResponse<Currency>.CreateError("Error retrieving default currency");
            }
        }

        public async Task<ApiResponse<Currency>> UpdateCurrencyAsync(Currency currency)
        {
            try
            {
                var existingCurrency = await _unitOfWork.Currencies.GetByIdAsync(currency.Id);
                if (existingCurrency == null)
                    return ApiResponse<Currency>.CreateError("Currency not found");

                // If this currency is being set as default, unset any existing default
                if (currency.IsDefault && !existingCurrency.IsDefault)
                {
                    var currentDefault = (await _unitOfWork.Currencies.GetAllAsync())
                        .FirstOrDefault(c => c.IsDefault);
                    
                    if (currentDefault != null)
                    {
                        currentDefault.IsDefault = false;
                        await _unitOfWork.Currencies.UpdateAsync(currentDefault);
                    }
                }

                await _unitOfWork.Currencies.UpdateAsync(currency);
                await _unitOfWork.CompleteAsync();

                return ApiResponse<Currency>.CreateSuccess(currency, "Currency updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating currency with ID: {Id}", currency.Id);
                return ApiResponse<Currency>.CreateError("Error updating currency");
            }
        }
    }
}
