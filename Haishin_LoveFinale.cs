using Cysharp.Threading.Tasks;
using DG.Tweening;
using NeedyEnums;
using NGO;
using ngov3;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Haishin_LoveFinale : LiveScenario
    {
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this._Live.isOiwai = false;
            this._Live.isUncontrollable = true;
            //playing.Add(SuperPlaying(false, ModdedEffectType.Vengeful.ToString(); delta: 50, AdditionalTension: 13000, color: ModdedSuperchatType.EVENT_SHADER.Swap()));

            string pre = ModdedAlphaType.LoveAlpha.ToString()+"2_";
            this.title = NgoEx.TenTalk(pre+"STREAMNAME", this._lang);
            Action<string, string> tenTalk = new Action<string, string>((name, anim) => this.playing.Add(new Playing(true, NgoEx.TenTalk(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            Action<string, string> kome = new Action<string, string>((name, anim) => this.playing.Add(new Playing(false, NgoEx.Kome(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            tenTalk("STREAM001", "stream_cho_reaction1");
            this.playing.Add(new Playing("first"));
            kome("CHAT001", "");
            kome("CHAT002", "");
            kome("CHAT003", "");
            tenTalk("STREAM002", "");
            tenTalk("STREAM003", "stream_cho_sorrow_smile");
            kome("CHAT004", "");
            kome("CHAT005", "");
            kome("CHAT006", "");
            tenTalk("STREAM004", "stream_cho_reaction2");
            tenTalk("STREAM005", "stream_cho_su");
            kome("CHAT007", "");
            kome("CHAT008", "");
            tenTalk("STREAM006", "");
            kome("CHAT009", "");
            kome("CHAT010", "");
            kome("CHAT011", "");
            kome("CHAT012", "");
            kome("CHAT013", "");
            tenTalk("STREAM007", "stream_cho_akaruku_superchat");
            tenTalk("STREAM008", "stream_cho_shobon");
            kome("CHAT014", "");
            kome("CHAT015", "");
            kome("CHAT016", "");
            kome("CHAT017", "");
            tenTalk("STREAM009", "");
            tenTalk("STREAM010", "stream_cho_teach2");
            kome("CHAT018", "");
            kome("CHAT019", "");
            kome("CHAT020", "");
            tenTalk("STREAM011", "stream_cho_reaction2");
            kome("CHAT021", "");
            kome("CHAT022", "");
            tenTalk("STREAM012", "stream_cho_su_smile");
            tenTalk("STREAM013", "stream_cho_teach2");
            kome("CHAT023", "");
            tenTalk("STREAM014", "stream_cho_shobon_smile");
            kome("CHAT024", "");
            kome("CHAT025", "");
            tenTalk("STREAM015", "");
            tenTalk("STREAM016", "stream_cho_teach");
            kome("CHAT026", "");
            kome("CHAT027", "");
            tenTalk("STREAM017", "");
            kome("CHAT028", "");
            kome("CHAT029", "");
            kome("CHAT030", "");
            kome("CHAT031", "");
            kome("CHAT032", "");
            tenTalk("STREAM018", "stream_cho_su_smile");
            tenTalk("STREAM019", "");
            kome("CHAT033", "");
            kome("CHAT034", "");
            kome("CHAT035", "");
            kome("CHAT036", "");
            kome("CHAT037", "");
            kome("CHAT038", "");
            kome("CHAT039", "");
            tenTalk("STREAM020", "stream_cho_angel");
            tenTalk("STREAM021", "");
            tenTalk("STREAM022", "stream_cho_shobon_smile");
            kome("CHAT040", "");
            kome("CHAT041", "");
            kome("CHAT042", "");
            kome("CHAT043", "");
            tenTalk("STREAM023", "stream_cho_reaction1");
            kome("CHAT044", "");
            kome("CHAT045", "");
            tenTalk("STREAM024", "stream_cho_hitotabi2");
            kome("CHAT046", "");
            tenTalk("STREAM025", "");
            tenTalk("STREAM026", "stream_cho_su_superchat");
            kome("CHAT047", "");
            kome("CHAT048", "");
            kome("CHAT049", "");
            kome("CHAT050", "");
            kome("CHAT051", "");
            kome("CHAT052", "");
            kome("CHAT053", "");


            playing.Add(new Playing("last"));
        }

        public override async UniTask StartScenario()
        {
            SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay = -1;
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_event_emo, true);
            PostEffectManager.Instance.ResetShader();
            await base.StartScenario();
            this._Live.HaishinClean();

            bool isCompleted = false;
            if (SteamManager.Initialized)
            {
                SteamUserStats.GetAchievement("Ending_Completed", out isCompleted);
            }


            if (isCompleted)
            {
                SingletonMonoBehaviour<EventManager>.Instance.AddEvent<Scenario_love_finale_Afterhaishin>();
            }
            else
            {
                SingletonMonoBehaviour<EventManager>.Instance.nowEnding = ModdedEndingType.Ending_Love.Swap();
                SingletonMonoBehaviour<NotificationManager>.Instance.osimai();
            }
        }
    }
}
