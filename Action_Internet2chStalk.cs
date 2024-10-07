using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace NeedyMintsOverdose
{
    public class Action_Internet2chStalk : NgoEvent
    {
        // Token: 0x0600166F RID: 5743 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001670 RID: 5744 RVA: 0x00085940 File Offset: 0x00083B40
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken, true);
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_mainloop_yami,true);
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_egosearching");
            SingletonMonoBehaviour<WindowManager>.Instance.GetNakamiFromApp(AppType.Keijiban).GetComponent<KitsuneView>().Jien();
            await NgoEvent.DelaySkippable(Constants.SLOW);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKDISCOVER_TWEET001.Swap(), null, null);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.STALKDISCOVER_TWEET002.Swap(), null, null);
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE006.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.FOLLOW_ST_JINE007.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.FOLLOW_ST_JINE007.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        public async void eventContinue1()
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.FOLLOW_ST_JINE010.Swap());
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber((StatusType)(int)ModdedStatusType.FollowerPlotFlag, (int)FollowerPlotFlagValues.StalkReveal);
            base.endEvent();
        }
    }
}
