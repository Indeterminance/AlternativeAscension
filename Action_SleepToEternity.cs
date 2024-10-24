using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NeedyMintsOverdose
{
    public class Action_SleepToEternity : NgoEvent
    {
        // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken, true);
            SingletonMonoBehaviour<WebCamManager>.Instance.GoOut();
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            PostEffectManager.Instance.AnmakuWeight(1f);
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().alpha = 0f;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("ShortCutParent").GetComponent<CanvasGroup>().blocksRaycasts = false;
            SingletonMonoBehaviour<TaskbarManager>.Instance.TaskBarGroup.alpha = 0f;
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.black;
            GameObject.Find("stack").SetActive(false);
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            PostEffectManager.Instance.AnmakuWeight(0.7f);
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Ending_Sleepy>();
            base.endEvent();
        }
    }
}
