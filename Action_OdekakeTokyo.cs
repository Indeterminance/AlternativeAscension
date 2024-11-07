using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using System.Collections.Generic;

namespace AlternativeAscension
{
    public class Action_OdekakeTokyo : NgoEvent
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
            await base.GoOut();
            StatusManager sm = SingletonMonoBehaviour<StatusManager>.Instance;
            bool firstTime = sm.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap()) == (int)FollowerPlotFlagValues.None;
            if (firstTime)
            {
                SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.TOKYO_TWEET001.Swap(), new List<ModdedKusoRepType>
                {
                    ModdedKusoRepType.TOKYO_TWEET001_KUSO001,
                    ModdedKusoRepType.TOKYO_TWEET001_KUSO002,
                    ModdedKusoRepType.TOKYO_TWEET001_KUSO003,
                    ModdedKusoRepType.TOKYO_TWEET001_KUSO004,
                }.Swap(), null);
                SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.TOKYO_TWEET002.Swap(), null, null);
            }
            await sm.UpdateStatus(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.VisitedComiket);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam, true);
            await base.BackFromOdekake();
            SingletonMonoBehaviour<JineManager>.Instance.StartStamp();
            base.endEvent();
        }
    }
}
