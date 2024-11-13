using Cysharp.Threading.Tasks;
using NeedyEnums;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Haishin_Comiket : LiveScenario
    {
        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            this._Live.isOiwai = false;
            this.defaultEffectType = EffectType.GoCrazy;
            //NeedyMintsMod.log.LogMessage($"Playing starts at : {playing.Count}");

            string pre = ModdedAlphaType.FollowerAlpha.ToString()+"1_";
            this.title = NgoEx.TenTalk(pre+"STREAMNAME", this._lang);
            Action<string, string> tenTalk = new Action<string, string>((name, anim) => this.playing.Add(new Playing(true, NgoEx.TenTalk(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            Action<string, string> kome = new Action<string, string>((name, anim) => this.playing.Add(new Playing(false, NgoEx.Kome(pre + name, this._lang), StatusType.Tension, 1, 0, "", "", anim, true, SuperchatType.White, false, "")));
            playing.Add(new Playing("first"));
            kome("CHAT_INTRO001", "");
            kome("CHAT_INTRO002", "");
            kome("CHAT_INTRO003", ""); 
            tenTalk("STREAM001", "stream_cho_kashikoma");
            tenTalk("STREAM002", "");
            kome("CHAT001", "");
            kome("CHAT002", "");
            kome("CHAT003", "");
            tenTalk("STREAM003", "stream_cho_kawaiku_superchat");
            kome("CHAT004", "");
            kome("CHAT005", "");
            kome("CHAT006", "");
            kome("CHAT007", "");
            tenTalk("STREAM004", "stream_cho_su_smile");
            kome("CHAT008", "");
            tenTalk("STREAM005", "");
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + "CHAT009", this._lang), StatusType.Tension, 1, 0, NgoEx.TenTalk(pre + "RESPONSE001", this._lang), "stream_cho_kawaiku_fever", "", true, SuperchatType.White, false, ""));
            //kome("CHAT009", ""); // RESPONSE001
            tenTalk("STREAM006", "");
            kome("CHAT010", "");
            kome("CHAT011", "");
            kome("CHAT012", "");
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + "CHAT013", this._lang), StatusType.Tension, 1, 0, NgoEx.TenTalk(pre + "RESPONSE002", this._lang), "stream_cho_reaction2", "", true, SuperchatType.White, false, ""));
            //kome("CHAT013", ""); // R2
            tenTalk("STREAM007", "stream_cho_akaruku_superchat");
            tenTalk("STREAM008", ""); // "stream_cho_akaruku_superchat"
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + "CHAT014", this._lang), StatusType.Tension, 1, 0, NgoEx.TenTalk(pre + "RESPONSE003", this._lang), "stream_cho_pointing", "", true, SuperchatType.White, false, ""));
            //kome("CHAT014", ""); // R3
            tenTalk("STREAM009", "");
            tenTalk("STREAM010", "stream_cho_su_smile");
            kome("CHAT015", "");
            kome("CHAT016", "");
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + "CHAT017", this._lang), StatusType.Tension, 1, 0, NgoEx.TenTalk(pre + "RESPONSE004", this._lang), "stream_cho_kawaiku_superchat", "", true, SuperchatType.White, false, ""));
            //kome("CHAT017", ""); //R4
            tenTalk("STREAM011", "stream_cho_peace");
            kome("CHAT018", "");
            kome("CHAT019", "");
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + "CHAT020", this._lang), StatusType.Tension, 1, 0, NgoEx.TenTalk(pre + "RESPONSE005", this._lang), "stream_cho_kobiru", "", true, SuperchatType.White, false, ""));
            //kome("CHAT020", ""); //R5
            tenTalk("STREAM012", "");
            this.playing.Add(new Playing(true));
            tenTalk("STREAM013", "");


            this.playing.Add(new Playing("last"));
            DateTime t = DateTime.Now;
            //NeedyMintsMod.log.LogMessage((t - dt).TotalMilliseconds + " ms");
        }

        // Token: 0x06000FD6 RID: 4054 RVA: 0x0004A57C File Offset: 0x0004877C
        public override async UniTask StartScenario()
        {
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_mainloop_happyarranged, true);
            await base.StartScenario();
            this._Live.EndHaishin();
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusToNumber(ModdedStatusType.FollowerPlotFlag.Swap(), (int)FollowerPlotFlagValues.PostComiket);
        }
    }
}
