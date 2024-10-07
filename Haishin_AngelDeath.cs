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
    public class Haishin_AngelDeath : LiveScenario
    {
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this._Live.isOiwai = false;
            this._Live.isUncontrollable = true;
            this._Live.SetSpeedLock(true);
            this._Live.Speed = 1;
            this._Live._HaisinSkip.gameObject.SetActive(false);
            this._Live._HaisinSpeed.gameObject.SetActive(false);


            playing.Add(SuperPlaying(false, ModdedEffectType.Vengeful.ToString(), delta: 50, AdditionalTension: 13000, color: ModdedSuperchatType.EVENT_SHADER.Swap()));

            string pre = ModdedAlphaType.FollowerAlpha.ToString()+"3_";
            this.title = NgoEx.TenTalk(pre+"STREAMNAME", this._lang);
            Action<string, string> tenTalk = new Action<string, string>((name, anim) => this.playing.Add(new Playing(true, NgoEx.TenTalk(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            Action<string, string> kome = new Action<string, string>((name, anim) => this.playing.Add(new Playing(false, NgoEx.Kome(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            tenTalk("RECENTEVENTS001","stream_cho_reaction2");
            kome("RECENTEVENTS001", "");
            kome("RECENTEVENTS002", "");
            tenTalk("RECENTEVENTS002", "");
            kome("RECENTEVENTS003", "");
            kome("RECENTEVENTS004", "");
            kome("RECENTEVENTS005", "");
            tenTalk("RECENTEVENTS003", "stream_cho_pout");
            kome("RECENTEVENTS006", "");
            tenTalk("RECENTEVENTS004", "stream_cho_horror_glare");
            kome("RECENTEVENTS007", "");
            tenTalk("RECENTEVENTS005", "");
            kome("RECENTEVENTS008", "");
            kome("RECENTEVENTS009", "");
            kome("RECENTEVENTS010", "");
            tenTalk("RECENTEVENTS006", "stream_cho_horror_omae");
            kome("RECENTEVENTS011", "");
            kome("RECENTEVENTS012", "");
            kome("RECENTEVENTS013", "");
            tenTalk("RECENTEVENTS007", "");
            kome("RECENTEVENTS014", "");
            tenTalk("RECENTEVENTS008", "stream_cho_craziness3");
            kome("RECENTEVENTS015", "");
            kome("RECENTEVENTS016", "");
            tenTalk("RECENTEVENTS009", "stream_cho_horror_lower");
            kome("RECENTEVENTS017", "");
            kome("RECENTEVENTS018", "");
            tenTalk("RECENTEVENTS010", "stream_cho_teach");
            tenTalk("RECENTEVENTS011", "");
            kome("RECENTEVENTS019", "");
            tenTalk("RECENTEVENTS012", "stream_cho_horror_laugh");
            tenTalk("RECENTEVENTS013", "stream_cho_horror_smile");
            kome("RECENTEVENTS020", "");
            kome("RECENTEVENTS021", "");
            tenTalk("RECENTEVENTS014", "stream_cho_horror_anger");
            tenTalk("RECENTEVENTS015", "");
            kome("RECENTEVENTS022", "");
            tenTalk("RECENTEVENTS016", "stream_cho_horror_lower");
            kome("RECENTEVENTS023", "");
            kome("RECENTEVENTS024", "");
            tenTalk("RECENTEVENTS017", "");
            kome("RECENTEVENTS025", "");
            kome("RECENTEVENTS026", "");
            tenTalk("RECENTEVENTS018", "stream_cho_horror_glare");
            kome("RECENTEVENTS027", "");
            kome("RECENTEVENTS028", "");
            tenTalk("RECENTEVENTS019", "stream_cho_bad");
            kome("RECENTEVENTS029", "");
            kome("RECENTEVENTS030", "");
            tenTalk("RECENTEVENTS020", "stream_cho_horror_omae");
            kome("RECENTEVENTS031", "");
            kome("RECENTEVENTS032", "");
            kome("RECENTEVENTS033", "");
            tenTalk("RECENTEVENTS021", "stream_cho_horror_glare");
            tenTalk("RECENTEVENTS022", "");
            tenTalk("RECENTEVENTS023", "");
            kome("RECENTEVENTS034", "");
            kome("RECENTEVENTS035", "");
            kome("RECENTEVENTS036", "");


            int apologies = UnityEngine.Random.Range(20, 25);
            for (int i = 0; i < apologies; i++)
            {
                kome("RECENTEVENTS_APOLOGETIC00" + UnityEngine.Random.Range(1, 6),"");
            }


            kome("RECENTEVENTS037", "");
            kome("RECENTEVENTS038", "");
            kome("RECENTEVENTS039", "");
            tenTalk("RECENTEVENTS024", "stream_cho_pout");
            kome("RECENTEVENTS040", "");
            tenTalk("RECENTEVENTS025", "");
            kome("RECENTEVENTS041", "");
            kome("RECENTEVENTS042", "");
            tenTalk("RECENTEVENTS026", "");
            tenTalk("RECENTEVENTS027", "stream_cho_horror_lower");
            tenTalk("RECENTEVENTS028", "");
            tenTalk("RECENTEVENTS029", "stream_cho_craziness3");
            playing.Add(new Playing(false, "", delta: 1000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            playing.Add(new Playing(false, "", color: ModdedSuperchatType.EVENT_SHADERWAIT.Swap()));
            //End


            //this.playing.Add(new Playing("last"));
            //NeedyMintsMod.log.LogMessage((t - dt).TotalMilliseconds + " ms");
        }

        // Token: 0x06000FD6 RID: 4054 RVA: 0x0004A57C File Offset: 0x0004877C
        public override async UniTask StartScenario()
        {
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_mainloop_shuban, true);
            PostEffectManager.Instance.ResetShader();
            await base.StartScenario();
            this._Live.HaishinClean();
            SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_AfterAngelDeath>();
        }
    }
}
