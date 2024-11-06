using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;

namespace NeedyMintsOverdose
{
    public class Event_MV_kowai: NgoEvent
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
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_c");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.EVENT_SONG_DENY006.Swap());
            base.endEvent();
        }
    }
}
