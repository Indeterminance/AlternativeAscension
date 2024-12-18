﻿using Cysharp.Threading.Tasks;
using DG.Tweening;
using NeedyEnums;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Haishin_AngelFuneral : LiveScenario
    {
        bool dark;
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this._Live.isOiwai = true;
            this._Live.isUncontrollable = true;
            this._Live.SetSpeedLock(true);
            this._Live.Speed = 0;
            //this._Live._HaisinSkip.gameObject.SetActive(false);
            this._Live._HaisinSpeed.gameObject.SetActive(false);
            this._Live.CommentParent.gameObject.SetActive(false);

            string pre = ModdedAlphaType.FollowerAlpha.ToString()+"4_";
            this.title = NgoEx.TenTalk(pre+"STREAMNAME", this._lang);
            Action<string, string> tenTalk = new Action<string, string>((name, anim) => this.playing.Add(new Playing(true, NgoEx.TenTalk(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            
            
            tenTalk("FAULT001", "stream_ame_idle_anxiety_c");
            tenTalk("FAULT002", "");
            tenTalk("FAULT003", "stream_ame_idle_iraira_c");
            tenTalk("FAULT004", "");
            tenTalk("FAULT005", "");
            tenTalk("FAULT006", "");
            tenTalk("FAULT007", "stream_ame_craziness");
            tenTalk("FAULT008", "");
            tenTalk("FAULT009", "stream_ame_idle_normal_c");
            tenTalk("FAULT010", "");
            tenTalk("FAULT011", "");
            tenTalk("FAULT012", "stream_ame_idle_anxiety_c");
            tenTalk("FAULT013", "");
            tenTalk("FAULT014", "");
            tenTalk("FAULT015", "stream_ame_yanderu");
            tenTalk("FAULT016", "");
            tenTalk("FAULT017", "");
            tenTalk("FAULT018", "");
            tenTalk("FAULT019", "");
            tenTalk("FAULT020", "");
            tenTalk("FAULT021", "");
            tenTalk("FAULT022", "stream_ame_follower_end");
            tenTalk("FAULT023", "");
            
            playing.Add(new Playing(false, "", delta: 500, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));


        }

        // Token: 0x06000FD6 RID: 4054 RVA: 0x0004A57C File Offset: 0x0004877C
        public override async UniTask StartScenario()
        {
            SingletonMonoBehaviour<EventManager>.Instance.nowEnding = ModdedEndingType.Ending_Followers.Swap();
            //await SingletonMonoBehaviour<AltAscModManager>.Instance.SetViewersInactive();
            GameObject.Find("stack").SetActive(false);
            PostEffectManager.Instance.ResetShader();
            AudioManager.Instance.StopBgm();
            PostEffectManager.Instance.ResetShader();
            await base.StartScenario();
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_follower_day4_finale>();
        }
    }
}
