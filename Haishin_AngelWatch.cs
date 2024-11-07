using Cysharp.Threading.Tasks;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using NeedyEnums;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AlternativeAscension.AAPatches;

namespace AlternativeAscension
{
    public class Haishin_AngelWatch : LiveScenario
    {
        bool SKIPINTRO = false;
        bool SKIPAMA = false;
        bool SKIPGAME = false;
        bool SKIPOVERDOSE = false;
        bool SKIPNIGHTMARE = false;
        internal static string pre = ModdedAlphaType.FollowerAlpha.ToString() + "2_";

        private void tenTalk(string dlgName, string anim = "", bool loopAnim = false)
        {
            playing.Add(new Playing(true, NgoEx.TenTalk(pre + dlgName, this._lang), StatusType.Tension, 1, 0, "", "", anim, loopAnim, SuperchatType.White, false, ""));
        }

        private void tenNoTalk(string anim, bool loopAnim = false)
        {
            playing.Add(new Playing(true, "", StatusType.Tension, 1, 0, "", "", anim, loopAnim, SuperchatType.White, false, ""));
        }

        private void kome(string komeName)
        {
            playing.Add(new Playing(false, NgoEx.Kome(pre + komeName, this._lang), StatusType.Tension, 1, 0, "", "", "", true, SuperchatType.White, false, ""));
        }

        private void sendJINE(ModdedJineType jine, string anim = "stream_cho_jine", bool isLoopAnim = true, bool idleAfter = true)
        {
            playing.Add(SuperPlaying(false, jine.ToString(), StatusType.Tension, 1, 0, "", "", anim, isLoopAnim, ModdedSuperchatType.JINE_SEND.Swap(), false, ""));
            if (idleAfter)
            {
                playing.Add(new Playing(false, "", delta: 400, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
                playing.Add(SuperPlaying(false, "", animation: "stream_cho_jine_idle", isLoopAnim: true, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            }
        }

        private void sendChoiceJINE(ModdedJineType jine)
        {
            playing.Add(new Playing(false, jine.ToString(), StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_CHOICE.Swap(), false, ""));
        }

        private void verifyJINE(ModdedJineType jine)
        {
            playing.Add(new Playing(false, jine.ToString(), StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_WAIT.Swap(), false, ""));
        }

        // Token: 0x06000FD5 RID: 4053 RVA: 0x00049280 File Offset: 0x00047480
        public override void Awake()
        {
            base.Awake();
            PostEffectManager.Instance.ResetShader();
            _Live.isOiwai = true;
            _Live._HaisinSkip.gameObject.SetActive(false);
            defaultEffectType = EffectType.GoCrazy;
            title = NgoEx.TenTalk(pre+"STREAMNAME", _lang);

            // Stream intro is basic
            if (!SKIPINTRO) StreamIntro();

            // AMA is an AMA, they ask questions, you answer
            if (!SKIPAMA) AMASequence();


            // Timeskip for AMA
            playing.Add(new Playing(false, "", delta: 1, color: ModdedSuperchatType.EVENT_TIMEPASS.Swap()));
            playing.Add(new Playing(false, "", delta: 2000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));

            // Playing the best horror game ever
            if (!SKIPGAME) Isolation();

            // Stream break section (Isolation goes until night)
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_TIMEPASS.Swap(), false, ""));
            playing.Add(new Playing(true, "", StatusType.Tension, 1, 0, "", "", "stream_cho_chance_blackout", false, SuperchatType.White, false, ""));
            playing.Add(new Playing(SoundType.SE_endHaishin, false));
            playing.Add(new Playing(true, "", StatusType.Tension, 1, 0, "", "", "stream_cho_break", false, SuperchatType.White, false, ""));

            // Why overdose on the internet when you can overdose on followers?
            if (!SKIPOVERDOSE) Overdose();

            // Overdosing takes her to the next day
            this.playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_TIMEPASS.Swap(), false, ""));


            // This is the real nightmare, though :P
            for (int i = 0; i < 40; i++)
            {
                playing.Add(new Playing(false, false, " "));
                //playing.Add(new Playing(true, "", delta: 10, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
                if (i % 40 == 0)
                {
                    playing.Add(new Playing(SoundType.SE_chime_horror, false));
                }
            }
            playing.Add(new Playing(false, "RED_EYES", color: ModdedSuperchatType.EVENT_MAINPANELCOLOR.Swap()));

            // Pain
            if (!SKIPNIGHTMARE) Nightmare();
        }

