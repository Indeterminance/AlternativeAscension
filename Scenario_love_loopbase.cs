using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using System;
using UnityEngine;
using ngov3.Effect;
using HarmonyLib;

namespace NeedyMintsOverdose
{
    public class Scenario_love_loopbase : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            Traverse dp = new Traverse(GameObject.Find("DayPassingCover").GetComponent<DayPassing2D>()).Field(nameof(DayPassing2D.playingAnimation));
            await UniTask.WaitUntil(() => dp.GetValue<bool>() == false);
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            BumpDayMax();


            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                await attemptLoop(cts);
                SingletonMonoBehaviour<StatusManager>.Instance.timePassingToNextMorning();
            }
            catch (OperationCanceledException ex) when (cts.IsCancellationRequested)
            {
                NeedyMintsMod.log.LogMessage($"{ex.Message}");
                SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_loopbreak>();
                await breakLoop();
            }
            cts.Dispose();
            base.endEvent();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public virtual async UniTask attemptLoop(CancellationTokenSource cts)
        {

        }

        protected async UniTask breakLoop()
        {
            NeedyMintsMod.log.LogMessage($"Loop broken!");
            //SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_loopbreak>();
        }

        protected async UniTask TimeoutJINE(ModdedJineType t, CancellationTokenSource cts)
        {
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                t.Swap()
            });

            bool breakLoop = true;
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == t.Swap()).Subscribe(delegate
            {
                breakLoop = false;
            }).AddTo(this.compositeDisposable);

            UniTask timeoutTask = UniTask.Delay(15000, false, PlayerLoopTiming.Update, default, false);
            UniTask noIgnoreTask = UniTask.WaitUntil(() => breakLoop == false);
            await UniTask.WhenAny(timeoutTask, noIgnoreTask);

            if (breakLoop)
            {
                NeedyMintsMod.log.LogMessage("Ignored message!");
                cts.Cancel();
            }
            else
            {
                await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            }
            cts.Token.ThrowIfCancellationRequested();
        }
    }
}
