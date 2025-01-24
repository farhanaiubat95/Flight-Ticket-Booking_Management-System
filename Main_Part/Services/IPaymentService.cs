using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main_Part.Services
{
    public interface IPaymentService
    {
        Task<(string transactionId, bool isSuccess)> InitiatePaymentAsync(decimal amount, string callbackUrl);
        Task<bool> VerifyPaymentAsync(string transactionId);
    }
}