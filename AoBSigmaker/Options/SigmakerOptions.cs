using System.IO;
using System.Threading.Tasks;

namespace AoBSigmaker.Options
{
    public class SigmakerOptions
    {
        public bool TrustValidity { get; set; }

        public async Task WriteToFile(FileInfo file) => await JsonHelper.WriteToFile(file, this).ConfigureAwait(false);
        public async Task WriteToFile(string filepath) => await JsonHelper.WriteToFile(filepath, this).ConfigureAwait(false);

        public static async Task<SigmakerOptions> LoadFromFile(FileInfo file) => await JsonHelper.LoadFromFile<SigmakerOptions>(file).ConfigureAwait(false);
        public static async Task<SigmakerOptions> LoadFromFile(string filepath) => await JsonHelper.LoadFromFile<SigmakerOptions>(filepath).ConfigureAwait(false);
    }
}
