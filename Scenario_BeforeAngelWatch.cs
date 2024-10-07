using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System.Collections.Generic;

namespace NeedyMintsOverdose
{
    public class Scenario_BeforeAngelWatch : NgoEvent
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
            List<ModdedJineType> dlg = new List<ModdedJineType>()
            {
                ModdedJineType.ENDING_FOLLOWER_JINE001,
                ModdedJineType.ENDING_FOLLOWER_JINE002,
                ModdedJineType.ENDING_FOLLOWER_JINE003,
                ModdedJineType.ENDING_FOLLOWER_JINE004,
                ModdedJineType.ENDING_FOLLOWER_JINE005,
                ModdedJineType.ENDING_FOLLOWER_JINE006,
                ModdedJineType.ENDING_FOLLOWER_JINE007,
                ModdedJineType.ENDING_FOLLOWER_JINE008,
                ModdedJineType.ENDING_FOLLOWER_JINE009,
                ModdedJineType.ENDING_FOLLOWER_JINE010,
                ModdedJineType.ENDING_FOLLOWER_JINE011,
                ModdedJineType.ENDING_FOLLOWER_JINE012,
            };
            foreach (ModdedJineType type in dlg)
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(type.Swap());
            }
            JineData data = new JineData(ModdedJineType.ENDING_FOLLOWER_JINE013.Swap());
            data.user = JineUserType.pi;
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(data);
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            List<ModdedJineType> dlg2 = new List<ModdedJineType>() {
                ModdedJineType.ENDING_FOLLOWER_JINE014,
                ModdedJineType.ENDING_FOLLOWER_JINE015,
                ModdedJineType.ENDING_FOLLOWER_JINE016
            };
            foreach(ModdedJineType type in dlg2)
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(type.Swap());
            }
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE017.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE018.Swap());
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Poketter);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.PREANGELWATCH_TWEET001.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.PREANGELWATCH_TWEET002.Swap());
            await UniTask.Delay(8000);
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.AngelWatch);
            SingletonMonoBehaviour<EventManager>.Instance.alpha = (AlphaType)(int)ModdedAlphaType.FollowerAlpha;
            SingletonMonoBehaviour<EventManager>.Instance.alphaLevel = 2;
            SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<Action_HaishinStart>();


            base.endEvent();
        }
    }
}
