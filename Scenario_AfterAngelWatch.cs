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
    public class Scenario_AfterAngelWatch : NgoEvent
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
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_wind, true);

            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.TaskManager);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);

            GameObject.Find("MainPanel").GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(0.3f);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE060.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE061.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE061.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue1()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE062.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE063.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE064.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE065.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE066.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE067.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE068.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE069.Swap());


            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE070.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE070.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue2()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE071.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE072.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE073.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE074.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE075.Swap());


            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE076.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE076.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue3();
            }).AddTo(this.compositeDisposable);
        }

        // Tweets happen here
        private async void eventContinue3()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE077.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE078.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE079.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE080.Swap());

            await NgoEvent.DelaySkippable(Constants.SLOW);

            IWindow Tweeter = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            Tweeter.Uncloseable();
            await NgoEvent.DelaySkippable(Constants.MIDDLE);

            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET001.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET002.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET003.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET004.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.POSTANGELWATCH_TWEET005.Swap(), null, null);
            await UniTask.Delay(17000);

            IWindow JINE = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.Jine);
            JINE.Touched();

            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE081.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE082.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE083.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE083.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
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
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE084.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE085.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE086.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE087.Swap());

            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE088.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE088.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue5();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue5()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE089.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE090.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE091.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE092.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE093.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE094.Swap());

            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE095.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE095.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue6();
            }).AddTo(this.compositeDisposable);
        }

        private async void eventContinue6()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE096.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE097.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE098.Swap());
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.BadPassword);
            BumpDayMax();
            SingletonMonoBehaviour<NotificationManager>.Instance.AddDayPassingNotifier();
            PostEffectManager.Instance.ResetShaderCalmly();
            base.endEvent();
        }
    }
}
