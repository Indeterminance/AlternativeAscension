using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NeedyEnums;
using AlternativeAscension;
using NGO;
using UniRx;
using static AlternativeAscension.AAPatches;

namespace ngov3
{
    // Token: 0x020005FF RID: 1535
    public class Scenario_love_day2_day : NgoEvent
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
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_endng_normal, true);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam).Uncloseable();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_e");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE005.Swap());

            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_f");
            await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE009.Swap());
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_g");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE010.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE011.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY2_JINE012.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY2_JINE012.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue1()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE014.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE015.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_DAY2_JINE016.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_DAY2_JINE016.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue2()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE017.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE018.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE019.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_DAY2_JINE020.Swap());
            SingletonMonoBehaviour<AltAscModManager>.Instance.isLoveLoop = true;
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_loop1_day>();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_f");
            //SingletonMonoBehaviour<WindowManager>.Instance.LoveOut(); ;
            PostEffectManager.Instance.SetShader((EffectType)(int)ModdedEffectType.Love);
            float weight = 0f;
            DOTween.To(() => weight, delegate (float x)
            {
                PostEffectManager.Instance.SetShaderWeight(x);
            }, 1f, 1.5f).Play().WaitForCompletion();
            base.endEvent();
        }
    }
}