using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine;

namespace AlternativeAscension
{
    public class Scenario_love_loop5_day : Scenario_love_loopbase
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        
        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask attemptLoop(CancellationTokenSource cts)
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP5_JINE001.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP5_JINE002, cts);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP5_JINE003.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP5_JINE004, cts);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP5_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP5_JINE006.Swap());
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_positive_g");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);

            await SingletonMonoBehaviour<Poem>.Instance.StartPoem(NgoEx.SystemTextFromType(ModdedSystemTextType.Poem_Love.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value));
            await NgoEvent.DelaySkippable(Constants.SLOW);
            SingletonMonoBehaviour<Poem>.Instance.CleanPoem();
        }
    }
}
