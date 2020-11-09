using UnityEngine;
using System.Collections;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
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

        public const int notSatedTextID = 36;

        public static void ShowNotSatedPopup() {
            DaggerfallMessageBox mb = new DaggerfallMessageBox(DaggerfallUI.Instance.UserInterfaceManager);
            mb.PreviousWindow = DaggerfallUI.Instance.UserInterfaceManager.TopWindow;
            mb.ClickAnywhereToClose = true;
            mb.SetTextTokens(notSatedTextID);
            mb.Show();
        }

        protected static bool PreventRestCondition() {
            if (DaggerfallUI.Instance.UserInterfaceManager.TopWindow is VampiricRestWindow) {
                VampiricRestWindow vrw = (VampiricRestWindow)DaggerfallUI.Instance.UserInterfaceManager.TopWindow;
                if (vrw.IsResting()) {
                    switch (VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Gameplay, VampiricOptions.Names.BloodlustAnxiety)) {
                        case VampiricOptions.BloodlustAnxiety.Moderate:
                            RacialOverrideEffect racialEffect = GameManager.Instance.PlayerEffectManager.GetRacialOverrideEffect();
                            if (racialEffect is VampiricOptionsEffect) {
                                VampiricOptionsEffect vampiricEffect = (VampiricOptionsEffect)racialEffect;
                                if (vampiricEffect.isBloodlust()) {
                                    return true;
                                }
                            }
                            break;
                    }
                }
            }
            return false;
        }

        [Invoke(StateManager.StateTypes.Start, 0)]
        public static void Init(InitParams initParams)
        {
            mod = initParams.Mod;
            mod_settings = mod.GetSettings();

            if (mod_settings == null) {
                return;
            }

            VampiricFaces.Load();

            GameManager.Instance.EntityEffectBroker.RegisterEffectTemplate((new VampiricOptionsEffect()), true);
            UIWindowFactory.RegisterCustomUIWindow(UIWindowType.Rest, typeof(VampiricRestWindow));
            GameManager.Instance.RegisterPreventRestCondition(
                PreventRestCondition,
                DaggerfallUnity.Instance.TextProvider.GetText(notSatedTextID)
            );

            mod.IsReady = true;
        }
    }
}
