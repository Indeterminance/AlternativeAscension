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

namespace AlternativeAscension
{
    public class Scenario_loop1_day0_night_multi_AfterLogin : NgoEvent
    {

        public override void Awake()
        {
            base.Awake();
        }

        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<WindowManager>.Instance.CloseApp(ModdedAppType.ScriptableLogin.Swap());
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_loop1_day0_night_AfterLogin>();
            base.endEvent();
        }
    }
}
