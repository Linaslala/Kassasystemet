using System;
using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager
{
    /// <summary>
    /// Innehåller filvägar för produktdata.
    /// </summary>
    internal static class ProductFilePath
    {
        private static readonly string BaseDir =
            System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "LinasKlubbLivs"
            );

        public static string Path =>
            System.IO.Path.Combine(BaseDir, "products.txt");
    }
}