        private void StreamIntro()
        {
            tenTalk("INTRO001", "stream_cho_sorrow_smile", true);
            kome("INTRO001");
            kome("INTRO002");
            kome("INTRO003");
            tenTalk("INTRO002");
            tenTalk("INTRO003", "stream_cho_su_smile", true);
            kome("INTRO004");
            kome("INTRO005");
            kome("INTRO006");
            kome("INTRO007");
            kome("INTRO008");
            tenTalk("INTRO004");
            kome("INTRO009");
            kome("INTRO010");
            tenTalk("INTRO005", "stream_cho_kobiru_win_stop", false);
            kome("INTRO011");
            kome("INTRO012");
            kome("INTRO013");
            kome("INTRO014");
            kome("INTRO015");
            tenTalk("INTRO006", "stream_cho_kawaiku_superchat", false);
        }

        private void AMASequence()
        {
            playing.Add(new Playing(true, "", delta: 1500, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            playing.Add(new Playing(SoundType.SE_kinkira, false));
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.AMA_START.Swap(), false, ""));
            playing.Add(new Playing(true, "", delta: 3000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            playing.Add(SuperPlaying(false, SoundType.BGM_mainloop_gedarranged.ToString(), isLoopAnim: true, color: ModdedSuperchatType.EVENT_MUSICCHANGE.Swap()));
            playing.Add(SuperPlaying(true, "", animation: "stream_cho_su_smile", isLoopAnim: false));

            // Generate AMA questions
            List<string[]> AMAS = new List<string[]>()
                {
                new string[]{ "AMA_Q001", "stream_cho_teach",                                           "RESPONSE" },
                new string[]{ "AMA_Q002", "stream_cho_reaction1",                                       "RESPONSE" },
                new string[]{ "AMA_Q003", "stream_cho_kashikoma",                                       "RESPONSE" },
                new string[]{ "AMA_Q004", "stream_cho_shobon",                                          "RESPONSE" },
                new string[]{ "AMA_Q005", "stream_cho_nyo___stream_cho_shobon_smile",                   "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q006", "stream_cho_reaction2",                                       "RESPONSE" },
                new string[]{ "AMA_Q007", "stream_cho_angel___stream_cho_kawaiku",                      "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q008", "stream_cho_dokuzetsu_superchat_stream_cho_kobiru_superchat", "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q009", "stream_cho_teach2",                                          "RESPONSE" },
                new string[]{ "AMA_Q010", "stream_cho_crying_angry___stream_cho_shobon",                "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q011", "stream_cho_kawaiku_fever",                                   "RESPONSE" },
                new string[]{ "AMA_Q012", "stream_cho_dokuzetsu_superchat",                             "RESPONSE" },
                new string[]{ "AMA_Q013", "stream_cho_kobiru_fever___stream_cho_kobiru_superchat",      "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q014", "stream_cho_dokuzetsu",                                       "RESPONSE" },
                new string[]{ "AMA_Q015", "stream_cho_su___stream_cho_teach2",                          "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q016", "stream_cho_eeto___stream_cho_kobiru",                        "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q017", "stream_cho_pointing",                                        "RESPONSE" },
                new string[]{ "AMA_Q018", "stream_cho_kobiru",                                          "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q019", "stream_cho_akaruku_superchat",                               "RESPONSE" },
                new string[]{ "AMA_Q020", "stream_cho_reaction1___stream_cho_teach",                    "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q021", "stream_cho_dokuzetsu_win_stop",                              "RESPONSE" },
                new string[]{ "AMA_Q022", "stream_cho_kobiru_win_stop",                                 "RESPONSE" },
                new string[]{ "AMA_Q023", "stream_cho_teach2___stream_cho_kobiru_superchat",            "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q024", "stream_cho_reaction1___stream_cho_teach2",                   "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q025", "stream_cho_eeto",                                            "RESPONSE" },
                new string[]{ "AMA_Q026", "stream_cho_eeto___stream_cho_kawaiku",                       "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q027", "stream_cho_pout___stream_cho_anken_business10",              "RESPONSE", "RESPONSE_B" },
                new string[]{ "AMA_Q028", "stream_cho_dokuzetsu",                                       "RESPONSE" },
                new string[]{ "AMA_Q029", "stream_cho_h",                                               "RESPONSE" },
                new string[]{ "AMA_Q030", "stream_cho_angel",                                           "RESPONSE" },
                new string[]{ "AMA_Q031", "stream_cho_reaction2",                                       "RESPONSE" },
                };

            while (AMAS.Count > 31 - SingletonMonoBehaviour<AltAscModManager>.Instance.QUESTIONS)
            {
                int index = UnityEngine.Random.Range(0, AMAS.Count);
                string[] ama = AMAS[index];
                GetAMALine(ama[0], ama[1], ama.Skip(2).ToArray());
                AMAS.RemoveAt(index);
            }

            // Move to night
            this.playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.AMA_END.Swap(), false, ""));
        }

