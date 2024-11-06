using Cysharp.Threading.Tasks;
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
using static NeedyMintsOverdose.MintyOverdosePatches;

namespace NeedyMintsOverdose
{
    public class Haishin_Love : LiveScenario
    {
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this._Live.isOiwai = false;
            _Live.isUncontrollable = true;
            this._Live.isUncontrollable = true;
            //playing.Add(SuperPlaying(false, ModdedEffectType.Vengeful.ToString(); delta: 50, AdditionalTension: 13000, color: ModdedSuperchatType.EVENT_SHADER.Swap()));

            string pre = ModdedAlphaType.LoveAlpha.ToString()+"1_";
            this.title = NgoEx.TenTalk(pre+"STREAMNAME", this._lang);
            Action<string, string> tenTalk = new Action<string, string>((name, anim) => this.playing.Add(new Playing(true, NgoEx.TenTalk(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            Action<string, string> kome = new Action<string, string>((name, anim) => this.playing.Add(new Playing(false, NgoEx.Kome(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));

            tenTalk("STREAM001", "stream_cho_angel");
            this.playing.Add(new Playing("first"));
            kome("CHAT001", "");
            kome("CHAT002", "");
            kome("CHAT003", "");
            tenTalk("STREAM002","stream_cho_kashikoma");
            kome("CHAT004", "");
            kome("CHAT005", "");
            kome("CHAT006", "");
            kome("CHAT007", "");
            tenTalk("STREAM003", "stream_cho_reaction2");
            tenTalk("STREAM004", "stream_cho_pray");
            kome("CHAT008", "");
            kome("CHAT009", "");
            kome("CHAT010", "");
            tenTalk("STREAM005","stream_cho_teach");
            kome("CHAT011", "");
            tenTalk("STREAM006", "stream_cho_kashikoma");
            tenTalk("STREAM007", "stream_cho_teach2");
            kome("CHAT012", "");
            kome("CHAT013", "");
            kome("CHAT014", "");
            tenTalk("STREAM008", "");
            kome("CHAT015", "");
            tenTalk("STREAM009", "stream_cho_dokuzetsu_superchat");
            tenTalk("STREAM010", "stream_cho_h");
            kome("CHAT016", "");
            kome("CHAT017", "");
            kome("CHAT018", "");
            tenTalk("STREAM011","stream_cho_su_smile");
            kome("CHAT019", "");
            kome("CHAT020", "");
            tenTalk("STREAM012", "stream_cho_kawaiku");
            kome("CHAT021", "");
            kome("CHAT022", "");
            kome("CHAT023", "");
            kome("CHAT024", "");
            tenTalk("STREAM013", "stream_cho_shobon_smile");
            kome("CHAT025", "");
            kome("CHAT026", "");
            kome("CHAT027", "");
            tenTalk("STREAM014", "stream_cho_su_smile");
            kome("CHAT028", "");
            kome("CHAT029", "");
            kome("CHAT030", "");
            kome("CHAT031", "");
            playing.Add(new Playing("last"));
        }

        public override async UniTask StartScenario()
        {
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_emo, true);
            PostEffectManager.Instance.ResetShader();
            await base.StartScenario();
            this._Live.HaishinClean();
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_day1_night>();
        }
    }
}
