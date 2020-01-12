using System;
using System.IO;
using AoBSigmaker.AoB;
using AoBSigmaker.Options;
using AoBSigmaker.ViewModels;
using RFReborn.Random;
using Stylet;
using StyletIoC;

namespace AoBSigmaker
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind<Random>().To<CryptoRandom>().InSingletonScope();
            builder.Bind<IAobGenerator>().To<AobGenerator>();
            builder.Bind<IAobShortener>().To<AobShortener>();

            // config
            SigmakerOptions options;
            if (File.Exists(SigmakerOptions.OptionsFile))
            {
                options = SigmakerOptions.LoadFromFile().GetAwaiter().GetResult();
            }
            else
            {
                options = new SigmakerOptions();
            }
            builder.Bind<SigmakerOptions>().ToInstance(options);

            base.ConfigureIoC(builder);
        }
    }
}
