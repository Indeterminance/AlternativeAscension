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
    public class Scenario_AfterAngelDeath : NgoEvent
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
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.black;
            PostEffectManager.Instance.SetShaderWeight(0.5f, (EffectType)(int)ModdedEffectType.Vengeful);
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.AngelFuneral);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            //await UniTask.Delay(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.CleanOnCommand();
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_wind, true);
            IWindow tweeter = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            tweeter.Uncloseable();
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusorepsAmeBreak(ModdedTweetType.DEADKANGEL_TWEET001.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusorepsAmeBreak(ModdedTweetType.DEADKANGEL_TWEET002.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusorepsAmeBreak(ModdedTweetType.DEADKANGEL_TWEET003.Swap());
            SingletonMonoBehaviour<NotificationManager>.Instance.AddDayPassingNotifier();
            base.endEvent();
        }
    }
}
