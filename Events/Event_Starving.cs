using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using UniRx;

namespace AlternativeAscension
{
    public class Event_Starving : NgoEvent
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
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_STARVING_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_STARVING_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_STARVING_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_STARVING_JINE004.Swap());
            (from v in SingletonMonoBehaviour<NotificationManager>.Instance.ObserveEveryValueChanged((NotificationManager _) => SingletonMonoBehaviour<NotificationManager>.Instance._notiferParent.childCount, FrameCountType.Update, false)
             where v == 0
             select v).First<int>().Subscribe(async delegate (int _)
             {
                 IWindow window = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.GoOut, true);
                 SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(window);
                 SingletonMonoBehaviour<JineManager>.Instance.StartStamp();
                 await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(StatusType.Love, 2);
                 base.endEvent();
             }).AddTo(this.compositeDisposable);
        }
    }
}
