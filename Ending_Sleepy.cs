using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeedyEnums;

namespace NeedyMintsOverdose
{
    internal partial class MintyOverdosePatches {
        public class Ending_Sleepy : NgoEvent
        {
            // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
            public override void Awake()
            {
                base.Awake();
            }

            // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
            public override async UniTask startEvent(CancellationToken cancellationToken = default)
            {
                base.startEvent(cancellationToken);
                AudioManager.Instance.StopBgm();
                SingletonMonoBehaviour<EventManager>.Instance.nowEnding = (EndingType)ModdedEndingType.Ending_Sleepy;
                SingletonMonoBehaviour<CommandManager>.Instance.disableAllCommands();
                SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
                SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
                SingletonMonoBehaviour<JineManager>.Instance.addEventSeparator("Sleep: Never enough");
                List<ModdedJineType> endingJines = new List<ModdedJineType>()
                {
                    ModdedJineType.ENDING_SLEEPY_JINE001,
                    ModdedJineType.ENDING_SLEEPY_JINE002,
                    ModdedJineType.ENDING_SLEEPY_JINE003,
                    ModdedJineType.ENDING_SLEEPY_JINE004,
                    ModdedJineType.ENDING_SLEEPY_JINE005,
                    ModdedJineType.ENDING_SLEEPY_JINE006,
                    ModdedJineType.ENDING_SLEEPY_JINE007,
                    ModdedJineType.ENDING_SLEEPY_JINE008,
                    ModdedJineType.ENDING_SLEEPY_JINE009,
                    ModdedJineType.ENDING_SLEEPY_JINE010
                };
                PostEffectManager pem = PostEffectManager.Instance;
                pem.SetShader(EffectType.Anmaku);
                foreach (ModdedJineType jine in endingJines)
                {
                    pem.SetShaderWeight((float)(endingJines.IndexOf(jine)+1)/11f,EffectType.Anmaku);
                    await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(jine.Swap());

                }
                pem.SetShaderWeight(0.98f, EffectType.Anmaku);
                IWindow ame = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam, true);
                ame.Uncloseable();
                SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_out_e");
                await UniTask.Delay(3400, false, PlayerLoopTiming.Update, default(CancellationToken), false);
                //SingletonMonoBehaviour<TooltipManager>.Instance.Show(TooltipType.Tooltip_yaru);
                await NgoEvent.DelaySkippable(Constants.MIDDLE);
                SingletonMonoBehaviour<NotificationManager>.Instance.osimai();
                base.endEvent();

            }
        }
    }
}
