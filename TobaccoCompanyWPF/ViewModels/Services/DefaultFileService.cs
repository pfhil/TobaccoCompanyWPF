using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.ViewModels.Services
{
    public class DefaultFileService : IFileService
    {
        public string SaveImageToPublic(string pathToImage, string? newFileName = null)
        {
            var productImagesFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"Public\ProductsImage");
            var copyFileName = Path.Combine(productImagesFolder,
                newFileName ?? Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(pathToImage));
            File.Copy(pathToImage, copyFileName, true);
            return Path.GetFileName(copyFileName);
        }
    }
}
