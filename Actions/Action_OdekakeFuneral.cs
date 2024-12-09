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
using static AlternativeAscension.Alternates;
using UnityEngine.UI;

namespace AlternativeAscension
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
                PostEffectManager.Instance.SetShader(EffectType.Bleeding);
                PostEffectManager.Instance.SetShaderWeight(1f, EffectType.WristCut);
                PostEffectManager.Instance.SetShaderWeight(1f, EffectType.Bleeding);
                PostEffectManager.Instance.SetShaderWeight(0.1f, EffectType.BloomLight);
                PostEffectManager.Instance.AnmakuWeight(0f);
                GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().alpha = 0.5f;
            }
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().blocksRaycasts = false;
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //SingletonMonoBehaviour<TaskbarManager>.Instance.TaskBarGroup.alpha = 0f;
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
            if (dark) await UniTask.Delay(Constants.SLOW);
            else await UniTask.Delay(Constants.FAST);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Webcam);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.GoOut);

            IWindow train = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Train_night, true);
            train.GameObjectTransform.position = new Vector3(-2.5f, 2f, 60f);
            if (dark) {
                SingletonMonoBehaviour<AltAscModManager>.Instance.viewing.Value = true;
                SingletonMonoBehaviour<AltAscModManager>.Instance.viewInterval.Value = 30; //80
            }

            AudioManager.Instance.PlayBgmByType(SoundType.BGM_Train, true);
            await UniTask.Delay(15000, false, PlayerLoopTiming.Update);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Train_night);
            //SingletonMonoBehaviour<NeedyMintsModManager>.Instance.viewing.Value = false;
        }
    }
}
