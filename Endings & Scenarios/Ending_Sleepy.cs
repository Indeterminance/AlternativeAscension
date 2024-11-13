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
using UniRx;

namespace AlternativeAscension
{
    internal partial class AAPatches {
        public class Ending_Sleepy : NgoEvent
        {
            JineManager jm;

            // Token: 0x06001CF2 RID: 7410 RVA: 0x000843F9 File Offset: 0x000825F9
            public override void Awake()
            {
                base.Awake();
            }

            // Token: 0x06001CF3 RID: 7411 RVA: 0x000BC20C File Offset: 0x000BA40C
            public override async UniTask startEvent(CancellationToken cancellationToken = default)
            {
                jm = SingletonMonoBehaviour<JineManager>.Instance;
                base.startEvent(cancellationToken);
                await NgoEvent.DelaySkippable(Constants.MIDDLE);
                SingletonMonoBehaviour<EventManager>.Instance.nowEnding = (EndingType)ModdedEndingType.Ending_Sleepy;
                SingletonMonoBehaviour<CommandManager>.Instance.disableAllCommands();
                SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
                SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
                SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();
                SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
                await NgoEvent.DelaySkippable(Constants.SLOW);
                AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_emo);
                jm.addEventSeparator("Sleep: Never enough");
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE001.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE002.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE003.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE004.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE005.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE006.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE007.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE008.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE009.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE010.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE011.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE012.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE013.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE014.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE015.Swap());
                SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
                {
                    ModdedJineType.ENDING_SLEEPY_JINE016.Swap()
                });
                SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_SLEEPY_JINE016.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
                {
                    await NgoEvent.DelaySkippable(Constants.FAST);
                    this.eventContinue1();
                }).AddTo(this.compositeDisposable);
            }

            private async UniTask eventContinue1()
            {
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE017.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE018.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE019.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE020.Swap());
                await jm.AddJineHistory(ModdedJineType.ENDING_SLEEPY_JINE021.Swap());
                PostEffectManager.Instance.AnmakuWeight(0.6f);
                SingletonMonoBehaviour<WebCamManager>.Instance.GoOut();
                await UniTask.Delay(Constants.MIDDLE * 2, false, PlayerLoopTiming.Update, default, false);
                await SingletonMonoBehaviour<Poem>.Instance.StartPoem(NgoEx.SystemTextFromType(ModdedSystemTextType.Poem_Sleepy.Swap(), SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value), false);
                await NgoEvent.DelaySkippable(Constants.SLOW);
                SingletonMonoBehaviour<Poem>.Instance.CleanPoem();
                SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Jine);
                SingletonMonoBehaviour<NotificationManager>.Instance.osimai();
            }
        }
    }
}
