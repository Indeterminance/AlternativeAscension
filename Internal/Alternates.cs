﻿using Cysharp.Threading.Tasks;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NeedyXML;
using NeedyEnums;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using static AlternativeAscension.AAPatches;
using System.Runtime.CompilerServices;
using ngov3.Effect;
using HarmonyLib;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine.Rendering;

namespace AlternativeAscension
{
    static internal class Alternates
    {
        // Replaces BackFromOdekake
        public static async UniTask BackFromPanicOdekake(int weight)
        {
            bool stalkOdekake = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.StalkReveal;
            //AltAscMod.log.LogMessage($"Follower plot flag: {stalkOdekake}");
            if (!stalkOdekake)
            {
                switch (weight)
                {
                    case 1:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ODEKAKEPANIC1_TWEET001.Swap(), null, null);
                        break;
                    case 3:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ODEKAKEPANIC2_TWEET001.Swap(), null, null);
                        break;
                    case 5:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ODEKAKEPANIC3_TWEET001.Swap(), null, null);
                        break;
                    case 7:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ODEKAKEPANIC4_TWEET001.Swap(), null, null);
                        break;
                    default: break;
                }
            }
            else
            {
                int odekakesLeft = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap());
                switch (odekakesLeft)
                {
                    case 2:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE1_TWEET001.Swap(), null, null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE1_TWEET002.Swap(), null, null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE1_TWEET003.Swap(), null, null);
                        AudioManager.Instance.PlaySeByType(SoundType.SE_chime);
                        break;
                    case 1:
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET001.Swap(), new List<ModdedKusoRepType>
                        {
                            ModdedKusoRepType.STALKODEKAKE2_TWEET001_KUSO001,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET001_KUSO002,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET001_KUSO003,
                        }.Swap(), null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET002.Swap(), null, null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET003.Swap(), new List<ModdedKusoRepType>
                        {
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO001,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO002,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO003,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO004,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO005,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO006,
                            ModdedKusoRepType.STALKODEKAKE2_TWEET003_KUSO007,
                        }.Swap(), null);
                        AudioManager.Instance.PlaySeByType(SoundType.SE_chime);
                        //SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<TimePassing1>();
                        SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.OdekakeBreak);
                        break;
                    default: break;
                }
                await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(ModdedStatusType.OdekakeCountdown.Swap(), -1);
            }
            await NgoEvent.DelaySkippable(Constants.MIDDLE);

            // Post-odekake quotes
            List<ModdedJineType> jines = new List<ModdedJineType>();
            switch (weight)
            {
                case 1:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC1_POST001,
                        ModdedJineType.ODEKAKEPANIC1_POST002,
                        ModdedJineType.ODEKAKEPANIC1_POST003
                    };
                    break;
                case 2:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC2_POST001,
                        ModdedJineType.ODEKAKEPANIC2_POST002,
                        ModdedJineType.ODEKAKEPANIC2_POST003,
                        ModdedJineType.ODEKAKEPANIC2_POST004
                    };
                    break;
                case 3:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC3_POST001,
                        ModdedJineType.ODEKAKEPANIC3_POST002,
                        ModdedJineType.ODEKAKEPANIC3_POST003,
                        ModdedJineType.ODEKAKEPANIC3_POST004,
                        ModdedJineType.ODEKAKEPANIC3_POST005
                    };
                    break;
                case 4:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC4_POST001,
                        ModdedJineType.ODEKAKEPANIC4_POST002,
                        ModdedJineType.ODEKAKEPANIC4_POST003,
                        ModdedJineType.ODEKAKEPANIC4_POST004,
                        ModdedJineType.ODEKAKEPANIC4_POST005
                    };
                    break;
                case 5:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC5_POST001,
                        ModdedJineType.ODEKAKEPANIC5_POST002,
                        ModdedJineType.ODEKAKEPANIC5_POST003,
                        ModdedJineType.ODEKAKEPANIC5_POST004,
                        ModdedJineType.ODEKAKEPANIC5_POST005,
                        ModdedJineType.ODEKAKEPANIC5_POST006
                    };
                    break;
                case 6:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC6_POST001,
                        ModdedJineType.ODEKAKEPANIC6_POST002,
                        ModdedJineType.ODEKAKEPANIC6_POST003,
                        ModdedJineType.ODEKAKEPANIC6_POST004,
                        ModdedJineType.ODEKAKEPANIC6_POST005
                    };
                    break;
                case 7:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC7_POST001,
                        ModdedJineType.ODEKAKEPANIC7_POST002,
                        ModdedJineType.ODEKAKEPANIC7_POST003,
                        ModdedJineType.ODEKAKEPANIC7_POST004,
                        ModdedJineType.ODEKAKEPANIC7_POST005
                    };
                    break;
                case 8:
                    jines = new List<ModdedJineType>
                    {
                        ModdedJineType.ODEKAKEPANIC8_POST001,
                        ModdedJineType.ODEKAKEPANIC8_POST002,
                        ModdedJineType.ODEKAKEPANIC8_POST003,
                        ModdedJineType.ODEKAKEPANIC8_POST004,
                        ModdedJineType.ODEKAKEPANIC8_POST005,
                        ModdedJineType.ODEKAKEPANIC8_POST006,
                        ModdedJineType.ODEKAKEPANIC8_POST007,
                        ModdedJineType.ODEKAKEPANIC8_POST008,
                        ModdedJineType.ODEKAKEPANIC8_POST009,
                        ModdedJineType.ODEKAKEPANIC8_POST010
                    };
                    break;
                default: break;
            }
            foreach (ModdedJineType jine in jines)
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(jine.Swap());
            }
        }

        // TODO: Investigate why the odekake shortcut doesn't instantiate properly
        public static void ShortcutStartAlternate(this Shortcut shortcut)
        {
            if (SingletonMonoBehaviour<StatusManager>.Instance == null) return;

            // Get private fields
            Button _shortcut = new Traverse(shortcut).Field(nameof(Shortcut._shortcut)).GetValue<Button>();
            TooltipCaller _tooltip = new Traverse(shortcut).Field(nameof(Shortcut._tooltip)).GetValue<TooltipCaller>();
            ReactiveProperty<int> _dayPart = new Traverse(shortcut).Field(nameof(Shortcut._dayPart)).GetValue<ReactiveProperty<int>>();

            if (shortcut.appType == AppType.Login) shortcut.appType = ModdedAppType.ScriptableLogin.Swap();

            SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(ModdedStatusType.FollowerPlotFlag.Swap()).Subscribe(delegate (int _)
            {
                if (_ == (int)FollowerPlotFlagValues.AngelFuneral)
                {
                    if (shortcut.appType != AppType.GoOut) _shortcut.interactable = false;
                    else _shortcut.interactable = true;
                    if (_tooltip != null) _tooltip.isShowTooltip = false;
                    return;
                }
            }).AddTo(shortcut.gameObject);

            SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(ModdedStatusType.OdekakeCountdown.Swap()).Subscribe(delegate (int _)
            {
                if (_ == 0 && shortcut.appType == AppType.GoOut && SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) != (int)FollowerPlotFlagValues.AngelFuneral)
                {
                    _shortcut.interactable = false;
                    _tooltip.isShowTooltip = true;
                    _tooltip.type = TooltipType.Tooltip_Angel;
                }
            }).AddTo(shortcut.gameObject);
            if (shortcut.appType == AppType.NetaChoose)
            {
                _shortcut.onClick.RemoveAllListeners();
                _shortcut.onClick.AddListener(delegate
                {
                    int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
                    int plotflag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());
                    if (SingletonMonoBehaviour<EventManager>.Instance.isGedatsu)
                    {
                        SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Action_HaishinStart>();
                        return;
                    }
                    if (day == 16 && plotflag == (int)FollowerPlotFlagValues.VisitedComiket)
                    {
                        SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Event_PostComiket>();
                        return;
                    }
                    SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(shortcut.appType, true);
                    return;
                });
                _dayPart.Subscribe(delegate (int v)
                {
                    if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) >= (int)FollowerPlotFlagValues.AngelFuneral)
                    {
                        _shortcut.interactable = false;
                        _tooltip.isShowTooltip = false;
                        return;
                    }
                }).AddTo(shortcut.gameObject);
            }


            /*/ Redo click listeners
            _shortcut.onClick.RemoveAllListeners();

            _shortcut.onClick.AddListener(delegate
            {
                //bool isFuneral = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelFuneral;
                //if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 0 &&
                //    !isFuneral)
                //{
                //    _shortcut.interactable = false;
                //    _tooltip.isShowTooltip = true;
                //    _tooltip.type = TooltipType.Tooltip_Angel;
                //    return;
                //}
                if (shortcut.appType == AppType.Pills)
                {

                }
                else
                {

                }
                IWindow w = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(shortcut.appType, true);
                //if (isFuneral) w.Uncloseable();
                return;
            });*/

            // Same thing as the click event, but we're gonna run it once beforehand too
            if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.AngelFuneral)
            {
                AltAscMod.log.LogMessage($"Shortcut plot Funeral");
                if (shortcut.appType != AppType.GoOut) _shortcut.interactable = false;
                else _shortcut.interactable = true;
                _tooltip.isShowTooltip = false;
            }
            else if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 0 && shortcut.appType == AppType.GoOut)
            {
                _shortcut.interactable = false;
                _tooltip.isShowTooltip = true;
                _tooltip.type = TooltipType.Tooltip_Angel;
            }
        }

        public static void DayPassingStartAlternate(this ngov3.Effect.DayPassing dayPass)
        {
            // I'm not actually sure this is ever used, but this is here for consistency's sake just in case.
            // ...in fact, I'm not sure if any of the "non-2D" variants of objects are used...
            dayPass._dayIndex = SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayIndex);
            dayPass._dayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart);
            dayPass._dayIndex.Where((int d) => true).Subscribe(delegate (int t)
            {
                if (t > SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.DayIndex)) return;
                dayPass.dayPass(dayPass._dayIndex.Value, 0, 0);
            }).AddTo(dayPass.gameObject);
            dayPass.DayPassingCanvas.alpha = 0f;
            dayPass.DayPassingCanvas.interactable = false;
            dayPass.DayPassingCanvas.blocksRaycasts = false;
            dayPass.Noise.weight = 0f;
        }

        public static async void DayPassing2DStartAlternate(this ngov3.Effect.DayPassing2D dayPass)
        {
            // Get (a lot) of private fields
            Traverse _dayIndex = new Traverse(dayPass).Field(nameof(DayPassing2D._dayIndex));
            Traverse _dayPart = new Traverse(dayPass).Field(nameof(DayPassing2D._dayPart));
            Traverse _heartSprite = new Traverse(dayPass).Field(nameof(DayPassing2D._heartSprite));
            Traverse _spriteFader = new Traverse(dayPass).Field(nameof(DayPassing2D._spriteFader));
            Traverse _tmpFader = new Traverse(dayPass).Field(nameof(DayPassing2D._tmpFader));
            Traverse Noise = new Traverse(dayPass).Field(nameof(DayPassing2D.Noise));
            Traverse _raycastBlocker = new Traverse(dayPass).Field(nameof(DayPassing2D._raycastBlocker));

            // Real start of the function :P
            _dayIndex.SetValue(SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayIndex));
            _dayPart.SetValue(SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart));
            _heartSprite.GetValue<SpriteRenderer>().gameObject.SetActive(false);
            _spriteFader.GetValue<SpriteRenderersFader>().Alpha = 1f;
            _tmpFader.GetValue<TMPTextsFader>().Alpha = 0f;
            Noise.GetValue<Volume>().weight = 0f;
            _raycastBlocker.GetValue<GameObject>().SetActive(true);
            await UniTask.DelayFrame(5, PlayerLoopTiming.Update, default(CancellationToken), false);
            _tmpFader.GetValue<TMPTextsFader>().Alpha = 1f;
            _heartSprite.GetValue<SpriteRenderer>().gameObject.SetActive(true);
            _dayIndex.GetValue<ReactiveProperty<int>>().Where((int d) => true).Subscribe(delegate (int t)
            {
                //NeedyMintsMod.log.LogMessage($"dayIndex: {_dayIndex.GetValue<ReactiveProperty<int>>().Value}");
                if (SingletonMonoBehaviour<AltAscModManager>.Instance.lockDayCount) return;
                Traverse method = new Traverse(dayPass).Method(nameof(DayPassing2D.dayPass), new Type[] {typeof(int), typeof(int), typeof(int)});
                method.GetValue(new object[] {
                    _dayIndex.GetValue<ReactiveProperty<int>>().Value, 0, 0
                });
            }).AddTo(dayPass.gameObject);
        }

        public static void SetChosenDayTextAlternate(this Boot boot, LanguageType lang, int _DataNumber)
        {
            // Get private fields
            TMP_Dropdown days = new Traverse(boot).Field(nameof(Boot.days)).GetValue<TMP_Dropdown>();
            Image DataIcon = new Traverse(boot).Field(nameof(Boot.DataIcon)).GetValue<Image>();
            Button NewAccount = new Traverse(boot).Field(nameof(Boot.NewAccount)).GetValue<Button>();
            Button StartButton = new Traverse(boot).Field(nameof(Boot.StartButton)).GetValue<Button>();

            // Get regexes for later
            Regex rx_get = new Regex(string.Format("Data{0}_Day\\d*\\{1}", _DataNumber, SaveRelayer.EXTENTION));
            Regex rx_remove = new Regex(string.Format("\\d*{0}", SaveRelayer.EXTENTION));

            // Vanilla clear
            days.options.Clear();

            // Find all days for this save slot, and convert those to ints
            List<string> files = Directory.GetFiles(Environment.CurrentDirectory + "/Windose_Data").Where(s => rx_get.IsMatch(s)).ToList();
            List<int> ints = files.Select(s => int.Parse(rx_remove.Match(s).Value.Replace(SaveRelayer.EXTENTION, ""))).Where(i => i >= 2).OrderByDescending(i => i).ToList();

            // Loop through each int and add the respective save file
            foreach (int day in ints)
            {
                if (SaveRelayer.IsSlotDataExists(string.Format("Data{0}_Day{1}{2}", _DataNumber, day, SaveRelayer.EXTENTION)))
                {
                    days.options.Add(new TMP_Dropdown.OptionData
                    {
                        text = NgoEx.DayText(day, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value)
                    });
                }
            }

            // Vanilla code from here onwards
            days.options.Add(new TMP_Dropdown.OptionData
            {
                text = NgoEx.DayText(1, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value)
            });
            days.RefreshShownValue();
            if (SaveRelayer.IsSlotDataExists(string.Format("Data{0}_Day1{1}", _DataNumber, SaveRelayer.EXTENTION)))
            {
                DataIcon.sprite = new Traverse(boot).Field(nameof(Boot.ten)).GetValue<Sprite>();
                NewAccount.gameObject.SetActive(true);
                NewAccount.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Start_Menu_Newgame01, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                StartButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Start_Menu_Continue, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
                return;
            }
            DataIcon.sprite = new Traverse(boot).Field(nameof(Boot.ame)).GetValue<Sprite>();
            NewAccount.gameObject.SetActive(false);
            StartButton.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Start_Menu_Newgame00, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
        }

        public static void AppLoadDataAlternate(this App_LoadDataComponent app)
        {
            // Get private fields
            int DataNumber = new Traverse(app).Field(nameof(App_LoadDataComponent._DataNumber)).GetValue<int>();
            TMP_Text Hyouji = new Traverse(app).Field(nameof(App_LoadDataComponent._Hyouji)).GetValue<TMP_Text>();
            DataPrefab DataPrefab = new Traverse(app).Field(nameof(App_LoadDataComponent._DataPrefab)).GetValue<DataPrefab>();
            Transform DataContainer = new Traverse(app).Field(nameof(App_LoadDataComponent._DataContainer)).GetValue<Transform>();
            Button NewGame = new Traverse(app).Field(nameof(App_LoadDataComponent._NewGame)).GetValue<Button>();
            
            
            Hyouji.text = string.Format("Data{0}", DataNumber);

            // The next section is pretty similar to the one from SetChosenDayTextAlternate,
            // maybe we could add a TODO to merge them at some point?

            // Get regexes for later
            Regex rx_get = new Regex(string.Format("Data{0}_Day\\d*\\{1}", DataNumber, SaveRelayer.EXTENTION));
            Regex rx_remove = new Regex(string.Format("\\d*{0}", SaveRelayer.EXTENTION));

            // Find all days for this save slot, and convert those to ints
            List<string> files = Directory.GetFiles(Environment.CurrentDirectory + "/Windose_Data").Where(s => rx_get.IsMatch(s)).ToList();
            List<int> ints = files.Select(s => int.Parse(rx_remove.Match(s).Value.Replace(SaveRelayer.EXTENTION, ""))).Where(i => i >= 2).OrderByDescending(i => i).ToList();

            // Loop through each int and add the respective save file
            foreach (int day in ints)
            {
                if (SaveRelayer.IsSlotDataExists(string.Format("Data{0}_Day{1}{2}", DataNumber, day, SaveRelayer.EXTENTION)))
                {
                    global::UnityEngine.Object.Instantiate<DataPrefab>(DataPrefab, DataContainer).SetLoadable(DataNumber, day);
                }
            }

            // Vanilla code from here

            if (SaveRelayer.IsSlotDataExists(string.Format("Data{0}_Day1{1}", DataNumber, SaveRelayer.EXTENTION)))
            {
                NewGame.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Start_Menu_Newgame01, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            }
            else
            {
                NewGame.GetComponentInChildren<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Start_Menu_Newgame00, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            }
            NewGame.OnClickAsObservable().Subscribe(delegate (Unit _)
            {
                new Traverse(app).Method(nameof(App_LoadDataComponent.StartGame)).GetValue();
            }).AddTo(app.gameObject);
        }

        public static TweetData AmeTweetData(TweetType t, int follower = 1, int day = 0, CmdType cmd = CmdType.None)
        {
            TweetData data = new TweetData();

            data.Type = t;
            data.honbun = "";
            data.IsOmote = false;
            data.day = day;
            data.kusoReps = new List<KusoRepType>();
            data.kusoRepString = new List<string>();
            data.cmdType = cmd;
            if (follower > 100000)
            {
                data.FavNumber = (int)((double)follower * 0.13555 - 5555.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerFav * global::UnityEngine.Random.Range(1, 10)));
                data.RtNumber = (int)((double)follower * 0.00888 + 1111.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerRT * global::UnityEngine.Random.Range(1, 10)));
            }
            else if (follower > 10000)
            {
                data.FavNumber = (int)((double)follower * 0.066 + 1333.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerFav * global::UnityEngine.Random.Range(1, 10)));
                data.RtNumber = (int)((double)follower * 0.018 + 166.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerRT * global::UnityEngine.Random.Range(1, 10)));
            }
            else if (follower > 1000)
            {
                data.FavNumber = (int)((double)follower * 0.88 + 111.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerFav * global::UnityEngine.Random.Range(0, 1)));
                data.RtNumber = (int)((double)follower * 0.034 + 5.0 + (double)(TweetFetcher.ConvertTypeToTweet(t).BuzzPowerRT * global::UnityEngine.Random.Range(0, 1)));
                return data;
            }
            else
            {
                data.FavNumber = (int)(Mathf.Max(Mathf.Log10((float)follower), 0.5f) * (float)TweetFetcher.ConvertTypeToTweet(t).BuzzPowerFav * 2f) + global::UnityEngine.Random.Range(1, 10);
                data.RtNumber = (int)(Mathf.Max(Mathf.Log10((float)follower), 1f) * (float)TweetFetcher.ConvertTypeToTweet(t).BuzzPowerRT) + global::UnityEngine.Random.Range(1, 10);
            }
            //NeedyMintsMod.log.LogMessage($"Created special tweet data with {data.FavNumber} favs and {data.RtNumber} rts");
            return data;
        }

        public static void AddQueueWithKusorepsAmeBreak(this PoketterManager pm, TweetType t, List<KusoRepType> kusoType = null, List<string> kusoString = null)
        {
            MethodInfo isValidTweetData = typeof(PoketterManager).GetMethod(nameof(PoketterManager.isValidTweetData), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            MethodInfo AddKusoRepToTweet = typeof(PoketterManager).GetMethod(nameof(PoketterManager.AddKusorepToTweet), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, null, CallingConventions.Standard, new Type[] { typeof(TweetData), typeof(KusoRepType) }, null);


            if (t == TweetType.None)
            {
                return;
            }
            TweetData testData = new TweetData(t, true, SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower), SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex), SingletonMonoBehaviour<EventManager>.Instance.executingAction);
            TweetData testData2 = new TweetData(t, false, SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower), SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex), SingletonMonoBehaviour<EventManager>.Instance.executingAction);
            bool testFlag = (bool)isValidTweetData.Invoke(pm, new object[] { testData });
            bool testFlag2 = (bool)isValidTweetData.Invoke(pm, new object[] { testData2 });
            if (testFlag || !testFlag2)
            {
                pm.AddQueueWithKusoreps(t,kusoType, kusoString);
                return;
            }

            int krp = new Traverse(pm).Field(nameof(PoketterManager.KUSOREPPROBABILITY)).GetValue<int>();

            TweetData tweetData = AmeTweetData(t, Mathf.RoundToInt(SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower)*1.5f), SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex), SingletonMonoBehaviour<EventManager>.Instance.executingAction);
            bool flag = (bool)isValidTweetData.Invoke(pm, new object[] { tweetData });
            if (kusoType != null)
            {
                foreach (KusoRepType kusoRepType in kusoType)
                {
                    tweetData.kusoReps.Add(kusoRepType);
                }
            }
            if (kusoString != null)
            {
                foreach (string text in kusoString)
                {
                    pm.AddKusorepToTweet(tweetData, text);
                }
            }
            if (kusoType == null)
            {
                int status = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower);
                if (status < 10000)
                {
                    if (UnityEngine.Random.Range(0, 100) < krp)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, NgoEx.smallKusoreps.RandomizedElement<KusoRepType>() });
                    }
                }
                else if (status < 100000)
                {
                    KusoRepType kusoRepType2 = NgoEx.middleKusoreps.RandomizedElement<KusoRepType>();
                    KusoRepType kusoRepType3 = NgoEx.middleKusoreps.RandomizedElement<KusoRepType>();
                    if (UnityEngine.Random.Range(0, 100) < krp)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, kusoRepType2 });
                    }
                    if (UnityEngine.Random.Range(0, 100) < krp && kusoRepType3 != kusoRepType2)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, kusoRepType3 });
                    }
                }
                else
                {
                    KusoRepType kusoRepType4 = NgoEx.largeKusoreps.RandomizedElement<KusoRepType>();
                    KusoRepType kusoRepType5 = NgoEx.largeKusoreps.RandomizedElement<KusoRepType>();
                    KusoRepType kusoRepType6 = NgoEx.largeKusoreps.RandomizedElement<KusoRepType>();
                    if (UnityEngine.Random.Range(0, 100) < krp)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, kusoRepType4 });
                    }
                    if (UnityEngine.Random.Range(0, 100) < krp && kusoRepType5 != kusoRepType4)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, kusoRepType5 });
                    }
                    if (UnityEngine.Random.Range(0, 100) < krp && kusoRepType6 != kusoRepType4 && kusoRepType6 != kusoRepType5)
                    {
                        AddKusoRepToTweet.Invoke(pm, new object[] { tweetData, kusoRepType6 });
                    }
                }
            }
            if (flag)
            {
                pm.AddTweet(tweetData);
            }
        }

        public static void FavRtMoveAlternate(this PoketterCell2D cell)
        {
            PoketterCell2D inst = cell;
            TweetDrawable tweetDrawable = new Traverse(cell).Field(nameof(PoketterCell2D.tweetDrawable)).GetValue<TweetDrawable>();
            int _buzzMillisecond = new Traverse(cell).Field(nameof(PoketterCell2D._buzzMillisecond)).GetValue<int>();
            TMP_Text _rtText = new Traverse(cell).Field(nameof(PoketterCell2D._rtText)).GetValue<TMP_Text>();
            TMP_Text _favText = new Traverse(cell).Field(nameof(PoketterCell2D._favText)).GetValue<TMP_Text>();


            Traverse ConvertGigaNumber = new Traverse(cell).Method(nameof(PoketterCell2D.ConvertGigaNumber), new Type[] { typeof(int) });

            //NeedyMintsMod.log.LogMessage($"Giga: {ConvertGigaNumber.GetValue(tweetDrawable.RtNumber)}");
            float num = (float)_buzzMillisecond / 1000f;
            if (tweetDrawable.RtNumber > 0)
            {
                _rtText.DOCounter(0, tweetDrawable.RtNumber, num, true, null).OnComplete(delegate
                {
                    _rtText.text = ConvertGigaNumber.GetValue<string>(tweetDrawable.RtNumber);
                }).Play<TweenerCore<int, int, NoOptions>>();
            }
            if (tweetDrawable.FavNumber > 0)
            {
                _favText.DOCounter(0, tweetDrawable.FavNumber, num, true, null).OnComplete(delegate
                {
                    _favText.text = ConvertGigaNumber.GetValue<string>(tweetDrawable.FavNumber);
                }).Play<TweenerCore<int, int, NoOptions>>();
            }
        }

        public static void showDeleteModeAme(this PoketterView2D poke)
        {
            Traverse t = new Traverse(poke);
            t.Field(nameof(PoketterView2D._Header)).GetValue<TMP_Text>().text = NgoEx.SystemTextFromType(ModdedSystemTextType.Poketter_AmeQuit.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            t.Field(nameof(PoketterView2D._Desc)).GetValue<TMP_Text>().text = NgoEx.SystemTextFromType(SystemTextType.Poketter_Deleteded_Description, SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value);
            t.Field(nameof(PoketterView2D._scrollRect)).GetValue<ScrollRect>().gameObject.SetActive(false);
            t.Field(nameof(PoketterView2D._Header)).GetValue<TMP_Text>().gameObject.SetActive(true);
            t.Field(nameof(PoketterView2D._Desc)).GetValue<TMP_Text>().gameObject.SetActive(true);
        }

        public static void AddLoveBake(this DayBaketter baketter)
        {
            TMP_Text label = new Traverse(baketter).Field(nameof(DayBaketter.label)).GetValue<TMP_Text>();

            if (label == null)
            {
                return;
            }
            int day;
            int.TryParse(label.text, out day);
            if (day > 31) label.text = "❤❤";
        }

        public static void AddModdedDayDialog(this List<string> eventString)
        {
            //AltAscMod.log.LogMessage("Patched dialog!");
            int stress = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Stress);
            int love = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Love);
            int yami = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Yami);
            bool stressCap = SingletonMonoBehaviour<StatusManager>.Instance.GetMaxStatus(StatusType.Stress) == 120;

            
            if (love > 40 && stress < 40)  eventString.Add(nameof(Event_Awakening));
            if (stress > 80)               eventString.Add(nameof(Event_Sick));
            if (love > 60)                 eventString.Add(nameof(Event_Starving));
            if (!stressCap && stress < 60) eventString.Add(nameof(Event_Snail));
            if (yami > 80)                 eventString.Add(nameof(Event_ICanFixHer));
            if (yami > 20)                 eventString.Add(nameof(Event_News));
            
            
        }
    }
}
