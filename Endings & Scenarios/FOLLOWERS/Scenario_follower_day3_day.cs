using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using UnityEngine;
using static AlternativeAscension.Alternates;
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
using UnityEngine.PlayerLoop;
using UnityEngine.AddressableAssets;

namespace AlternativeAscension
{
    public class Scenario_follower_day3_day : NgoEvent
    {
        private ScriptableLogin loginComp;
        public override void Awake()
        {
            base.Awake();
        }


        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            SingletonMonoBehaviour<AltAscModManager>.Instance.isFollowerBG.Value = true;
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<StatusManager>.Instance.timePassing(1);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.4f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            AudioManager.Instance.StopBgm();
            SingletonMonoBehaviour<WindowManager>.Instance.CleanAll();

            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine);
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_iraira_b");
            //await UniTask.Delay(Constants.MIDDLE, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE002.Swap());
            await SetupLogin();
            await NgoEvent.DelaySkippable(800);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE003.Swap());
            await NgoEvent.DelaySkippable(1000);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE005.Swap());
            string password = SingletonMonoBehaviour<AltAscModManager>.Instance.password;
            string baseMessage = JineDataConverter.GetJineTextFromTypeId(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE006.Swap());
            string message = baseMessage.Replace(@"{}", password);

            JineData d = new JineData()
            {
                day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex),
                id = JineType.None,
                responseType = ResponseType.Freeform,
                stampType = StampType.None,
                user = JineUserType.ame,
                freeMessage = message
            };
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(d);
            loginComp._input.interactable = true;
        }

        private async UniTask SetupLogin()
        {
            IWindow window = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow((AppType)(int)ModdedAppType.ScriptableLogin);
            window.Uncloseable();

            loginComp = window.nakamiApp.GetComponent<ScriptableLogin>();
            await UniTask.Delay(5, false, PlayerLoopTiming.Update, default, false);
            loginComp.isBadLogin.Value = true;
            Queue<Func<UniTask>> actionQueue = loginComp.loginActions;
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt1()));
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt2()));
            actionQueue.Enqueue(new Func<UniTask>(async () => await attempt3()));
        }

        public async UniTask attempt1(CancellationToken cancellationToken = default(CancellationToken))
        {
            loginComp.isInvalidLogin.Value = true;
            PostEffectManager.Instance.SetShader(EffectType.OD);
            PostEffectManager.Instance.SetShaderWeight(0.01f);
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_c");
            //await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE009.Swap());
            await UniTask.Delay(Constants.FAST);
        }

        public async UniTask attempt2(CancellationToken cancellationToken = default(CancellationToken))
        {
            PostEffectManager.Instance.SetShader(EffectType.OD);
            PostEffectManager.Instance.SetShaderWeight(0.03f);
            await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE010.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE011.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE012.Swap());
            
            await UniTask.Delay(Constants.FAST);
            IWindow login = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp((AppType)(int)ModdedAppType.ScriptableLogin);
            login.Touched();

            string pass = SingletonMonoBehaviour<AltAscModManager>.Instance.password;
            for (int i = 0; i < pass.Length+1; i++)
            {
                string input = string.Concat(pass.Take(i));
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
        }

        public async UniTask attempt3(CancellationToken cancellationToken = default(CancellationToken))
        {
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_yami, true);
            GameObject.Find("MainPanel").GetComponent<Image>().color = Color.red;
            PostEffectManager.Instance.SetShader(EffectType.Kakusei);
            PostEffectManager.Instance.SetShaderWeight(0.1f);
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_iraira_c");
            await UniTask.Delay(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE013.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE014.Swap());
            if (SingletonMonoBehaviour<AltAscModManager>.Instance.password == "angelkawaii2")
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE015.Swap());
            }
            else
            {
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_ALTLOGIN_JINE001.Swap());
                await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_ALTLOGIN_JINE002.Swap());

            }
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_craziness");
            //await UniTask.Delay(Constants.MIDDLE * 2, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE016.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE017.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE018.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE019.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE020.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE021.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE022.Swap());
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_craziness");
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE023.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE023.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
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
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_iraira_d");
            await UniTask.Delay(2000);
            SingletonMonoBehaviour<AltAscModManager>.Instance.LargeViewer.SetActive(true);
            Image viewColor = SingletonMonoBehaviour<AltAscModManager>.Instance.LargeViewer.GetComponent<Image>();
            viewColor.color = new Color(0, 0, 0, 0);
            float viewWeight = 0.0f;
            DOTween.To(() => viewWeight, delegate (float x)
            {
                viewColor.color = new Color(0, 0, 0, x);
            }, 0.06f, 10f).SetEase(Ease.InOutSine).Play();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Jine).Uncloseable();
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE024.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE025.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE025.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_3();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_3(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE026.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE027.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE028.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE028.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_4();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_4(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE029.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE030.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE031.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE031.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_5();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_5(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE032.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE033.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE033.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_6();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_6(CancellationToken cancellationToken = default(CancellationToken))
        {
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_anxiety_g");
            //await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE034.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE035.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE035.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_7();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_7(CancellationToken cancellationToken = default(CancellationToken))
        {
            //SingletonMonoBehaviour<WebCamManager>.Instance.PlayAnim("stream_ame_idle_iraira_g");
            //await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE036.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE037.Swap());
            SingletonMonoBehaviour<JineManager>.Instance.StartOption(new List<JineType>
            {
                ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE038.Swap()
            });
            SingletonMonoBehaviour<JineManager>.Instance.OnChangeHistory.Where((CollectionAddEvent<JineData> x) => x.Value.id == ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE038.Swap()).Subscribe(async delegate (CollectionAddEvent<JineData> _)
            {
                this.attempt3_8();
            }).AddTo(this.compositeDisposable);
        }

        public async UniTask attempt3_8(CancellationToken cancellationToken = default(CancellationToken))
        {
            await NgoEvent.DelaySkippable(Constants.FAST);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE039.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE040.Swap());

            // Queue the event ahead of time, just in case
            loginComp.loginActions.Enqueue(new Func<UniTask>(async () => { await attempt4(); }));

            IWindow login = SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp((AppType)(int)ModdedAppType.ScriptableLogin);
            login.Touched();
            string password = SingletonMonoBehaviour<AltAscModManager>.Instance.password;
            for (int i = 0; i < password.Length+1; i++)
            {
                string input = string.Concat(password.Take(i));
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
        }

        public async UniTask attempt4(CancellationToken cancellationToken = default(CancellationToken)) {
            PostEffectManager.Instance.ResetShader();
            AudioManager.Instance.PlaySeByType(SoundType.SE_command_execute);
            float startColor = 1.0f;
            DOTween.To(() => startColor, x => GameObject.Find("MainPanel").GetComponent<Image>().color = new Color(x, 0f, 0f, 1f), 0f, 2).SetEase(Ease.InSine).Play();
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Webcam);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp((AppType)(int)ModdedAppType.ScriptableLogin);
            SingletonMonoBehaviour<WebCamManager>.Instance.SetBaseAnim("stream_ame_idle_anxiety_d");
            await UniTask.Delay(2000);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE041.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE042.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE043.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE044.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.ENDING_FOLLOWER_DAY3_LOGIN_JINE045.Swap());
            await NgoEvent.DelaySkippable(Constants.MIDDLE);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            SingletonMonoBehaviour<WindowManager>.Instance.Uncloseable(AppType.Poketter);
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.BADPASSWORD_TWEET001.Swap(), new List<ModdedKusoRepType>
            {
                ModdedKusoRepType.BADPASSWORD_TWEET001_KUSO001,
                ModdedKusoRepType.BADPASSWORD_TWEET001_KUSO002,
                ModdedKusoRepType.BADPASSWORD_TWEET001_KUSO003,
                ModdedKusoRepType.BADPASSWORD_TWEET001_KUSO004,
            }.Swap());
            SingletonMonoBehaviour<PoketterManager>.Instance.AddQueueWithKusoreps(ModdedTweetType.BADPASSWORD_TWEET002.Swap());
            BumpDayMax();
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.AngelDeath);
            await UniTask.Delay(8000);
            await SingletonMonoBehaviour<StatusManager>.Instance.timePassing(1);
            base.endEvent();
        }
    }
}
