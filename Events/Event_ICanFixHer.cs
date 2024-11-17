using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Event_ICanFixHer: NgoEvent
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
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ICANFIXHER_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ICANFIXHER_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ICANFIXHER_JINE003.Swap());
            await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(StatusType.Yami, 1);
            base.endEvent();
        }
    }
}
