using System;
using AoBSigmaker.AoB;
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

            base.ConfigureIoC(builder);
        }
    }
}
