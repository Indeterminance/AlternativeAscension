using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NeedyEnums;
using NeedyMintsOverdose;
using NGO;
using UniRx;
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace ngov3
{
    // Token: 0x020005FF RID: 1535
    public class Scenario_love_finale_beforeHaishin : NgoEvent
    {
        // Token: 0x06001CEC RID: 7404 RVA: 0x000843F9 File Offset: 0x000825F9
        public override void Awake()
        {
            base.Awake();
        }

        // Token: 0x06001CED RID: 7405 RVA: 0x000BC11C File Offset: 0x000BA31C
        public override async UniTask startEvent(CancellationToken cancellationToken = default(CancellationToken))
        {
            base.startEvent(cancellationToken);
            SingletonMonoBehaviour<EventManager>.Instance.SetShortcutState(false, 0f);
            SingletonMonoBehaviour<TaskbarManager>.Instance.SetTaskbarInteractive(false);
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Action_HaishinStart>();
            base.endEvent();
        }
    }
}