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

namespace AlternativeAscension
{
    public class Scenario_loop1_day0_night_multi : NgoEvent
    {
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
            login = SingletonMonoBehaviour<WindowManager>.Instance.GetNakamiFromApp(ModdedAppType.ScriptableLogin.Swap())?.GetComponent<ScriptableLogin>();

            if (login == null) return;

            List<EndingType> endings = SingletonMonoBehaviour<Settings>.Instance.mitaEnd;
            
            // 20% chance of activating invalid login if FOLLOWERS has been obtained...
            bool genericActivation = UnityEngine.Random.Range(0, 5) == 0 && endings.Contains(ModdedEndingType.Ending_Followers.Swap());

            // But if FOLLOWERS was the last ending obtained, guarantee an invalid login.
            bool lastFollowers = endings.Last() == ModdedEndingType.Ending_Followers.Swap() && endings.FindAll(e => e == ModdedEndingType.Ending_Followers.Swap()).Count() == 1;

            if (genericActivation || lastFollowers)
            {
                login.loginActions.Enqueue(new Func<UniTask>(async () =>
                {
                    SingletonMonoBehaviour<WindowManager>.Instance.GetWindowFromApp(ModdedAppType.ScriptableLogin.Swap()).Uncloseable();
                    await Invalidate();
                }));

                if (UnityEngine.Random.Range(0, 10) == 0)
                {
                    login.loginActions.Enqueue(new Func<UniTask>(async () =>
                    {
                        SceneManager.LoadScene("BiosToLoad");
                    }));
                }
            }
            login.loginActions.Enqueue(new Func<UniTask>(async () =>
            {
                SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_loop1_day0_night_multi_AfterLogin>();
                base.endEvent();
            }));
            login._input.interactable = true;
        }

        private async UniTask Invalidate()
        {
            login.isInvalidLogin.Value = true;
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN001.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN002.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN003.Swap());
            await SingletonMonoBehaviour<JineManager>.Instance.AddJineHistoryFromType(ModdedJineType.EVENT_ALTLOGIN004.Swap());
        }


    }
}
