using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;

namespace AlternativeAscension
{
    public class Scenario_follower_day1_day : NgoEvent
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
            base.startEvent(cancellationToken);

            // FOLLOWERS can go past Day 30!
            BumpDayMax();

            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.TaskManager);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);


            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_c");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE010.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE011.Swap());

            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_negative_c");
            await UniTask.Delay(Constants.MIDDLE * 2, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE012.Swap());

            
            JineData data = new JineData(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE013.Swap());
            data.user = JineUserType.pi;
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(data);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_anxiety_c");
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            List<ModdedJineType> dlg2 = new List<ModdedJineType>() {
                ModdedJineType.ENDING_FOLLOWER_DAY1_JINE014,
                ModdedJineType.ENDING_FOLLOWER_DAY1_JINE015,
                ModdedJineType.ENDING_FOLLOWER_DAY1_JINE016
            };
            foreach(ModdedJineType type in dlg2)
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(type.Swap());
            }
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE017.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY1_JINE018.Swap());
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_talk_c");
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Poketter);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.PREANGELWATCH_TWEET001.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.PREANGELWATCH_TWEET001_KUSO001,
                ModdedKusoRepType.PREANGELWATCH_TWEET001_KUSO002,
                ModdedKusoRepType.PREANGELWATCH_TWEET001_KUSO003,
                ModdedKusoRepType.PREANGELWATCH_TWEET001_KUSO004,
                ModdedKusoRepType.PREANGELWATCH_TWEET001_KUSO005,
            }.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.PREANGELWATCH_TWEET002.Swap());
            await UniTask.Delay(16000);
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.AngelWatch);
            SingletonMonoBehaviour<EventManager>.Instance.alpha = (AlphaType)(int)ModdedAlphaType.FollowerAlpha;
            SingletonMonoBehaviour<EventManager>.Instance.alphaLevel = 2;
            PostEffectManager.Instance.SetShader(EffectType.GoCrazy);
            PostEffectManager.Instance.SetShaderWeight(1f);
            SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<Action_HaishinStart>();


            base.endEvent();
        }
    }
}
