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
    public class Scenario_BeforeAngelDeath : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            base.startEvent(cancellationToken);
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.black;
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.CleanOnCommand();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
            await UniTask.Delay(Constants.MIDDLE);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE144.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_JINE145.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE146.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE146.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                await NgoEvent.DelaySkippable(Constants.FAST);
                await this.eventContinue1();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask eventContinue1(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<ModdedJineType> jines = new List<ModdedJineType>()
            {
                ModdedJineType.ENDING_FOLLOWER_JINE147,
                ModdedJineType.ENDING_FOLLOWER_JINE148,
                ModdedJineType.ENDING_FOLLOWER_JINE149,
                ModdedJineType.ENDING_FOLLOWER_JINE150,
                ModdedJineType.ENDING_FOLLOWER_JINE151,
                ModdedJineType.ENDING_FOLLOWER_JINE152,
                ModdedJineType.ENDING_FOLLOWER_JINE153,
                ModdedJineType.ENDING_FOLLOWER_JINE154,
                ModdedJineType.ENDING_FOLLOWER_JINE155,
                ModdedJineType.ENDING_FOLLOWER_JINE156,
                ModdedJineType.ENDING_FOLLOWER_JINE157,
            };
            foreach (ModdedJineType jine in jines)
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(jine.Swap());
            }

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
