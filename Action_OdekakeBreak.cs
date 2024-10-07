using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using NGO;
using System;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using DG.Tweening;
using System.Collections.Generic;
using static NeedyMintsOverdose.Alternates;

namespace NeedyMintsOverdose
{
    public class Action_OdekakeBreak : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken, true);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            int weight = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap());
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(1);
            await GoOut();
            //await UniTask.Delay(Constants.MIDDLE);
            AudioManager.Instance.PlaySeByType(SoundType.SE_chime);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam, true);
            await BackFromPanicOdekake(0);
            await Mistweet();
            PostEffectManager.Instance.ResetShaderCalmly();
            SingletonMonoBehaviour<JineManager>.Instance.StartStamp();
            base.endEvent();
        }

        protected async UniTask GoOut()
        {
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            AudioManager.Instance.PlaySeByType(SoundType.SE_Odekake_zazaza, false);
            SingletonMonoBehaviour<WebCamManager>.Instance.GoOut();
            await UniTask.Delay(2000);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Webcam);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.GoOut);
            AppType TrainTime = AppType.Train;

            switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart))
            {
                case 0:
                    TrainTime = AppType.Train;
                    break;
                case 1:
                    TrainTime = AppType.Train_twilight;
                    break;
                case 2:
                    TrainTime = AppType.Train_night;
                    break;
                default: goto case 0;
            }

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(TrainTime, true);
            this.ClickableAllScreen(true);

            AudioManager.Instance.PlayBgmByType(SoundType.BGM_Train, false);
            await UniTask.Delay(10000, false, PlayerLoopTiming.Update);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(TrainTime);


            SoundType soundType = SoundType.BGM_mainloop_kenjo;
            int status2 = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower);
            if (status2 >= 10000)
            {
                soundType = SoundType.BGM_mainloop_normal;
            }
            if (status2 >= 100000)
            {
                soundType = SoundType.BGM_mainloop_yami;
            }
            if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex) >= 11)
            {
                soundType = SoundType.BGM_mainloop_middle;
            }
            if (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex) >= 20)
            {
                soundType = SoundType.BGM_mainloop_shuban;
            }
            if (AudioManager.Instance != null)
            {
                if (AudioManager.Instance.PlayingBgm.Value != null)
                {
                    AudioManager instance = AudioManager.Instance;
                    if (!(((instance != null) ? instance.PlayingBgm.Value.Sound.Id : null) != soundType.ToString()))
                    {
                        return;
                    }
                }
                AudioManager instance2 = AudioManager.Instance;
                if (instance2 != null)
                {
                    instance2.PlayBgmByType(soundType, true);
                }
            }
            this.ClickableAllScreen(false);
        }

        public async UniTask Mistweet()
        {
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Poketter);
            await UniTask.Delay(12500);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(2);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE003.Swap());
            await NgoEx.DelayNGO(Constants.MIDDLE);
            await DeleteFirstTweet();

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_MISTWEET_JINE009.Swap());
            SingletonMonoBehaviour<WindowManager>.Instance.Closeable(AppType.Poketter);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
        }

        public async UniTask DeleteFirstTweet()
        {
            PoketterView2D tweeterWindow = SingletonMonoBehaviour<PoketterView2D>.Instance;
            RectTransform trav = (RectTransform)Traverse.Create(tweeterWindow).Field(nameof(PoketterView2D._Waku)).GetValue();
            Fader2D fader = trav.GetChild(1).gameObject.GetComponent<Fader2D>();
            await fader.DOFade(0.5f, 0f);
            trav.GetChild(1).gameObject.SetActive(false);

            PoketterManager tweeterManager = SingletonMonoBehaviour<PoketterManager>.Instance;
            Traverse tweetTraverse = Traverse.Create(tweeterManager).Field(nameof(PoketterManager.history));
            List<TweetData> tweets =tweetTraverse.GetValue() as List<TweetData>;
            tweets.RemoveAt(tweets.Count - 1);
            tweetTraverse.SetValue(tweets);
            

            return;
        }
    }
}
