﻿using Aurora.Profiles.ETS2;
using Aurora.Profiles.ETS2.GSI;
using Aurora.Profiles.ETS2.Layers;
using System.Collections.Generic;

namespace Aurora.Profiles.ATS
{
    public class ATS : Application
    {

        // American Truck Simulator uses the same engine and telemetry DLL as Euro Truck Simulator 2, so
        // we will just use ETS2's classes and layers etc.

        public ATS() : base(new LightEventConfig()
        {
            Name = "American Truck Simulator",
            ID = "ats",
            AppID = "270880",
            ProcessNames = new[] { "amtrucks.exe" },
            SettingsType = typeof(FirstTimeApplicationSettings),
            ProfileType = typeof(ETS2Profile),
            GameStateType = typeof(GameState_ETS2),
            Event = new GameEvent_ETS2("amtrucks"),
            IconURI = "Resources/ats_64x64.png"
        })
        {

            List<LayerHandlerEntry> atsLayers = new List<LayerHandlerEntry> {
                new LayerHandlerEntry("ETS2BlinkerIndicator", "ATS Blinker", typeof(ETS2BlinkerLayerHandler)),
                new LayerHandlerEntry("ETS2Beacon", "ATS Beacon", typeof(ETS2BeaconLayerHandler))
            };

            AuroraCore.Instance.LightingStateManager.RegisterLayerHandlers(atsLayers, false);
            foreach (var layer in atsLayers)
                Config.ExtraAvailableLayers.Add(layer.Key);
        }
    }
}