        private void Isolation()
        {
            // TODO: Work on expressions
            playing.Add(SuperPlaying(false, SoundType.BGM_mainloop_shuban.ToString(), isLoopAnim: true, color: ModdedSuperchatType.EVENT_MUSICCHANGE.Swap()));
            tenTalk("GAMING001", "stream_cho_kawaiku", true);
            kome("GAMING001");
            tenTalk("GAMING002");
            kome("GAMING002");
            kome("GAMING003");
            tenTalk("GAMING003");
            tenTalk("GAMING004", "stream_cho_reaction1", true);
            kome("GAMING004");
            tenTalk("GAMING005");
            kome("GAMING005");
            tenTalk("GAMING006");
            kome("GAMING006");
            kome("GAMING007");
            tenTalk("GAMING007", "stream_cho_su_smile", true);
            kome("GAMING008");
            tenTalk("GAMING008", "stream_cho_game_isolation2", true);
            kome("GAMING009");
            kome("GAMING010");
            kome("GAMING011");
            tenTalk("GAMING009", "stream_cho_game_isolation1", true);
            kome("GAMING012");
            kome("GAMING013");
            tenTalk("GAMING010", "stream_cho_game_isolation3", true);
            kome("GAMING014");
            kome("GAMING015");
            kome("GAMING016");
            kome("GAMING017");
            kome("GAMING018");
            tenTalk("GAMING011", "stream_cho_game_isolation1", true);
            kome("GAMING019");
            tenTalk("GAMING012");
            kome("GAMING020");
            kome("GAMING021");
            kome("GAMING022");
            tenTalk("GAMING013", "stream_cho_game_isolation4", false);
            kome("GAMING023");
            kome("GAMING024");
            kome("GAMING025");
            tenTalk("GAMING014", "stream_cho_game_isolation5", true);
            kome("GAMING026");
            kome("GAMING027");
            kome("GAMING028");
            tenTalk("GAMING015", "stream_cho_game_isolation1", true);
            kome("GAMING029");
            kome("GAMING030");
            tenTalk("GAMING016", "stream_cho_unrest", true);
            tenTalk("GAMING017", "stream_cho_su_fever", true);
        }

