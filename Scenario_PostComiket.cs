using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace NeedyMintsOverdose
{
    public class Scenario_PostComiket : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<EventManager>.Instance.alpha = (AlphaType)(int)ModdedAlphaType.FollowerAlpha;
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.PostComiket);
            SingletonMonoBehaviour<EventManager>.Instance.alphaLevel = 1;
            SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<Action_HaishinStart>();
            base.endEvent();
        }
    }
}
