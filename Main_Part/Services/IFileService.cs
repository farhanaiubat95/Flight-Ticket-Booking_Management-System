using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main_Part.Services
{
    public interface IFileService
    {
        Tuple<int, string> SaveImage(IFormFile imageFile);
        public bool DeleteImage(string imageFileName);
    }
}