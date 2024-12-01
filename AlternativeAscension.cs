using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HarmonyLib.Tools;
using NeedyXML;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using UniRx;
using UnityEngine;
using NeedyEnums;
using Stream = NeedyXML.Stream;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Reflection.Emit;
using Thread = NeedyXML.Thread;
using Component = UnityEngine.Component;
using TMPro;
using UnityEngine.AddressableAssets;
using ngov3.Effect;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine.Rendering;
using Extensions;
using UnityEngine.Audio;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AlternativeAscension
{
    [BepInPlugin(id, modName, ver)]
    [HarmonyDebug]
    public class AltAscMod : BaseUnityPlugin
    {
        public const string id = "nso.altascension";
        public const string modName = "Needy Streamer Overload: Alternative Ascension";
        public const string ver = "1.0.0";
        public const string assetdec = "NSOAltAsc";
        public static string FILEPATH;

        public const int SLEEPS_BEFORE_SLEEPY = 6;
        public const bool SHOWANIMSTREAM = false;

        private static readonly Harmony harmony = new Harmony(id);
        public static ManualLogSource log;
        public static ModData DATA;

        private static void FillXMLData()
        {
            TextAsset stringXML = Addressables.LoadAssetAsync<TextAsset>("AltAsc_strings.xml").WaitForCompletion();


            string asmname = Assembly.GetAssembly(typeof(AltAscMod)).GetName().Name + ".dll";
            FILEPATH = Assembly.GetAssembly(typeof(AltAscMod)).Location.Replace(asmname, "");

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(ModData));
            using (StringReader sr = new StringReader(stringXML.text))
            {
                DATA = (ModData)serializer.Deserialize(sr);
            }
        }

        private void SetCatalogPath()
        {
            Assembly asm = this.GetType().Assembly;
            string catalogPath = new Uri(asm.CodeBase).LocalPath.Replace(asm.GetName().Name + ".dll", "catalog.json");
            string assetPath = new Uri(asm.CodeBase).LocalPath.Replace(asm.GetName().Name + ".dll", "altasc_assets.bundle");

            bool generateCatalog = true;
            if (File.Exists(catalogPath))
            {
                try
                {
                    AsyncOperationHandle<IResourceLocator> loadCatalogOperation = Addressables.LoadContentCatalogAsync(catalogPath);
                    loadCatalogOperation.WaitForCompletion();
                    if (loadCatalogOperation.Result == null) throw new Exception("Failed to load catalog!");

                    generateCatalog = false;
                }
                catch (Exception e)
                {
                    log.LogMessage("Couldn't find mod addressable catalog, generating...");
                }
            }
            if (generateCatalog)
            {
                AltAscMod.log.LogMessage("Fresh install, setting asset path!");

                string catalogInternalAddress = Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList().Find(s => s.Contains("catalog.json"));
                string catalogData;
                using (StreamReader sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(catalogInternalAddress)))
                {
                    catalogData = sr.ReadToEndAsync().GetAwaiter().GetResult();
                    catalogData = catalogData.Replace("[NMO_ASSETS_DIRECTORY]", assetPath).Replace(@"\",@"/");
                    log.LogMessage(catalogPath);
                }

                using (StreamWriter sw = new StreamWriter(catalogPath))
                {
                    sw.Write(catalogData);
                }
                Addressables.LoadContentCatalogAsync(catalogPath).WaitForCompletion();
                log.LogMessage("Path is " + catalogPath);
            }
            AltAscMod.log.LogMessage("Loaded catalog!");
        }

        private async void Awake()
        {
            // Plugin startup logic
            HarmonyFileLog.Enabled = true;
            log = Logger;
            SetCatalogPath();
            try
            {
                FillXMLData();
                harmony.PatchAll();
                //foreach (MethodBase patch in harmony.GetPatchedMethods())
                //{
                //    log.LogMessage($"{patch.DeclaringType}.{patch.Name}");
                //}

                AltAscMod.log.LogMessage($"Patched {harmony.GetPatchedMethods().Count()} methods...");

                log.LogMessage("Patched NSO, press to continue...");
                //Console.ReadKey();
            }
            catch (Exception e)
            {
                log.LogMessage("Failed to patch NSO, press to quit");
                log.LogWarning(e);
                Console.ReadKey();
                System.Environment.Exit(0);
            }
        }
    }
}

