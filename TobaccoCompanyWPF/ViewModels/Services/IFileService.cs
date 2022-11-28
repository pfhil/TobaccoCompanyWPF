using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.ViewModels.Services
{
    public interface IFileService
    {
        string SaveImageToPublic(string pathToImage, string? newFileName = null);
    }
}
