using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using UniRx;

namespace AlternativeAscension
{
    public class Event_Sick : NgoEvent
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
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_idle_anxiety_c");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE003.Swap());
            await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_vomiting2");
            await SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatus(StatusType.Stress, 2);
            await UniTask.Delay(Constants.SLOW, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_SICK_JINE007.Swap());

            (from v in SingletonMonoBehaviour<NotificationManager>.Instance.ObserveEveryValueChanged((NotificationManager _) => SingletonMonoBehaviour<NotificationManager>.Instance._notiferParent.childCount, FrameCountType.Update, false)
             where v == 0
             select v).Subscribe(async delegate (int _)
             {
                 await NgoEvent.DelaySkippable(Constants.MIDDLE);
                 await SingletonMonoBehaviour<EventManager>.Instance.ExecuteActionConfirmed(ActionType.SleepToTomorrow, CmdType.SleepToTomorrow3, true);
                 base.endEvent();
             }).AddTo(this.compositeDisposable);
        }
    }
}
