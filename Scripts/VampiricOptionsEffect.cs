using UnityEngine;
using System.Collections;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.MagicAndEffects.MagicEffects;

namespace LGDmods
{
    public class VampiricOptionsEffect : VampirismEffect
    {
        public bool isBloodlust() {
            switch (VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Gameplay, VampiricOptions.Names.BloodlustAnxiety)) {
                case VampiricOptions.BloodlustAnxiety.None:
                    return false;
            }
            int daysToBloodlust = VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Gameplay, VampiricOptions.Names.DaysToBloodlust);
            if (DaggerfallUnity.Instance.WorldTime.DaggerfallDateTime.ToClassicDaggerfallTime() - getLastTimeFed() > (DaggerfallDateTime.MinutesPerDay * daysToBloodlust)) {
                return true;
            }
            return false;
        }

        public override bool GetCustomHeadImageData(PlayerEntity entity, out ImageData imageDataOut)
        {
            switch ((VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Visual, VampiricOptions.Names.PortraitSwap))) {
                case VampiricOptions.PortraitSwap.Always:
                    break;
                case VampiricOptions.PortraitSwap.DuringBloodlust:
                    if (!isBloodlust()) {
                        imageDataOut = new ImageData();
                        return false;
                    }
                    break;
                case VampiricOptions.PortraitSwap.Never:
                    imageDataOut = new ImageData();
                    return false;
            }
            switch ((VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Visual, VampiricOptions.Names.PortraitAssets))) {
                case VampiricOptions.PortraitAssets.Custom:
                    return VampiricFaces.GetImageData(entity, out imageDataOut);
            }
            return base.GetCustomHeadImageData(entity, out imageDataOut);
        }

        public override bool CheckStartRest(PlayerEntity playerEntity)
        {
            switch (VampiricOptions.mod_settings.GetValue<int>(VampiricOptions.Names.Gameplay, VampiricOptions.Names.BloodlustAnxiety)) {
                case VampiricOptions.BloodlustAnxiety.Severe:
                    if (isBloodlust()) {
                        VampiricOptions.ShowNotSatedPopup();
                        return false;
                    }
                    break;
            }
            return true;
        }

        public uint getLastTimeFed() {
            CustomSaveData_v1 data = (CustomSaveData_v1)GetSaveData();
            return data.lastTimeFed;
        }
    }
}
