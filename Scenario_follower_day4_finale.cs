﻿using Cysharp.Threading.Tasks;
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
    public class Scenario_follower_day4_finale : NgoEvent
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
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isFollowerBG.Value = true;
            SingletonMonoBehaviour<EventManager>.Instance.nowEnding = (EndingType)(int)ModdedEndingType.Ending_Followers;
            IWindow poke = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.Poketter);
            poke.Uncloseable();
            poke.GameObjectTransform.position = new Vector2(-10, 5);
            await UniTask.Delay(1000);
            IWindow altPoke = SingletonMonoBehaviour<WindowManager>.Instance.NewWindow((AppType)(int)ModdedAppType.AltPoketter);
            poke.GameObjectTransform.position = new Vector2(10, -5);
            altPoke.Uncloseable();
            //altPoke.nakamiApp.transform.position = poke.nakamiApp.transform.position + new Vector3(10, 10);
            NeedyMintsMod.log.LogMessage($"old pos: {poke.nakamiApp.transform.position}");
            await UniTask.Delay(Constants.FAST);
            SingletonMonoBehaviour<PoketterManager>.Instance.isDeleted.Value = true;
            await UniTask.Delay(1000);
            SingletonMonoBehaviour<NeedyMintsModManager>.Instance.isAmeDelete.Value = true;
            await UniTask.Delay(Constants.FAST);
            SingletonMonoBehaviour<WindowManager>.Instance.NewWindow(AppType.TaskManager).Uncloseable();
            int followers = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower);
            DOTween.To(() => followers, x => {
                SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(StatusType.Follower, x);
            }, 0, 1).Play();
            await UniTask.Delay(1000);
            SingletonMonoBehaviour<NotificationManager>.Instance.osimai();
        }
    }
}