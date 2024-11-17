using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using UniRx.Triggers;
using UnityEngine.EventSystems;

namespace AlternativeAscension
{
    public class Scenario_follower_day4_day : NgoEvent
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
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);

            SingletonMonoBehaviour<AltAscModManager>.Instance.isFollowerBG.Value = true;
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.white;

            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();
            IWindow jine = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine, true);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Jine);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE005.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.addEventSeparator(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE_DELETEPI.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE006.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(ModdedJineType.ENDING_FOLLOWER_DAY4_JINE007.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            for (int i = 0; i < 10; i++)
            {
                AudioManager.Instance.PlaySeByType(SoundType.SE_click_error, false);
                jine.nakamiApp.transform.localScale = new Vector3(1, -1, 1);
                await UniTask.Delay(20);
                AudioManager.Instance.PlaySeByType(SoundType.SE_Error, false);
                jine.nakamiApp.transform.localScale = new Vector3(1, 1, 1);
                await UniTask.Delay(20);
            }
            AudioManager.Instance.PlaySeByType(SoundType.SE_status_down);
            SingletonMonoBehaviour<WindowManager>.Instance.Close(jine);

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter).Uncloseable();
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusorepsAmeBreak(ModdedTweetType.ANGELFUNERAL_TWEET001.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusorepsAmeBreak(ModdedTweetType.ANGELFUNERAL_TWEET002.Swap());
            await UniTask.Delay(10000);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Poketter);

            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(true, 0.1f);
            //SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(true);
            // TODO: The below "end event" function isn't running. Figure out why.
            base.endEvent();
        }
    }
}
