using Cysharp.Threading.Tasks;
using ngov3;
using System.Threading;
using NeedyEnums;
using static AlternativeAscension.AAPatches;
using NGO;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace AlternativeAscension
{
    public class Scenario_loop1_day0_night_multi : NgoEvent
    {
        bool DEBUG = false;
        bool changedPass = false;
        ScriptableLogin login;
        bool finishEvent = false;

        public override void Awake()
        {
            base.Awake();
        }

        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            await UniTask.Delay(2700, false, PlayerLoopTiming.Update, default(CancellationToken), false);
            SingletonMonoBehaviour<StatusManager>.Instance.Moved.Value = true;
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0.2f);
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(JineType.Day0_JINE001);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(JineType.Event_Password_JINE001);
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().alpha = 1f;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().blocksRaycasts = true;
            (from v in SingletonMonoBehaviour<NotificationManager>.Instance.ObserveEveryValueChanged((NotificationManager c) => SingletonMonoBehaviour<NotificationManager>.Instance._notiferParent.childCount, FrameCountType.Update, false)
             where v == 0
             select v).Take(1).Subscribe(delegate (int _)
             {
                 SingletonMonoBehaviour<TooltipManager>.Instance.ShowTutorial(TooltipType.tutorial_first, "");
             }).AddTo(this.compositeDisposable);

            SingletonMonoBehaviour<WindowManager>.Instance.ObserveEveryValueChanged((WindowManager wm) => wm.WindowList, FrameCountType.Update, false).Subscribe(delegate (List<IWindow> windows)
            {
                HookLoginWindow();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask HookLoginWindow()
        {
            //AltAscMod.log.LogMessage($"Hooking!");
            login = SingletonMonoBehaviour<WindowManager>.Instance.GetNakamiFromApp(ModdedAppType.ScriptableLogin.Swap())?.GetComponent<ScriptableLogin>();

            if (login == null) return;
            //AltAscMod.log.LogMessage($"Hooked!");

            List<EndingType> endings = SingletonMonoBehaviour<Settings>.Instance.mitaEnd;

            // 20% chance of activating invalid login if FOLLOWERS has been obtained...
            bool genericActivation = UnityEngine.Random.Range(0, 5) == 0 && endings.Contains(ModdedEndingType.Ending_Followers.Swap());

            // But if FOLLOWERS was the last ending obtained, guarantee an invalid login.
            bool lastFollowers;
            if (endings.Count > 0) lastFollowers = endings.Last() == ModdedEndingType.Ending_Followers.Swap() && endings.FindAll(e => e == ModdedEndingType.Ending_Followers.Swap()).Count() == 1;
            else lastFollowers = false;

            if ((genericActivation || lastFollowers || DEBUG) && !changedPass)
            {
                login.loginActions.Enqueue(new Func<UniTask>(async () =>
                {
                    SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(ModdedAppType.ScriptableLogin.Swap()).Uncloseable();
                    await Invalidate();
                }));
                login.loginActions.Enqueue(new Func<UniTask>(async () =>
                {
                    SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(ModdedAppType.ScriptableLogin.Swap());
                    await ChangePass();
                }));


                if (UnityEngine.Random.Range(0, 10) == 0)
                {
                    login.loginActions.Enqueue(new Func<UniTask>(async () =>
                    {
                        SceneManager.LoadScene("BiosToLoad");
                    }));
                }
            }
            else
            {
                login.loginActions.Enqueue(new Func<UniTask>(async () =>
                {
                    SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_loop1_day0_night_multi_AfterLogin>();
                    base.endEvent();
                }));
            }
            login._input.interactable = true;
        }

        private async UniTask Invalidate()
        {
            login.isInvalidLogin.Value = true;
            login._login.interactable = false;
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE004.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE005.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE006.Swap());

            login._login.interactable = true;
        }

        private async UniTask ChangePass()
        {
            await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            IWindow changePassWin = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(ModdedAppType.ChangePassword.Swap());
            changePassWin.Uncloseable();
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().alpha = 0.4f;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().interactable = false;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().blocksRaycasts = true;
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE007.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE008.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE009.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE010.Swap());

            SingletonMonoBehaviour<AltAscModManager>.Instance.ObserveEveryValueChanged((AltAscModManager a) => a.password, FrameCountType.Update, false).Subscribe(async delegate (string pass)
            {
                if (pass == "angelkawaii2") return;
                await ConfirmedPass();
            }).AddTo(this.compositeDisposable);
        }

        private async UniTask ConfirmedPass()
        {
            //await UniTask.Delay(Constants.FAST, false, PlayerLoopTiming.Update, default, false);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE011.Swap());
            string password = SingletonMonoBehaviour<AltAscModManager>.Instance.password;
            string baseMessage = JineDataConverter.GetJineTextFromTypeId(ModdedJineType.EVENT_ALTLOGIN_JINE012.Swap());
            //AltAscMod.log.LogMessage($"Original message: {baseMessage}");
            string message = baseMessage.Replace(@"{}",$"\"{password}\"");


            JineData d = new JineData()
            {
                day = 1,
                id = JineType.None,
                responseType = ResponseType.Freeform,
                stampType = StampType.None,
                user = JineUserType.ame,
                freeMessage = message
            };
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistory(d);
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN_JINE013.Swap());
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().alpha = 1f;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().interactable = true;
            GameObject.Find("LoginShortCut").GetComponent<CanvasGroup>().blocksRaycasts = true;
            changedPass = true;
        }
    }
}