        private void Overdose()
        {
            // TODO: Work on filters and maybe timing
            // Animations to work on:
            //     - Take pill
            //     - Multiple texting animations
            //     - Fall asleep frames
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_INIT.Swap(), false, ""));
            playing.Add(new Playing(false, "", delta: 2000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE001, anim: "", idleAfter: false);
            kome("OVERDOSE001");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE002, anim: "", idleAfter: false);
            kome("OVERDOSE002");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE003, anim: "", idleAfter: false);
            kome("OVERDOSE003");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE004, anim: "", idleAfter: false);
            kome("OVERDOSE004");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE005, anim: "", idleAfter: false);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE006, anim: "", idleAfter: false);
            kome("OVERDOSE005");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE007, anim: "", idleAfter: false);
            kome("OVERDOSE006");
            sendChoiceJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE008); //choice
            verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE008);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE009, anim: "", idleAfter: false);
            kome("OVERDOSE007");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE010, anim: "", idleAfter: false);
            kome("OVERDOSE008");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE011, anim: "", idleAfter: false);
            kome("OVERDOSE009");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE012, anim: "", idleAfter: false);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE013, anim: "", idleAfter: false);
            kome("OVERDOSE010");
            kome("OVERDOSE011");
            sendChoiceJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE014); // choice
            kome("OVERDOSE012");
            verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE014);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE015, anim: "", idleAfter: false);
            kome("OVERDOSE013");
            sendChoiceJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE016); // choice!!!
            verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE016);
            kome("OVERDOSE014");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE017, anim: "", idleAfter: false);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE018, anim: "", idleAfter: false);
            kome("OVERDOSE015");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE019, anim: "", idleAfter: false);
            kome("OVERDOSE016");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE020, anim: "", idleAfter: false);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE021, anim: "", idleAfter: false);
            kome("OVERDOSE017");
            tenNoTalk("stream_cho_chance_blackout_back", true);
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_DESTROY.Swap(), false, ""));
            tenTalk("OVERDOSE001", "stream_cho_su_fever", true);
            kome("OVERDOSE018");
            kome("OVERDOSE019");
            tenTalk("OVERDOSE002");
            tenTalk("OVERDOSE003", "stream_cho_kakkoyoku_superchat", false);

            // Overdose!
            for (int i = 0; i < 12; i++) playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_CREATEDEPAZ.Swap(), false, ""));
            playing.Add(new Playing(false, ModdedEffectType.Hazy.ToString(), delta: 100, AdditionalTension: 500, isLoopAnim: true, color: ModdedSuperchatType.EVENT_SHADER.Swap()));
            for (int i = 0; i < 12; i++) playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.EVENT_DOSE.Swap(), false, ""));

            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_INIT.Swap(), false, ""));
            //playing.Add(new Playing(false, "OD3", delta: 80, AdditionalTension: 200, color: ModdedSuperchatType.EVENT_SHADER.Swap()));
            playing.Add(new Playing(false, "", delta: 5000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE022, anim: "", idleAfter: false);
            kome("OVERDOSE020");
            kome("OVERDOSE021");
            kome("OVERDOSE022");
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_DESTROY.Swap(), false, ""));
            playing.Add(new Playing(false, "", delta: 1000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            tenTalk("OVERDOSE004", "stream_cho_teach2");
            kome("OVERDOSE023");
            tenTalk("OVERDOSE005");
            kome("OVERDOSE024");
            tenTalk("OVERDOSE006", "stream_cho_su_smile", false);
            tenTalk("OVERDOSE007");
            tenTalk("OVERDOSE008", "stream_cho_shobon_smile", false);
            kome("OVERDOSE025");
            kome("OVERDOSE026");
            kome("OVERDOSE027");
            tenTalk("OVERDOSE009", "stream_cho_watch_sleepy", false);
            tenTalk("OVERDOSE010");
            kome("OVERDOSE028");
            tenTalk("OVERDOSE011", "stream_cho_watch_sleepy2", false);
            playing.Add(new Playing(false, "", delta: 8000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            tenTalk("OVERDOSE012");
            //playing.Add(SuperPlaying(false, "OD3", delta: 0, AdditionalTension: 1000, henji: "0.8f", color: ModdedSuperchatType.EVENT_SHADER.Swap()));
            playing.Add(new Playing(false, "", delta: 1000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));
            kome("OVERDOSE029");
            kome("OVERDOSE030");
            kome("OVERDOSE031");
            kome("OVERDOSE032");
            kome("OVERDOSE033");
            kome("OVERDOSE034");
            kome("OVERDOSE035");
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_INIT.Swap(), false, ""));
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE023, anim: "", idleAfter: false);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE024, anim: "", idleAfter: false);
            playing.Add(new Playing(SoundType.SE_chime, false));
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_DESTROY.Swap(), false, ""));
            //playing.Add(SuperPlaying(false, "", color: ModdedSuperchatType.EVENT_SHADERWAIT.Swap()));
            playing.Add(SuperPlaying(false, "Anmaku", delta: 90, AdditionalTension: 250, color: ModdedSuperchatType.EVENT_SHADER.Swap()));
            kome("OVERDOSE036");
            kome("OVERDOSE037");
            playing.Add(new Playing(SoundType.SE_chime, false));
            kome("OVERDOSE038");
            kome("OVERDOSE039");
            playing.Add(new Playing(SoundType.SE_chime, false));
            kome("OVERDOSE040");
            kome("OVERDOSE041");
            kome("OVERDOSE042");
            playing.Add(new Playing(SoundType.SE_chime, false));
            kome("OVERDOSE043");
            kome("OVERDOSE044");
            playing.Add(new Playing(SoundType.SE_chime, false));
            playing.Add(SuperPlaying(false, SoundType.BGM_wind.ToString(), isLoopAnim: true, color: ModdedSuperchatType.EVENT_MUSICCHANGE.Swap()));
            playing.Add(SuperPlaying(false, "", color: ModdedSuperchatType.EVENT_SHADERWAIT.Swap()));
            playing.Add(new Playing(false, "", delta: 1000, color: ModdedSuperchatType.EVENT_DELAYFRAME.Swap()));

        }

        private void Nightmare()
        {
            // Add expressions, work on filters, add daypass
            // Expressions:
            //    - Wake up in horror
            //    - Sleepy text
            //    - Horrified text expression
            //playing.Add(new Playing(SoundType.SE_chime_horror, false));
            kome("NIGHTMARE001");
            kome("NIGHTMARE002");
            kome("NIGHTMARE003");
            kome("NIGHTMARE004");
            playing.Add(new Playing(SoundType.SE_chime_horror, false));
            kome("NIGHTMARE005");
            kome("NIGHTMARE006");
            kome("NIGHTMARE007");
            kome("NIGHTMARE008");
            //playing.Add(new Playing(SoundType.SE_chime_horror, false));
            kome("NIGHTMARE009");
            kome("NIGHTMARE010");
            kome("NIGHTMARE011");
            playing.Add(new Playing(SoundType.SE_chime_horror, false));
            tenTalk("NIGHTMARE001", "stream_cho_craziness1", false);
            kome("NIGHTMARE012");
            kome("NIGHTMARE013");
            kome("NIGHTMARE014");
            kome("NIGHTMARE015");
            kome("NIGHTMARE016");
            tenTalk("NIGHTMARE002", "stream_cho_craziness3", false);
            kome("NIGHTMARE017");
            kome("NIGHTMARE018");
            tenTalk("NIGHTMARE003");
            kome("NIGHTMARE019");
            kome("NIGHTMARE020");
            kome("NIGHTMARE021");
            kome("NIGHTMARE022");
            tenTalk("NIGHTMARE004", "stream_cho_craziness2", false);
            kome("NIGHTMARE023");
            kome("NIGHTMARE024");
            kome("NIGHTMARE025");
            kome("NIGHTMARE026");
            tenTalk("NIGHTMARE005", "stream_cho_craziness1", false);
            tenTalk("NIGHTMARE006", "stream_cho_jine_idle", false);
            kome("NIGHTMARE027");
            kome("NIGHTMARE028"); // Break point 1
            kome("NIGHTMARE029");
            kome("NIGHTMARE030");
            kome("NIGHTMARE031");
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_INIT.Swap(), false, ""));
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE025);
            kome("NIGHTMARE032");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE026);
            kome("NIGHTMARE033");
            kome("NIGHTMARE034");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE027);
            kome("NIGHTMARE035");
            kome("NIGHTMARE036");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE028);
            kome("NIGHTMARE037");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE029);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE030);
            sendChoiceJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE031);
            //kome("NIGHTMARE037");
            verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE031);
            tenTalk("NIGHTMARE007");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE032); // Break point 2
            kome("NIGHTMARE038");
            kome("NIGHTMARE039");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE033);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE034);
            kome("NIGHTMARE040");
            kome("NIGHTMARE041");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE035, "", false, false);
            //verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE053);
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE036);
            tenTalk("NIGHTMARE008");
            kome("NIGHTMARE042");
            kome("NIGHTMARE043");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE037);
            playing.Add(new Playing(false, "", isLoopAnim: true, color: ModdedSuperchatType.EVENT_MUSICCHANGE.Swap()));
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE038);
            kome("NIGHTMARE044");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE039);
            kome("NIGHTMARE045");
            sendJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE040);
            kome("NIGHTMARE046"); // Break point 3
            kome("NIGHTMARE047");
            sendChoiceJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE041);
            verifyJINE(ModdedJineType.ENDING_FOLLOWER_ANGELWATCH_JINE041);
            playing.Add(new Playing(false, EffectType.GoCrazy.ToString(), delta: 30, AdditionalTension: 0, isLoopAnim: false, color: ModdedSuperchatType.EVENT_SHADER.Swap()));
            playing.Add(new Playing(false, "", StatusType.Tension, 1, 0, "", "", "", false, ModdedSuperchatType.JINE_DESTROY.Swap(), false, ""));
            tenTalk("NIGHTMARE009");
            tenTalk("NIGHTMARE010", "stream_cho_craziness1");
            kome("NIGHTMARE048");
            tenTalk("NIGHTMARE011", "stream_cho_craziness2");
        }

        private void GetAMALine(string question, string anim, string[] responses)
        {
            string pre = ModdedAlphaType.FollowerAlpha.ToString() + "2_";

            string[] anims = anim.Split(new string[]{"___"},StringSplitOptions.RemoveEmptyEntries);
            if (anims.Length != responses.Length) return;
            string response = "";
            foreach (string partResponse in responses)
            {
                //NeedyMintsMod.log.LogMessage($"{question} has {partResponse}!");
                response += NgoEx.TenTalk(pre+question+"_"+partResponse, this._lang)+"___";
            }

            bool loopAnim = question != "AMA_Q003";
            this.playing.Add(new Playing(false, NgoEx.Kome(pre + question, this._lang), ModdedStatusType.AMAStress.Swap(), SingletonMonoBehaviour<AltAscModManager>.Instance.GetAMAStressDelta(question), 0, response, anim, "", loopAnim, ModdedSuperchatType.AMA_White.Swap(), false, ""));
        }


        // Token: 0x06000FD6 RID: 4054 RVA: 0x0004A57C File Offset: 0x0004877C
        public override async UniTask StartScenario()
        {
            SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
            SingletonMonoBehaviour<JineManager>.Instance.Uncontrolable();
            AudioManager.Instance.PlayBgmByType(SoundType.BGM_mainloop_shuban, true);
            await base.StartScenario();
            this._Live.HaishinClean();
            SingletonMonoBehaviour<JineManager>.Instance.StartStamp();
            SingletonMonoBehaviour<EventManager>.Instance.AddEventQueue<Scenario_follower_day2_AfterAllNighterhaishin>();
        }

        public new async UniTask EndScenario()
        {
            SingletonMonoBehaviour<AltAscModManager>.Instance.overnightStreamStartDay = 0;
            await base.EndScenario();
        }
    }
}
