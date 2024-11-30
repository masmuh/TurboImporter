using System.Reflection.PortableExecutable;

namespace TurboImporter
{
    public class FileHelper
    {
        public static List<string> ReadFile(string pathFile, int totalRows, int batchSize, int skip)
        {
            List<string> fileReader = new List<string>();
            int rowMax = skip + batchSize;
            int rowCount = 0;
            using (FileStream fs = new FileStream(pathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (rowCount >= skip && rowCount < rowMax)
                    {
                        fileReader.Add(line);
                    }

                    if (rowCount > rowMax) break;
                    rowCount++;
                }
            }
            return fileReader;
        }
    }
}
