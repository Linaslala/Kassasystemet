namespace Kassasystemet_refac
{
    public static class FileSystemHelper
    {
        //Läser alla rader från en fil
        //Om filen inte finns returneras
        //en tom lista istället för en exception.
        public static string[] ReadLinesIfExists(string path)
        {
            if (!File.Exists(path))
                return Array.Empty<string>();

            return File.ReadAllLines(path);
        }
    }
}
