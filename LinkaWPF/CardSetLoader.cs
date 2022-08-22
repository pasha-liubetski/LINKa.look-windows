using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;

namespace LinkaWPF
{
    public class CardSetLoader
    {
        public CardSetFile LoadFromFile(string filePath, string tempPath)
        {
            ZipFile.ExtractToDirectory(filePath, tempPath);

            using (var file = File.OpenText(tempPath + "\\config.json"))
            {
                var serializer = new JsonSerializer();
                return (CardSetFile)serializer.Deserialize(file, typeof(CardSetFile));
            }
        }
    }
}
