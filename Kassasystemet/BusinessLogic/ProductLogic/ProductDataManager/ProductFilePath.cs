using System.IO;

namespace LinasKlubbLivs.BusinessLogic.ProductLogic.ProductDataManager
{
    /// <summary>
    /// Innehåller filvägar för produktdata.
    /// Läser/Skriver från projektets Data-mapp.
    /// </summary>
    internal static class ProductFilePath
    {
        private static string EnsureDataDir()
        {
            var dir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(dir); // skapar om den saknas
            return dir;
        }

        private static readonly string DataDir = EnsureDataDir();

        public static string Path => System.IO.Path.Combine(DataDir, "products.txt");
    }
}