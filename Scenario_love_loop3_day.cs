using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using System;

namespace AlternativeAscension
{
    public class Scenario_love_loop3_day : Scenario_love_loopbase
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        
        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask attemptLoop(CancellationTokenSource cts)
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP3_JINE001.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP3_JINE002, cts);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP3_JINE003.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP3_JINE004, cts);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP3_JINE005.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP3_JINE006, cts);
        }
    }
}
