using System.IO;
using System.Threading.Tasks;

namespace AoBSigmaker.Options
{
    public class SigmakerOptions : Copyable<SigmakerOptions>
    {
        public const string OptionsFile = "aobsigmaker.cfg";

        private bool _trustValidity;

        public bool TrustValidity
        {
            get => _trustValidity;
            set
            {
                if (value != _trustValidity)
                {
                    _trustValidity = value;
                    NotifOfPropertyChange();
                }
            }
        }

        public async Task WriteToFile(FileInfo file) => await JsonHelper.WriteToFile(file, this).ConfigureAwait(false);
        public async Task WriteToFile(string filepath) => await JsonHelper.WriteToFile(filepath, this).ConfigureAwait(false);
        public async Task WriteToFile() => await JsonHelper.WriteToFile(OptionsFile, this).ConfigureAwait(false);

        public static async Task<SigmakerOptions> LoadFromFile(FileInfo file) => await JsonHelper.LoadFromFile<SigmakerOptions>(file).ConfigureAwait(false);
        public static async Task<SigmakerOptions> LoadFromFile(string filepath) => await JsonHelper.LoadFromFile<SigmakerOptions>(filepath).ConfigureAwait(false);
        public static async Task<SigmakerOptions> LoadFromFile() => await JsonHelper.LoadFromFile<SigmakerOptions>(OptionsFile).ConfigureAwait(false);
    }
}
