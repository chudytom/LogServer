using System.IO;

namespace LogServer.Infrastructure.Files
{
    public class FileReader
    {
        public string[] ReadAllLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}
