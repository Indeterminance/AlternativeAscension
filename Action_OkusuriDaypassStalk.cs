using Cysharp.Threading.Tasks;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NeedyEnums;
using System.Security.Policy;

namespace NeedyMintsOverdose
{
    public class Action_OkusuriDaypassStalk : NgoEvent
    {
        public override void Awake()
        {
            base.Awake();
        }


        public override async UniTask startEvent(CancellationToken token = default)
        {
            base.startEvent(token);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim(SingletonMonoBehaviour<EventManager>.Instance.PlatformDiffAnimationMaster.GetAnimationNameFromKey(PlatformDiffAnimationKey.DAYPASS));
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.DARKNIGHT_TWEET001.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.DARKNIGHT_TWEET002.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.DARKNIGHT_TWEET003.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.DARKNIGHT_TWEET004.Swap(), null, null);
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.PostDepaz);
            await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(ModdedStatusType.OdekakeStressMultiplier.Swap(), 1);
            base.endEvent();
        }
    }
}
