using UnityEngine;
using System;
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
    public class VampiricFaces
    {
        const int faceCount = 10;
        const int minFaceIndex = 0;
        const int maxFaceIndex = faceCount - 1;

        public static readonly string Prefix = "VampiricFaces";
        public static Dictionary<string, ImageData> faceData;

        private static Texture2D LoadTexture(string name)
        {
            /*
            Texture2D tex;
            if(TextureReplacement.TryImportTextureFromLooseFiles(Path.Combine(VampiricOptions.TexturesFolder, name), true, false, false, out tex)) {
                return tex;               
            }
            */
            return VampiricOptions.mod.GetAsset<Texture2D>(name);
        }

        private static string GetOriginalFilename(int raceID, Genders gender) {
            var race = RaceTemplate.GetRaceDictionary()[raceID];
            return (gender == Genders.Male) ? race.PaperDollHeadsMale : race.PaperDollHeadsFemale;
        }

        private static string GetCustomFilename(int raceID, Genders gender, int faceIndex) {
            return Prefix+"-"+raceID+"-"+((int)gender)+"-"+faceIndex;
        }

        public static bool GetCustomHeadImageData(PlayerEntity entity, out ImageData imageDataOut) {
            imageDataOut = faceData[GetCustomFilename(entity.RaceTemplate.ID, entity.Gender, entity.FaceIndex)];
            return true;
        }

        public static void Load() {
            faceData = new Dictionary<string, ImageData>();
            int[] raceKeys = new int[]{1,2,3,4,5,6,7,8};
            //foreach (var raceKey in RaceTemplate.GetRaceDictionary().Keys) {
            foreach (var raceKey in raceKeys) {
                var race = RaceTemplate.GetRaceDictionary()[raceKey];
                foreach(Genders gender in Enum.GetValues(typeof(Genders))) {
                    string originalFilename = GetOriginalFilename(race.ID, gender);
                    for(int index = minFaceIndex;index <= maxFaceIndex; index++) {
                        string customFilename = GetCustomFilename(race.ID, gender, index);
                        ImageData imageData = ImageReader.GetImageData(originalFilename, index, 0, true, true);
                        imageData.texture = LoadTexture(customFilename);
                        faceData[customFilename] = imageData;
                    }
                }
            }
        }
    }
}
