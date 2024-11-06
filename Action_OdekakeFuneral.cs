using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using NGO;
using System;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Reflection;
using HarmonyLib;
using DG.Tweening;
using System.Collections.Generic;
using static NeedyMintsOverdose.Alternates;

namespace NeedyMintsOverdose
{
    public class Action_OdekakeFuneral : NgoEvent
    {
        bool dark = false;
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken, true);
            dark = (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.OdekakeStressMultiplier.Swap()) > 7 &&
                SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.AMAStress.Swap()) >= 15);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            if (dark)
            {
                PostEffectManager.Instance.SetShader(EffectType.WristCut);
                PostEffectManager.Instance.SetShaderWeight(1f);
                PostEffectManager.Instance.SetShader(EffectType.Bleeding);
                PostEffectManager.Instance.SetShaderWeight(0.8f);
                GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().alpha = 0.5f;
                SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_out_d");
                await UniTask.Delay(3000, false, PlayerLoopTiming.Update, default, false);
            }
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().blocksRaycasts = false;
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<TaskbarManager>.Instance.TaskBarGroup.alpha = 0f;
            //NeedyMintsMod.log.LogMessage($"Followers!");
            await GoOut();


            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();
            IWindow taiki = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow_Compact((AppType)(int)ModdedAppType.Follower_taiki, true);
            taiki.Uncloseable();
            taiki.UnMovable();
            SingletonMonoBehaviour<EventManager>.Instance.ClearEventQueue();
            base.endEvent();
        }

        protected async UniTask GoOut()
        {
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            AudioManager.Instance.PlaySeByType(SoundType.SE_Odekake_zazaza, false);
            SingletonMonoBehaviour<WebCamManager>.Instance.GoOut();
            await UniTask.Delay(2000);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Webcam);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.GoOut);
            AppType TrainTime = AppType.Train;

            switch (SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart))
            {
                case 0:
                    TrainTime = AppType.Train;
                    break;
                case 1:
                    TrainTime = AppType.Train_twilight;
                    break;
                case 2:
                    TrainTime = AppType.Train_night;
                    break;
                default: goto case 0;
            }

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(TrainTime, true);
            if (dark) SingletonMonoBehaviour<NeedyMintsModManager>.Instance.viewing.Value = true;
            if (dark) SingletonMonoBehaviour<NeedyMintsModManager>.Instance.viewInterval.Value = 80;
            this.ClickableAllScreen(true);

            AudioManager.Instance.PlayBgmByType(SoundType.BGM_Train, false);
            await UniTask.Delay(15000, false, PlayerLoopTiming.Update);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(TrainTime);

            this.ClickableAllScreen(false);
            //SingletonMonoBehaviour<NeedyMintsModManager>.Instance.viewing.Value = false;
        }
    }
}
