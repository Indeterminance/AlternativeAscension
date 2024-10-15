using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine;
using static NeedyMintsOverdose.Alternates;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;

namespace NeedyMintsOverdose
{
    public class Scenario_follower_day2_AfterAllNighterhaishin : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);

            // Angel Watch might still be running, so we'll wait to make sure there's
            // no active stream window
            WindowManager windose = SingletonMonoBehaviour<WindowManager>.Instance;
            await UniTask.WaitUntil(() => windose.GetWindowFromApp(AppType.Broadcast) == null);
            base.startEvent(cancellationToken, true);

            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isFollowerBG.Value = true;
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_wind, true);

            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.TaskManager);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);

            GameObject.Find("MainPanel").GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(0.3f);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE001.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE002.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE002.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue1()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE010.Swap());


            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE011.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE011.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue2()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE012.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE014.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE015.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE016.Swap());


            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE017.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE017.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue3();
            }).AddTo(this.compositeDisposable);
        }

        // Tweets happen here
        private async void eventContinue3()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE018.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE019.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE020.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE021.Swap());

            await NgoEvent.DelaySkippable(Constants.SLOW);

            IWindow Tweeter = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            Tweeter.Uncloseable();
            await NgoEvent.DelaySkippable(Constants.MIDDLE);

            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET001.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.POSTANGELWATCH_TWEET001_KUSO001,
                ModdedKusoRepType.POSTANGELWATCH_TWEET001_KUSO002,
                ModdedKusoRepType.POSTANGELWATCH_TWEET001_KUSO003,
                ModdedKusoRepType.POSTANGELWATCH_TWEET001_KUSO004,
            }.Swap(), null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET002.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.POSTANGELWATCH_TWEET002_KUSO001,
                ModdedKusoRepType.POSTANGELWATCH_TWEET002_KUSO002,
                ModdedKusoRepType.POSTANGELWATCH_TWEET002_KUSO003,
            }.Swap(), null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET003.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.POSTANGELWATCH_TWEET003_KUSO001,
                ModdedKusoRepType.POSTANGELWATCH_TWEET003_KUSO002,
                ModdedKusoRepType.POSTANGELWATCH_TWEET003_KUSO003,
                ModdedKusoRepType.POSTANGELWATCH_TWEET003_KUSO004,
                ModdedKusoRepType.POSTANGELWATCH_TWEET003_KUSO005,
            }.Swap(), null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET004.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET005.Swap(), null, null);
            await UniTask.Delay(17000);

            IWindow JINE = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.Jine);
            JINE.Touched();

            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE022.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE023.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE024.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE024.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue4();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue4()
        {
            float startColor = 0f;
            DOTween.To(() => startColor, x => GameObject.Find("MainPanel").GetComponent<Image>().color = new Color(1f, x, x, 1f), 1f, 5).SetEase(Ease.InSine).Play();
            await UniTask.Delay(7000);
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_emo, true);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE025.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE026.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE027.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE028.Swap());

            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE029.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE029.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue5();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue5()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE030.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE031.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE032.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE033.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE034.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE035.Swap());

            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY2_JINE036.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY2_JINE036.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue6();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue6()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE037.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE038.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY2_JINE039.Swap());
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.BadPassword);
            BumpDayMax();
            SingletonMonoBehaviour<NotificationManager>.Instance.AddDayPassingNotifier();
            PostEffectManager.Instance.ResetShaderCalmly();
            base.endEvent();
        }
    }
}
