using System.Threading.Tasks;
using AoBSigmaker.Options;
using Stylet;

namespace AoBSigmaker.ViewModels
{
    public class OptionsViewModel : Screen
    {
        private readonly SigmakerOptions _options;

        public OptionsViewModel(SigmakerOptions options)
        {
            _options = options;
            Options = new SigmakerOptions();
            _options.CopyTo(Options);
        }

        public SigmakerOptions Options { get; set; }

        public async Task Save()
        {
            Options.CopyTo(_options);
            await _options.WriteToFile().ConfigureAwait(false);
            Execute.OnUIThread(() => RequestClose(true));
        }

        public void Cancel() => RequestClose(false);
    }
}
