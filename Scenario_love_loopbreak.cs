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
    public class Scenario_love_loopbreak : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isLoveLoop = false;
            PostEffectManager.Instance.ResetShaderCalmly();
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_a");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE005.Swap());
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_a");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE008.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_BREAKUP_JINE009.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_BREAKUP_JINE009.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue1()
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_iraira_b");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE010.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE011.Swap());
            await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE012.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE014.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_BREAKUP_JINE015.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_BREAKUP_JINE015.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue2();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue2()
        {
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_e");
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE016.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE017.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_LOVE_BREAKUP_JINE018.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_LOVE_BREAKUP_JINE018.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                this.eventContinue3();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask eventContinue3()
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE019.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE020.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_LOVE_BREAKUP_JINE021.Swap());

            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_out_a");
            await UniTask.Delay(Constants.SLOW, false, PlayerLoopTiming.Update, default, false);
            await GameObject.Find("DayPassingCover").GetComponent<IDayPassing>().yearsPass(true);
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isLoveLoop = false;

            StatusManager sm = SingletonMonoBehaviour<StatusManager>.Instance;
            int followers = sm.GetStatus(StatusType.Follower);
            if (followers < 100000)
            {
                followers = UnityEngine.Random.Range(800000, 1400000);
            }
            else if (followers < 500000)
            {
                followers = UnityEngine.Random.Range(1200000, 2500000);
            }
            else if (followers < 1000000)
            {
                followers = UnityEngine.Random.Range(3000000, 6000000);
            }
            else if (followers < 2000000)
            {
                followers = UnityEngine.Random.Range(8000000, 9500000);
            }
            else followers = 9999999;
            sm.UpdateStatusToNumber(StatusType.Follower, followers);

            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_finale_beforeHaishin>();
            base.endEvent();
        }
    }
}
