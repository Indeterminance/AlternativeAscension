using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NeedyEnums;
using NeedyMintsOverdose;
using NGO;
using UniRx;
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace ngov3
{
    // Token: 0x020005FF RID: 1535
    public class Scenario_love_finale_Afterhaishin : NgoEvent
    {
        // Token: 0x06001CEC RID: 7404 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CED RID: 7405 RVA: 0x000BC11C File Offset: 0x000BA31C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<EventManager>.Instance.nowEnding = ModdedEndingType.Ending_Love.Swap();
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_talk_e");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_FINALE_JINE010.Swap());
            SingletonMonoBehaviour<NotificationManager>.Instance.osimai();
            base.endEvent();
        }
    }
}