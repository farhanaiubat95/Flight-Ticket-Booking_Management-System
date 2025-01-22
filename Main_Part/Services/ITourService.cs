using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main_Part.Models;

namespace Main_Part.Services
{
    public interface ITourService
    {
        Task<IEnumerable<Tours>> GetAvailableToursAsync();
        Task<bool> BookTourAsync(int tourId, string userId);
    }
}