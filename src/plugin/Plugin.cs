using BepInEx;
using System.Security.Permissions;
using System.Security;
using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using SlugBase;
using DressMySlugcat;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace TemplateCat
{

    //-- Setting slugbase and DMS as dependencies to ensure our mod loads after them
    //-- You can set DMS as a soft dependency since the mod will still work without it, it just won't have custom graphics
    [BepInDependency("slime-cubed.slugbase")]
    [BepInDependency("dressmyslugcat", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("dressmyslugcat.templatecat", "TemplateCat", "1.0.0")]
    public partial class Plugin : BaseUnityPlugin
    {
        private void OnEnable()
        {
            On.RainWorld.PostModsInit += RainWorld_PostModsInit;
        }

        public static bool IsPostInit;
        private void RainWorld_PostModsInit(On.RainWorld.orig_PostModsInit orig, RainWorld self)
        {
            try
            {
                if (IsPostInit) return;
                IsPostInit = true;

                //-- Creating a blank slugbase slugcat, you don't need this if you are using the json
                var templateCat = SlugBaseCharacter.Create("dmstemplatecat");
                templateCat.DisplayName = "Template Cat";
                templateCat.Description = "Example of how to set up slugbase integration with DMS";

                //-- You can have the DMS sprite setup in a separate method and only call it if DMS is loaded
                //-- With this the mod will still work even if DMS isn't installed
                if (ModManager.ActiveMods.Any(mod => mod.id == "dressmyslugcat"))
                {
                    SetupDMSSprites();
                }

                Debug.Log($"Plugin dressmyslugcat.templatecat is loaded!");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void SetupDMSSprites()
        {
            //-- The ID of the spritesheet we will be using as the default sprites for our slugcat
            var sheetID = "vigaro.templatecat";

            //-- Each player slot (0, 1, 2, 3) can be customized individually
            for (int i = 0; i < 4; i++)
            {
                SpriteDefinitions.AddSlugcatDefault(new Customization()
                {
                    //-- Make sure to use the same ID as the one used for our slugcat
                    Slugcat = "dmstemplatecat",
                    PlayerNumber = i,
                    CustomSprites = new List<CustomSprite>
                    {
                        //-- You can customize which spritesheet and color each body part will use
                        new CustomSprite() { Sprite = "HEAD", SpriteSheetID = sheetID },
                        new CustomSprite() { Sprite = "FACE", SpriteSheetID = sheetID, Color = Color.red },
                        new CustomSprite() { Sprite = "BODY", SpriteSheetID = sheetID },
                        new CustomSprite() { Sprite = "ARMS", SpriteSheetID = sheetID },
                        new CustomSprite() { Sprite = "HIPS", SpriteSheetID = sheetID },
                        new CustomSprite() { Sprite = "TAIL", SpriteSheetID = sheetID }
                    },

                    //-- Customizing the tail size and color is also supported, values should be set between 0 and 1
                    CustomTail = new CustomTail()
                    {
                        Length = i / 4f,
                        Wideness = i / 4f,
                        Roundness = 0.3f
                    }
                });
            }
        }
    }
}