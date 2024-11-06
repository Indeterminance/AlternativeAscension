using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NeedyMintsOverdose
{
    public class Scenario_follower_day3_night : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            //await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            base.startEvent(cancellationToken);
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.black;
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isFollowerBG.Value = true;
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.CleanOnCommand();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);

            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_normal_c");
            await UniTask.Delay(Constants.MIDDLE);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE002.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE003.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE003.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                await this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask eventContinue1(CancellationToken cancellationToken = default(CancellationToken))
        {
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE010.Swap());
            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_happy_d");
            await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE011.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE012.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY3_PRESTREAM_JINE014.Swap());

            SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim(SingletonMonoBehaviour<EventManager>.Instance.PlatformDiffAnimationMaster.GetAnimationNameFromKey(PlatformDiffAnimationKey.DAYPASS));
            await UniTask.Delay(Constants.FAST,false, PlayerLoopTiming.Update, default, false);
            Queue<IWindow> pillWindows = new Queue<IWindow> { };
            for (int i = 0; i < 20; i++)
            {
                IWindow pill = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow_NoInteractive((AppType)(int)ModdedAppType.PillDaypass_Follower);
                pill.setRandomPosition();
                pillWindows.Enqueue(pill);
                await UniTask.Delay(30);
            }
            while (pillWindows.Count > 0)
            {
                IWindow pill = pillWindows.Dequeue();
                SheetView sheet = pill.nakamiApp.GetComponentInChildren<SheetView>();
                for (int i = 0; i < 3; i++)
                {
                    sheet.OnDose();
                    await UniTask.Delay(30);
                }
                SingletonMonoBehaviour<WindowManager>.Instance.Close(pill);
            }
            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();
            PostEffectManager.Instance.SetShader(EffectType.BloomLight);
            PostEffectManager.Instance.SetShaderWeight(0.1f);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Bank);
            AudioManager.Instance.PlayBgmByType(SoundType.BANK_bank, false);
            await UniTask.Delay(6000, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            await UniTask.Delay(10000, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Bank);
            // Follower plot flag is set to AngelDeath, so LiveScenario.SetScenario should find the correct stream
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Broadcast, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Broadcast);
            SingletonMonoBehaviour<WindowManager>.Instance.UnMovable(AppType.Broadcast);
            base.endEvent();
        } 
    }
}
