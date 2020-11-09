using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Utility.AssetInjection;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace LGDmods
{
    public class VampiricFaces
    {
        const int faceCount = 10;
        const int minFaceIndex = 0;
        const int maxFaceIndex = faceCount - 1;

        public static readonly string Prefix = "VampiricFaces";
        public static readonly string LoosePath = Path.Combine(
                    TextureReplacement.TexturesPath,
                    VampiricOptions.TexturesFolder
                );
        public static Dictionary<string, ImageData> faceData;

        private static bool LoadImageData(int raceID, int genderID, int faceIndex, out ImageData newData) {
            string originalFilename = GetOriginalFilename(raceID, genderID);
            string customFilename = GetCustomFilename(raceID, genderID, faceIndex);
            ImageData originalData = ImageReader.GetImageData(
                                         originalFilename,
                                         faceIndex,
                                         0,
                                         true,
                                         true
                                     );
            Texture2D originalTexture2D = originalData.texture;
            //Texture2D modTexture2D = VampiricOptions.mod.GetAsset<Texture2D>(customFilename);
            newData = originalData;
            //newData.texture = modTexture2D;
            Texture2D textureOverride;
            if (DaggerfallUnity.Settings.AssetInjection)
            {
                if (TextureReplacement.TryImportTextureFromDisk(
                            Path.Combine(LoosePath, customFilename),
                            false,
                            false,
                            out textureOverride)) {
                    textureOverride.filterMode = originalTexture2D.filterMode;
                    //todo: load xml data and apply
                    newData.texture = textureOverride;
                    return true;
                }
                if (ModManager.Instance) {
                    if (ModManager.Instance.TryGetAsset(
                                customFilename,
                                null,
                                out textureOverride)) {
                        newData.texture = textureOverride;
                    }
                }
            }
            return true;
        }

        private static string GetOriginalFilename(int raceID, int genderID) {
            var race = RaceTemplate.GetRaceDictionary()[raceID];
            return ((Genders)genderID == Genders.Male) ? race.PaperDollHeadsMale : race.PaperDollHeadsFemale;
        }

        private static string GetCustomFilename(int raceID, int genderID, int faceIndex) {
            return Prefix + "-" + raceID + "-" + genderID + "-" + faceIndex;
        }

        public static bool GetImageData(PlayerEntity entity, out ImageData imageData) {
            return faceData.TryGetValue(
                       GetCustomFilename(
                           entity.RaceTemplate.ID,
                           (int)entity.Gender,
                           entity.FaceIndex
                       ),
                       out imageData
                   );
        }

        public static void Load() {
            faceData = new Dictionary<string, ImageData>();
            foreach (var raceKey in RaceTemplate.GetRaceDictionary().Keys) {
                var race = RaceTemplate.GetRaceDictionary()[raceKey];
                foreach (Genders gender in Enum.GetValues(typeof(Genders))) {
                    for (int faceIndex = minFaceIndex; faceIndex <= maxFaceIndex; faceIndex++) {
                        ImageData imageData;
                        if (LoadImageData(race.ID, (int)gender, faceIndex, out imageData)) {
                            faceData[GetCustomFilename(race.ID, (int)gender, faceIndex)] = imageData;
                        }
                    }
                }
            }
        }
    }
}
