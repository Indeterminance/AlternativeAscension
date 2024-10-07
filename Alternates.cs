using Cysharp.Threading.Tasks;
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
using static NeedyMintsOverdose.MintyOverdosePatches;
using System.Runtime.CompilerServices;
using ngov3.Effect;
using HarmonyLib;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine.Rendering;

namespace NeedyMintsOverdose
{
    static internal class Alternates
    {
        // Replaces BackFromOdekake
        public static async UniTask BackFromPanicOdekake(int weight)
        {
            bool stalkOdekake = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.StalkReveal;
            NeedyMintsMod.log.LogMessage($"Follower plot flag: {stalkOdekake}");
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
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET001.Swap(), null, null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET002.Swap(), null, null);
                        SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKODEKAKE2_TWEET003.Swap(), null, null);
                        AudioManager.Instance.PlaySeByType(SoundType.SE_chime);
                        //SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<TimePassing1>();
                        SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.OdekakeBreak);
                        break;
                    default: break;
                }
                await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(ModdedStatusType.OdekakeCountdown.Swap(), -1);
            }
        }

        public static void PanicQuitOdekake(Shortcut shortcut)
        {
            FieldInfo shortcutInfo = typeof(Shortcut).GetField(nameof(Shortcut._shortcut), BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo tooltipInfo = typeof(Shortcut).GetField(nameof(Shortcut._tooltip), BindingFlags.NonPublic | BindingFlags.Instance);

            Button oldButton = shortcutInfo.GetValue(shortcut) as Button;
            oldButton.interactable = false;
            shortcutInfo.SetValue(shortcut, oldButton);

            TooltipCaller oldTooltip = tooltipInfo.GetValue(shortcut) as TooltipCaller;
            oldTooltip.isShowTooltip = true;
            oldTooltip.type = TooltipType.Tooltip_Angel;
            tooltipInfo.SetValue(shortcut, oldTooltip);

            //SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.GoOut).Close();
        }


        // TODO: Investigate why the odekake shortcut doesn't instantiate properly
        public static void PanicQuitOdekakeShortcut(this Shortcut shortcut, UnityEngine.UI.Button button)
        {
            // Get private fields
            Button _shortcut = new Traverse(shortcut).Field(nameof(Shortcut._shortcut)).GetValue<Button>();
            TooltipCaller _tooltip = new Traverse(shortcut).Field(nameof(Shortcut._tooltip)).GetValue<TooltipCaller>();
            Traverse _dayPart = new Traverse(shortcut).Field(nameof(Shortcut._dayPart));

            BindingFlags flag = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic;
            MethodInfo AddLabel = typeof(Shortcut).GetMethod(nameof(Shortcut.AddLabel), flag);
            MethodInfo AddHint = typeof(Shortcut).GetMethod(nameof(Shortcut.AddHint), flag);
            MethodInfo SetTooltipText = typeof(Shortcut).GetMethod(nameof(Shortcut.SetTooltipText), flag);

            // Add hints
            if (SingletonMonoBehaviour<StatusManager>.Instance != null)
            {
                _dayPart.SetValue(SingletonMonoBehaviour<StatusManager>.Instance.GetStatusObservable(StatusType.DayPart));
                _dayPart.GetValue<ReactiveProperty<int>>().Subscribe(delegate (int _)
                {
                    AddHint.Invoke(shortcut, null);
                }).AddTo(shortcut.gameObject);
                AddHint.Invoke(shortcut, null);
            }

            // Redo click listeners
            _shortcut.onClick.RemoveAllListeners();
            _shortcut.onClick.AddListener(delegate
            {
                // Don't click if the odekake countdown has ticked to its limit!
                if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 0)
                {
                    _shortcut.interactable = false;
                    _tooltip.isShowTooltip = true;
                    _tooltip.type = TooltipType.Tooltip_Angel;
                    return;
                }

                SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(shortcut.appType, true);
                return;
            });

            // Same thing as the click event, but we're gonna run it once beforehand too
            if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeCountdown.Swap()) == 0)
            {
                _shortcut.interactable = false;
                _tooltip.isShowTooltip = true;
                _tooltip.type = TooltipType.Tooltip_Angel;
                return;
            }
            
            // Add the shortcut label
            SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Subscribe(delegate (LanguageType _)
            {
                AddLabel.Invoke(shortcut, null);
                SystemTextType tt = new Traverse(shortcut).Field(nameof(Shortcut.tooltipTextType)).GetValue<SystemTextType>();
                SetTooltipText.Invoke(shortcut, new object[] { tt });
            }).AddTo(_shortcut);
            AddLabel.Invoke(shortcut, null);
        }

        public static void DayPassingStartAlternate(this ngov3.Effect.DayPassing dayPass)
        {
            // I'm not actually sure this is ever used, but this is here for consistency's sake just in case.
            // ...in fact, I'm not sure if any of the "non-2D" variants of objects are used...
            NeedyMintsMod.log.LogMessage("DayPassing");
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
            NeedyMintsMod.log.LogMessage("DayPassing2D");
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
                NeedyMintsMod.log.LogMessage($"dayIndex: {_dayIndex.GetValue<ReactiveProperty<int>>().Value}");
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
                app.StartGame();
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
            NeedyMintsMod.log.LogMessage($"Created special tweet data with {data.FavNumber} favs and {data.RtNumber} rts");
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

            NeedyMintsMod.log.LogMessage($"Giga: {ConvertGigaNumber.GetValue(tweetDrawable.RtNumber)}");
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
    }
}
