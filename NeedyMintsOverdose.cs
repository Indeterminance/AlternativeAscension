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
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UniRx;
using UnityEngine;
using NeedyEnums;
using Stream = NeedyXML.Stream;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Diagnostics;
using Thread = NeedyXML.Thread;
using Component = UnityEngine.Component;
using UnityEngine.UI;
using System.Xml.Linq;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using static NeedyMintsOverdose.Alternates;
using ngov3.Effect;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.PlayerLoop;
using System.Security.Cryptography.X509Certificates;
using Extensions;
using Rewired.UI.ControlMapper;
using static ES3Spreadsheet;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.Networking.UnityWebRequest;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Audio;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace NeedyMintsOverdose
{
    [BepInPlugin(id, modName, ver)]
    [HarmonyDebug]
    public class NeedyMintsMod : BaseUnityPlugin
    {
        public const string id = "nso.needymintsoverdose";
        public const string modName = "Needy Mints Overdose";
        public const string ver = "0.0.1";
        public const string assetdec = "NeedyMintsOD";
        public static string FILEPATH;

        public const int SLEEPS_TO_SLEEPY = 7;

        private static readonly Harmony harmony = new Harmony(id);
        public static ManualLogSource log;
        public static ModData DATA;

        private static void FillXMLData()
        {
            string asmname = Assembly.GetAssembly(typeof(NeedyMintsMod)).GetName().Name + ".dll";
            FILEPATH = Assembly.GetAssembly(typeof(NeedyMintsMod)).Location.Replace(asmname, "");

            System.Xml.Serialization.XmlSerializer serializer = new XmlSerializer(typeof(ModData));
            using (StreamReader sr = new StreamReader(FILEPATH + "Strings.xml"))
            {
                DATA = (ModData)serializer.Deserialize(sr);
            }
        }

        private void SetCatalogPath()
        {
            Assembly asm = this.GetType().Assembly;
            string catalogPath = new Uri(asm.CodeBase).LocalPath.Replace(asm.GetName().Name + ".dll", "catalog.json");
            string assetPath = new Uri(asm.CodeBase).LocalPath.Replace(asm.GetName().Name + ".dll", "nmo_assets.bundle");
            string catalogData;
            using (StreamReader sr = new StreamReader(catalogPath))
            {
                catalogData = sr.ReadToEndAsync().GetAwaiter().GetResult();
                if (!catalogData.Contains("nmo_assets.bundle") && catalogData.Contains("\"m_InternalIds\":[\"Assets/"))
                {
                    NeedyMintsMod.log.LogMessage("Fresh install, setting asset path!");
                    catalogData = catalogData.Replace("\"m_InternalIds\":[\"Assets/", $"\"m_InternalIds\":[\"{assetPath.Replace("\\", "/")}\",\"Assets/");
                }
            }

            using (StreamWriter sw = new StreamWriter(catalogPath))
            {
                sw.Write(catalogData);
            }

            Addressables.LoadContentCatalogAsync(catalogPath).WaitForCompletion();
            log.LogMessage("Path is " + catalogPath);
        }

        private async void Awake()
        {
            FillXMLData();

            // Plugin startup logic
            HarmonyFileLog.Enabled = true;
            log = Logger;
            try
            {
                SetCatalogPath();
                harmony.PatchAll();
                foreach (MethodBase patch in harmony.GetPatchedMethods())
                {
                    log.LogMessage($"{patch.DeclaringType}.{patch.Name}");
                }

                log.LogMessage("Patched NSO, press to continue...");
                Console.ReadKey();
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
     
    internal partial class MintyOverdosePatches
    {
        [HarmonyPatch(typeof(StatusManager))]
        public static class StatusManagerPatches
        {
            [HarmonyPatch(nameof(StatusManager.NewStatus))]
            [HarmonyPostfix]
            public static void NewStatusPrefix(StatusManager __instance)
            {
                __instance.statuses.Add(new Status(ModdedStatusType.SleepyAmeCounter.Swap(), 0, 30, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeCounter.Swap(), 0, 99, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeStressMultiplier.Swap(), 0, 11, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.FollowerPlotFlag.Swap(), 0, 20, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeCountdown.Swap(), 2, 2, 0, false));
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(StatusManager.Diffs))]
            public static void DiffsPrefix(CmdMaster.Param cmd, ref StatusManager __instance, ref List<StatusDiff> __result)
            {
                NeedyMintsMod.log.LogMessage($"Diffing!");
                NeedyMintsMod.log.LogMessage($"Getting diff for {cmd.Id}");
                int stressMult = __instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap());
                if (stressMult > 0 && (cmd.Id.StartsWith("Odekake")))
                {
                    int index = __result.FindIndex(diff => diff.statusType == StatusType.Stress);
                    StatusDiff sd = __result[index];
                    //NeedyMintsMod.log.LogMessage($"Updating odekake stress diff from {sd.delta} (original) to {sd.delta*(1-stressMult)} (with Follower stress)");
                    sd.delta = sd.delta * (1 - stressMult);
                    __result[index] = sd;
                }
            }


            [HarmonyPrefix]
            [HarmonyPatch(nameof(StatusManager.UpdateStatusMaxToNumber))]
            public static void UpdateStatusMaxToNumberPrefix(ref StatusManager __instance, StatusType type, int to)
            {
                if (type != StatusType.DayIndex) return;

                // Get new maximum
                int oldMax = __instance.statuses.Find(s => s.statusType == type).maxValue.Value;
                int destMax = Mathf.Max(30, to);


                NeedyMintsMod.log.LogMessage($"Updating day max from {oldMax} to {to}");
                if (oldMax == destMax) return;

                // Get info / traversal info
                DayPassing2D dayPassing2D = GameObject.Find("DayPassingCover").GetComponent<DayPassing2D>();
                MethodInfo dayPassMethod = typeof(DayPassing2D).GetMethod(nameof(DayPassing2D.dayPass), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                Traverse tmpTextLabels = new Traverse(dayPassing2D).Field(nameof(DayPassing2D._tmpFader)).Field(nameof(TMPTextsFader._tmpTexts));
                Traverse CalendarScroll = new Traverse(dayPassing2D).Field(nameof(DayPassing2D.CalendarScroll));
                Traverse SpriteFader = new Traverse(dayPassing2D).Field(nameof(DayPassing2D._spriteFader)).Field(nameof(SpriteRenderersFader._renderers));

                // Get day 
                ReactiveProperty<int> dayIndex = SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayIndex);

                // Get data from traversals
                List<TMP_Text> textRenderers = tmpTextLabels.GetValue<List<TMP_Text>>();
                RectTransform Calendar = CalendarScroll.GetValue<RectTransform>();

                // Remove old subscriptions
                dayIndex.Where((int d) => true).Take(1);

                // Get data for adding new days
                Vector3 offset = Calendar.GetChild(1).transform.position - Calendar.GetChild(0).transform.position;
                Transform dayPrefab = Calendar.GetChild(30);
                Vector3 dayPos = dayPrefab.position;


                // TODO: Calendar is broken since we're trying to add arrows, pls fix ame will be proud of you AND saves will work properly!!!!
                // And then you can work on the login stuff and that'll be cool too!!!!! You're almost there!!!!!!!!!!
                int arrowIndex = textRenderers.FindIndex(t => t.text == "30") + 1;
                TMP_Text arrowPrefab = textRenderers[arrowIndex];
                Vector3 arrowPos = arrowPrefab.transform.position;


                NeedyMintsMod.log.LogMessage($"Calendar children: {Calendar.childCount}");

                if (to > oldMax)
                {
                    // Create new labels
                    for (int i = oldMax+1; i < to+1; i++)
                    {
                        Transform newDayObj = UnityEngine.Object.Instantiate(dayPrefab, dayPos + offset * (i - 30), Quaternion.identity, Calendar);
                        //TMP_Text newArrow = UnityEngine.Object.Instantiate(arrowPrefab, arrowPos + offset * (i - 30), Quaternion.identity, Calendar);
                        newDayObj.name = $"Day {i}";
                        NeedyMintsMod.log.LogMessage($"newDayObj childcount: {newDayObj.transform.childCount}");
                        //Transform newArrowObj = UnityEngine.Object.Instantiate(arrowsPrefab, arrowsPos + offset * (i - 30), Quaternion.identity, Calendar);

                        // Add day text to opacity
                        TextMeshPro dayText = newDayObj.GetComponent<TextMeshPro>();
                        dayText.text = i.ToString();
                        textRenderers.Add(dayText);
                        
                        // Add progression arrow
                        TextMeshPro arrow = newDayObj.GetChild(0).GetComponent<TextMeshPro>();
                        textRenderers.Add(arrow);
                    }
                }

                tmpTextLabels.SetValue(textRenderers);

                // Today prevention is to make sure DayPassing doesn't rerender if we change
                // the maximum during an event (for example, during Angel Watch)
                int today = __instance.statuses.Find(s => s.statusType == type).currentValue.Value;
                dayIndex.Where((int d) => d != today).Subscribe(delegate (int t)
                {
                    //if (t > SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex)) return;
                    dayPassMethod.Invoke(dayPassing2D, new object[] { dayIndex.Value, 0, 0 });
                    //nonRefDayPass2D.dayPass(dayIndex.Value, 0, 0);
                }).AddTo(dayPassing2D.gameObject);
            }
        }

        [HarmonyPatch(typeof(Action_SleepToTomorrow3))]
        public static class SleepToTomorrowPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Action_SleepToTomorrow3.startEvent))]
            public static void SleepToTomorrowPostfix()
            {
                StatusManager evMan = SingletonMonoBehaviour<StatusManager>.Instance;
                evMan.UpdateStatus(ModdedStatusType.SleepyAmeCounter.Swap(), 1);
                int sleepCount = evMan.GetStatus(ModdedStatusType.SleepyAmeCounter.Swap());
                NeedyMintsMod.log.LogMessage($"Slept {sleepCount} time(s)!");
                if (sleepCount >= NeedyMintsMod.SLEEPS_TO_SLEEPY)
                {
                    SingletonMonoBehaviour<EventManager>.Instance.nowEnding = (EndingType)ModdedEndingType.Ending_Sleepy;
                    SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<Ending_Sleepy>();


                    Queue<NgoEvent> eventQueue = SingletonMonoBehaviour<EventManager>.Instance.eventQueue;
                }
            }
        }

        [HarmonyPatch(typeof(NetaManager))]
        public static class NetaManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(NetaManager.fetchNextActionHint))]
            public static void fetchNextActionPrune(ref List<Tuple<ActionType, AlphaLevel>> __result)
            {
                int sleeps = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.SleepyAmeCounter.Swap());
                for (int i = 0; i < __result.Count; i++)
                {
                    // Remove "Sleep To Tomorrow" hints when a "Sleepy" ending is about to trigger
                    if (__result[i].Item1 == ActionType.SleepToTomorrow && sleeps + 1 >= NeedyMintsMod.SLEEPS_TO_SLEEPY)
                    {
                        NeedyMintsMod.log.LogMessage($"Removing hint event!");
                        __result.RemoveAt(i);
                        i--;
                    }
                }
                if (CheckTokyoAvailable())
                {
                    AlphaLevel alpha = new AlphaLevel((AlphaType)(int)ModdedAlphaType.FollowerAlpha, 1);
                    __result.Add(Tuple.Create((ActionType)(int)ModdedActionType.OdekakeTokyo, alpha));
                }
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) >= (int)FollowerPlotFlagValues.PostDepaz)
                {
                    __result.RemoveAll(t => t.Item1 == ActionType.Internet2ch || t.Item1 == (ActionType)(int)ModdedActionType.Internet2chStalk);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NetaManager.Haishined))]
            public static void HaishinedPrefix(AlphaType alpha, int level)
            {
                NeedyMintsMod.log.LogMessage($"Haishined {alpha} level {level}");
            }
        }

        [HarmonyPatch(typeof(JineDataConverter))]
        public static class JineDataConverterPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(JineDataConverter.getJineRawList))]
            public static void getJineRawListPrefix(out bool __state, ref List<LineMaster.Param> ___JineRawList)
            {
                __state = ___JineRawList == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(JineDataConverter.getJineRawList))]
            public static void getJineRawListPostfix(bool __state, ref List<LineMaster.Param> ___JineRawList, ref List<LineMaster.Param> __result)
            {
                if (!__state) return;
                foreach (Jine jine in NeedyMintsMod.DATA.Jines.Jine)
                {
                    LineMaster.Param newParam = new LineMaster.Param()
                    {
                        BodyEn = jine.BodyEN,
                        BodyCn = "",
                        BodyFr = "",
                        BodyGe = "",
                        BodyIt = "",
                        BodyJp = "[Not translated]",
                        BodyKo = "",
                        BodyRu = "",
                        BodySp = "",
                        BodyTw = "",
                        BodyVn = "",
                        ParentId = jine.Id,
                        Id = jine.Id,
                        Speaker = jine.Speaker,
                        ArgumentType = "N/A",
                        ImageId = "N/A"
                    };
                    NeedyMintsMod.log.LogMessage($"Adding jine {newParam.Id} to jines");
                    __result.Add(newParam);
                }
                ___JineRawList = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(JineDataConverter.getJineFromTypeId))]
            public static bool getJineFromTypeIdPrefix(JineType t)
            {
                return !CheckModdedPrefix(typeof(JineType),typeof(ModdedJineType),t).Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(JineDataConverter.getJineFromTypeId))]
            public static void getJineFromTypeIdPostfix(JineType t, ref LineMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(JineType), typeof(ModdedJineType), t).Item1) return;

                ModdedJineType modT = (ModdedJineType)(int)t;
                __result =  JineDataConverter.getJineRawList().Find((LineMaster.Param j) => j.Id == modT.ToString());
            }
        }

        
        [HarmonyPatch(typeof(EndingDialog))]
        public static class EndingDialogPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(EndingDialog.OnLanguageChanged))]
            public static void OnLanguageChangedPostfix(ref EndingDialog __instance, ref UnityEngine.UI.Button ____submitButton, EndingType ___end)
            {
                if (!CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), ___end).Item1) return;

                if ((ModdedEndingType)(int)___end == ModdedEndingType.Ending_Followers)
                {

                    ____submitButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType((SystemTextType)(int)ModdedSystemTextType.System_InternetYamero, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }
        }

        [HarmonyPatch(typeof(NgoEx))]
        public static class NgoExPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.ReasonTextFromEDType))]
            public static bool ReasonTextFromEDTypePrefix(EndingType type, LanguageType lang, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), type);
                __state = tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.ReasonTextFromEDType))]
            public static void ReasonTextFromEDTypePostfix(ModdedEndingType __state, ref string __result)
            {
                NeedyMintsMod.log.LogMessage($"End: {__state}");
                if (__state == 0) return;

                // Get modded reason
                switch (__state)
                {
                    case ModdedEndingType.Ending_Sleepy:
                        __result = NeedyMintsMod.DATA.Endings.Ending.Find(e => e.Id == "Ending_Sleepy").Osimai;
                        break;
                    case ModdedEndingType.Ending_Followers:
                        __result = NeedyMintsMod.DATA.Endings.Ending.Find(e => e.Id == "Ending_Followers").Osimai;
                        break;
                    default: return;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.CmdFromType))]
            public static bool CmdFromTypePrefix(CmdType type, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(CmdType), typeof(ModdedCmdType), type);

                string v;
                if (tuple.Item1) v = ((ModdedCmdType)tuple.Item2).ToString();
                else
                {
                    //NeedyMintsMod.log.LogMessage($"Found nat cmdtype {(CmdType)tuple.Item2} {type}");
                    v = ((CmdType)tuple.Item2).ToString();
                }
                NeedyMintsMod.log.LogMessage($"{v} modded? {tuple} {type}");
                
                
                __state = tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.CmdFromType))]
            public static void CmdFromTypePostfix(int __state, ref CmdMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(CmdType), typeof(ModdedCmdType), __state).Item1) return;

                switch ((ModdedCmdType)__state)
                {
                    case ModdedCmdType.OdekakeTokyo:
                        __result = ModdedCommandParams.OdekakeTokyoParam;
                        break;
                    case ModdedCmdType.OkusuriDaypassStalk:
                        __result = ModdedCommandParams.OkusuriDaypassStalkParam;
                        break;
                    case ModdedCmdType.HaishinComiket:
                        __result = ModdedCommandParams.HaishinComiketParam;
                        break;
                    case ModdedCmdType.OdekakePanic1:
                        __result = ModdedCommandParams.OdekakePanic1Param;
                        break;
                    case ModdedCmdType.OdekakePanic2:
                        __result = ModdedCommandParams.OdekakePanic2Param;
                        break;
                    case ModdedCmdType.OdekakePanic3:
                        __result = ModdedCommandParams.OdekakePanic3Param;
                        break;
                    case ModdedCmdType.OdekakePanic4:
                        __result = ModdedCommandParams.OdekakePanic4Param;
                        break;
                    case ModdedCmdType.OdekakeBreak:
                        __result = ModdedCommandParams.OdekakeBreakParam;
                        break;
                    case ModdedCmdType.Internet2chStalk:
                        __result = ModdedCommandParams.Internet2chStalkParam;
                        break;
                    case ModdedCmdType.HaishinAngelWatch:
                        __result = ModdedCommandParams.HaishinAngelWatch;
                        break;
                    case ModdedCmdType.HaishinAngelDeath:
                        __result = ModdedCommandParams.HaishinAngelDeath;
                        break;
                    default:
                        NeedyMintsMod.log.LogMessage("Invalid modded cmd type!");
                        break;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.ActFromType))]
            public static bool ActfromTypePrefix(ActionType type, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), type);
                __state = tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.ActFromType))]
            public static void ActFromTypePostfix(ref ActMaster.Param __result, int __state)
            {
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), __state).Item1) return;
                ModdedActionType type = (ModdedActionType)__state;
                switch (type)
                {
                    case ModdedActionType.OdekakeTokyo:
                        __result = ModdedActionParams.OdekakeTokyoParam;
                        break;
                    case ModdedActionType.OdekakePanic1:
                        __result = ModdedActionParams.OdekakePanic1Param;
                        break;
                    case ModdedActionType.OdekakePanic2:
                        __result = ModdedActionParams.OdekakePanic2Param;
                        break;
                    case ModdedActionType.OdekakePanic3:
                        __result = ModdedActionParams.OdekakePanic3Param;
                        break;
                    case ModdedActionType.OdekakePanic4:
                        __result = ModdedActionParams.OdekakePanic4Param;
                        break;
                    case ModdedActionType.OdekakeBreak:
                        __result = ModdedActionParams.OdekakeBreakParam;
                        break;
                    default:
                        break;
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.CmdName), new Type[] { typeof(CmdType), typeof(LanguageType) })]
            public static bool CmdNamePrefix(CmdType type)
            {
                return !CheckModdedPrefix(typeof(CmdType), typeof(ModdedCmdType), type).Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.CmdName),new Type[] { typeof(CmdType), typeof(LanguageType)})]
            public static void CmdNamePostfix(CmdType type, LanguageType lang, ref string __result)
            {
                if (!CheckModdedPrefix(typeof(CmdType), typeof(ModdedCmdType), type).Item1) return;

                ModdedCmdType modType = (ModdedCmdType)(int)type;
                CmdMaster.Param param = NgoEx.getCmds().FirstOrDefault((CmdMaster.Param x) => x.Id == modType.ToString());
                switch (lang)
                {
                    case LanguageType.JP:
                        __result = param.LabelJp;
                        return;
                    case LanguageType.CN:
                        __result = param.LabelCn;
                        return;
                    case LanguageType.KO:
                        __result = param.LabelKo;
                        return;
                    case LanguageType.TW:
                        __result = param.LabelTw;
                        return;
                    case LanguageType.VN:
                        __result = param.LabelVn;
                        return;
                    case LanguageType.FR:
                        __result = param.LabelFr;
                        return;
                    case LanguageType.IT:
                        __result = param.LabelIt;
                        return;
                    case LanguageType.GE:
                        __result = param.LabelGe;
                        return;
                    case LanguageType.SP:
                        __result = param.LabelSp;
                        return;
                    case LanguageType.RU:
                        __result = param.LabelRu;
                        return;
                }
                __result = param.LabelEn;
                NeedyMintsMod.log.LogMessage($"Outputting {__result} from {Environment.StackTrace}");
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getCmds))]
            public static void getCmdsPrefix(out bool __state, List<CmdMaster.Param> ___Cmds)
            {
                //NeedyMintsMod.log.LogMessage($"{Environment.StackTrace}");
                __state = ___Cmds == null;
            }


            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getCmds))]
            public static void getCmdsPostfix(bool __state, ref List<CmdMaster.Param> __result, ref List<CmdMaster.Param> ___Cmds)
            {
                if (!__state) return;

                __result.Add(ModdedCommandParams.OdekakeTokyoParam);
                __result.Add(ModdedCommandParams.OdekakePanic1Param);
                __result.Add(ModdedCommandParams.OdekakePanic2Param);
                __result.Add(ModdedCommandParams.OdekakePanic3Param);
                __result.Add(ModdedCommandParams.OdekakePanic4Param);
                __result.Add(ModdedCommandParams.OdekakeBreakParam);
                __result.Add(ModdedCommandParams.OkusuriDaypassStalkParam);
                __result.Add(ModdedCommandParams.HaishinComiketParam);
                __result.Add(ModdedCommandParams.HaishinAngelWatch);
                __result.Add(ModdedCommandParams.HaishinAngelDeath);
                __result.Add(ModdedCommandParams.HaishinAngelFuneral);
                __result.Add(ModdedCommandParams.Internet2chStalkParam);

                ___Cmds = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getTenComments))]
            public static void getTenCommentsPrefix(out bool __state, List<TenCommentMaster.Param> ___TenComments)
            {
                __state = ___TenComments == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getTenComments))]
            public static void getTenCommentsPostfix(bool __state, ref List<TenCommentMaster.Param> __result, ref List<TenCommentMaster.Param> ___TenComments)
            {
                if (!__state) return;

                foreach (Stream stream in NeedyMintsMod.DATA.Streams.Stream)
                {
                    foreach (Speak spk in stream.Dialogue.Speak)
                    {
                        if (!__result.Any(t => t.Id == spk.Id))
                        {
                            TenCommentMaster.Param newParam = new TenCommentMaster.Param()
                            {
                                BodyEn = spk.BodyEN,
                                Id = stream.AlphaType+stream.AlphaLevel+"_"+spk.Id
                            };
                            __result.Add(newParam);
                        }
                    }

                    if (!__result.Any(t => t.Id == stream.AlphaType+stream.AlphaLevel+"_STREAMNAME"))
                    {
                        TenCommentMaster.Param newParam = new TenCommentMaster.Param()
                        {
                            BodyEn = stream.Name,
                            Id = stream.AlphaType + stream.AlphaLevel + "_STREAMNAME"
                        };
                        __result.Add(newParam);
                    }
                }
                ___TenComments = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getKusoComments))]
            public static void getKusoCommentsPrefix(out bool __state, List<KusoCommentMaster.Param> ___Kusokomes)
            {
                __state = ___Kusokomes == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getKusoComments))]
            public static void getKusoCommentsPostfix(bool __state, ref List<KusoCommentMaster.Param> __result, ref List<KusoCommentMaster.Param> ___Kusokomes)
            {
                if (!__state) return;

                foreach (Stream stream in NeedyMintsMod.DATA.Streams.Stream)
                {
                    foreach (Msg msg in stream.Chat.Msg)
                    {
                        if (!__result.Any(t => t.Id == msg.Id))
                        {
                            KusoCommentMaster.Param newParam = new KusoCommentMaster.Param()
                            {
                                BodyEn = msg.BodyEN,
                                Id = stream.AlphaType + stream.AlphaLevel + "_" + msg.Id
                            };
                            //NeedyMintsMod.log.LogMessage($"Added {newParam.Id} to kusos");
                            __result.Add(newParam);
                        }
                    }
                }
                ___Kusokomes = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getKitunes))]
            public static void getKitunesPrefix(out bool __state, List<KituneMaster.Param> ___Kitunes)
            {
                __state = ___Kitunes == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getKitunes))]
            public static void getKitunesPostfix(bool __state, ref List<KituneMaster.Param> ___Kitunes, ref List<KituneMaster.Param> __result)
            {
                if (!__state) return;
                foreach (Thread thread in NeedyMintsMod.DATA.ST.Threads)
                {
                    foreach (Comment com in thread.Comments.Comment)
                    {
                        if (!__result.Any(t => t.Id == com.Id))
                        {
                            Regex rx = new Regex(@"\d*$");
                            int res = int.Parse(rx.Match(com.Id).Value);


                            KituneMaster.Param newParam = new KituneMaster.Param()
                            {
                                BodyEn = com.BodyEN,
                                FollowerRank = thread.Id,
                                Id = "Kitune_" + thread.Id + "_" + com.Id,
                                ResNumber = res
                            };
                            //NeedyMintsMod.log.LogMessage($"Added {newParam.Id} to kusos");
                            __result.Add(newParam);
                        }
                    }
                }
                ___Kitunes = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getSuretais))]
            public static void getSuretaisPrefix(out bool __state, List<KituneSuretaiMaster.Param> ___Suretais)
            {
                __state = ___Suretais == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getSuretais))]
            public static void getSuretaisPostfix(bool __state, ref List<KituneSuretaiMaster.Param> ___Suretais, ref List<KituneSuretaiMaster.Param> __result)
            {
                if (!__state) return;
                foreach (Thread thread in NeedyMintsMod.DATA.ST.Threads)
                {

                    KituneSuretaiMaster.Param newParam = new KituneSuretaiMaster.Param()
                    {
                        Id = "Suretai_" + thread.Id,
                        BodyEn = thread.Title.BodyEN
                    };
                    __result.Add(newParam);
                }
                ___Suretais = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.getSystemTexts))]
            public static void getSystemTextsPrefix(out bool __state, List<SystemTextMaster.Param> ___systemTexts)
            {
                __state = ___systemTexts == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getSystemTexts))]
            public static void getSystemTextsPostfix(bool __state, ref List<SystemTextMaster.Param> ___systemTexts, ref List<SystemTextMaster.Param> __result)
            {
                if (!__state) return;

                NeedyMintsMod.log.LogMessage($"TESTING: {NeedyMintsMod.DATA.ST.Threads}");
                foreach (Thread thread in NeedyMintsMod.DATA.ST.Threads)
                {

                    SystemTextMaster.Param newParam = new SystemTextMaster.Param()
                    {
                        Id = "Suretai_" + thread.Id,
                        BodyEn = thread.Title.BodyEN
                    };
                    NeedyMintsMod.log.LogMessage($"Adding new system type suretai {newParam.Id}");
                    __result.Add(newParam);
                }

                foreach (NeedyXML.String str in NeedyMintsMod.DATA.SysStrings.Strings)
                {
                    SystemTextMaster.Param newParam = new SystemTextMaster.Param()
                    {
                        Id = str.Id,
                        BodyEn = str.BodyEN
                    };
                    NeedyMintsMod.log.LogMessage($"Adding new system type {newParam.Id}");
                    __result.Add(newParam);
                }
                ___systemTexts = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.SystemTextFromTypeString))]
            public static void SystemTypeFromTextStringPrefix(string type)
            {
                NeedyMintsMod.log.LogMessage($"Attempting to get system type {type}");
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.SystemTextFromType))]
            public static bool SystemTextFromTypePrefix(SystemTextType type, out bool __state)
            {
                __state = CheckModdedPrefix(typeof(SystemTextType), typeof(ModdedSystemTextType), type).Item1;
                return !__state;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.SystemTextFromType))]
            public static void SystemTextFromTypePostfix(SystemTextType type, LanguageType lang,  bool __state, ref string __result)
            {
                if (!__state) return;

                ModdedSystemTextType modType = (ModdedSystemTextType)(int)type;
                SystemTextMaster.Param param = NgoEx.getSystemTexts().FirstOrDefault((SystemTextMaster.Param x) => x.Id == modType.ToString());
                switch (lang)
                {
                    case LanguageType.JP:
                        __result = param.BodyJp;
                        return;
                    case LanguageType.CN:
                        __result = param.BodyCn;
                        return;
                    case LanguageType.KO:
                        __result = param.BodyKo;
                        return;
                    case LanguageType.TW:
                        __result = param.BodyTw;
                        return;
                    case LanguageType.VN:
                        __result = param.BodyVn;
                        return;
                    case LanguageType.FR:
                        __result = param.BodyFr;
                        return;
                    case LanguageType.IT:
                        __result = param.BodyIt;
                        return;
                    case LanguageType.GE:
                        __result = param.BodyGe;
                        return;
                    case LanguageType.SP:
                        __result = param.BodySp;
                        return;
                    case LanguageType.RU:
                        __result = param.BodyRu;
                        return;
                }
                __result = param.BodyEn;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.TenTalk), new Type[] { typeof(TenCommentType), typeof(LanguageType) })]
            public static bool TenTalkPrefix(TenCommentType type, LanguageType lang, ref string __result)
            {
                if (!CheckModdedPrefix(typeof(TenCommentType), typeof(ModdedTenCommentType), type).Item1) return true;


                string id = ((ModdedTenCommentType)(int)type).ToString();

                NeedyMintsMod.log.LogMessage(id);
                if (id == "None")
                {
                    __result = "";
                    return false;
                }
                switch (lang)
                {
                    case LanguageType.JP:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyJP;
                        return false;
                    case LanguageType.CN:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyCn;
                        return false;
                    case LanguageType.KO:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyKo;
                        return false;
                    case LanguageType.TW:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyTw;
                        return false;
                    case LanguageType.VN:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyVn;
                        return false;
                    case LanguageType.FR:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyFr;
                        return false;
                    case LanguageType.IT:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyIt;
                        return false;
                    case LanguageType.GE:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyGe;
                        return false;
                    case LanguageType.SP:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodySp;
                        return false;
                    case LanguageType.RU:
                        __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyRu;
                        return false;
                }
                __result = NgoEx.getTenComments().FirstOrDefault((TenCommentMaster.Param x) => x.Id == id).BodyEn;
                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.TenTalk), new Type[] { typeof(string), typeof(LanguageType)})]
            public static void TenTalk2Prefix(string id, LanguageType lang)
            {
                NeedyMintsMod.log.LogMessage($"Tried to get {id}");
            }
        }

        [HarmonyPatch(typeof(EndingManager))]
        public static class EndingManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(EndingManager.GetEndingName))]
            public static bool GetEndingNamePre(EndingType end, out ModdedEndingType __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), end);
                __state = (ModdedEndingType)(int)tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EndingManager.GetEndingName))]
            public static void GetEndingNamePost(EndingType end, ref string __result)
            {
                Tuple<bool, int> data = CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), end);
                if (!data.Item1) return;
                __result = NeedyMintsMod.DATA.GetEndingByID(((ModdedEndingType)(int)end).ToString()).Name;
            }
        }

        [HarmonyPatch(typeof(ngov3.Odekake))]
        public static class OdekakePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Odekake.Awake))]
            public static void AwakePostfix(Odekake __instance)
            {
                FieldInfo actionTypeField = typeof(ActionButton).GetField(nameof(ActionButton.actionType), BindingFlags.Instance | BindingFlags.NonPublic);
                FieldInfo field = typeof(Odekake).GetField(nameof(Odekake._odekakeButtonObjs), BindingFlags.Instance | BindingFlags.NonPublic);
                List<GameObject> buttons = field.GetValue(__instance) as List<GameObject>;


                if (CheckTokyoAvailable())
                {
                    GameObject obj = __instance.SelectableObjects.First();
                    GameObject OdekakeTokyo = obj as GameObject;
                    ActionButton actionButton = OdekakeTokyo.GetComponent<ActionButton>();
                    actionTypeField.SetValue(actionButton, (int)ModdedActionType.OdekakeTokyo);
                    OdekakeTokyo.transform.position = new Vector3(0.65f, -2.5f, 100f);
                    buttons.Add(OdekakeTokyo);
                }

                ActionType replaceOdekakeType = ActionType.None;
                switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap())) {
                    case 0: return;
                    case 1:
                    case 2:
                        replaceOdekakeType = ModdedActionType.OdekakePanic1.Swap();
                        break;
                    case 3:
                    case 4:
                        replaceOdekakeType = ModdedActionType.OdekakePanic2.Swap();
                        break;
                    case 5:
                    case 6:
                        replaceOdekakeType = ModdedActionType.OdekakePanic3.Swap();
                        break;
                    default:
                        replaceOdekakeType = ModdedActionType.OdekakePanic4.Swap();
                        break;
                }
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.StalkReveal &&
                    SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 1)
                {
                    replaceOdekakeType = (ActionType)(int)ModdedActionType.OdekakeBreak;
                }
                NeedyMintsMod.log.LogMessage($"Found odekake replacement of {replaceOdekakeType}");
                if (replaceOdekakeType == ActionType.None) return;
                foreach (GameObject button in buttons)
                {
                    actionTypeField.SetValue(button.GetComponent<ActionButton>(), replaceOdekakeType);
                }
                field.SetValue(__instance, buttons);
            }

        }

        [HarmonyPatch(typeof(CommandHelper))]
        public static class CommandHelperPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(CommandHelper.GetCommandTitle), new Type[] { typeof(ActionType), typeof(LanguageType) })]
            public static bool GetCommandTitlePrefix(ActionType type, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), type);
                __state = (int)tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(CommandHelper.GetCommandTitle), new Type[] { typeof(ActionType), typeof(LanguageType) })]
            public static void GetCommandTitlePostfix(int __state, ref string __result)
            {
                //NeedyMintsMod.log.LogMessage(__state);
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), __state).Item1) return;

                NeedyXML.Command com = NeedyMintsMod.DATA.Commands.Command.Find(c => c.Id == ((ModdedActionType)__state).ToString());
                __result = com.Name.BodyEN;
            }
        }

        [HarmonyPatch(typeof(CommandManager))]
        public static class CommandManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(CommandManager.ChooseCommand), new Type[] { typeof(ActionType) })]
            public static bool ChooseCommandPrefix(ActionType a, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), a);
                __state = (int)tuple.Item2;



                return !tuple.Item1 || a == ActionType.Haishin || a == ActionType.OkusuriDaypassModerate || a == ActionType.Internet2ch;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(CommandManager.ChooseCommand), new Type[] { typeof(ActionType) })]
            public static void ChooseCommandPost(ActionType a, int __state, ref CmdType __result)
            {
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), __state).Item1 && a != ActionType.Haishin && a != ActionType.OkusuriDaypassModerate && a != ActionType.Internet2ch) return;
                
                if ((ActionType)__state == ActionType.Haishin)
                {
                    NeedyMintsMod.log.LogMessage($"Found modded haishin!");

                    ModdedAlphaType alphaType = (ModdedAlphaType)(int)SingletonMonoBehaviour<EventManager>.Instance.alpha;
                    int level = SingletonMonoBehaviour<EventManager>.Instance.alphaLevel;
                    if (alphaType == ModdedAlphaType.FollowerAlpha && level == 1)
                    {
                        __result = (CmdType)(int)ModdedCmdType.HaishinComiket;
                    }
                    else if (alphaType == ModdedAlphaType.FollowerAlpha && level == 2)
                    {
                        __result = (CmdType)(int)ModdedCmdType.HaishinAngelWatch;
                    }
                    else if (alphaType == ModdedAlphaType.FollowerAlpha && level == 3)
                    {
                        __result = (CmdType)(int)ModdedCmdType.HaishinAngelDeath;
                    }
                    return;
                }

                else if ((ActionType)__state == ActionType.OkusuriDaypassModerate)
                {
                    NeedyMintsMod.log.LogMessage($"Took a moderate amount of depaz!");
                    bool usedNaturalHabitat = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.PostComiket;
                    bool isNight = SingletonMonoBehaviour<StatusManager>.Instance.isNight();
                    NeedyMintsMod.log.LogMessage($"Starting Depaz rec dose event with flags {usedNaturalHabitat} {isNight}");
                    if (usedNaturalHabitat && isNight)
                    {
                        __result = (CmdType)(int)ModdedCmdType.OkusuriDaypassStalk;
                    }
                    return;
                }

                else if ((ActionType)__state == ActionType.Internet2ch)
                {
                    NeedyMintsMod.log.LogMessage($"Found 2ch!");
                    int goOutBreak = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap());
                    int plotFlag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                    bool stressBreak = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.Stress) > 100;
                    if (goOutBreak > 2 && plotFlag == (int)FollowerPlotFlagValues.PostDepaz && stressBreak)
                    {
                        __result = (CmdType)(int)ModdedCmdType.Internet2chStalk;
                    }
                    return;
                }

                switch ((ModdedActionType)__state)
                {
                    case ModdedActionType.OdekakeTokyo:
                        __result = (CmdType)(int)ModdedCmdType.OdekakeTokyo;
                        break;
                    case ModdedActionType.OkusuriDaypassStalk:
                        __result = (CmdType)(int)ModdedCmdType.OkusuriDaypassStalk;
                        break;
                    case ModdedActionType.OdekakePanic1:
                        __result = (CmdType)(int)ModdedCmdType.OdekakePanic1;
                        break;
                    case ModdedActionType.OdekakePanic2:
                        __result = (CmdType)(int)ModdedCmdType.OdekakePanic2;
                        break;
                    case ModdedActionType.OdekakePanic3:
                        __result = (CmdType)(int)ModdedCmdType.OdekakePanic3;
                        break;
                    case ModdedActionType.OdekakePanic4:
                        __result = (CmdType)(int)ModdedCmdType.OdekakePanic4;
                        break;
                    case ModdedActionType.OdekakeBreak:
                        __result = (CmdType)(int)ModdedCmdType.OdekakeBreak;
                        break;
                    case ModdedActionType.Internet2chStalk:
                        __result = (CmdType)(int)ModdedCmdType.Internet2chStalk;
                        break;
                    default:
                        __result = CmdType.Error;
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(ActionButton))]
        public static class ActionButtonPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(ActionButton.Awake))]
            public static bool AwakePrefix(ActionButton __instance, out int __state)
            {
                //NeedyMintsMod.log.LogMessage($"Parent of button is {__instance.transform.parent.name}");

                FieldInfo fieldInfo = typeof(ActionButton).GetField(nameof(ActionButton.actionType), BindingFlags.NonPublic | BindingFlags.Instance);
                int actionVal = (int)fieldInfo.GetValue(__instance);

                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), actionVal);
                __state = (int)tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(ActionButton.Awake))]
            public static void AwakePostfix(ActionButton __instance, int __state)
            {
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), __state).Item1) return;

                FieldInfo dayField = typeof(ActionButton).GetField(nameof(ActionButton._dayIndex), BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo dayPart = typeof(ActionButton).GetField(nameof(ActionButton._dayPart), BindingFlags.NonPublic | BindingFlags.Instance);
                FieldInfo actionType = typeof(ActionButton).GetField(nameof(ActionButton.actionType), BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo setLabel = typeof(ActionButton).GetMethod(nameof(ActionButton.SetLabel), BindingFlags.Instance | BindingFlags.NonPublic);
                MethodInfo setStatus = typeof(ActionButton).GetMethod(nameof(ActionButton.SetStatus), BindingFlags.Instance | BindingFlags.NonPublic);

                dayField.SetValue(__instance, SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayIndex));
                dayPart.SetValue(__instance, SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart));
                setLabel.Invoke(__instance,null);

                List<ModdedActionType> newActions = new List<ModdedActionType>()
                {
                    ModdedActionType.OdekakeTokyo,
                    ModdedActionType.OdekakePanic1,
                    ModdedActionType.OdekakePanic2,
                    ModdedActionType.OdekakePanic3,
                    ModdedActionType.OdekakePanic4,
                    ModdedActionType.OdekakeBreak,
                    ModdedActionType.Internet2chStalk,
                };

                SingletonMonoBehaviour<CommandManager>.Instance.commandStatus.Subscribe(delegate (Dictionary<ActionType, ActionStatus> s)
                {
                    foreach (ModdedActionType action in  newActions)
                    {
                        if (!s.ContainsKey(action.Swap()))
                        {
                            s.Add(action.Swap(), ActionStatus.Executable);
                        }
                    }
                    setStatus.Invoke(__instance, new object[] { s[(ActionType)actionType.GetValue(__instance)] });
                }).AddTo(__instance);
                (dayPart.GetValue(__instance) as ReactiveProperty<int>).Subscribe(delegate (int _)
                {
                    setStatus.Invoke(__instance, new object[] {ActionStatus.Executable});
                }).AddTo(__instance);
            }
        }

        [HarmonyPatch(typeof(TooltipManager))]
        public static class TooltipManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(TooltipManager.ShowAction))]
            public static void ShowActionPrefix(ActionType a)
            {
                NeedyMintsMod.log.LogMessage($"Showing tooltip for {a}");
            }
        }

        [HarmonyPatch(typeof(EventManager))]
        public static class EventManagerPatches
        {
            /*[HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.ExecuteActionConfirmed))]

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.ExecuteActionConfirmed))]
            public static void ExecuteActionConfirmed(ActionType ac, CmdType a, ref EventManager __instance)
            {
                NeedyMintsMod.log.LogMessage($"Confirming action {ac} | cmd {a}");
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), ac).Item1) return;

                switch ((ModdedActionType)(int)ac)
                {
                    case ModdedActionType.ODEKAKE_TOKYO:
                        __instance.AddEventQueue<Action_OdekakeTokyo>();
                        break;
                    default:
                        break;
                }
            }*/

            /*[HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.AddEventQueue),new Type[] { typeof(string)})]
            public static bool AddEventQueuePrefix(string EventName, ref EventManager __instance)
            {

                Regex rx = new Regex(@"(^.*?_|.*$)");
                Regex rx2 = new Regex(@"_.*");
                string[] list = rx.Matches(EventName).Cast<Match>().Select(m => m.Value).ToArray();
                //NeedyMintsMod.log.LogMessage($"Queueing prefix {list} from {EventName}");

                int actionInt;
                bool modded = int.TryParse(rx2.Replace(list[1], ""), out actionInt);
                if (!modded) return true;
                object ac = (ModdedCmdType)actionInt;
                int dmp;
                if (int.TryParse(ac.ToString(), out dmp)) ac = (ModdedAlphaType)ac;

                string typeStr;
                if (ac.GetType() == typeof(ModdedAlphaType))
                {
                    int tier = int.Parse(rx2.Match(list[1]).Value.Replace("_","")) + 1;

                    typeStr = "NeedyMintsOverdose." + list[0] + ac.ToString() + "_" + tier.ToString() ;
                }
                else typeStr = "NeedyMintsOverdose." + list[0] + ac.ToString();
                NeedyMintsMod.log.LogMessage(typeStr);
                Type type = Type.GetType(typeStr);
                NgoEvent ev = Activator.CreateInstance(type) as NgoEvent;
                __instance.eventQueue.Enqueue(ev);
                return false;

            }*/

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchNightEvent))]
            public static bool FetchNightEventPrefix(ref EventManager __instance)
            {
                NeedyMintsMod.log.LogMessage("Fetching night...");
                int status = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                int status2 = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());

                if (!__instance.isHorror && status2 == (int)FollowerPlotFlagValues.OdekakeBreak)
                {
                    SingletonMonoBehaviour<StatusManager>.Instance.timePassingToNextMorning();
                    return false;
                }

                if (status == 16 && status2 == (int)FollowerPlotFlagValues.VisitedComiket &&
                    !(__instance.isWristCut && __instance.beforeWristCut) &&
                    !(__instance.isHakkyo && SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.Stress) == 100))
                {
                    __instance.AddEvent<Scenario_PostComiket>();
                    return false;
                }
                if (status2 == (int)FollowerPlotFlagValues.AngelWatch)
                {
                    return false;
                }
                if (status2 == (int)FollowerPlotFlagValues.AngelDeath)
                {
                    __instance.AddEvent<Scenario_follower_day3_night>();
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchDayEvent))]
            public static bool FetchDayEventPrefix(ref EventManager __instance)
            {
                NeedyMintsMod.log.LogMessage("DayEvent");
                int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                int plotflag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                if (!__instance.isHorror && day == 27 && SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower) >= 500000 &&
                    SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 0)
                {
                    __instance.AddEvent<Event_MV_kowai>();
                    return false;
                }
                else if (plotflag == (int)FollowerPlotFlagValues.VisitedComiket && SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex) == 16)
                {
                    __instance.AddEvent<Event_ComiketAlert>();
                    return false;
                }
                else if (!__instance.isHorror && plotflag == (int)FollowerPlotFlagValues.OdekakeBreak)
                {
                    __instance.AddEvent<Scenario_follower_day1_day>();
                    return false;
                }
                else if (!__instance.isHorror && plotflag == (int)FollowerPlotFlagValues.AngelWatch)
                {
                    if (SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.Broadcast) == null)
                    {
                        __instance.AddEvent<Scenario_follower_day2_AfterAllNighterhaishin>();
                    }
                    return false;
                }
                else if (plotflag == (int)FollowerPlotFlagValues.BadPassword)
                {
                    __instance.AddEvent<Scenario_follower_day3_day>();
                    return false;
                }
                else if (plotflag == (int)FollowerPlotFlagValues.AngelFuneral)
                {
                    __instance.AddEvent<Scenario_follower_day4_day>();
                    return false;
                }
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.ApplyStatus))]
            public static void ApplyStatusPostfix(CmdType a)
            {
                if (!(a.ToString().StartsWith("Odekake") || ((ModdedCmdType)(int)a).ToString().StartsWith("Odekake")) || SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) < 1) return;
                SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(ModdedStatusType.OdekakeStressMultiplier.Swap(), 1);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.SetShortcutState))]
            public static void SetShortcutStatePostfix(bool isEnable, float disabledAlpha)
            {
                NeedyMintsMod.log.LogMessage($"Set shortcut state to {isEnable} with alpha {disabledAlpha}");
                //if (isEnable) NeedyMintsMod.log.LogMessage(Environment.StackTrace);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchDialog))]
            public static bool FetchDialogPrefix()
            {
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelWatch)
                {
                    return false;
                }
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.Awake))]
            public static void AwakePostfix(ref EventManager __instance)
            {
                __instance.executingAction = CmdType.None;

                __instance.transform.AddComponent<NeedyMintsModManager>();
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.Start))]
            public static bool StartPrefix(ref EventManager __instance)
            {
                EventManager inst = __instance;

                UniTask.Delay(20, false, PlayerLoopTiming.Update, default(CancellationToken), false);
                (from d in SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayIndex)
                 where d < SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex) + 1
                 select d).DistinctUntilChanged<int>().Subscribe(async delegate (int day)
                 {
                     SingletonMonoBehaviour<CursorManager>.Instance.DisableLiveCursorMode();
                     if (SingletonMonoBehaviour<Settings>.Instance.isBackToLoad)
                     {
                         SingletonMonoBehaviour<Settings>.Instance.isBackToLoad = false;
                     }
                     else
                     {
                         inst.Save(day);
                     }


                     (new Traverse(inst).Method(nameof(EventManager.AwakePsycheDiary))).GetValue();
                     (new Traverse(inst).Method(nameof(EventManager.AwakeLoveDiary))).GetValue();
                     inst.AddEventQueue<Event_CheckBGM>();
                     await inst.FetchDayEvent();
                 }).AddTo(inst.gameObject);
                (from part in SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart).DistinctUntilChanged<int>()
                 where part == 1
                 select part).Subscribe(delegate (int _)
                 {
                     (new Traverse(inst).Method(nameof(EventManager.FetchUzagarami))).GetValue();
                 }).AddTo(inst.gameObject);
                (from part in SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart).DistinctUntilChanged<int>()
                 where part == 2
                 select part).Subscribe(delegate (int _)
                 {
                     inst.FetchNightEvent();
                 }).AddTo(inst.gameObject);
                SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart).DistinctUntilChanged<int>().Subscribe(delegate (int _)
                {
                    inst.fetchNextActionHint();
                })
                    .AddTo(inst.gameObject);
                return false;
            }

        }

        [HarmonyPatch(typeof(TweetFetcher))]
        public static class TweetFetcherPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(TweetFetcher.getTweetRawList))]
            public static void getTweetRawListPostfix(ref List<TweetMaster.Param> __result)
            {
                FieldInfo fieldInfo = typeof(TweetFetcher).GetField(nameof(TweetFetcher._tweetRawList), BindingFlags.Static | BindingFlags.NonPublic);
                List<TweetMaster.Param> impTweets = new List<TweetMaster.Param>();
                
                foreach (Tweet twt in NeedyMintsMod.DATA.Tweets.Tweet)
                {
                    TweetMaster.Param param = new TweetMaster.Param()
                    {
                        Id = twt.Id,
                        CommandID = twt.cmdId,
                        Result = "Success",
                        OmoteBodyEn = twt.User == "kangel" ? twt.BodyEN : "",
                        UraBodyEn = twt.User == "ame" ? twt.BodyEN : "",
                        isNight = false,
                        isDay = false,
                    };
                    __result.Add(param);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(TweetFetcher.getKusorepRawList))]
            public static void getKusorepRawListPostfix(ref List<KRepMaster.Param> __result)
            {
                FieldInfo fieldInfo = typeof(TweetFetcher).GetField(nameof(TweetFetcher._KusorepRawList), BindingFlags.Static | BindingFlags.NonPublic);
                List<KRepMaster.Param> impKusos = new List<KRepMaster.Param>();

                foreach (KusoRep kuso in NeedyMintsMod.DATA.KusoReps.KusoRep)
                {
                    KRepMaster.Param param = new KRepMaster.Param()
                    {
                        Id = kuso.Id,
                        BodyEn = kuso.BodyEN,
                        IconId = "N/A",
                        UserId = "N/A"
                    };
                    __result.Add(param);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(TweetFetcher.IsEqualCommandId))]
            public static bool isEqualCommandIdPrefix(out bool __state, CommandType c)
            {
                __state = CheckModdedPrefix(typeof(CommandType), typeof(ModdedCommandType), c).Item1;
                return !__state;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(TweetFetcher.IsEqualCommandId))]
            public static void isEqualCommandIdPostfix(bool __state, TweetMaster.Param t, CommandType c, ref bool __result)
            {
                if (!__state) return;
                ModdedCommandType mc = (ModdedCommandType)(int)c;
                if (t.CommandID == "N/A")
                {
                    __result = mc.ToString() == "None";
                    return;
                }
                __result = t.CommandID == mc.ToString();
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(TweetFetcher.ConvertTypeToTweet))]
            public static void ConvertTypeToTweetPostfix(TweetType t, ref TweetMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(TweetType), typeof(ModdedTweetType), t).Item1) return;
                ModdedTweetType mt = (ModdedTweetType)(int)t;

                MethodInfo info = typeof(TweetFetcher).GetMethod(nameof(TweetFetcher.getTweetRawList), BindingFlags.NonPublic | BindingFlags.Static);

                __result = (info.Invoke(null,null) as List<TweetMaster.Param>).Find((TweetMaster.Param tw) => tw.Id == mt.ToString());
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(TweetFetcher.ConvertTypeToKusorep))]
            public static void ConvertTypeToKusorepPostfix(KusoRepType t, ref KRepMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(KusoRepType), typeof(ModdedKusoRepType), t).Item1) return;
                ModdedKusoRepType mt = (ModdedKusoRepType)(int)t;

                MethodInfo info = typeof(TweetFetcher).GetMethod(nameof(TweetFetcher.getKusorepRawList), BindingFlags.NonPublic | BindingFlags.Static);

                __result = (info.Invoke(null, null) as List<KRepMaster.Param>).Find((KRepMaster.Param tw) => tw.Id == mt.ToString());
            }
        }

        [HarmonyPatch(typeof(PoketterManager))]
        public static class PoketterManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterManager.isValidTweetData))]
            public static void isValidTweetDataPostfix(TweetData tw, ref bool __result)
            {
                if (!CheckModdedPrefix(typeof(TweetType), typeof(ModdedTweetType), tw.Type).Item1) return;
                if (tw.IsOmote)
                {
                    __result = TweetFetcher.ConvertTypeToTweet(tw.Type).OmoteBodyEn.IsNotEmpty() || TweetFetcher.ConvertTypeToTweet(tw.Type).OmoteImageId.IsNotEmpty();
                    NeedyMintsMod.log.LogMessage($"TweetData for {tw.Type} is {__result}");
                    return;
                }
                __result =  TweetFetcher.ConvertTypeToTweet(tw.Type).UraBodyEn.IsNotEmpty();
                NeedyMintsMod.log.LogMessage($"TweetData for {tw.Type} is {__result}");
            }

        }

        [HarmonyPatch(typeof(LoadNetaData))]
        public static class LoadNetaDataPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(LoadNetaData.ReadNetaContent))]
            public static void ReadNetaContentPostfix(AlphaType NetaType, int level, ref AlphaTypeToData __result)
            {
                if (!CheckModdedPrefix(typeof(AlphaType),typeof(ModdedAlphaType), NetaType).Item1) return;

                Stream stream = NeedyMintsMod.DATA.Streams.Stream.First(s => s.AlphaType == ((ModdedAlphaType)(int)NetaType).ToString() && s.AlphaLevel == (level).ToString());

                ModdedTenCommentType tenComment = (ModdedTenCommentType)Enum.Parse(typeof(ModdedTenCommentType), stream.AlphaType + stream.AlphaLevel + "_STREAMNAME");
                AlphaTypeToData data = new AlphaTypeToData()
                {
                    NetaType = NetaType,
                    level = level,
                    NetaGenreJP = "Genre",
                    NetaNameJP = "Name",
                    getJouken = (ActionType)(int)Enum.Parse(typeof(ModdedActionType), stream.ActionType),
                    netaGenre = (CmdType)(int)Enum.Parse(typeof(ModdedCmdType),stream.CmdType),
                    netaName = (TenCommentType)(int)tenComment
                };

                NeedyMintsMod.log.LogMessage($"Read neta content with netaname {data.netaName}");
                __result = data;
            }
        }

        [HarmonyPatch(typeof(NgoEvent))]
        public static class NgoEventPatches
        {
            
        }

        [HarmonyPatch(typeof(Live))]
        public static class LivePatches
        {
            [HarmonyPatch]
            public static class SetScenarioPatcher
            {
                public static MethodInfo TargetMethod()
                {
                    return typeof(Live).GetMethods().Where(m => m.Name == nameof(Live.SetScenario) && !m.IsGenericMethod).First();
                }

                public static bool Prefix(out bool __state)
                {
                    NeedyMintsMod.log.LogMessage("SetScenario patch!");
                    int status = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                    NeedyMintsMod.log.LogMessage($"Haishin plotflag value: {(FollowerPlotFlagValues)status}");

                    bool streamValue = (FollowerPlotFlagValues)status == FollowerPlotFlagValues.AngelFuneral ||
                                       (FollowerPlotFlagValues)status == FollowerPlotFlagValues.AngelWatch ||
                                       (FollowerPlotFlagValues)status == FollowerPlotFlagValues.AngelDeath ||
                                       (FollowerPlotFlagValues)status == FollowerPlotFlagValues.PostComiket;

                    __state = streamValue;

                    return !__state;
                }

                public static void Postfix(ref Live __instance, ref LiveScenario __result, bool __state)
                {
                    NeedyMintsMod.log.LogMessage($"Follower stream: {__state}");
                    if (__state)
                    {
                        SingletonMonoBehaviour<StatusManager>.Instance.isTodayHaishined = true;
                        SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(StatusType.RenzokuHaishinCount, 1);

                        int status = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                        NeedyMintsMod.log.LogMessage($"Haishin plot status: {(FollowerPlotFlagValues)status}");

                        if (status == (int)FollowerPlotFlagValues.PostComiket)
                        {
                            __result = __instance.SetScenario<Haishin_Comiket>();
                        }
                        else if (status == (int)FollowerPlotFlagValues.AngelWatch)
                        {
                            __result = __instance.SetScenario<Haishin_AngelWatch>();
                        }
                        else if (status == (int)FollowerPlotFlagValues.AngelDeath)
                        {
                            __result = __instance.SetScenario<Haishin_AngelDeath>();
                        }
                        else if (status == (int)FollowerPlotFlagValues.AngelFuneral)
                        {
                            __result = __instance.SetScenario<Haishin_AngelFuneral>();
                        }
                        return;
                    }


                    NeedyMintsMod.log.LogMessage($"SetScenarioPostfix!");
                    EventManager em = SingletonMonoBehaviour<EventManager>.Instance;

                    if (!CheckModdedPrefix(typeof(AlphaType), typeof(ModdedAlphaType), em.alpha).Item1) return;
                    string text = string.Format("Netatip_{0}_Level{1}", (ModdedAlphaType)(int)em.alpha, em.alphaLevel);
                    __result = __instance.SetScenario(text);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(Live.isActiveReaction))]
            public static bool isActiveReactionPrefix(ref Live __instance, ref bool __result)
            {
                //NeedyMintsMod.log.LogMessage($"isActiveReactionPrefix: {__instance.isUncontrollable} {__instance.isOiwai} {AMAManager.isAMA}");
                if (__instance.isUncontrollable && __instance.isOiwai && SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAMA)
                {
                    NeedyMintsMod.log.LogMessage($"AMA reroute!");
                    SingletonMonoBehaviour<CursorManager>.Instance.SetCursor(null, __instance.hotSpot, __instance.cursorMode);
                    __result = true;
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(Live.RefreshYomuCommentLabel))]
            public static bool RefreshYomuCommentLabelPrefix(ref Live __instance)
            {
                NeedyMintsMod.log.LogMessage("RefreshYomuCommentLabelPostfix");
                if (!SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAMA) return true;
                NeedyMintsMod.log.LogMessage("RefreshYomuCommentLabelPostfix modcheck");
                LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
                SystemTextType type = (SystemTextType)(int)ModdedSystemTextType.System_AMARead;

                NeedyMintsMod.log.LogMessage($"AMA systext {type} {lang}!");
                string text = NgoEx.SystemTextFromType(type, lang);
                NeedyMintsMod.log.LogMessage($"AMA systext {text}!");
                __instance.CommentLabel.text = text;
                //__instance.CommentBg.color = new Color(0.8f, 0.85f, 0.8f, 1f);
                return false;
            }

            

            [HarmonyPostfix]
            [HarmonyPatch(nameof(Live.Awake))]
            public static void AwakePostfix(ref Live __instance)
            {
                /* This patch would normally not be necessary,
                 * but ANGEL WATCH (FOLLOWERS' first ending stream)
                 * goes over the chat limit and needs more objects
                 * in order to not break towards the end of the stream.
                 * 
                 * Good job, ANGEL WATCH!
                */

                int lastNaturalChild = __instance.CommentParent.childCount - 1;
                Transform lastObject = __instance.CommentParent.GetChild(lastNaturalChild);
                Transform firstObject = __instance.CommentParent.GetChild(0);
                for (int i = lastNaturalChild; i < 160 + lastNaturalChild; i++)
                {
                    Transform newObj = UnityEngine.Object.Instantiate(firstObject, __instance.CommentParent);
                }
            }

            /*[HarmonyFinalizer]
            [HarmonyPatch(nameof(Live.Awake))]
            public static Exception AwakeFinalizer(Exception __exception)
            {
                if (__exception != null ) NeedyMintsMod.log.LogMessage(__exception.Message);
                return null;
            }*/
        }

        [HarmonyPatch(typeof(LiveComment))]
        public static class LiveCommentPatches
        {
            // hirou is only called by superchats on click
            // sakujo is only called by regular chats on click
            [HarmonyPrefix]
            [HarmonyPatch(nameof(LiveComment.SetContent))]
            public static bool SetContentPrefix(ref Playing playing, out SuperchatType __state)
            {
                __state = playing.color;
                if (!CheckModdedPrefix(typeof(SuperchatType),typeof(ModdedSuperchatType),playing.color).Item1) return true;
                switch (playing.color.Swap())
                {
                    case ModdedSuperchatType.AMA_White:
                        playing.color = SuperchatType.White;
                        break;
                    case ModdedSuperchatType.AMA_Blue:
                        playing.color = SuperchatType.Blue;
                        break;
                    case ModdedSuperchatType.AMA_Cyan:
                        playing.color = SuperchatType.Cyan;
                        break;
                    case ModdedSuperchatType.AMA_LightGreen:
                        playing.color = SuperchatType.LightGreen;
                        break;
                    case ModdedSuperchatType.AMA_Yellow:
                        playing.color = SuperchatType.Yellow;
                        break;
                    case ModdedSuperchatType.AMA_Orange:
                        playing.color = SuperchatType.Orange;
                        break;
                    case ModdedSuperchatType.AMA_Magenta:
                        playing.color = SuperchatType.Magenta;
                        break;
                    case ModdedSuperchatType.AMA_Red:
                        playing.color = SuperchatType.Red;
                        break;
                    default:
                        NeedyMintsMod.log.LogMessage($"Attempted to set content of color {playing.color.Swap()}");
                        return false;
                }
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(LiveComment.SetContent))]
            public static void SetContentPostfix(ref Playing playing, SuperchatType __state)
            {
                playing.color = __state;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(LiveComment.hirou))]
            public static void hirouPostfix(Playing playing, ref LiveComment __instance)
            {
                Live live = UnityEngine.Object.FindObjectOfType<Live>();

                // Cancel AMA request if either asked already, message isn't a valid AMA, or isn't AMAing with the right stream settings
                bool askedAlready = SingletonMonoBehaviour<NeedyMintsModManager>.Instance.playedQuestions.Contains(playing);
                bool isAMAChat = playing.henji != ""; 
                NeedyMintsMod.log.LogMessage($"hirou activated for {playing}, while askedAlready is {askedAlready} and isAMAChat is {isAMAChat} with color {playing.color}");
                if (!SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAMA || askedAlready ||
                    !isAMAChat || !live.isOiwai) return;

                //NeedyMintsMod.log.LogMessage($"Selected AMA \"{playing.nakami} with status {playing.diffStatusType} {playing.delta}\"");
                //NeedyMintsMod.log.LogMessage($"AMA response is \"{playing.henji}\" with anim {playing.henjiAnim}");
                
                // AMA stress system
                if (playing.diffStatusType == StatusType.Stress)
                {
                    if (playing.delta != -99)
                    {
                        SingletonMonoBehaviour<NeedyMintsModManager>.Instance.StressDelta += playing.delta;
                    }
                    else
                    {
                        SingletonMonoBehaviour<NeedyMintsModManager>.Instance.deleteComment = __instance;
                        //NeedyMintsMod.log.LogMessage($"Deletecomment is {AMAManager.deleteComment.playing.nakami}");
                    }
                }
                
                // Get position to insert AMA into
                int newAMAPos = 1;
                try
                {
                    int lastAMAPos = live.NowPlaying.playing.FindLastIndex(p => p.diffStatusType == ModdedStatusType.AMAStatus.Swap());
                    newAMAPos = lastAMAPos + 1;
                }
                catch (Exception e)
                {
                    NeedyMintsMod.log.LogMessage("No AMAs currently in queue!");
                }

                // Split multi-message responses and their associated expressions
                string[] henjiAnims = playing.henjiAnim.Split(new string[] { "___" }, StringSplitOptions.None).Reverse().ToArray();
                string[] henjis = playing.henji.Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
                for (int i = 0; i < henjis.Length; i++)
                {
                    live.NowPlaying.playing.Insert(newAMAPos, new Playing(true, henjis[i], ModdedStatusType.AMAStatus.Swap(), 1, 0, "", "", (i < henjiAnims.Length) ? henjiAnims[i] : "", playing.isLoopAnim, SuperchatType.White, false, ""));
                }

                // Add anticomment
                live.NowPlaying.playing.Insert(newAMAPos, new Playing(true, "", ModdedStatusType.AMAStatus.Swap(), 1,0,"","",playing.henjiAnim,true,SuperchatType.White,true, playing.nakami));
                
                // Register AMA as asked
                SingletonMonoBehaviour<NeedyMintsModManager>.Instance.playedQuestions.Add(playing);


                // Change color of message to show its already been asked
                Traverse trav = new Traverse(__instance).Field(nameof(LiveComment.bgDefault));
                trav.SetValue((Color)trav.GetValue() * 0.8f);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(LiveComment.highlighted))]
            public static void highlightedPostfix(LiveComment __instance)
            {
                if (!SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAMA) return;


                Live live = typeof(LiveComment).GetField(nameof(LiveComment._live), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as Live;
                if (live.isUncontrollable && live.isOiwai && !SingletonMonoBehaviour<NeedyMintsModManager>.Instance.playedQuestions.Contains(__instance.playing))
                {
                    NeedyMintsMod.log.LogMessage($"AMA reroute!");
                    SingletonMonoBehaviour<CursorManager>.Instance.SetCursor(null, live.hotSpot, live.cursorMode);
                }
                //NeedyMintsMod.log.LogMessage("Highlighted!");
            }
        }

        [HarmonyPatch(typeof(LiveScenario))]
        public static class LiveScenarioPatches
        {

            [HarmonyPrefix]
            [HarmonyPatch(nameof(LiveScenario.Play))]
            public static bool PlayPrefix(Playing context, ref LiveScenario __instance)
            {
                NeedyMintsMod.log.LogMessage($"Playing {context.nakami}");
                if (!CheckModdedPrefix(typeof(SuperchatType),typeof(ModdedSuperchatType),context.color).Item1) return true;

                Live live = UnityEngine.Object.FindObjectOfType<Live>();

                switch (context.color.Swap())
                {
                    case ModdedSuperchatType.AMA_START:
                        SingletonMonoBehaviour<NeedyMintsModManager>.Instance.StartAMA(ref __instance);
                        break;

                    case ModdedSuperchatType.AMA_END:
                        SingletonMonoBehaviour<NeedyMintsModManager>.Instance.FinishAMA(ref __instance);
                        break;

                    case ModdedSuperchatType.EVENT_TIMEPASS:
                        SingletonMonoBehaviour<StatusManager>.Instance.timePassing(context.delta);
                        break;

                    case ModdedSuperchatType.JINE_INIT:
                        IWindow jine = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine);
                        jine.GameObjectTransform.position = Vector3.left;
                        SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
                        break;

                    case ModdedSuperchatType.JINE_DESTROY:
                        SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Jine);
                        break;

                    case ModdedSuperchatType.JINE_SEND:
                        ModdedJineType sendType;
                        bool foundJINE = Enum.TryParse<ModdedJineType>(context.nakami, false, out sendType);
                        JineData data = new JineData((JineType)(int)sendType);
                        SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(data);

                        NeedyMintsMod.log.LogMessage($"Jine send anim: {context.animation}");

                        if (context.animation != "" && data.user == JineUserType.ame)
                        {
                            live.ChotenAnimation(context.animation, context.isLoopAnim);
                        }
                        //__instance.playing.Insert(0, new Playing(true, context.nakami, StatusType.Tension, 1, 0, "", "", "", false, (SuperchatType)(int)ModdedSuperchatType.JINE_WAIT, false, ""));
                        break;

                    case ModdedSuperchatType.JINE_CHOICE:
                        string[] choiceStrings = context.nakami.Split(new string[] { "___"}, StringSplitOptions.RemoveEmptyEntries);
                        List<JineType> choices = choiceStrings.Select(x => (JineType)(int)Enum.Parse(typeof(ModdedJineType), x)).ToList();
                        SingletonMonoBehaviour<JineManager>.Instance.StartOption(choices);
                        break;

                    case ModdedSuperchatType.JINE_WAIT:
                        ModdedJineType waitType;
                        bool foundWaitJINE = Enum.TryParse(context.nakami, false, out waitType);
                        bool wasSent = SingletonMonoBehaviour<JineManager>.Instance.history.Any(j => j.id == waitType.Swap());
                        if (!wasSent)
                        {
                            __instance.playing.Insert(0, new Playing(true, context.nakami, StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_WAIT.Swap(), false, ""));
                            IWindow refocusJINE = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.Jine);
                            SingletonMonoBehaviour<WindowManager>.Instance.SetForeground(refocusJINE);
                        }
                        break;

                    case ModdedSuperchatType.EVENT_CREATEDEPAZ:
                        IWindow pill = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow_NoInteractive((AppType)(int)ModdedAppType.PillDaypass_Follower);
                        pill.Uncloseable();
                        NeedyMintsMod.log.LogMessage($"Created pill {pill}");
                        pill.setRandomPosition();
                        SingletonMonoBehaviour<NeedyMintsModManager>.Instance.pills.Add(pill);
                        break;

                    case ModdedSuperchatType.EVENT_DOSE:
                        IWindow takePill = SingletonMonoBehaviour<NeedyMintsModManager>.Instance.pills.Last();
                        SheetView sheet = takePill.nakamiApp.GetComponentInChildren<SheetView>();

                        takePill.Touched();
                        sheet.OnDose();
                        if (sheet.CurrentDoseCount.Value >= 3)
                        {
                            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.pills.Remove(takePill);
                            SingletonMonoBehaviour<WindowManager>.Instance.Close(takePill);
                        }
                        else
                        {
                            __instance.playing.Insert(0, new Playing(true, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_DOSE.Swap(), false, ""));
                        }
                        break;

                    case ModdedSuperchatType.EVENT_SHADERWAIT:

                        bool shaderCompleted = SingletonMonoBehaviour<NeedyMintsModManager>.Instance.anim.IsComplete();

                        if (!shaderCompleted)
                        {
                            __instance.playing.Insert(0, new Playing(true, context.nakami, StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_SHADERWAIT.Swap(), false, ""));
                        }
                        break;

                    case ModdedSuperchatType.EVENT_DELAYFRAME:
                        if (context.animation == "") break;
                        live.ChotenAnimation(context.animation, context.isLoopAnim);
                        break;

                    case ModdedSuperchatType.EVENT_SHADER:
                        EffectType vanillaEffect;
                        ModdedEffectType moddedEffect;

                        bool vanillaEffectVal = Enum.TryParse<EffectType>(context.nakami, true, out vanillaEffect);
                        bool moddedEffectVal = Enum.TryParse<ModdedEffectType>(context.nakami, true, out moddedEffect);

                        object type;
                        if (moddedEffectVal) type = moddedEffect;
                        else if (vanillaEffectVal) type = vanillaEffect;
                        else return true;


                        float endVal = (float)context.delta * 0.01f;
                        float duration = (float)context.additionalTension * 0.01f;
                        float startVal = 0f;
                        float.TryParse(context.henji, out startVal);

                        PostEffectManager.Instance.SetShader((EffectType)(int)type);

                        if (context.isLoopAnim)
                        {
                            float weight = 0f;
                            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.anim = DOTween.To(() => weight, delegate (float x)
                            {
                                PostEffectManager.Instance.SetShaderWeight(x);
                            }, endVal, duration).SetEase(Ease.InExpo).SetAutoKill(false);

                            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.anim.Play<TweenerCore<float, float, FloatOptions>>();
                        }
                        else
                        {
                            PostEffectManager.Instance.SetShaderWeight(endVal);
                        }
                        break;

                    case ModdedSuperchatType.EVENT_MUSICCHANGE:
                        if (context.nakami == "")
                        {
                            AudioManager.Instance.StopBgm();
                        }
                        else
                        {
                            AudioManager.Instance.PlayBgmById(context.nakami, context.isLoopAnim);
                        }
                        break;

                    case ModdedSuperchatType.EVENT_MAINPANELCOLOR:
                        Color color = new Color(1f, 1f, 1f, 1f);

                        switch (context.nakami)
                        {
                            case "RED":
                                color = new Color(1f, 0f, 0f, 1f);
                                break;
                            case "BLUE":
                                color = new Color(1f, 0f, 0f, 1f);
                                break;
                            case "WHITE":
                                color = new Color(1f, 1f, 1f, 1f);
                                break;
                            case "BLACK":
                                color = new Color(0f, 0f, 0f, 1f);
                                break;

                            default: break;
                        }

                        GameObject.Find("MainPanel").GetComponent<UnityEngine.UI.Image>().color = color;
                        break;

                    default: return true;
                };
                return false;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(LiveScenario.Play))]
            public static void PlayPostfix(ref LiveScenario __instance, ref Playing context)
            {
                NeedyMintsMod.log.LogMessage($"PlayPostfix!");
                NeedyMintsModManager nmmm = SingletonMonoBehaviour<NeedyMintsModManager>.Instance;
                if (SingletonMonoBehaviour<NeedyMintsModManager>.Instance.deleteComment != null && context.antiComment == nmmm.deleteComment.playing.nakami)
                {
                    nmmm.deleteTime = true;
                }

                switch (context.color.Swap())
                {
                    case ModdedSuperchatType.JINE_SEND:
                        __instance.BASESPEED = 2000;
                        return;
                    case ModdedSuperchatType.EVENT_SHADERWAIT:
                    case ModdedSuperchatType.JINE_WAIT:
                        __instance.BASESPEED = 1000;
                        return;
                    case ModdedSuperchatType.EVENT_CREATEDEPAZ:
                    case ModdedSuperchatType.EVENT_DOSE:
                        __instance.BASESPEED = 30;
                        return;
                    case ModdedSuperchatType.JINE_INIT:
                    case ModdedSuperchatType.JINE_DESTROY:
                    case ModdedSuperchatType.AMA_START:
                    case ModdedSuperchatType.AMA_END:
                    case ModdedSuperchatType.EVENT_TIMEPASS:
                    case ModdedSuperchatType.EVENT_MUSICCHANGE:
                    case ModdedSuperchatType.EVENT_SHADER:
                    case ModdedSuperchatType.EVENT_MAINPANELCOLOR:
                        __instance.BASESPEED = 1;
                        return;
                    case ModdedSuperchatType.EVENT_DELAYFRAME:
                        NeedyMintsMod.log.LogMessage($"Delaying for {context.delta} ms");
                        __instance.BASESPEED = context.delta;
                        return;
                    default: break;
                }




                if (!nmmm.isAMA || context.nakami == "") return;

                Live live = UnityEngine.Object.FindObjectOfType<Live>();

                float speed;
                if (context.showAnti)
                {
                    speed = 1500;
                }
                else
                {

                    float randMultiplier = (float)Math.Pow((double)(UnityEngine.Random.Range(1, 200) / 100f), 0.5f);
                    speed = (randMultiplier * nmmm.PlannedAMALength / nmmm.QUESTIONS);

                    NeedyMintsMod.log.LogMessage($"deleteComment {nmmm.deleteComment}");
                    NeedyMintsMod.log.LogMessage($"deleteTime {nmmm.deleteTime}");

                    if (nmmm.deleteComment != null && nmmm.deleteTime)
                    {
                        //AMAManager.deleteComment.honbun = "";
                        nmmm.deleteComment.honbunView.text = "";
                        nmmm.deleteComment = null;
                        nmmm.deleteTime = false;
                    }

                }
                __instance.BASESPEED = (int)(speed * Mathf.Pow(0.8f, (float)live.Speed));

                NeedyMintsMod.log.LogMessage($"Adjusted BASESPEED for AMA question to {__instance.BASESPEED}");
            }
        }
        

        [HarmonyDebug]
        [HarmonyPatch(typeof(Type))]
        public static class TypePatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Type.GetType), new Type[] { typeof(string) })]
            public static void GetMultiType(string typeName, ref Type __result)
            {
                NeedyMintsMod.log.LogMessage(Environment.StackTrace);
                NeedyMintsMod.log.LogMessage($"Type {typeName} requested");
                if (typeName.StartsWith("ngov3.") && __result == null)
                {
                    if (typeName.EndsWith("_0"))
                    {
                        NeedyMintsMod.log.LogMessage("Found lvl 0 stream");
                        //NeedyMintsMod.log.LogError(System.Environment.StackTrace);
                    }
                    string tmpTypeName = typeName.Replace("ngov3.", "NeedyMintsOverdose.");
                    Regex rx = new Regex(@"_\d*");
                    string intStr = rx.Match(tmpTypeName).Value.Replace("_", "");

                    int val = int.Parse(intStr);
                    string moddedType = null;
                    if (tmpTypeName.StartsWith("NeedyMintsOverdose.Netatip") ||
                        tmpTypeName.StartsWith("NeedyMintsOverdose.ChipGet"))
                    {

                        moddedType = "_" + (ModdedAlphaType)val;

                    }
                    else if (tmpTypeName.StartsWith("NeedyMintsOverdose.Action")) moddedType = "_" + (ModdedCmdType)val;



                    string finalType = rx.Replace(tmpTypeName, moddedType, 1);
                    __result = Type.GetType(finalType);
                    NeedyMintsMod.log.LogMessage($"Failed type get of {typeName} got replaced with {finalType} with return value {__result}");
                }
            }
        }

        [HarmonyPatch(typeof(KitsuneView))]
        public static class KitsuneViewPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(KitsuneView.ThreadRank))]
            public static void ThreadRankPostfix(ref string __result, ref KitsuneView __instance)
            {
                StatusManager sm = SingletonMonoBehaviour<StatusManager>.Instance;
                int goOutStress = sm.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap());
                int plotFlag = sm.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                bool stressBreak = sm.GetMaxStatus(StatusType.Stress) > 100;
                if (goOutStress > 2 && plotFlag == (int)FollowerPlotFlagValues.PostDepaz && stressBreak)
                {
                    __result = "STALKDISCOVER";
                }
            }
        }

        [HarmonyPatch(typeof(Shortcut))]
        public static class ShortcutPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Shortcut.Start))]
            public static bool StartPrefix(Shortcut __instance, UnityEngine.UI.Button ____shortcut)
            {
                Alternates.PanicQuitOdekakeShortcut(__instance, ____shortcut);
                return false;
            }

            /*[HarmonyPostfix]
            [HarmonyPatch(nameof(Shortcut.Start))]
            public static void StartPostfix(Button ____shortcut, Shortcut __instance)
            {
                if (__instance.appType == AppType.GoOut && SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 0)
                {
                    ____shortcut.onClick.AddListener(() => PanicQuitOdekake(__instance));
                }
            }*/
        }

        [HarmonyPatch(typeof(LoadAppData))]
        public static class LoadAppDataPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(LoadAppData.ReadAppContent))]
            public static void ReadAppContentPostfix(AppType appType, ref AppTypeToData __result)
            {
                FieldInfo app2DataInfo = typeof(LoadAppData).GetField(nameof(LoadAppData.app2data), BindingFlags.NonPublic | BindingFlags.Static);
                AppTypeToDataAsset app2data = app2DataInfo.GetValue(null) as AppTypeToDataAsset;

                FieldInfo mergedAppsInfo = typeof(AppTypeToDataAsset).GetField(nameof(AppTypeToDataAsset.mergedApps),BindingFlags.Instance | BindingFlags.NonPublic);
                object mergedAppstmp = mergedAppsInfo.GetValue(app2data);
                NeedyMintsMod.log.LogMessage("HELPME");
                List<AppTypeToData> mergedApps;
                if (mergedAppstmp == null)
                {
                    mergedApps = new List<AppTypeToData>();
                }
                else mergedApps = (mergedAppstmp as IEnumerable<AppTypeToData>).ToList();

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.Follower_taiki)) {
                    AppTypeToData taiki = mergedApps.Find(App => App.appType == AppType.Ideon_taiki);
                    AppTypeToData followTaiki = new AppTypeToData(false)
                    {
                        appIcon = taiki.appIcon,
                        AppName = taiki.AppName,
                        AppNameJP = taiki.AppNameJP,
                        appType = (AppType)(int)ModdedAppType.Follower_taiki,
                        FirstHeight = taiki.FirstHeight,
                        FirstWidth = taiki.FirstWidth,
                        FirstPosX = taiki.FirstPosX,
                        FirstPosY = taiki.FirstPosY,
                        is2DWindow = taiki.is2DWindow,
                        InnerContent = UnityEngine.Object.Instantiate(taiki.InnerContent)
                    };
                    Transform trans = followTaiki.InnerContent.transform.GetChild(0).GetChild(5);

                    GameObject.Destroy(trans.GetChild(0).gameObject);

                    mergedApps.Add(followTaiki);
                }

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.AltPoketter))
                {
                    AppTypeToData poketter = mergedApps.Find(App => App.appType == AppType.Poketter);
                    AppTypeToData altPoketter = new AppTypeToData(false)
                    {
                        appIcon = poketter.appIcon,
                        AppName = poketter.AppName,
                        AppNameJP = poketter.AppNameJP,
                        appType = (AppType)(int)ModdedAppType.AltPoketter,
                        FirstHeight = poketter.FirstHeight,
                        FirstWidth = poketter.FirstWidth,
                        FirstPosX = poketter.FirstPosX,
                        FirstPosY = poketter.FirstPosY,
                        is2DWindow = poketter.is2DWindow,
                        InnerContent = UnityEngine.Object.Instantiate(poketter.InnerContent),
                        isOnly = true
                    };
                    PoketterView2D view = altPoketter.InnerContent.GetComponent<PoketterView2D>();
                    NeedyMintsMod.log.LogMessage($"Poketter component: {view}");

                    AltPoketter newPoke = altPoketter.InnerContent.AddComponent<AltPoketter>();

                    mergedApps.Add(altPoketter);
                }


                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.PillDaypass_Follower))
                {
                    AppTypeToData depaz = mergedApps.Find(App => App.appType == AppType.PillDypass);
                    mergedApps.Add(new AppTypeToData(false)
                    {
                        appIcon = depaz.appIcon,
                        AppName = depaz.AppName,
                        AppNameJP = depaz.AppNameJP,
                        appType = (AppType)(int)ModdedAppType.PillDaypass_Follower,
                        FirstHeight = depaz.FirstHeight,
                        FirstWidth = depaz.FirstWidth,
                        FirstPosX = depaz.FirstPosX,
                        FirstPosY = depaz.FirstPosY,
                        is2DWindow = depaz.is2DWindow,
                        InnerContent = UnityEngine.Object.Instantiate(depaz.InnerContent)
                    });
                }

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.Login_Hacked))
                {
                    AppTypeToData login = mergedApps.Find(App => App.appType == AppType.Login);
                    AppTypeToData hackedLogin = new AppTypeToData(false)
                    {
                        appIcon = login.appIcon,
                        AppName = login.AppName,
                        AppNameJP = login.AppNameJP,
                        appType = (AppType)(int)ModdedAppType.Login_Hacked,
                        FirstHeight = login.FirstHeight,
                        FirstWidth = login.FirstWidth,
                        FirstPosX = login.FirstPosX,
                        FirstPosY = login.FirstPosY,
                        is2DWindow = login.is2DWindow,
                        InnerContent = UnityEngine.Object.Instantiate(login.InnerContent)
                    };

                    Login loginComponent = hackedLogin.InnerContent.GetComponent<Login>();
                    LoginHacked hackedComponent = hackedLogin.InnerContent.AddComponent<LoginHacked>();
                    Traverse loginTrav = new Traverse(loginComponent);

                    hackedComponent._badge = loginTrav.Field(nameof(Login._badge)).GetValue<GameObject>();
                    hackedComponent._input = loginTrav.Field(nameof(Login._input)).GetValue<TMP_InputField>();
                    hackedComponent._login = loginTrav.Field(nameof(Login._login)).GetValue<UnityEngine.UI.Button>();
                    hackedComponent._passText = loginTrav.Field(nameof(Login._passText)).GetValue<TMP_Text>();
                    hackedComponent._placeholderText = loginTrav.Field(nameof(Login._placeholderText)).GetValue<TMP_Text>();
                    hackedComponent._placeholderText.transform.position += new Vector3(0, -50,0);
                    hackedComponent._placeholderText.fontSize = 16;
                    hackedComponent._placeholderText.color = Color.red;
                    hackedComponent._imageContainer = hackedLogin.InnerContent.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
                    hackedComponent.baseImage = Addressables.LoadAssetAsync<Sprite>("login_bad.png").WaitForCompletion();
                    hackedComponent.invalidPasswordImage = Addressables.LoadAssetAsync<Sprite>("login_bad_invalid.png").WaitForCompletion();
                    hackedComponent._imageContainer.sprite = hackedComponent.baseImage;

                    GameObject.Destroy(loginComponent);

                    mergedApps.Add(hackedLogin);
                }
                mergedAppsInfo.SetValue(app2data, mergedApps);
                app2DataInfo.SetValue(null, app2data);

                if (__result == null)
                {
                    __result = LoadAppData.ReadAppContent(appType);
                }
            }
        }

        [HarmonyPatch(typeof(ngov3.Effect.DayPassing))]
        public static class DayPassingPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(ngov3.Effect.DayPassing.Start))]
            public static bool StartPrefix(ref ngov3.Effect.DayPassing __instance)
            {
                // Pass off to the alt version
                Alternates.DayPassingStartAlternate(__instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(ngov3.Effect.DayPassing2D))]
        public static class DayPassing2DPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(ngov3.Effect.DayPassing2D.Start))]
            public static bool StartPrefix(ref ngov3.Effect.DayPassing2D __instance)
            {
                // Pass off to the alt version
                Alternates.DayPassing2DStartAlternate(__instance);
                return false;
            }
        }

        [HarmonyPatch(typeof(DayView))]
        public static class DayViewPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DayView.UpdateDay))]
            public static bool UpdateDayPrefix(int index, TMP_Text ____dayText, Status ___dayIndex)
            {
                if (index > ___dayIndex.maxValue.Value)
                {
                    ____dayText.text = "????";
                    return false;
                }
                ____dayText.text = NgoEx.DayText(index, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                return false;
            }
        }

        [HarmonyPatch(typeof(PoketterCell))]
        public static class PoketterCellPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell.SetData))]
            public static void SetDataPostfix(ref PoketterCell __instance, TweetDrawable nakami, ref TMP_Text ____dateText)
            {
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");


                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell.SetDataStatic))]
            public static void SetDataStaticPostfix(ref PoketterCell __instance, TweetDrawable nakami, ref TMP_Text ____dateText)
            {
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");

                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell.OnLanguageUpdated))]
            public static void OnLanguageUpdatedPostfix(ref PoketterCell __instance, ref TMP_Text ____dateText)
            {
                TweetDrawable nakami = (TweetDrawable)(new Traverse(__instance).Field(nameof(PoketterCell.tweetDrawable)).GetValue());
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");

                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }
        }

        [HarmonyPatch(typeof(PoketterCell2D))]
        public static class PoketterCell2DPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell2D.SetData))]
            public static void SetDataPostfix(ref PoketterCell __instance, TweetDrawable nakami, ref TMP_Text ____dateText)
            {
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");
                NeedyMintsMod.log.LogMessage($"Setting data: {nakami.FavNumber} favs & {nakami.RtNumber} rts");


                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell2D.SetDataStatic))]
            public static void SetDataStaticPostfix(ref PoketterCell __instance, TweetDrawable nakami, ref TMP_Text ____dateText)
            {
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");

                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell2D.OnLanguageUpdated))]
            public static void OnLanguageUpdatedPostfix(ref PoketterCell __instance, ref TMP_Text ____dateText)
            {
                TweetDrawable nakami = (TweetDrawable)(new Traverse(__instance).Field(nameof(PoketterCell.tweetDrawable)).GetValue());
                int dayMax = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex);
                NeedyMintsMod.log.LogMessage($"Tweet day max: {dayMax}");

                if (dayMax >= nakami.Day && dayMax > 30)
                {
                    ____dateText.text = NgoEx.DayText(nakami.Day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value) + " " + NgoEx.CmdName(nakami.cmdType, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(PoketterCell2D.FavRtMove))]
            public static bool FavRtMovePrefix(ref PoketterCell2D __instance, TweetDrawable ___tweetDrawable)
            {
                bool notCustomAnimate = ___tweetDrawable.IsOmote || (___tweetDrawable.RtNumber == 0 && ___tweetDrawable.FavNumber == 0);
                if (notCustomAnimate) return true;

                PoketterCell2D inst = __instance;
                Alternates.FavRtMoveAlternate(inst);
                return false;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterCell2D.Awake))]
            public static void AwakePostfix(ref PoketterCell2D __instance, ref TweetDrawable ___tweetDrawable)
            {
                PoketterCell2D inst = __instance;
                if (!___tweetDrawable.IsOmote) return;
                SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAmeDelete.Where((bool v) => v).Subscribe(delegate (bool _)
                {
                    inst.gameObject.SetActive(false);
                }).AddTo(__instance.gameObject);
            }
        }


        [HarmonyPatch(typeof(Boot))]
        public static class BootPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Boot.SetChosenDayText))]
            public static bool SetChosenDayTextPrefix(ref Boot __instance, LanguageType lang, int _DataNumber)
            {
                Boot inst = __instance;
                Alternates.SetChosenDayTextAlternate(inst, lang, _DataNumber);
                return false;
            }

            [HarmonyTranspiler]
            [HarmonyPatch(nameof(Boot.Start))]
            public static IEnumerable<CodeInstruction> StartTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> baseInstructions = instructions.ToList();
                List<CodeInstruction> newIns = new List<CodeInstruction>();
                bool nextPop = false;

                for (int i = 0; i < baseInstructions.Count; i++)
                {
                    newIns.Add(baseInstructions[i]);
                    if (baseInstructions[i].opcode == OpCodes.Callvirt && (baseInstructions[i].operand as MethodInfo)?.Name == nameof(List<EndingType>.Remove))
                    {
                        nextPop = true;
                    }
                    if (baseInstructions[i].opcode == OpCodes.Pop && nextPop)
                    {
                        nextPop = false;
                        foreach (ModdedEndingType ending in Enum.GetValues(typeof(ModdedEndingType)))
                        {
                            newIns.Add(new CodeInstruction(OpCodes.Ldc_I4_S, (int)ending));
                            newIns.Add(new CodeInstruction(OpCodes.Callvirt, typeof(List<EndingType>).GetMethod(nameof(List<EndingType>.Remove), BindingFlags.Instance)));
                            newIns.Add(new CodeInstruction(OpCodes.Pop));
                        }
                    }
                }
                return newIns;
            }
        }

        [HarmonyPatch(typeof(App_LoadDataComponent))]
        public static class App_LoadDataComponentPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(App_LoadDataComponent.Awake))]
            public static bool AwakePrefix(ref App_LoadDataComponent __instance)
            {
                App_LoadDataComponent inst = __instance;
                Alternates.AppLoadDataAlternate(inst);
                return false;
            }
        }

        [HarmonyPatch(typeof(PostEffectManager))]
        public static class PostEffectManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PostEffectManager.Start))]
            public static void StartPostfix(ref PostEffectManager __instance, ref ReactiveProperty<EffectType> ____currentShaderType,
                ref Volume ____overdose,
                ref Volume ____overdosePron,
                ref Volume ____overdoseHyporon,
                ref Volume ____psyche,
                ref Volume ____weed, 
                ref Volume ____goCrazy,
                ref Volume ____kyouizon,
                ref Volume ____bleeding,
                ref Volume ____satujinNoise,
                ref Volume ____satujinGata,
                ref Volume ____anmaku, 
                ref Volume ____otona,
                ref Volume ____noisy,
                ref Volume ____invert, 
                ref Volume ____horror, 
                ref Volume ____yugami, 
                ref Volume ____bloomlight,
                ref Volume ____kakusei,
                ref Volume ____powapowa,
                ref ShaderAudioFader ____audioEffect)
            {
                PostEffectManager inst = __instance;
                Volume _overdose = ____overdose;
                Volume _overdosePron = ____overdosePron;
                Volume _overdoseHyporon = ____overdoseHyporon;
                Volume _psyche = ____psyche;
                Volume _weed = ____weed;
                Volume _goCrazy = ____goCrazy;
                Volume _kyouizon = ____kyouizon;
                Volume _bleeding = ____bleeding;
                Volume _satujinNoise = ____satujinNoise;
                Volume _satujinGata = ____satujinGata;
                Volume _anmaku = ____anmaku;
                Volume _otona = ____otona;
                Volume _noisy = ____noisy;
                Volume _invert = ____invert;
                Volume _horror = ____horror;
                Volume _yugami = ____yugami;
                Volume _bloomlight = ____bloomlight;
                Volume _kakusei = ____kakusei;
                Volume _powapowa = ____powapowa;
                ShaderAudioFader _audioEffect = ____audioEffect;


                ____currentShaderType.SkipLatestValueOnSubscribe<EffectType>().Subscribe(delegate (EffectType type)
                 {
                     if (type == (EffectType)(int)ModdedEffectType.Vengeful)
                     {
                         _overdose.enabled = true;
                         _psyche.enabled = false;
                         _weed.enabled = false;
                         _bleeding.enabled = true;
                         _goCrazy.enabled = true;
                         _anmaku.enabled = true;
                     }
                     if (type == (EffectType)(int)ModdedEffectType.Hazy)
                     {
                         _bloomlight.enabled = true;
                     }
                 }).AddTo(__instance.gameObject);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(PostEffectManager.SetShaderWeight), new Type[] { typeof(float), typeof(EffectType) })]
            public static void SetShaderWeightPostfix(ref PostEffectManager __instance, ref ReactiveProperty<EffectType> ____currentShaderType,
                float weight, EffectType type,
                ref Volume ____overdose,
                ref Volume ____overdosePron,
                ref Volume ____overdoseHyporon,
                ref Volume ____psyche,
                ref Volume ____weed,
                ref Volume ____goCrazy,
                ref Volume ____kyouizon,
                ref Volume ____bleeding,
                ref Volume ____satujinNoise,
                ref Volume ____satujinGata,
                ref Volume ____anmaku,
                ref Volume ____otona,
                ref Volume ____noisy,
                ref Volume ____invert,
                ref Volume ____horror,
                ref Volume ____yugami,
                ref Volume ____bloomlight,
                ref Volume ____kakusei,
                ref Volume ____powapowa,
                ref ShaderAudioFader ____audioEffect)
            {
                if (type == (EffectType)(int)ModdedEffectType.Vengeful)
                {
                    ____overdose.weight *= 0.3f;
                    ____goCrazy.weight *= 0.1f;
                    ____anmaku.weight *= 0.07f;
                }
                else if (type == (EffectType)(int)ModdedEffectType.Hazy)
                {
                    ____bloomlight.weight = 0.2f * weight;
                    ____audioEffect.UpdateAudioEffect((EffectType)(int)ModdedEffectType.Hazy, weight);
                }
            }
        }

        [HarmonyPatch(typeof(ShaderAudioFader))]
        public static class ShaderAudioFaderPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(ShaderAudioFader.UpdateAudioEffect))]
            public static void UpdateAudioEffectPostfix(ref ShaderAudioFader __instance, EffectType type, float weight, float ____ageSlider, ref AudioMixer ____audioMixer)
            {

                switch ((ModdedEffectType)(int)type)
                {
                    case ModdedEffectType.Vengeful:
                        ____audioMixer.SetFloat("Master_Pitch", 1f - ____ageSlider * 3f);
                        ____audioMixer.SetFloat("Dist_Level", 0f);
                        ____audioMixer.SetFloat("PS_Pitch", 1f);
                        ____audioMixer.SetFloat("Flange_Drymix", 1f);
                        ____audioMixer.SetFloat("Flange_WetMix", 0f);
                        ____audioMixer.SetFloat("Flange_Depth", 1f);
                        ____audioMixer.SetFloat("Flange_Rate", 0.1f);
                        ____audioMixer.SetFloat("Lowpass_Cutoff", 22000f - ____ageSlider * 20000f);
                        ____audioMixer.SetFloat("Lowpass_Resonance", 1f + ____ageSlider * 20f);
                        ____audioMixer.SetFloat("Echo_Delay", 800f + ____ageSlider * 200f);
                        ____audioMixer.SetFloat("Echo_Decay", 0.7f - ____ageSlider / 2f);
                        ____audioMixer.SetFloat("Echo_DryMix", 1f - ____ageSlider);
                        ____audioMixer.SetFloat("Echo_WetMix", 0f + ____ageSlider);
                        ____audioMixer.SetFloat("Highpass_Cutoff", 10f);
                        return;
                    case ModdedEffectType.Hazy:
                        ____audioMixer.SetFloat("Master_Pitch", 1f - weight*0.3f);
                        ____audioMixer.SetFloat("Dist_Level", 0f);
                        ____audioMixer.SetFloat("PS_Pitch", 1f);
                        ____audioMixer.SetFloat("Flange_Drymix", 1f);
                        ____audioMixer.SetFloat("Flange_WetMix", weight * 0.5f);
                        ____audioMixer.SetFloat("Flange_Depth", 1f + 2f * weight);
                        ____audioMixer.SetFloat("Flange_Rate", 0.3f + 0.2f * weight);
                        ____audioMixer.SetFloat("Lowpass_Cutoff", 22000f);
                        ____audioMixer.SetFloat("Lowpass_Resonance", 1f);
                        ____audioMixer.SetFloat("Echo_Delay", 500f);
                        ____audioMixer.SetFloat("Echo_Decay", 0f);
                        ____audioMixer.SetFloat("Echo_DryMix", 1f + 2f * weight);
                        ____audioMixer.SetFloat("Echo_WetMix", 0.5f * weight);
                        ____audioMixer.SetFloat("Highpass_Cutoff", 10f);
                        return;
                    default: return;
                }
            }
        }

        [HarmonyPatch(typeof(TenchanView))]
        public static class TenchanViewPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(TenchanView.OnEndStream))]
            public static bool OnEndStreamPrefix()
            {
                return SingletonMonoBehaviour<EventManager>.Instance.nowEnding != (EndingType)(int)ModdedEndingType.Ending_Followers;
            }
        }

        [HarmonyPatch(typeof(PoketterView2D))]
        public static class PoketterView2DPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(PoketterView2D.Start))]
            public static void StartPostfix(ref PoketterView2D __instance)
            {
                PoketterView2D inst = __instance;
                if (!inst.HasComponent<AltPoketter>()) return;
                SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAmeDelete.Where((bool v) => v).Subscribe(delegate (bool _)
                {
                    Alternates.showDeleteModeAme(inst);
                }).AddTo(__instance.gameObject);

            }
            
            [HarmonyPatch]
            public static class ModePatches
            {
                [HarmonyTargetMethods]
                public static IEnumerable<MethodBase> Target()
                {
                    List<MethodBase> bases = new List<MethodBase>() {
                        AccessTools.Method(typeof(PoketterView2D), nameof(PoketterView2D.showBlockMode)),
                        AccessTools.Method(typeof(PoketterView2D), nameof(PoketterView2D.showDeleteMode))
                    };
                    return bases;
                }

                public static bool Prefix(ref PoketterView2D __instance)
                {
                    foreach (Component c in __instance.gameObject.GetComponents<Component>())
                    {
                        NeedyMintsMod.log.LogMessage($"Mode component: {c}");
                    }




                    return !__instance.gameObject.HasComponent<AltPoketter>();
                }
            }
        }


        /*[HarmonyPatch(typeof(DayBaketter))]
        public static class DayBaketterPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DayBaketter.Start))]
            public static void StartPrefix(ref DayBaketter __instance)
            {
                Transform obj = __instance.transform.parent.parent;
                foreach (Component comp in obj.GetComponents<Component>())
                {
                    NeedyMintsMod.log.LogMessage(comp);
                }
                NeedyMintsMod.log.LogMessage($"{obj.parent} ->{obj} -> {obj.childCount}");


                // Testing
                FieldInfo info = typeof(DayPassing2D).GetField(nameof(DayPassing2D.CalendarScroll), BindingFlags.NonPublic | BindingFlags.Instance);
                DayPassing2D p = UnityEngine.Object.FindObjectOfType<DayPassing2D>();
                NeedyMintsMod.log.LogMessage("DayPassing2D: " + (info.GetValue(p) as RectTransform));
            }
        }*/

        /*[HarmonyPatch(typeof(Action_HaishinEnd))]
        public static class Action_HaishinEndPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(Action_HaishinEnd.startEvent))]
            public static void startEventPrefix()
            {
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelWatch)
                {
                    NeedyMintsMod.log.LogMessage("Adding Scenario_AfterAngelWatch!");
                    SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_AfterAngelWatch>();
                }
            }
        }*/
        
        /*
        [HarmonyPatch(typeof(EventManager))]
        public static class EventManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.Save))]
            public static bool SavePre() { return false; }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.Save))]
            public static void SavePost(EventManager __instance, int day)
            {

                string text = string.Format("Data{0}_Day{1}{2}", SingletonMonoBehaviour<Settings>.Instance.saveNumber, day, SaveRelayer.EXTENTION);
                SaveRelayer.SaveSlotData(text, new ModdedSlotData
                {
                    jineHistory = SingletonMonoBehaviour<JineManager>.Instance.history,
                    poketterHistory = SingletonMonoBehaviour<PoketterManager>.Instance.history,
                    eventsHistory = __instance.eventsHistory,
                    dayActionHistory = __instance.dayActionHistory,
                    loop = __instance.loop,
                    midokumushi = __instance.midokumushi,
                    psycheCount = __instance.psycheCount,
                    stats = SingletonMonoBehaviour<StatusManager>.Instance.statuses,
                    havingNetas = SingletonMonoBehaviour<NetaManager>.Instance.GotAlpha,
                    usedNetas = SingletonMonoBehaviour<NetaManager>.Instance.usedAlpha,
                    isJuncho = __instance.isJuncho,
                    isHearTrauma = __instance.isHearTrauma,
                    trauma = __instance.Trauma,
                    firstDate = __instance.FirstDate,
                    isHappaOK = __instance.isHappaOK,
                    isHorror = __instance.isHorror,
                    isGedatsu = __instance.isGedatsu,
                    wishlist = __instance.wishlist,
                    isWristCut = __instance.isWristCut,
                    isHakkyo = __instance.isHakkyo,
                    beforeWristCut = __instance.beforeWristCut,
                    isShurokued = __instance.isShurokued,
                    kyuusiCount = __instance.kyuusiCount,
                    loveDiary = __instance.loveDiary,
                    isOpenGinga = __instance.isOpenGinga,
                    is150mil = __instance.is150mil,
                    is300mil = __instance.is300mil,
                    is500mil = __instance.is500mil,
                    sleepCount = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus((StatusType)ModdedStatusType.SleepyAmeCounter)
                });
                Debug.Log("スロットデータ：" + text + "のセーブが完了しました。");
                __instance.CleanPassingSaveData(text);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.Load))]
            public static bool LoadPre() { return false; }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.Load))]
            public static void LoadPost(EventManager __instance)
            {
                int oldSleepCount = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus((StatusType)ModdedStatusType.SleepyAmeCounter);

                string nowSaveFile = SingletonMonoBehaviour<Settings>.Instance.nowSaveFile;
                __instance.ClearEventQueue();
                SingletonMonoBehaviour<NotificationManager>.Instance.Clean();
                ModdedSlotData slotData = LoadModdedSlotByES3(nowSaveFile);
                SingletonMonoBehaviour<JineManager>.Instance.history = slotData.jineHistory;
                SingletonMonoBehaviour<PoketterManager>.Instance.history = slotData.poketterHistory;
                __instance.eventsHistory = slotData.eventsHistory;
                __instance.dayActionHistory = slotData.dayActionHistory;
                __instance.loop = slotData.loop;
                __instance.midokumushi = slotData.midokumushi;
                __instance.psycheCount = slotData.psycheCount;
                SingletonMonoBehaviour<NetaManager>.Instance.GotAlpha = slotData.havingNetas;
                SingletonMonoBehaviour<NetaManager>.Instance.usedAlpha = slotData.usedNetas;
                __instance.isJuncho = slotData.isJuncho;
                __instance.isHearTrauma = slotData.isHearTrauma;
                __instance.Trauma = slotData.trauma;
                __instance.FirstDate = slotData.firstDate;
                __instance.isHappaOK = slotData.isHappaOK;
                __instance.isHorror = slotData.isHorror;
                __instance.isGedatsu = slotData.isGedatsu;
                __instance.beforeWristCut = slotData.beforeWristCut;
                __instance.isWristCut = slotData.isWristCut;
                __instance.isHakkyo = slotData.isHakkyo;
                __instance.wishlist = slotData.wishlist;
                __instance.loveDiary = slotData.loveDiary;
                __instance.isShurokued = slotData.isShurokued;
                __instance.kyuusiCount = slotData.kyuusiCount;
                __instance.isOpenGinga = slotData.isOpenGinga;
                __instance.is150mil = slotData.is150mil;
                __instance.is300mil = slotData.is300mil;
                __instance.is500mil = slotData.is500mil;
                SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus((StatusType)ModdedStatusType.SleepyAmeCounter, slotData.sleepCount - oldSleepCount);
                List<Status> stats = slotData.stats;
                SingletonMonoBehaviour<StatusManager>.Instance.setNewStatusList(stats);
                if (nowSaveFile == "Data0_Day30" + SaveRelayer.EXTENTION)
                {
                    __instance.AddEvent<Ending_Completed_Day30_afterOut>();
                }
                SingletonMonoBehaviour<WindowManager>.Instance.CleanOnLoad();
                __instance.fetchNextActionHint();
                Debug.Log("スロットデータ：" + nowSaveFile + "のロードが完了しました。");
            }
        }

        [HarmonyPatch(typeof(SaveRelayer))]
        public static class SaveRelayerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(SaveRelayer.SaveSlotByES3))]
            public static void SaveSlotByES3(string fileName, ModdedSlotData data)
            {
                ES3.Save<int>("SLEEPCOUNT", data.sleepCount);
            }
        }
        */
    }
}

