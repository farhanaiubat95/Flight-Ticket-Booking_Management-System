using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main_Part.Services
{
    public class FakePaymentService : IPaymentService
    {
        public async Task<(string transactionId, bool isSuccess)> InitiatePaymentAsync(decimal amount, string callbackUrl)
        {
            // Simulate payment initiation
            string transactionId = Guid.NewGuid().ToString();
            return await Task.FromResult((transactionId, true));
        }

        public async Task<bool> VerifyPaymentAsync(string transactionId)
        {
            // Simulate payment verification
            return await Task.FromResult(true); // Assume payment is successful for testing
        }
    }
}