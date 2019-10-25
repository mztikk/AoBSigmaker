using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AoBSigmaker
{
    internal static class JsonHelper
    {
        public static async Task<T> LoadFromFile<T>(FileInfo file)
        {
            using (FileStream stream = file.OpenRead())
            {
                return await JsonSerializer.DeserializeAsync<T>(stream).ConfigureAwait(false);
            }
        }

        public static async Task WriteToFile<T>(FileInfo file, T Value)
        {
            using (FileStream stream = file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                await JsonSerializer.SerializeAsync(stream, Value).ConfigureAwait(false);
            }
        }
    }
}
