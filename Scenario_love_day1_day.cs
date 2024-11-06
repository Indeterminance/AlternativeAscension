using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NeedyEnums;
using NeedyMintsOverdose;
using NGO;
using UniRx;
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace ngov3
{
    // Token: 0x020005FF RID: 1535
    public class Scenario_love_day1_day : NgoEvent
    {
        // Token: 0x06001CEC RID: 7404 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CED RID: 7405 RVA: 0x000BC11C File Offset: 0x000BA31C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isLove = true;
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_endng_normal, true);

            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam).Uncloseable();
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE009.Swap());
            
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_JINE010.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_JINE010.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue1()
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE011.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE012.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE014.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE015.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE016.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE017.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE018.Swap());

            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_JINE019.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_JINE019.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue2()
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_positive_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE020.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE021.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE022.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE023.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE024.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_JINE025.Swap());
            IWindow Poketter = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            Poketter.Uncloseable();
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ENDING_LOVE_DAY1_TWEET001.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ENDING_LOVE_DAY1_TWEET002.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET002_KUSO001,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET002_KUSO002,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET002_KUSO003,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET002_KUSO004,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET002_KUSO005,
            }.Swap(), null);
            await UniTask.Delay(13000, false, PlayerLoopTiming.Update, default, false);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_e");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTTWEET_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTTWEET_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTTWEET_JINE003.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            BumpDayMax();
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Action_HaishinStart>();
            base.endEvent();
        }
    }
}