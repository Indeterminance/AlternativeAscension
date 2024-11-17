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
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ComponentModel;

namespace AlternativeAscension
{
    internal partial class AAPatches
    {
        [HarmonyPatch(typeof(StatusManager))]
        public static class StatusManagerPatches
        {
            [HarmonyPatch(nameof(StatusManager.NewStatus))]
            [HarmonyPostfix]
            public static void NewStatusPrefix(StatusManager __instance)
            {
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeCounter.Swap(), 0, 99, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeStressMultiplier.Swap(), 0, 11, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.FollowerPlotFlag.Swap(), 0, 20, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.OdekakeCountdown.Swap(), 2, 2, 0, false));
                __instance.statuses.Add(new Status(ModdedStatusType.AMAStress.Swap(), 0, 99, -99, false));
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(StatusManager.Diffs))]
            public static void DiffsPrefix(CmdMaster.Param cmd, ref StatusManager __instance, ref List<StatusDiff> __result)
            {
                AltAscMod.log.LogMessage($"Diffing!");
                AltAscMod.log.LogMessage($"Getting diff for {cmd?.Id}");
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
                if (type != StatusType.DayIndex || SingletonMonoBehaviour<AltAscModManager>.Instance.lockDayCount) return;

                DayPassing2D dayPassing2D = GameObject.Find("DayPassingCover").GetComponent<DayPassing2D>();
                //if (dayPassing2D.playingAnimation && to == 100) return;

                // Get new maximum
                int oldMax = __instance.statuses.Find(s => s.statusType == type).maxValue.Value;
                int destMax = Mathf.Max(30, to);


                AltAscMod.log.LogMessage($"Updating day max from {oldMax} to {to}");
                if (oldMax == destMax) return;

                // Get info / traversal info
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


                AltAscMod.log.LogMessage($"Calendar children: {Calendar.childCount}");

                if (to > oldMax)
                {
                    // Create new labels
                    for (int i = oldMax + 1; i < to + 1; i++)
                    {
                        Transform newDayObj = UnityEngine.Object.Instantiate(dayPrefab, dayPos + offset * (i - 30), Quaternion.identity, Calendar);
                        //TMP_Text newArrow = UnityEngine.Object.Instantiate(arrowPrefab, arrowPos + offset * (i - 30), Quaternion.identity, Calendar);
                        newDayObj.name = $"Day {i}";
                        AltAscMod.log.LogMessage($"newDayObj childcount: {newDayObj.transform.childCount}");
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
                    if (SingletonMonoBehaviour<AltAscModManager>.Instance.lockDayCount) return;
                    dayPassMethod.Invoke(dayPassing2D, new object[] { dayIndex.Value, 0, 0 });
                    //nonRefDayPass2D.dayPass(dayIndex.Value, 0, 0);
                }).AddTo(dayPassing2D.gameObject);
            }
        }

        [HarmonyPatch(typeof(Action_SleepToTomorrow3))]
        public static class SleepToTomorrowPatches
        {
            /*
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
            }*/
        }

        [HarmonyPatch(typeof(NetaManager))]
        public static class NetaManagerPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(NetaManager.fetchNextActionHint))]
            public static void fetchNextActionPrune(ref List<Tuple<ActionType, AlphaLevel>> __result)
            {
                int sleeps = SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount;

                if (sleeps + 1 >= AltAscMod.SLEEPS_BEFORE_SLEEPY) __result.RemoveAll(t => t.Item1 == ActionType.SleepToTomorrow);
                if (CheckTokyoAvailable())
                {
                    AlphaLevel alpha = new AlphaLevel((AlphaType)(int)ModdedAlphaType.FollowerAlpha, 1);
                    __result.Add(Tuple.Create((ActionType)(int)ModdedActionType.OdekakeTokyo, alpha));
                }
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) >= (int)FollowerPlotFlagValues.PostDepaz)
                {
                    __result.RemoveAll(t => t.Item1 == ActionType.Internet2ch || t.Item1 == (ActionType)(int)ModdedActionType.Internet2chStalk);
                }
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelFuneral)
                {
                    __result.Clear();
                }
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NetaManager.Haishined))]
            public static void HaishinedPrefix(AlphaType alpha, int level)
            {
                AltAscMod.log.LogMessage($"Haishined {alpha} level {level}");
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
                foreach (Jine jine in AltAscMod.DATA.Jines.Jine)
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
                    AltAscMod.log.LogMessage($"Adding jine {newParam.Id} to jines");
                    __result.Add(newParam);
                }
                ___JineRawList = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(JineDataConverter.getJineFromTypeId))]
            public static bool getJineFromTypeIdPrefix(JineType t)
            {
                return !CheckModdedPrefix(typeof(JineType), typeof(ModdedJineType), t).Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(JineDataConverter.getJineFromTypeId))]
            public static void getJineFromTypeIdPostfix(JineType t, ref LineMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(JineType), typeof(ModdedJineType), t).Item1) return;

                ModdedJineType modT = (ModdedJineType)(int)t;
                __result = JineDataConverter.getJineRawList().Find((LineMaster.Param j) => j.Id == modT.ToString());
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

                if (___end == ModdedEndingType.Ending_Followers.Swap())
                {
                    if (!(SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 7 &&
                        SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.AMAStress.Swap()) >= 15))
                    {
                        ____submitButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(ModdedSystemTextType.System_InternetYamero.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                    }
                    else
                    {
                        ____submitButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(ModdedSystemTextType.System_RealYamero.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                    }
                }
                else if (___end == ModdedEndingType.Ending_Love.Swap())
                {
                    ____submitButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(ModdedSystemTextType.System_LoveConfirm.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
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
                AltAscMod.log.LogMessage($"End: {__state}");
                if (__state == 0) return;

                // Get modded reason
                switch (__state)
                {
                    case ModdedEndingType.Ending_Sleepy:
                        __result = AltAscMod.DATA.Endings.Ending.Find(e => e.Id == "Ending_Sleepy").Osimai;
                        break;
                    case ModdedEndingType.Ending_Followers:
                        __result = AltAscMod.DATA.Endings.Ending.Find(e => e.Id == "Ending_Followers").Osimai;
                        break;
                    case ModdedEndingType.Ending_Love:
                        __result = AltAscMod.DATA.Endings.Ending.Find(e => e.Id == "Ending_Love").Osimai;
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
                    v = ((CmdType)(int)tuple.Item2).ToString();
                }
                AltAscMod.log.LogMessage($"{v} modded? {tuple} {type}");


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
                    case ModdedCmdType.OdekakeFuneral:
                        __result = ModdedCommandParams.OdekakeFuneralParam;
                        break;
                    case ModdedCmdType.SleepToEternity:
                        __result = ModdedCommandParams.SleepToEternityParam;
                        break;
                    default:
                        AltAscMod.log.LogMessage("Invalid modded cmd type!");
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
                    case ModdedActionType.OdekakeFuneral:
                        __result = ModdedActionParams.OdekakeFuneralParam;
                        break;
                    case ModdedActionType.SleepToEternity:
                        __result = ModdedActionParams.SleepToEternityParam;
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
            [HarmonyPatch(nameof(NgoEx.CmdName), new Type[] { typeof(CmdType), typeof(LanguageType) })]
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
                //NeedyMintsMod.log.LogMessage($"Outputting {__result} from {Environment.StackTrace}");
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
                __result.Add(ModdedCommandParams.OdekakeFuneralParam);

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

                foreach (Stream stream in AltAscMod.DATA.Streams.Stream)
                {
                    foreach (Speak spk in stream.Dialogue.Speak)
                    {
                        if (!__result.Any(t => t.Id == spk.Id))
                        {
                            TenCommentMaster.Param newParam = new TenCommentMaster.Param()
                            {
                                BodyEn = spk.BodyEN,
                                Id = stream.AlphaType + stream.AlphaLevel + "_" + spk.Id
                            };
                            __result.Add(newParam);
                        }
                    }

                    if (!__result.Any(t => t.Id == stream.AlphaType + stream.AlphaLevel + "_STREAMNAME"))
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
            [HarmonyPatch(nameof(NgoEx.getEndings))]
            public static void getEndingsPrefix(out bool __state, List<EndingMaster.Param> ___Endings)
            {
                __state = ___Endings == null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.getEndings))]
            public static void getEndingsPostfix(bool __state, ref List<EndingMaster.Param> __result, ref List<EndingMaster.Param> ___Endings)
            {
                if (!__state) return;

                foreach (object obj in AltAscMod.DATA.Endings.Ending)

                    foreach (Ending ending in AltAscMod.DATA.Endings.Ending)
                    {
                        if (!__result.Any(t => t.Id == ending.Id))
                        {
                            EndingMaster.Param newParam = new EndingMaster.Param()
                            {
                                Id = ending.Id,
                                EndingNameEn = ending.Name,
                                ReasonEn = ending.Osimai,
                                JissekiEn = ending.JissekiEn
                            };
                            __result.Add(newParam);
                        }
                    }
                ___Endings = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.EndingFromType))]
            public static bool EndingFromTypePrefix(EndingType type, out int __state)
            {
                Tuple<bool, int> tuple = CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), type);

                string v;
                if (tuple.Item1) v = ((ModdedEndingType)tuple.Item2).ToString();
                else
                {
                    //NeedyMintsMod.log.LogMessage($"Found nat cmdtype {(CmdType)tuple.Item2} {type}");
                    v = ((EndingType)(int)tuple.Item2).ToString();
                }
                AltAscMod.log.LogMessage($"{v} modded? {tuple} {type}");


                __state = tuple.Item2;
                return !tuple.Item1;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.EndingFromType))]
            public static void EndingFromTypePostfix(int __state, EndingType type, ref EndingMaster.Param __result)
            {
                if (!CheckModdedPrefix(typeof(EndingType), typeof(ModdedEndingType), __state).Item1) return;

                __result = NgoEx.getEndings().FirstOrDefault((EndingMaster.Param x) => x.Id == ((ModdedEndingType)(int)type).ToString());
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

                foreach (Stream stream in AltAscMod.DATA.Streams.Stream)
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
                foreach (Thread thread in AltAscMod.DATA.ST.Threads)
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
                foreach (Thread thread in AltAscMod.DATA.ST.Threads)
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

                AltAscMod.log.LogMessage($"TESTING: {AltAscMod.DATA.ST.Threads}");
                foreach (Thread thread in AltAscMod.DATA.ST.Threads)
                {

                    SystemTextMaster.Param newParam = new SystemTextMaster.Param()
                    {
                        Id = "Suretai_" + thread.Id,
                        BodyEn = thread.Title.BodyEN
                    };
                    AltAscMod.log.LogMessage($"Adding new system type suretai {newParam.Id}");
                    __result.Add(newParam);
                }

                foreach (NeedyXML.String str in AltAscMod.DATA.SysStrings.Strings)
                {
                    SystemTextMaster.Param newParam = new SystemTextMaster.Param()
                    {
                        Id = str.Id,
                        BodyEn = str.BodyEN
                    };
                    AltAscMod.log.LogMessage($"Adding new system type {newParam.Id}");
                    __result.Add(newParam);
                }
                ___systemTexts = __result;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(NgoEx.SystemTextFromTypeString))]
            public static void SystemTypeFromTextStringPrefix(string type)
            {
                AltAscMod.log.LogMessage($"Attempting to get system type {type}");
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
            public static void SystemTextFromTypePostfix(SystemTextType type, LanguageType lang, bool __state, ref string __result)
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

                AltAscMod.log.LogMessage(id);
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
            [HarmonyPatch(nameof(NgoEx.TenTalk), new Type[] { typeof(string), typeof(LanguageType) })]
            public static void TenTalk2Prefix(string id, LanguageType lang)
            {
                AltAscMod.log.LogMessage($"Tried to get {id}");
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(NgoEx.DayText))]
            public static void DayTextPostfix(LanguageType lang, bool isLive, ref string __result)
            {
                try
                {
                    if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                    {
                        __result = NgoEx.SystemTextFromType(ModdedSystemTextType.System_LoveDay.Swap(), lang);
                    }
                }
                catch
                {
                    return;
                }
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
                __result = AltAscMod.DATA.GetEndingByID(((ModdedEndingType)(int)end).ToString()).Name;
            }

            /*[HarmonyTranspiler]
            [HarmonyPatch(nameof(EndingManager.Awake))]
            public static IEnumerable<CodeInstruction> AwakeTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> ins = instructions.ToList();
                List<CodeInstruction> outs = new List<CodeInstruction>();

                for (int i = 0; i < ins.Count; i++)
                {
                    if (ins[i].opcode == OpCodes.Ldtoken && ins[i].operand == typeof(EndingType) &&
                        ins[i + 2].opcode == OpCodes.Call && (ins[i + 2].operand as MethodInfo)?.Name == nameof(Enum.GetNames))
                    {
                        outs.Add(new CodeInstruction(OpCodes.Call, typeof(MintyOverdosePatches).GetMethod(nameof(MintyOverdosePatches.GetBothEndingNames))));
                        i += 2;
                    }
                    else outs.Add(ins[i]);
                }
                return outs;
            }*/

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EndingManager.Awake))]
            public static void AwakePostfix(GameObject ____endingAchievement, ref EndingManager __instance, GameObject ____achievingBlock,
                                            GameObject ____achievedBlock, GameObject ____unachievedBlock, EndingType ___end)
            {
                Transform endParent = ____endingAchievement.transform;
                EndingBlockMaker(endParent, 36);
                List<EndingType> mitaEnd = SingletonMonoBehaviour<Settings>.Instance.mitaEnd;

                string[] moddedNames = Enum.GetNames(typeof(ModdedEndingType));
                for (int i = 0; i < moddedNames.Length; i++)
                {
                    string id = moddedNames[i];
                    if (id == ((ModdedEndingType)(int)___end).ToString())
                    {
                        global::UnityEngine.Object.Instantiate<GameObject>(____achievingBlock, endParent);
                    }
                    else if (mitaEnd.Exists((EndingType gotend) => ((ModdedEndingType)(int)gotend).ToString() == id))
                    {
                        global::UnityEngine.Object.Instantiate<GameObject>(____achievedBlock, endParent);
                    }
                    else
                    {
                        global::UnityEngine.Object.Instantiate<GameObject>(____unachievedBlock, endParent);
                    }
                }




                foreach (Component comp in endParent.GetComponents<Component>())
                {
                    AltAscMod.log.LogMessage($"EndingManager Comp: {comp}");
                }

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

                bool angelFuneral = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelFuneral;

                if (CheckTokyoAvailable())
                {
                    GameObject obj = __instance.SelectableObjects.First();
                    GameObject OdekakeTokyo = obj as GameObject;
                    ActionButton actionButtonTokyo = OdekakeTokyo.GetComponent<ActionButton>();
                    actionTypeField.SetValue(actionButtonTokyo, (int)ModdedActionType.OdekakeTokyo);
                    OdekakeTokyo.transform.position = new Vector3(0.65f, -2.5f, 100f);
                    new Traverse(actionButtonTokyo).Method(nameof(actionButtonTokyo.SetStatus), new object[] { ActionStatus.Executable });
                    buttons.Add(OdekakeTokyo);
                }
                if (angelFuneral)
                {
                    GameObject obj = __instance.SelectableObjects.First();
                    GameObject OdekakeFuneral = obj as GameObject;
                    ActionButton actionButtonFuneral = OdekakeFuneral.GetComponent<ActionButton>();
                    actionTypeField.SetValue(actionButtonFuneral, (int)ModdedActionType.OdekakeFuneral);
                    OdekakeFuneral.transform.position = new Vector3(0f, 0f, 100f);
                    buttons.Add(OdekakeFuneral);
                    foreach (GameObject button in buttons)
                    {
                        if (button != OdekakeFuneral) button.SetActive(false);
                    }
                    return;
                }

                ActionType replaceOdekakeType = ActionType.None;
                switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()))
                {
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
                    replaceOdekakeType = ModdedActionType.OdekakeBreak.Swap();
                }
                AltAscMod.log.LogMessage($"Found odekake replacement of {replaceOdekakeType}");
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

                NeedyXML.Command com = AltAscMod.DATA.Commands.Command.Find(c => c.Id == ((ModdedActionType)__state).ToString());
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



                return !tuple.Item1 || a == ActionType.Haishin || a == ActionType.OkusuriDaypassModerate || a == ActionType.Internet2ch || a == ActionType.SleepToTomorrow;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(CommandManager.ChooseCommand), new Type[] { typeof(ActionType) })]
            public static void ChooseCommandPost(ActionType a, int __state, ref CmdType __result)
            {
                if (!CheckModdedPrefix(typeof(ActionType), typeof(ModdedActionType), __state).Item1 && a != ActionType.Haishin && a != ActionType.OkusuriDaypassModerate && a != ActionType.Internet2ch && a != ActionType.SleepToTomorrow) return;

                if ((ActionType)__state == ActionType.Haishin)
                {
                    AltAscMod.log.LogMessage($"Found modded haishin!");

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
                    AltAscMod.log.LogMessage($"Took a moderate amount of depaz!");
                    bool usedNaturalHabitat = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.PostComiket;
                    bool isNight = SingletonMonoBehaviour<StatusManager>.Instance.isNight();
                    AltAscMod.log.LogMessage($"Starting Depaz rec dose event with flags {usedNaturalHabitat} {isNight}");
                    if (usedNaturalHabitat && isNight)
                    {
                        __result = (CmdType)(int)ModdedCmdType.OkusuriDaypassStalk;
                    }
                    return;
                }

                else if ((ActionType)__state == ActionType.Internet2ch)
                {
                    AltAscMod.log.LogMessage($"Found 2ch!");
                    int goOutBreak = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap());
                    int plotFlag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                    bool stressBreak = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.Stress) > 100;
                    if (goOutBreak > 2 && plotFlag == (int)FollowerPlotFlagValues.PostDepaz && stressBreak)
                    {
                        __result = (CmdType)(int)ModdedCmdType.Internet2chStalk;
                    }
                    return;
                }

                else if ((ActionType)__state == ActionType.SleepToTomorrow)
                {
                    if (SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount >= AltAscMod.SLEEPS_BEFORE_SLEEPY)
                    {
                        AltAscMod.log.LogMessage($"Found supersleep!");
                        __result = ModdedCmdType.SleepToEternity.Swap();
                    }
                    return;
                }

                switch ((ModdedActionType)__state)
                {
                    case ModdedActionType.OdekakeTokyo:
                        __result = ModdedCmdType.OdekakeTokyo.Swap();
                        break;
                    case ModdedActionType.OkusuriDaypassStalk:
                        __result = ModdedCmdType.OkusuriDaypassStalk.Swap();
                        break;
                    case ModdedActionType.OdekakePanic1:
                        __result = ModdedCmdType.OdekakePanic1.Swap();
                        break;
                    case ModdedActionType.OdekakePanic2:
                        __result = ModdedCmdType.OdekakePanic2.Swap();
                        break;
                    case ModdedActionType.OdekakePanic3:
                        __result = ModdedCmdType.OdekakePanic3.Swap();
                        break;
                    case ModdedActionType.OdekakePanic4:
                        __result = ModdedCmdType.OdekakePanic4.Swap();
                        break;
                    case ModdedActionType.OdekakeBreak:
                        __result = ModdedCmdType.OdekakeBreak.Swap();
                        break;
                    case ModdedActionType.Internet2chStalk:
                        __result = ModdedCmdType.Internet2chStalk.Swap();
                        break;
                    case ModdedActionType.OdekakeFuneral:
                        __result = ModdedCmdType.OdekakeFuneral.Swap();
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
                setLabel.Invoke(__instance, null);

                List<ModdedActionType> newActions = new List<ModdedActionType>()
                {
                    ModdedActionType.OdekakeTokyo,
                    ModdedActionType.OdekakePanic1,
                    ModdedActionType.OdekakePanic2,
                    ModdedActionType.OdekakePanic3,
                    ModdedActionType.OdekakePanic4,
                    ModdedActionType.OdekakeBreak,
                    ModdedActionType.Internet2chStalk,
                    ModdedActionType.OdekakeFuneral,
                    ModdedActionType.SleepToEternity
                };

                SingletonMonoBehaviour<CommandManager>.Instance.commandStatus.Subscribe(delegate (Dictionary<ActionType, ActionStatus> s)
                {
                    foreach (ModdedActionType action in newActions)
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
                    setStatus.Invoke(__instance, new object[] { ActionStatus.Executable });
                }).AddTo(__instance);
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(ActionButton.OnCommand))]
            public static bool OnCommandPrefix(ActionType ___actionType)
            {
                AltAscMod.log.LogMessage($"OnCommand!");
                if (___actionType == ModdedActionType.OdekakeFuneral.Swap())
                {
                    SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Action_OdekakeFuneral>();
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(TooltipManager))]
        public static class TooltipManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(TooltipManager.ShowAction))]
            public static void ShowActionPrefix(ActionType a)
            {
                AltAscMod.log.LogMessage($"Showing tooltip for {a}");
            }
        }

        [HarmonyPatch(typeof(EventManager))]
        public static class EventManagerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchNightEvent))]
            public static bool FetchNightEventPrefix(ref EventManager __instance)
            {
                AltAscMod.log.LogMessage("Fetching night...");
                int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                int plotflag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());

                if (!__instance.isTestScene && day == 1)
                {
                    __instance.AddEvent<Scenario_loop1_day0_night_multi>();
                    return false;
                }

                if (!__instance.isHorror && plotflag == (int)FollowerPlotFlagValues.OdekakeBreak)
                {
                    SingletonMonoBehaviour<StatusManager>.Instance.timePassingToNextMorning();
                    return false;
                }

                if (day == 16 && plotflag == (int)FollowerPlotFlagValues.VisitedComiket &&
                    !(__instance.isWristCut && __instance.beforeWristCut) &&
                    !(__instance.isHakkyo && SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.Stress) == 100))
                {
                    __instance.AddEvent<Event_PostComiket>();
                    return false;
                }
                if (plotflag == (int)FollowerPlotFlagValues.AngelWatch)
                {
                    return false;
                }
                if (plotflag == (int)FollowerPlotFlagValues.AngelDeath)
                {
                    __instance.AddEvent<Scenario_follower_day3_night>();
                    return false;
                }

                if (plotflag == (int)FollowerPlotFlagValues.AngelFuneral)
                {
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchDayEvent))]
            public static bool FetchDayEventPrefix(ref EventManager __instance)
            {
                AltAscMod.log.LogMessage("DayEvent");
                AltAscModManager nmmm = SingletonMonoBehaviour<AltAscModManager>.Instance;
                AltAscMod.log.LogMessage($"Love: {nmmm.isLove} {nmmm.isLoveLoop}");
                int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                int plotflag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                if (day == 2 && AltAscMod.SHOWANIMSTREAM)
                {
                    __instance.AddEvent<Action_HaishinStart>();
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
                else if (plotflag == (int)FollowerPlotFlagValues.AngelWatch)
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
                    if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 7 &&
                        SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.AMAStress.Swap()) >= 15)
                    {
                        __instance.AddEvent<Scenario_follower_day4_night>();
                    }
                    else __instance.AddEvent<Scenario_follower_day4_day>();
                    return false;
                }
                else if (!__instance.isHorror && day == 27 && SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower) >= 500000 &&
                    SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 0)
                {
                    __instance.AddEvent<Event_MV_kowai>();
                    return false;
                }
                else if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLove &&
                         SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                {
                    List<string> events = new List<string>
                    {
                        "Scenario_love_loop1_day",
                        "Scenario_love_loop2_day",
                        "Scenario_love_loop3_day",
                        "Scenario_love_loop4_day",
                        "Scenario_love_loop5_day",
                    };
                    int choice = UnityEngine.Random.Range(0, events.Count);

                    SingletonMonoBehaviour<EventManager>.Instance.AddEvent(events[choice]);
                    return false;
                }
                else if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLove)
                {
                    __instance.AddEvent<Scenario_love_day2_day>();
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

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchDialog))]
            public static bool FetchDialogPrefix()
            {
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelWatch ||
                    SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                {
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.Awake))]
            public static void AwakePrefix(ref EventManager __instance)
            {
                __instance.executingAction = CmdType.None;

                __instance.transform.AddComponent<AltAscModManager>();
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
                     else if (!SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                     {
                         inst.Save(day);
                     }


                     (new Traverse(inst).Method(nameof(EventManager.AwakePsycheDiary))).GetValue();
                     (new Traverse(inst).Method(nameof(EventManager.AwakeLoveDiary))).GetValue();
                     if (!SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop) inst.AddEventQueue<Event_CheckBGM>();
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

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchScenarioEvent))]
            public static bool FetchScenarioEventPrexix(ref bool __result, EventManager __instance)
            {
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) >= (int)FollowerPlotFlagValues.AngelWatch)
                {
                    __result = false;
                    return false;
                }
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLove)
                {
                    __result = false;
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.ExecuteActionConfirmed))]
            public static void ExecuteActionConfirmedPrefix(EventManager __instance, ref ActionType ac, ref CmdType a, bool isEventCommand)
            {
                StatusManager sm = SingletonMonoBehaviour<StatusManager>.Instance;
                if (ac == ActionType.SleepToTomorrow)
                {
                    SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount++;
                }
                else
                {
                    SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount = 0;
                }
                AltAscMod.log.LogMessage($"Sleeps after : {SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount}");
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.CleanPassingSaveData))]
            public static bool CleanPassingSaveDataPrefix(string savefile)
            {
                // Example savefile : Data2_Day32.es3
                Regex dataReg = new Regex(@"Data\d*");
                Regex dayReg = new Regex(@"Day\d*");

                string dataString = dataReg.Match(savefile).Value.Replace("Data", "");
                int dataNum = int.Parse(dataString);

                string dayString = dayReg.Match(savefile).Value.Replace("Day", "");
                int dayNum = int.Parse(dayString);

                Regex rx_get = new Regex(string.Format("Data{0}_Day\\d*\\{1}", dataNum, SaveRelayer.EXTENTION));

                List<string> list = new List<string>();
                List<string> files = Directory.GetFiles(Environment.CurrentDirectory + "/Windose_Data").Where(s => rx_get.IsMatch(s)).ToList();
                files = files.OrderByDescending(s => int.Parse(dayReg.Match(s).Value.Replace("Day", ""))).ToList();
                foreach (string file in files)
                {
                    string saveDay = dayReg.Match(file).Value.Replace("Day", "");
                    if (int.Parse(saveDay) > dayNum)
                    {
                        list.Add(file);
                    }
                    else break;
                }
                SaveRelayer.DeleteDatas(list);
                return false;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.Save))]
            public static bool SavePrefix(int day, EventManager __instance, bool ___isJuncho, bool ___isHearTrauma)
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
                    isJuncho = ___isJuncho,
                    isHearTrauma = ___isHearTrauma,
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
                    sleepCount = SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount,
                    isLove = SingletonMonoBehaviour<AltAscModManager>.Instance.isLove,
                    isLoveLoop = SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop,
                    password = SingletonMonoBehaviour<AltAscModManager>.Instance.password,
                });
                UnityEngine.Debug.Log("スロットデータ：" + text + "のセーブが完了しました。");
                new Traverse(__instance).Method(nameof(EventManager.CleanPassingSaveData), new Type[] { typeof(string) }).GetValue(new object[] { text });
                return false;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.Load))]
            public static void LoadPostfix()
            {
                string nowSaveFile = SingletonMonoBehaviour<Settings>.Instance.nowSaveFile;
                ModdedSlotData slotData = SaveRelayer.LoadSlotData(nowSaveFile) as ModdedSlotData;

                if (slotData.sleepCount == null) return;

                SingletonMonoBehaviour<AltAscModManager>.Instance.sleepCount = slotData.sleepCount;
                SingletonMonoBehaviour<AltAscModManager>.Instance.isLove = slotData.isLove;
                SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop = slotData.isLoveLoop;
                SingletonMonoBehaviour<AltAscModManager>.Instance.password = slotData.password;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(EventManager.StartOver))]
            public static void StartOverPostfix()
            {
                AltAscModManager nmmm = SingletonMonoBehaviour<AltAscModManager>.Instance;
                nmmm.sleepCount = 0;
                nmmm.isLove = false;
                nmmm.isLoveLoop = false;
                nmmm.password = "angelkawaii2";
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.chooseMaturo))]
            public static bool chooseMaturoPrefix(ref EventManager __instance)
            {
                StatusManager sm = SingletonMonoBehaviour<StatusManager>.Instance;
                if (sm.GetMaxStatus(StatusType.Love) == 120 && sm.GetStatus(StatusType.Love) >= 100 && __instance.midokumushi == 0)
                {
                    __instance.SetShortcutState(false, 0.4f);
                    __instance.AddEvent<EndingSeparator>();
                    __instance.AddEvent<Scenario_love_day1_day>();
                    return false;
                }
                return true;
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(EventManager.FetchUzagarami))]
            public static bool FetchUzagaramiPrefix()
            {
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                {
                    return false;
                }
                return true;
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

                foreach (Tweet twt in AltAscMod.DATA.Tweets.Tweet)
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

                foreach (KusoRep kuso in AltAscMod.DATA.KusoReps.KusoRep)
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

                __result = (info.Invoke(null, null) as List<TweetMaster.Param>).Find((TweetMaster.Param tw) => tw.Id == mt.ToString());
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
                    AltAscMod.log.LogMessage($"TweetData for {tw.Type} is {__result}");
                    return;
                }
                __result = TweetFetcher.ConvertTypeToTweet(tw.Type).UraBodyEn.IsNotEmpty();
                AltAscMod.log.LogMessage($"TweetData for {tw.Type} is {__result}");
            }

        }

        [HarmonyPatch(typeof(LoadNetaData))]
        public static class LoadNetaDataPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(LoadNetaData.ReadNetaContent))]
            public static void ReadNetaContentPostfix(AlphaType NetaType, int level, ref AlphaTypeToData __result)
            {
                if (!CheckModdedPrefix(typeof(AlphaType), typeof(ModdedAlphaType), NetaType).Item1) return;

                Stream stream = AltAscMod.DATA.Streams.Stream.First(s => s.AlphaType == ((ModdedAlphaType)(int)NetaType).ToString() && s.AlphaLevel == (level).ToString());

                ModdedTenCommentType tenComment = (ModdedTenCommentType)Enum.Parse(typeof(ModdedTenCommentType), stream.AlphaType + stream.AlphaLevel + "_STREAMNAME");
                AlphaTypeToData data = new AlphaTypeToData()
                {
                    NetaType = NetaType,
                    level = level,
                    NetaGenreJP = "Genre",
                    NetaNameJP = "Name",
                    getJouken = (ActionType)(int)Enum.Parse(typeof(ModdedActionType), stream.ActionType),
                    netaGenre = (CmdType)(int)Enum.Parse(typeof(ModdedCmdType), stream.CmdType),
                    netaName = (TenCommentType)(int)tenComment
                };

                AltAscMod.log.LogMessage($"Read neta content with netaname {data.netaName}");
                __result = data;
            }
        }

        [HarmonyPatch(typeof(NgoEvent))]
        public static class NgoEventPatches
        {
            [HarmonyTargetMethods]
            public static IEnumerable<MethodInfo> GetMethods()
            {
                return typeof(NgoEvent).GetMethods().Where(m => m.Name == nameof(NgoEvent.startEvent));
            }

            [HarmonyPrefix]
            public static void startEventPrefix(NgoEvent __instance)
            {
                AltAscMod.log.LogMessage($"Starting event: {__instance.eventName} / {__instance.GetType().Name}");
            }
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
                    AltAscMod.log.LogMessage("SetScenario patch!");
                    int plotFlag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                    int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                    AltAscMod.log.LogMessage($"Haishin plotflag value: {(FollowerPlotFlagValues)plotFlag}");

                    bool streamValue = (FollowerPlotFlagValues)plotFlag == FollowerPlotFlagValues.AngelFuneral ||
                                       (FollowerPlotFlagValues)plotFlag == FollowerPlotFlagValues.AngelWatch ||
                                       (FollowerPlotFlagValues)plotFlag == FollowerPlotFlagValues.AngelDeath ||
                                       ((FollowerPlotFlagValues)plotFlag == FollowerPlotFlagValues.VisitedComiket && day == 16) ||
                                       SingletonMonoBehaviour<AltAscModManager>.Instance.isLove ||
                                       (day == 2 && AltAscMod.SHOWANIMSTREAM);

                    __state = streamValue;

                    return !__state;
                }

                public static void Postfix(ref Live __instance, ref LiveScenario __result, bool __state)
                {
                    AltAscMod.log.LogMessage($"Follower stream: {__state}");
                    if (__state)
                    {
                        SingletonMonoBehaviour<StatusManager>.Instance.isTodayHaishined = true;
                        SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(StatusType.RenzokuHaishinCount, 1);

                        int plotFlag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                        int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                        AltAscMod.log.LogMessage($"Day: {day}");
                        AltAscMod.log.LogMessage($"Haishin plot status: {(FollowerPlotFlagValues)plotFlag}");
                        AltAscMod.log.LogMessage($"Haishin love: {SingletonMonoBehaviour<AltAscModManager>.Instance.isLove}");
                        if (day == 2 && AltAscMod.SHOWANIMSTREAM)
                        {
                            __result = __instance.SetScenario<Test_ShowAllAnim>();
                        }
                        else if (plotFlag == (int)FollowerPlotFlagValues.VisitedComiket && day == 16)
                        {
                            __result = __instance.SetScenario<Haishin_Comiket>();
                        }
                        else if (plotFlag == (int)FollowerPlotFlagValues.AngelWatch)
                        {
                            __result = __instance.SetScenario<Haishin_AngelWatch>();
                        }
                        else if (plotFlag == (int)FollowerPlotFlagValues.AngelDeath)
                        {
                            __result = __instance.SetScenario<Haishin_AngelDeath>();
                        }
                        else if (plotFlag == (int)FollowerPlotFlagValues.AngelFuneral)
                        {
                            if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 7 &&
                                SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.AMAStress.Swap()) >= 15)
                            {
                                __result = __instance.SetScenario<Haishin_AngelFuneral_dark>();
                            }
                            else __result = __instance.SetScenario<Haishin_AngelFuneral>();
                        }
                        else if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLove)
                        {
                            if (day > 30)
                            {
                                __result = __instance.SetScenario<Haishin_LoveFinale>();
                                return;
                            }
                            __result = __instance.SetScenario<Haishin_Love>();
                        }
                        return;
                    }



                    AltAscMod.log.LogMessage($"SetScenarioPostfix!");
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
                if (__instance.isUncontrollable && __instance.isOiwai && SingletonMonoBehaviour<AltAscModManager>.Instance.isAMA)
                {
                    AltAscMod.log.LogMessage($"AMA reroute!");
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
                AltAscMod.log.LogMessage("RefreshYomuCommentLabelPostfix");
                LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.isAMA)
                {
                    SystemTextType type = ModdedSystemTextType.System_AMARead.Swap();
                    string text = NgoEx.SystemTextFromType(type, lang);
                    __instance.CommentLabel.text = text;
                    return false;
                }
                else if (SingletonMonoBehaviour<EventManager>.Instance.nowEnding == ModdedEndingType.Ending_Followers.Swap())
                {
                    SystemTextType type = ModdedSystemTextType.System_NoRead.Swap();
                    string text = NgoEx.SystemTextFromType(type, lang);
                    __instance.CommentLabel.text = text;
                    return false;
                }
                else return true;
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

            [HarmonyPostfix]
            [HarmonyPatch(nameof(Live.UpdateDetail))]
            public static void UpdateDetail(ref Live __instance, ref TMP_Text ___haisinDetail, int ___watcher, LanguageType ____lang)
            {
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay != 0)
                {

                    string day;
                    if (SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay != -1)
                    {
                        day = NgoEx.DayText(SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay, ____lang);
                    }
                    else day = $"{NgoEx.SystemTextFromType(SystemTextType.Day_Live, ____lang)} ????";



                    ___haisinDetail.text = string.Format("{0} {1} ・ {2} {3}", new object[]
                    {
                        ___watcher,
                        NgoEx.SystemTextFromType(SystemTextType.Haisin_Watching_Number, ____lang),
                        NgoEx.SystemTextFromType(SystemTextType.Haisin_Started_Day, ____lang),
                        day
                    });
                }
            }
            /*[HarmonyFinalizer]
            [HarmonyPatch(nameof(Live.Awake))]
            public static Exception AwakeFinalizer(Exception __exception)
            {
                if (__exception != null ) NeedyMintsMod.log.LogMessage(__exception.Message);
                return null;
            }*/

            [HarmonyPostfix]
            [HarmonyPatch(nameof(Live.bgView))]
            public static void bgViewPostfix(ref Live __instance)
            {
                AltAscModManager nmmm = SingletonMonoBehaviour<AltAscModManager>.Instance;
                AltAscMod.log.LogMessage($"BGVIEW: {SingletonMonoBehaviour<EventManager>.Instance.nowEnding}");
                if (SingletonMonoBehaviour<EventManager>.Instance.nowEnding == ModdedEndingType.Ending_Followers.Swap())
                {
                    if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 7 &&
                        SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.AMAStress.Swap()) >= 15)
                    {
                        __instance.Tenchan._backGround.sprite = nmmm.followerDarkEndBG;
                    }
                    else
                    {
                        __instance.Tenchan._backGround.sprite = nmmm.followerEndBG;
                    }
                    return;
                }
            }
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
                if (!CheckModdedPrefix(typeof(SuperchatType), typeof(ModdedSuperchatType), playing.color).Item1) return true;
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
                        AltAscMod.log.LogMessage($"Attempted to set content of color {playing.color.Swap()}");
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
                bool askedAlready = SingletonMonoBehaviour<AltAscModManager>.Instance.playedQuestions.Contains(playing);
                bool isAMAChat = playing.henji != "";
                AltAscMod.log.LogMessage($"hirou activated for {playing}, while askedAlready is {askedAlready} and isAMAChat is {isAMAChat} with color {playing.color}");
                if (!SingletonMonoBehaviour<AltAscModManager>.Instance.isAMA || askedAlready ||
                    !isAMAChat || !live.isOiwai) return;

                //NeedyMintsMod.log.LogMessage($"Selected AMA \"{playing.nakami} with status {playing.diffStatusType} {playing.delta}\"");
                //NeedyMintsMod.log.LogMessage($"AMA response is \"{playing.henji}\" with anim {playing.henjiAnim}");

                // AMA stress system
                if (playing.diffStatusType == ModdedStatusType.AMAStress.Swap())
                {
                    if (playing.delta != -99)
                    {
                        SingletonMonoBehaviour<AltAscModManager>.Instance.StressDelta += playing.delta;
                    }
                    else
                    {
                        SingletonMonoBehaviour<AltAscModManager>.Instance.deleteComment = __instance;
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
                    AltAscMod.log.LogMessage("No AMAs currently in queue!");
                }

                // Split multi-message responses and their associated expressions
                string[] henjiAnims = playing.henjiAnim.Split(new string[] { "___" }, StringSplitOptions.None).Reverse().ToArray();
                string[] henjis = playing.henji.Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
                for (int i = 0; i < henjis.Length; i++)
                {
                    live.NowPlaying.playing.Insert(newAMAPos, new Playing(true, henjis[i], ModdedStatusType.AMAStatus.Swap(), 1, 0, "", "", (i < henjiAnims.Length) ? henjiAnims[i] : "", playing.isLoopAnim, SuperchatType.White, false, ""));
                }

                // Add anticomment
                live.NowPlaying.playing.Insert(newAMAPos, new Playing(true, "", ModdedStatusType.AMAStatus.Swap(), 1, 0, "", "", playing.henjiAnim, true, SuperchatType.White, true, playing.nakami));

                // Register AMA as asked
                SingletonMonoBehaviour<AltAscModManager>.Instance.playedQuestions.Add(playing);


                // Change color of message to show its already been asked
                Traverse trav = new Traverse(__instance).Field(nameof(LiveComment.bgDefault));
                trav.SetValue((Color)trav.GetValue() * 0.8f);
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(LiveComment.highlighted))]
            public static void highlightedPostfix(LiveComment __instance)
            {
                if (!SingletonMonoBehaviour<AltAscModManager>.Instance.isAMA) return;


                Live live = typeof(LiveComment).GetField(nameof(LiveComment._live), BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance) as Live;
                if (live.isUncontrollable && live.isOiwai && !SingletonMonoBehaviour<AltAscModManager>.Instance.playedQuestions.Contains(__instance.playing))
                {
                    AltAscMod.log.LogMessage($"AMA reroute!");
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
                AltAscMod.log.LogMessage($"Playing {context.nakami}");


                if (!CheckModdedPrefix(typeof(SuperchatType), typeof(ModdedSuperchatType), context.color).Item1) return true;

                Live live = UnityEngine.Object.FindObjectOfType<Live>();

                switch (context.color.Swap())
                {
                    case ModdedSuperchatType.AMA_START:
                        SingletonMonoBehaviour<AltAscModManager>.Instance.StartAMA(ref __instance);
                        break;

                    case ModdedSuperchatType.AMA_END:
                        SingletonMonoBehaviour<AltAscModManager>.Instance.FinishAMA(ref __instance);
                        break;

                    case ModdedSuperchatType.EVENT_TIMEPASS:
                        SingletonMonoBehaviour<StatusManager>.Instance.timePassing(context.delta);
                        break;

                    case ModdedSuperchatType.JINE_INIT:
                        IWindow jine = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine);
                        jine.Uncloseable();
                        SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
                        jine.GameObjectTransform.position = Vector3.zero;
                        break;

                    case ModdedSuperchatType.JINE_DESTROY:
                        SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Jine);
                        break;

                    case ModdedSuperchatType.JINE_SEND:
                        ModdedJineType sendType;
                        bool foundJINE = Enum.TryParse<ModdedJineType>(context.nakami, false, out sendType);
                        JineData data = new JineData((JineType)(int)sendType);
                        SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(data);

                        AltAscMod.log.LogMessage($"Jine send anim: {context.animation}");

                        if (context.animation != "" && data.user == JineUserType.ame)
                        {
                            live.ChotenAnimation(context.animation, context.isLoopAnim);
                        }
                        //__instance.playing.Insert(0, new Playing(true, context.nakami, StatusType.Tension, 1, 0, "", "", "", false, (SuperchatType)(int)ModdedSuperchatType.JINE_WAIT, false, ""));
                        break;

                    case ModdedSuperchatType.JINE_CHOICE:
                        string[] choiceStrings = context.nakami.Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries);
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
                        AltAscMod.log.LogMessage($"Created pill {pill}");
                        pill.setRandomPosition();
                        SingletonMonoBehaviour<AltAscModManager>.Instance.pills.Add(pill);
                        break;

                    case ModdedSuperchatType.EVENT_DOSE:
                        IWindow takePill = SingletonMonoBehaviour<AltAscModManager>.Instance.pills.Last();
                        SheetView sheet = takePill.nakamiApp.GetComponentInChildren<SheetView>();

                        takePill.Touched();
                        sheet.OnDose();
                        if (sheet.CurrentDoseCount.Value >= 3)
                        {
                            SingletonMonoBehaviour<AltAscModManager>.Instance.pills.Remove(takePill);
                            SingletonMonoBehaviour<WindowManager>.Instance.Close(takePill);
                        }
                        else
                        {
                            __instance.playing.Insert(0, new Playing(true, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_DOSE.Swap(), false, ""));
                        }
                        break;

                    case ModdedSuperchatType.EVENT_SHADERWAIT:

                        bool shaderCompleted = SingletonMonoBehaviour<AltAscModManager>.Instance.anim.IsComplete();

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
                            SingletonMonoBehaviour<AltAscModManager>.Instance.anim = DOTween.To(() => weight, delegate (float x)
                            {
                                PostEffectManager.Instance.SetShaderWeight(x);
                            }, endVal, duration).SetEase(Ease.InExpo).SetAutoKill(false);

                            SingletonMonoBehaviour<AltAscModManager>.Instance.anim.Play<TweenerCore<float, float, FloatOptions>>();
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

                        string[] strs = context.nakami.Split("_".ToCharArray());

                        switch (strs[0])
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
                        if (strs.Length > 1 && strs[1] == "EYES")
                        {
                            SingletonMonoBehaviour<AltAscModManager>.Instance.ChangeBG();
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
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex) == 2 && AltAscMod.SHOWANIMSTREAM)
                {
                    UnityEngine.UI.Image im = new Traverse(__instance).Field(nameof(LiveScenario._Live)).Field(nameof(Live.Tenchan)).GetValue<TenchanView>().GetComponent<UnityEngine.UI.Image>();
                    //im.sprite.texture.EncodeToPNG();
                    //__instance.BASESPEED = 1;
                    Console.ReadKey();
                    return;
                }

                AltAscMod.log.LogMessage($"PlayPostfix!");
                AltAscModManager nmmm = SingletonMonoBehaviour<AltAscModManager>.Instance;
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.deleteComment != null && context.antiComment == nmmm.deleteComment.playing.nakami)
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
                        __instance.BASESPEED = 30; //30
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
                        AltAscMod.log.LogMessage($"Delaying for {context.delta} ms");
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

                    AltAscMod.log.LogMessage($"deleteComment {nmmm.deleteComment}");
                    AltAscMod.log.LogMessage($"deleteTime {nmmm.deleteTime}");

                    if (nmmm.deleteComment != null && nmmm.deleteTime)
                    {
                        //AMAManager.deleteComment.honbun = "";
                        nmmm.deleteComment.honbunView.text = "";
                        nmmm.deleteComment = null;
                        nmmm.deleteTime = false;
                    }

                }
                __instance.BASESPEED = (int)(speed * Mathf.Pow(0.8f, (float)live.Speed));

                AltAscMod.log.LogMessage($"Adjusted BASESPEED for AMA question to {__instance.BASESPEED}");
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
                //NeedyMintsMod.log.LogMessage(Environment.StackTrace);
                AltAscMod.log.LogMessage($"Type {typeName} requested");
                if (typeName.StartsWith("ngov3.") && __result == null)
                {
                    if (typeName.EndsWith("_0"))
                    {
                        AltAscMod.log.LogMessage("Found lvl 0 stream");
                        //NeedyMintsMod.log.LogError(System.Environment.StackTrace);
                    }
                    string tmpTypeName = typeName.Replace("ngov3.", nameof(AlternativeAscension)+".");
                    AltAscMod.log.LogMessage($"Temp type name: {tmpTypeName}");
                    Type nmoType = Type.GetType(tmpTypeName);
                    if (nmoType != null)
                    {
                        __result = nmoType;
                        return;
                    }
                    Regex rx = new Regex(@"_\d*");
                    string intStr = rx.Match(tmpTypeName).Value.Replace("_", "");

                    int val = int.Parse(intStr);
                    string moddedType = null;
                    if (tmpTypeName.StartsWith(nameof(AlternativeAscension) +".Netatip") ||
                        tmpTypeName.StartsWith(nameof(AlternativeAscension) +".ChipGet"))
                    {

                        moddedType = "_" + (ModdedAlphaType)val;

                    }
                    else if (tmpTypeName.StartsWith(nameof(AlternativeAscension) + ".Action")) moddedType = "_" + (ModdedCmdType)val;



                    string finalType = rx.Replace(tmpTypeName, moddedType, 1);
                    __result = Type.GetType(finalType);
                    AltAscMod.log.LogMessage($"Failed type get of {typeName} got replaced with {finalType} with return value {__result}");
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
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Shortcut.Start))]
            public static void StartPostfix(ref Shortcut __instance)
            {
                Shortcut inst = __instance;
                Alternates.ShortcutStartAlternate(inst);
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
                bool retryLoad = false;
                FieldInfo app2DataInfo = typeof(LoadAppData).GetField(nameof(LoadAppData.app2data), BindingFlags.NonPublic | BindingFlags.Static);
                AppTypeToDataAsset app2data = app2DataInfo.GetValue(null) as AppTypeToDataAsset;

                FieldInfo mergedAppsInfo = typeof(AppTypeToDataAsset).GetField(nameof(AppTypeToDataAsset.mergedApps), BindingFlags.Instance | BindingFlags.NonPublic);
                object mergedAppstmp = mergedAppsInfo.GetValue(app2data);
                //AltAscMod.log.LogMessage($"HELPME {appType} {__result}");
                List<AppTypeToData> mergedApps;

                if (mergedAppstmp == null)
                {
                    mergedApps = new List<AppTypeToData>();
                }
                else mergedApps = (mergedAppstmp as IEnumerable<AppTypeToData>).ToList();


                AppTypeToData login = mergedApps.Find(App => App.appType == AppType.Login);

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.Follower_taiki))
                {
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
                    UnityEngine.Object.DontDestroyOnLoad(followTaiki.InnerContent);
                    //AltAscMod.log.LogMessage($"Taiki content: {taiki.InnerContent}");
                    Transform trans = followTaiki.InnerContent.transform.GetChild(0).GetChild(5);

                    GameObject.Destroy(trans.GetChild(0).gameObject);

                    mergedApps.Add(followTaiki);
                    retryLoad = appType == (AppType)(int)ModdedAppType.Follower_taiki || retryLoad;

                }

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.AltPoketter))
                {
                    AppTypeToData poketter = mergedApps.Find(App => App.appType == AppType.Poketter);
                    AppTypeToData altPoketter = new AppTypeToData(true)
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
                        InnerContent = UnityEngine.Object.Instantiate(poketter.InnerContent)
                    };
                    UnityEngine.Object.DontDestroyOnLoad(altPoketter.InnerContent);
                    PoketterView2D view = altPoketter.InnerContent.GetComponent<PoketterView2D>();
                    //AltAscMod.log.LogMessage($"Poketter component: {view}");

                    AltPoketter newPoke = altPoketter.InnerContent.AddComponent<AltPoketter>();

                    mergedApps.Add(altPoketter);
                    retryLoad = appType == (AppType)(int)ModdedAppType.AltPoketter || retryLoad;
                }


                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.PillDaypass_Follower))
                {
                    AppTypeToData depaz = mergedApps.Find(App => App.appType == AppType.PillDypass);
                    AppTypeToData horrorDepaz = new AppTypeToData(false)
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
                    };
                    UnityEngine.Object.DontDestroyOnLoad(horrorDepaz.InnerContent);

                    mergedApps.Add(horrorDepaz);
                    retryLoad = appType == (AppType)(int)ModdedAppType.PillDaypass_Follower || retryLoad;
                }

                if (!mergedApps.Any(app => app.appType == (AppType)(int)ModdedAppType.ScriptableLogin))
                {
                    // TEST STUFF: Get login object :)
                    //ExploreGameObject(login.InnerContent);


                    AppTypeToData hackedLogin = new AppTypeToData(true)
                    {
                        appIcon = login.appIcon,
                        AppName = login.AppName,
                        AppNameJP = login.AppNameJP,
                        appType = (AppType)(int)ModdedAppType.ScriptableLogin,
                        FirstHeight = login.FirstHeight,
                        FirstWidth = login.FirstWidth,
                        FirstPosX = login.FirstPosX,
                        FirstPosY = login.FirstPosY,
                        is2DWindow = login.is2DWindow,
                        InnerContent = UnityEngine.Object.Instantiate(login.InnerContent)
                    };

                    Login loginComponent = hackedLogin.InnerContent.GetComponent<Login>();
                    ScriptableLogin hackedComponent = hackedLogin.InnerContent.AddComponent<ScriptableLogin>();
                    Traverse loginTrav = new Traverse(loginComponent);

                    hackedComponent._badge                    = loginTrav.Field(nameof(Login._badge)).GetValue<GameObject>();
                    hackedComponent._input                    = loginTrav.Field(nameof(Login._input)).GetValue<TMP_InputField>();
                    hackedComponent._login                    = loginTrav.Field(nameof(Login._login)).GetValue<UnityEngine.UI.Button>();
                    hackedComponent._passText                 = loginTrav.Field(nameof(Login._passText)).GetValue<TMP_Text>();
                    hackedComponent._placeholderText          = loginTrav.Field(nameof(Login._placeholderText)).GetValue<TMP_Text>();
                    hackedComponent._placeholderText.transform.position += new Vector3(0, -50, 0);
                    hackedComponent._placeholderText.fontSize = 16;
                    hackedComponent._placeholderText.color    = Color.red;
                    hackedComponent._imageContainer           = hackedLogin.InnerContent.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();
                    hackedComponent.baseImage                 = (Sprite)hackedLogin.InnerContent.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
                    hackedComponent.baseImageBad              = Addressables.LoadAssetAsync<Sprite>("login_bad.png").WaitForCompletion();
                    hackedComponent.invalidPasswordImage      = Addressables.LoadAssetAsync<Sprite>("login_invalid.png").WaitForCompletion();
                    hackedComponent.invalidPasswordImageBad   = Addressables.LoadAssetAsync<Sprite>("login_bad_invalid.png").WaitForCompletion();
                    hackedComponent._imageContainer.sprite    = hackedComponent.baseImage;

                    UnityEngine.Object.DontDestroyOnLoad(hackedLogin.InnerContent);
                    GameObject.Destroy(loginComponent);

                    mergedApps.Add(hackedLogin);
                    retryLoad = appType == (AppType)(int)ModdedAppType.Follower_taiki || retryLoad;
                }

                if (!mergedApps.Any(app => app.appType == ModdedAppType.ChangePassword.Swap()))
                {
                    AppTypeToData changePassApp = new AppTypeToData(true)
                    {
                        appIcon = login.appIcon,
                        AppName = ModdedSystemTextType.System_ChangePasswordApp.Swap(),
                        AppNameJP = null,
                        appType = ModdedAppType.ChangePassword.Swap(),
                        FirstHeight = login.FirstHeight, //422
                        FirstWidth = login.FirstWidth, // 641
                        FirstPosX = login.FirstPosX,
                        FirstPosY = login.FirstPosY,
                        is2DWindow = login.is2DWindow,
                        InnerContent = Addressables.LoadAssetAsync<GameObject>("App_ChangePassword.prefab").WaitForCompletion()
                    };

                    changePassApp.InnerContent.AddComponent<ChangePassword>();
                    UnityEngine.Object.DontDestroyOnLoad(changePassApp.InnerContent);

                    //AltAscMod.log.LogMessage("ChangePass render mode: " + changePassApp.InnerContent.GetComponent<Canvas>().renderMode);
                    //AltAscMod.log.LogMessage("ChangePass image type: " + changePassApp.InnerContent.GetComponent<Image>().type);

                    mergedApps.Add(changePassApp);
                    retryLoad = appType == (AppType)(int)ModdedAppType.PillDaypass_Follower || retryLoad;
                }

                login.InnerContent.transform.GetChild(0).gameObject.SetActive(false);


                mergedAppsInfo.SetValue(app2data, mergedApps);
                app2DataInfo.SetValue(null, app2data);

                if (__result == null || retryLoad)
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

            [HarmonyPrefix]
            [HarmonyPatch(nameof(ngov3.Effect.DayPassing2D.yearsPass))]
            public static void yearsPassPrefix()
            {
                SingletonMonoBehaviour<AltAscModManager>.Instance.lockDayCount = true;
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

            [HarmonyPrefix]
            [HarmonyPatch(nameof(DayView.UpdateIcon))]
            public static bool UpdateIconPrefix(int index, ref Image ____dayIcon)
            {
                AltAscMod.log.LogMessage($"Updating icon...");
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                {
                    ____dayIcon.sprite = SingletonMonoBehaviour<AltAscModManager>.Instance.desktop_icon_love;
                    return false;
                }
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(DayView.Start))]
            public static void StartPostfix(ref DayView __instance)
            {
                DayView inst = __instance;
                SingletonMonoBehaviour<AltAscModManager>.Instance.ObserveEveryValueChanged((AltAscModManager n) => n.isLoveLoop, FrameCountType.Update, false).Subscribe(delegate (bool b)
                {
                    int dayPart = new Traverse(inst).Field(nameof(DayView.dayPart)).GetValue<Status>().currentValue.Value;
                    new Traverse(inst).Method(nameof(DayView.UpdateIcon), new object[] { dayPart }).GetValue();
                }).AddTo(__instance.gameObject);
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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");


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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");

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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");

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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");
                AltAscMod.log.LogMessage($"Setting data: {nakami.FavNumber} favs & {nakami.RtNumber} rts");


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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");

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
                //AltAscMod.log.LogMessage($"Tweet day max: {dayMax}");

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
                SingletonMonoBehaviour<AltAscModManager>.Instance.isAmeDelete.Where((bool v) => v).Subscribe(delegate (bool _)
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

            [HarmonyPrefix]
            [HarmonyPatch(nameof(Boot.Awake))]
            public static void AwakePrefix(Boot __instance)
            {
                __instance.gameObject.AddComponent<AltBoot>();
            }

            [HarmonyPrefix]
            [HarmonyPatch(nameof(Boot.waitAccept))]
            public static bool waitAcceptPrefix(ref Boot __instance, CanvasGroup ___Login, CanvasGroup ___ChooseDay, CanvasGroup ___ChooseUser, CanvasGroup ___Welcome,
                                                Button ___Ok, Button ___Cancel, TMP_Text ___Caution_Header, TMP_Text ___Caution_Nakami)
            {
                AltBoot altBoot = __instance.GetComponent<AltBoot>();
                Boot instance = __instance;
                if (altBoot.shownModIntro) return true;
                Debug.Log("waitAcceptModded:1");
                AudioManager.Instance.PlaySeByType(SoundType.SE_Boot_Caution, false);
                AudioManager.Instance.PlayBgmById("BGM_OP_PV", true);
                ___Login.alpha = 1f;
                ___Login.interactable = true;
                ___ChooseDay.alpha = 0f;
                ___ChooseDay.interactable = false;
                ___ChooseDay.blocksRaycasts = false;
                ___ChooseUser.alpha = 0f;
                ___ChooseUser.interactable = false;
                ___ChooseUser.blocksRaycasts = false;
                ___Welcome.alpha = 0f;
                ___Welcome.interactable = false;
                ___Welcome.blocksRaycasts = false;
                if (SingletonMonoBehaviour<ControllerGuideManager>.Instance != null)
                {
                    SingletonMonoBehaviour<ControllerGuideManager>.Instance.IsReady = true;
                }
                Debug.Log("waitAcceptModded:2");
                Rect r = (___Caution_Header.transform as RectTransform).rect;
                r.width = 999;
                ___Caution_Header.overflowMode = TextOverflowModes.Overflow;
                ___Caution_Header.enableWordWrapping = false;
                ___Caution_Header.text = NgoEx.SystemTextFromType(ModdedSystemTextType.AltAsc_ModTitle.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                ___Caution_Nakami.text = NgoEx.SystemTextFromType(ModdedSystemTextType.Boot_ModIntro.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);

                Vector3 returnPos = ___Ok.transform.position;
                Vector3 centerPos = (___Ok.transform.position + ___Cancel.transform.position) / 2f;



                ___Ok.transform.position = centerPos;
                ___Ok.OnClickAsObservable().Subscribe(delegate (Unit _)
                {
                    if (altBoot.shownModIntro) return;
                    ___Ok.transform.position = returnPos;
                    ___Cancel.gameObject.SetActive(true);
                    Debug.Log("waitAcceptModded:3");
                    altBoot.shownModIntro = true;
                    new Traverse(instance).Method(nameof(Boot.waitAccept)).GetValue();
                }).AddTo(___Ok);
                ___Cancel.gameObject.SetActive(false);
                Debug.Log("waitAcceptModded:END");
                return false;
            }

            /*[HarmonyTranspiler]
            [HarmonyPatch(nameof(Boot.ShowEnds))]
            public static IEnumerable<CodeInstruction> ShowEndsTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> ins = instructions.ToList();
                List<CodeInstruction> outs = new List<CodeInstruction>();
                for (int i = 0; i < ins.Count; i++)
                {
                    CodeInstruction instruction = ins[i];
                    if (instruction.opcode == OpCodes.Ldtoken && instruction.operand == typeof(EndingType) &&
                        ins[i + 2].opcode == OpCodes.Call && (ins[i + 2].operand as MethodInfo)?.Name == nameof(Enum.GetValues))
                    {
                        outs.Add(new CodeInstruction(OpCodes.Call, typeof(MintyOverdosePatches).GetMethod(nameof(MintyOverdosePatches.GetBothEndings))));
                        i += 2;
                        NeedyMintsMod.log.LogMessage($"End iterator found!");
                    }
                    else outs.Add(instruction);
                }
                foreach (CodeInstruction inst in outs)
                {
                    NeedyMintsMod.log.LogMessage($"ShowEnds : {inst.opcode} > {inst.operand}");
                }
                return outs;
            }*/

            [HarmonyPostfix]
            [HarmonyPatch(nameof(Boot.ShowEnds))]
            public static void ShowEndsPostfix(Boot __instance, ref Transform ___endingParent, GameObject ____achievedBlock, GameObject ____unachievedBlock, ref List<GameObject> ___endingBlocks)
            {
                EndingBlockMaker(___endingParent, 24);

                List<ModdedEndingType> mitaEnd = SingletonMonoBehaviour<Settings>.Instance.mitaEnd.Where(e => Enum.IsDefined(typeof(ModdedEndingType), (int)e)).Select(e => (ModdedEndingType)(int)e).ToList();

                string[] moddedNames = Enum.GetNames(typeof(ModdedEndingType));
                foreach (object obj in Enum.GetValues(typeof(ModdedEndingType)))
                {
                    EndingType endingType = ((ModdedEndingType)obj).Swap();
                    EndingMaster.Param param = NgoEx.EndingFromType(endingType);
                    string id = ((ModdedEndingType)obj).ToString();
                    string text;
                    string text2;
                    NgoEx.SetEndingTextByLanguage(param, out text, out text2);
                    if (mitaEnd.Any(e => e.ToString() == id))
                    {
                        GameObject go = UnityEngine.Object.Instantiate<GameObject>(____achievedBlock, ___endingParent);
                        go.GetComponent<TooltipCaller>().isShowTooltip = true;
                        go.GetComponent<TooltipCaller>().textNakami = text + "\n" + text2;
                        ___endingBlocks.Add(go);
                    }
                    else
                    {
                        GameObject go = UnityEngine.Object.Instantiate<GameObject>(____unachievedBlock, ___endingParent);
                        go.GetComponent<TooltipCaller>().isShowTooltip = true;
                        go.GetComponent<TooltipCaller>().textNakami = text + "\n" + text2;
                        ___endingBlocks.Add(go);
                    }
                }

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
                    AltAscMod.log.LogMessage($"Switching to effect {type}");
                    switch (type)
                    {
                        case (EffectType)(int)ModdedEffectType.Vengeful:
                            _overdose.enabled = true;
                            _psyche.enabled = false;
                            _weed.enabled = false;
                            _bleeding.enabled = true;
                            _goCrazy.enabled = true;
                            _anmaku.enabled = true;
                            SingletonMonoBehaviour<AltAscModManager>.Instance.loveEffect.GetComponent<Volume>().enabled = false;
                            break;

                        case (EffectType)(int)ModdedEffectType.Love:
                            SingletonMonoBehaviour<AltAscModManager>.Instance.loveEffect.GetComponent<Volume>().enabled = true;
                            break;

                        default:
                            SingletonMonoBehaviour<AltAscModManager>.Instance.loveEffect.GetComponent<Volume>().enabled = false;
                            break;
                    };
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
                    ____audioEffect.UpdateAudioEffect((EffectType)(int)ModdedEffectType.Hazy, weight);
                }
                SingletonMonoBehaviour<AltAscModManager>.Instance.loveEffect.GetComponent<Volume>().weight = weight;
            }


        }

        [HarmonyPatch(typeof(ShaderAudioFader))]
        public static class ShaderAudioFaderPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(ShaderAudioFader.UpdateAudioEffect))]
            public static void UpdateAudioEffectPostfix(ref ShaderAudioFader __instance, EffectType type, float weight, float ____ageSlider, ref AudioMixer ____audioMixer)
            {
                /*  Audio mixing is controlled by Audio Effects, documentation for that
                 *  is here: https://docs.unity3d.com/Manual/class-AudioEffectMixer.html
                 * 
                 *  Used Effects:
                 *      - Low Pass      (https://docs.unity3d.com/Manual/class-AudioLowPassEffect.html)
                 *      - High Pass     (https://docs.unity3d.com/Manual/class-AudioHighPassEffect.html)
                 *      - Echo          (https://docs.unity3d.com/Manual/class-AudioEchoEffect.html)
                 *      - Flange        (https://docs.unity3d.com/Manual/class-AudioFlangeEffect.html)
                 *      - Distortion    (https://docs.unity3d.com/Manual/class-AudioDistortionEffect.html)
                 *      - Pitch Shifter (https://docs.unity3d.com/Manual/class-AudioPitchShifterEffect.html)
                 *      
                 *  Keyvals:
                 *      - Master_Pitch      = Unknown (likely uses Pitch Shifter too)
                 *      - Dist_Level        = Distortion Effect > Level
                 *      - PS_Pitch          = Pitch Shifter Effect > Pitch
                 *      - Flange_Drymix     = Flange Effect > Drymix
                 *      - Flange_Wetmix     = Flange Effect > Wetmix
                 *      - Flange_Depth      = Flange Effect > Depth
                 *      - Flange_Rate       = Flange Effect > Rate
                 *      - Lowpass_Cutoff    = Low Pass Effect > Cutoff freq
                 *      - Lowpass_Resonance = Low Pass Effect > Resonance
                 *      - Echo_Delay        = Echo Effect > Delay
                 *      - Echo_Decay        = Echo Effect > Decay
                 *      - Echo_DryMix       = Echo Effect > Drymix
                 *      - Echo_WetMix       = Echo Effect > Wetmix
                 *      - Highpass_Cutoff   = High Pass Effect > Cutoff freq
                 */
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
                        ____audioMixer.SetFloat("Master_Pitch", 1f - weight * 0.3f);
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
                    case ModdedEffectType.Love:
                        ____audioMixer.SetFloat("Master_Pitch", 1f + 0.2f * weight); // DF 1
                        ____audioMixer.SetFloat("Dist_Level", 0f + 0.1f * weight);
                        ____audioMixer.SetFloat("PS_Pitch", 1f);
                        ____audioMixer.SetFloat("Flange_Drymix", 1f);
                        ____audioMixer.SetFloat("Flange_WetMix", 0f);
                        ____audioMixer.SetFloat("Flange_Depth", 1f);
                        ____audioMixer.SetFloat("Flange_Rate", 0.1f);
                        ____audioMixer.SetFloat("Lowpass_Cutoff", 22000f);
                        ____audioMixer.SetFloat("Lowpass_Resonance", 1f);
                        ____audioMixer.SetFloat("Echo_Delay", 500f);
                        ____audioMixer.SetFloat("Echo_Decay", 0f);
                        ____audioMixer.SetFloat("Echo_DryMix", 1f - 0.15f * weight);
                        ____audioMixer.SetFloat("Echo_WetMix", 0.15f * weight);
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
                // TODO: Loading fails because various classes cannot access NMMM when booting a new save. Fix.
                // Possible causes:
                //      - EventManager creating NMMM instead of NMMM being created with the scene
                //      - EventManager creating NMMM in AwakePostfix, resulting in no NMMM being active
                //        for the Load called at the end of EventManager.Awake
                // Solution: Either create NMMM in a scene, or move it to before EventManagerPatches.AwakePostfix (maybe in a transpiler?)
                PoketterView2D inst = __instance;
                if (!inst.HasComponent<AltPoketter>() || SingletonMonoBehaviour<AltAscModManager>.Instance == null) return;
                SingletonMonoBehaviour<AltAscModManager>.Instance.isAmeDelete.Where((bool v) => v).Subscribe(delegate (bool _)
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

        [HarmonyPatch(typeof(SaveRelayer))]
        public static class SaveRelayerPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(SaveRelayer.LoadSlotByES3))]
            public static bool LoadSlotByES3Prefix(string fileName, ref SlotData __result)
            {
                __result = new ModdedSlotData
                {
                    jineHistory = ES3.Load<List<JineData>>("JINEHISTORY", fileName),
                    poketterHistory = ES3.Load<List<TweetData>>("POKETTERHISTORY", fileName),
                    eventsHistory = ES3.Load<List<string>>("EVENTHISTORY", fileName),
                    dayActionHistory = ES3.Load<List<string>>("DAYACTIONHISTORY", fileName, new List<string>()),
                    loop = ES3.Load<int>("LOOPCOUNT", fileName),
                    midokumushi = ES3.Load<int>("MIDOKUCOUNT", fileName),
                    psycheCount = ES3.Load<int>("PSYCHECOUNT", fileName, 0),
                    havingNetas = ES3.Load<List<AlphaLevel>>("HAVINGNETAS", fileName),
                    usedNetas = ES3.Load<List<AlphaLevel>>("USEDNETAS", fileName),
                    isJuncho = ES3.Load<bool>("ISJUNCHO", fileName),
                    isHearTrauma = ES3.Load<bool>("ISHEARTRAUMA", fileName),
                    trauma = ES3.Load<JineType>("TRAUMA", fileName),
                    firstDate = ES3.Load<CmdType>("FIRSTDATE", fileName, CmdType.None),
                    isHappaOK = ES3.Load<bool>("ISHAPPAOK", fileName),
                    isHorror = ES3.Load<bool>("ISHORROR", fileName),
                    isGedatsu = ES3.Load<bool>("ISGEDATSU", fileName),
                    beforeWristCut = ES3.Load<bool>("BEFOREWRISTCUT", fileName, false),
                    isWristCut = ES3.Load<bool>("ISWRISTCUT", fileName, false),
                    isHakkyo = ES3.Load<bool>("ISHAKKYO", fileName, false),
                    wishlist = ES3.Load<int>("WISHLIST", fileName, 0),
                    loveDiary = ES3.Load<int>("LOVEDIARY", fileName, 0),
                    isShurokued = ES3.Load<bool>("ISSHUROKUED", fileName, false),
                    kyuusiCount = ES3.Load<int>("KYUUSICOUNT", fileName, 0),
                    isOpenGinga = ES3.Load<bool>("ISOPENGINGA", fileName, false),
                    is150mil = ES3.Load<bool>("IS150MIL", fileName, false),
                    is300mil = ES3.Load<bool>("IS300MIL", fileName, false),
                    is500mil = ES3.Load<bool>("IS500MIL", fileName, false),
                    stats = ES3.Load<List<Status>>("STATUS", fileName),
                    sleepCount = ES3.Load<int>("SLEEPCOUNT", fileName, 0),
                    isLove = ES3.Load<bool>("ISLOVE", fileName, false),
                    isLoveLoop = ES3.Load<bool>("ISLOVELOOP", fileName, false),
                    password = ES3.Load<string>("PASSWORD", fileName, "angelkawaii2"),
                };
                return false;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(SaveRelayer.SaveSlotByES3))]
            public static void SaveSlotByES3Postfix(string fileName, SlotData data)
            {
                ModdedSlotData modData = data as ModdedSlotData;
                if (modData == null) return;

                ES3.Save<int>("SLEEPCOUNT", modData.sleepCount, fileName);
                ES3.Save<bool>("ISLOVE", modData.isLove, fileName);
                ES3.Save<bool>("ISLOVELOOP", modData.isLoveLoop, fileName);
                ES3.Save<string>("PASSWORD", modData.password, fileName);
            }
        }

        [HarmonyPatch(typeof(DayBaketter))]
        public static class DayBaketterPatches
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DayBaketter.Start))]
            public static void StartPostfix(ref DayBaketter __instance)
            {
                DayBaketter baketter = __instance;
                (from n in SingletonMonoBehaviour<AltAscModManager>.Instance.ObserveEveryValueChanged((AltAscModManager n) => n.isLoveLoop, FrameCountType.Update, false)
                 where n
                 select n).Subscribe(delegate (bool _)
                 {
                     AltAscMod.log.LogMessage("Love bake!");
                     baketter.AddLoveBake();
                 }).AddTo(__instance.gameObject);
            }
        }

        [HarmonyPatch(typeof(Event_Uzagarami_Kaiwa))]
        public static class Event_Uzagarami_KaiwaPatches
        {
            [HarmonyTranspiler]
            [HarmonyPatch(nameof(Event_Uzagarami_Kaiwa.getUniqueUzagaramiId))]
            public static IEnumerable<CodeInstruction> getUniqueUzagaramiIdTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> ins = instructions.ToList();
                List<CodeInstruction> outs = new List<CodeInstruction>();
                bool addedInsertion = false;
                for (int i = 0; i < ins.Count; i++)
                {
                    outs.Add(ins[i]);
                    if (ins[i].opcode == OpCodes.Stloc_S && !addedInsertion)
                    {
                        AltAscMod.log.LogMessage($"Added day dialog patch!");
                        addedInsertion = true;
                        outs.Add(new CodeInstruction(OpCodes.Ldloc_0));
                        outs.Add(new CodeInstruction(OpCodes.Callvirt, typeof(Alternates).GetMethod(nameof(Alternates.AddModdedDayDialog))));
                    }
                }

                return outs;
            }
        }

        /*[HarmonyPatch(typeof(DayAndNight))]
        public static class DayAndNightPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DayAndNight.DayToNight))]
            public static bool DayToNightPrefix(ref DayAndNight __instance, ref Image ___Right, ref Image ___Left)
            {
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop)
                {
                    AltAscMod.log.LogMessage("Skipping DayToNight, as we're in love.");
                    Sprite s = SingletonMonoBehaviour<AltAscModManager>.Instance.desktop_icon_love;
                    ___Right.sprite = s;
                    ___Left.sprite = s;
                    return false;
                }
                return true;
            }

            [HarmonyPostfix]
            [HarmonyPatch(nameof(DayAndNight.Start))]
            public static void StartPostfix(ref DayAndNight __instance, ref Image ___Right, ref Image ___Left)
            {
                Image Left = ___Left;
                Image Right = ___Right;
                SingletonMonoBehaviour<AltAscModManager>.Instance.ObserveEveryValueChanged((AltAscModManager n) => n.isLoveLoop, FrameCountType.Update, false).Subscribe(delegate (bool b)
                {
                    AltAscMod.log.LogMessage("Observing love loop...");
                    if (!b) return;
                    AltAscMod.log.LogMessage("Observed love loop!");
                    Sprite s = SingletonMonoBehaviour<AltAscModManager>.Instance.desktop_icon_love;
                    Left.sprite = s;
                    Right.sprite = s;
                }).AddTo(__instance.gameObject);
            }
        }*/
    }
}
