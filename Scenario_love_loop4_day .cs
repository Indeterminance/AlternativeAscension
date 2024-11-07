using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using System;

namespace AlternativeAscension
{
    public class Scenario_love_loop4_day : Scenario_love_loopbase
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        
        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask attemptLoop(CancellationTokenSource cts)
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_g");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP4_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP4_JINE002.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP4_JINE003, cts);

            // This is taken pretty much 1:1 from Action_PlayMakeLove from the vanilla code
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            IWindow ame = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam, true);
            ame.Uncloseable();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_out_f");
            await UniTask.Delay(3400, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            await SingletonMonoBehaviour<WindowManager>.Instance.GetNakamiFromApp(AppType.Webcam).GetComponent<App_Webcam>().Yusayusa();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("");
            await NgoEvent.DelaySkippable(Constants.FAST);

            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP4_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP4_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP4_JINE006.Swap());

        }
    }
}
