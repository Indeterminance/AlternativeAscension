using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static NeedyMintsOverdose.MintyOverdosePatches;
using NGO;
using System;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine;
using static NeedyMintsOverdose.Alternates;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using DG.Tweening;
using System.Runtime.Remoting.Messaging;
using Rewired.UI.ControlMapper;
using System.Linq;
using TMPro;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace NeedyMintsOverdose
{
    public class Scenario_Hacked : NgoEvent
    {
        private LoginHacked loginComp;
        public override void Awake()
        {
            base.Awake();
        }

        // TODO: WHY ISN'T THIS BEING RUN????
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<StatusManager>.Instance.timePassing(1);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            AudioManager.Instance.StopBgm();
            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE099.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE100.Swap());
            SetupLogin();
            await NgoEvent.DelaySkippable(800);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE101.Swap());
            await NgoEvent.DelaySkippable(1000);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE102.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE103.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE104.Swap());
            loginComp._input.interactable = true;
        }

        private void SetupLogin()
        {
            IWindow window = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow((AppType)(int)ModdedAppType.Login_Hacked);
            window.Uncloseable();

            loginComp = window.nakamiApp.GetComponent<LoginHacked>();
            Queue<Func<UniTask>> actionQueue = loginComp.loginActions;
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt1()));
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt2()));
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt3()));
        }

        public async UniTask attempt1(CancellationToken cancellationToken = default(CancellationToken))
        {
            PostEffectManager.Instance.SetShader(EffectType.OD);
            PostEffectManager.Instance.SetShaderWeight(0.01f);
            await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE105.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE106.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE107.Swap());
            await UniTask.Delay(Constants.FAST);
        }

        public async UniTask attempt2(CancellationToken cancellationToken = default(CancellationToken))
        {
            PostEffectManager.Instance.SetShader(EffectType.OD);
            PostEffectManager.Instance.SetShaderWeight(0.03f);
            await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE108.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE109.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE110.Swap());
            
            await UniTask.Delay(Constants.FAST);
            IWindow login = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp((AppType)(int)ModdedAppType.Login_Hacked);
            login.Touched();

            for (int i = 0; i < 13; i++)
            {
                string input = string.Concat("angelkawaii2".Take(i));
                await UniTask.Delay(60);
                loginComp._input.text = input;
                //await loginComp._input.ObserveEveryValueChanged((TMP_InputField t) => t.text, FrameCountType.Update, false);
            }
            await UniTask.Delay(1380);
            login.Touched();
            ExecuteEvents.Execute(loginComp._login.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            await UniTask.Delay(60);
            login.Touched();
            ExecuteEvents.Execute(loginComp._login.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            loginComp._input.interactable = false;
        }

        public async UniTask attempt3(CancellationToken cancellationToken = default(CancellationToken))
        {
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_yami, true);
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.red;
            PostEffectManager.Instance.SetShader(EffectType.Kakusei);
            PostEffectManager.Instance.SetShaderWeight(0.1f);
            await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE111.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE112.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE113.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE114.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE115.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE116.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE117.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE118.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE119.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE120.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE121.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE121.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_2();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_2(CancellationToken cancellationToken = default(CancellationToken))
        {
            IWindow app = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(AppType.Jine);
            for (int i = 0; i < 5; i++)
            {
                AudioManager.Instance.PlaySeByType(SoundType.SE_click_error, false);
                app.nakamiApp.transform.localScale = new Vector3(1, -1, 1);
                await UniTask.Delay(50);
                AudioManager.Instance.PlaySeByType(SoundType.SE_Error, false);
                app.nakamiApp.transform.localScale = new Vector3(1, 1, 1);
                await UniTask.Delay(50);
            }
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(AppType.Jine);
            AudioManager.Instance.PlaySeByType(SoundType.BGM_zarazaranoise_lv3, false);
            await UniTask.Delay(7900);
            float weight = 1.0f;
            DOTween.To(() => weight, x => {
                PostEffectManager.Instance.SetShaderWeight(x);
            }, 0.03f, 2).SetEase(Ease.InSine).Play();
            await UniTask.Delay(2000);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE122.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE123.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE123.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_3();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_3(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE124.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE125.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE126.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE126.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_4();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_4(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE127.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE128.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE129.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE129.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_5();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_5(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE130.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE131.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE131.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_6();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_6(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE132.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE133.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE133.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_7();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_7(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE134.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE135.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_JINE136.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_JINE136.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_8();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_8(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE137.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE138.Swap());

            IWindow login = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp((AppType)(int)ModdedAppType.Login_Hacked);
            login.Touched();
            for (int i = 0; i < 13; i++)
            {
                string input = string.Concat("angelkawaii2".Take(i));
                await UniTask.Delay(60);
                loginComp._input.text = input;
                //await loginComp._input.ObserveEveryValueChanged((TMP_InputField t) => t.text, FrameCountType.Update, false);
            }
            await UniTask.Delay(1380);
            login.Touched();
            ExecuteEvents.Execute(loginComp._login.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            await UniTask.Delay(60);
            login.Touched();
            loginComp.loginActions.Enqueue(new Func<UniTask>(async () => { await attempt4(); }));
            ExecuteEvents.Execute(loginComp._login.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        }

        public async UniTask attempt4(CancellationToken cancellationToken = default(CancellationToken)) {
            PostEffectManager.Instance.ResetShader();
            AudioManager.Instance.PlaySeByType(SoundType.SE_command_execute);
            float startColor = 1.0f;
            DOTween.To(() => startColor, x => GameObject.Find("MainPanel").GetComponent<Image>().color = new Color(x, 0f, 0f, 1f), 0f, 2).SetEase(Ease.InSine).Play();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp((AppType)(int)ModdedAppType.Login_Hacked);
            await UniTask.Delay(2000);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE139.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE140.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE141.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE142.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_JINE143.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Poketter);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.BADPASSWORD_TWEET001.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.BADPASSWORD_TWEET002.Swap());
            BumpDayMax();
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.AngelDeath);
            SingletonMonoBehaviour<NotificationManager>.Instance.AddTimePassingNotifier(1);
            base.endEvent();
        }
    }
}
