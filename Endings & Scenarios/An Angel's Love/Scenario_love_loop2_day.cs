﻿using Cysharp.Threading.Tasks;
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
    public class Scenario_love_loop2_day : Scenario_love_loopbase
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        
        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask attemptLoop(CancellationTokenSource cts)
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_idle_iraira_f");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP2_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP2_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP2_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP2_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_LOOP2_JINE005.Swap());
            await TimeoutJINE(ModdedJineType.ENDING_LOVE_LOOP2_JINE006, cts);
        }
    }
}
