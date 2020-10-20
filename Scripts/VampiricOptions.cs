using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using DaggerfallWorkshop;
using DaggerfallConnect;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Banking;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Serialization;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.Utility.ModSupport.ModSettings;
using DaggerfallWorkshop.Game.MagicAndEffects.MagicEffects;

namespace LGDmods
{
    public class VampiricOptions : MonoBehaviour
    {
        public static Mod mod;
        public static ModSettings mod_settings;

        public static readonly string TexturesFolder = "VampiricOptions";

        public static class Names
        {
            public static readonly string Visual = "Visual";
            public static readonly string Gameplay = "Gameplay";
            public static readonly string PortraitSwap = "PortraitSwap";
            public static readonly string PortraitAssets = "PortraitAssets";
            public static readonly string BloodlustAnxiety = "BloodlustAnxiety";
            public static readonly string DaysToBloodlust = "DaysToBloodlust";
        }

        public static class PortraitSwap
        {
            public const int Always = 0;
            public const int DuringBloodlust = 1;
            public const int Never = 2;
        }

        public static class PortraitAssets
        {
            public const int Vanilla = 0;
            public const int Custom = 1;
        }

        public static class BloodlustAnxiety
        {
            public const int Severe = 0;
            public const int Moderate = 1;
            public const int None = 2;
        }

        public static void ShowNotSatedPopup() {
            const int notSatedTextID = 36;
            DaggerfallMessageBox mb = new DaggerfallMessageBox(DaggerfallUI.Instance.UserInterfaceManager);
            mb.PreviousWindow = DaggerfallUI.Instance.UserInterfaceManager.TopWindow;
            mb.ClickAnywhereToClose = true;
            mb.SetTextTokens(notSatedTextID);
            mb.Show();
        }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            mod_settings = mod.GetSettings();
            if(mod_settings == null) {
                return;
            }
            VampiricFaces.Load();
            GameManager.Instance.EntityEffectBroker.RegisterEffectTemplate((new VampiricOptionsEffect()), true);
            UIWindowFactory.RegisterCustomUIWindow(UIWindowType.Rest, typeof(VampiricRestWindow));
            mod.IsReady = true;
        }
    }
}
