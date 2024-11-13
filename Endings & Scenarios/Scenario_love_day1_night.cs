using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NeedyEnums;
using AlternativeAscension;
using NGO;
using UniRx;
using static AlternativeAscension.AAPatches;

namespace ngov3
{
    // Token: 0x020005FF RID: 1535
    public class Scenario_love_day1_night : NgoEvent
    {
        // Token: 0x06001CEC RID: 7404 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CED RID: 7405 RVA: 0x000BC11C File Offset: 0x000BA31C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam).Uncloseable();
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE007.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE008.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE008.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue1()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE010.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE011.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE011.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue2()
        {

            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_idle_happy_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE012.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_POSTSTREAM_JINE013.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);

            // This is taken pretty much 1:1 from Action_PlayMakeLove from the vanilla code
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            PostEffectManager.Instance.SetShader(EffectType.Otona);
            IWindow ame = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam, true);
            ame.Uncloseable();
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_out_e");
            await UniTask.Delay(3400, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            await SingletonMonoBehaviour<WindowManager>.Instance.GetNakamiFromApp(AppType.Webcam).GetComponent<App_Webcam>().Yusayusa();
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("");
            PostEffectManager.Instance.ResetShaderCalmly(true);
            await NgoEvent.DelaySkippable(Constants.FAST);

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter).Uncloseable();
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ENDING_LOVE_DAY1_TWEET003.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.ENDING_LOVE_DAY1_TWEET004.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET004_KUSO001,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET004_KUSO002,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET004_KUSO003,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET004_KUSO004,
                ModdedKusoRepType.ENDING_LOVE_DAY1_TWEET004_KUSO005,
            }.Swap());
            await UniTask.Delay(13000);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE003.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE004.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE004.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue3();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue3()
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_idle_happy_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE009.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE010.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY1_AFTERGLOW_JINE010.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue4();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue4()
        {
            EventManager em = SingletonMonoBehaviour<EventManager>.Instance;
            SingletonMonoBehaviour<StatusManager>.Instance.timePassingToNextMorning();
            base.endEvent();
        }
    }
}