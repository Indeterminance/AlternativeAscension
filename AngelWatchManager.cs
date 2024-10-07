using Cysharp.Threading.Tasks;
using NGO;
using ngov3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NeedyXML;
using NeedyEnums;
using System.Reflection;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using HarmonyLib;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;

namespace NeedyMintsOverdose
{
    public static class AngelWatchManager
    {
        // AMA stuff
        public static int QUESTIONS = 20;
        public static int QUESTIONS_ASKED = 0;
        public static int PlannedAMALength = 120000;
        public static List<Playing> playedQuestions = new List<Playing>();
        public static bool deleteTime = false;
        public static LiveComment deleteComment = null;
        public static bool isAMA = false;
        public static bool wasUncontrollable = false;
        public static int oldSpeed = 1;
        public static DateTime startTime;
        public static int delta;
        public static int StressDelta = 0;

        public static TweenerCore<float, float, FloatOptions> anim;
        public static List<IWindow> pills = new List<IWindow>();

        public static int GetStressDelta(string question)
        {
            //NeedyMintsMod.log.LogMessage($"Getting delta for question \"{question}\"");
            int delta = 0;
            switch (question)
            {
                case "AMA_Q001": delta = 3;
                    break;
                case "AMA_Q006": delta = 10;
                    break;
                case "AMA_Q007": delta = 1;
                    break;
                case "AMA_Q014": delta = 8;
                    break;
                case "AMA_Q019": delta = 4;
                    break;
                case "AMA_Q020":
                    delta = -99;
                    goto default;
                case "AMA_Q023": delta = 1;
                    break;
                case "AMA_Q027": delta = 10;
                    break;
                case "AMA_Q031": delta = 8;
                    break;
                default: break;
            }
            return delta;
        }

        public static List<Playing> GetAMAEnd()
        {
            string prefix = ModdedAlphaType.FollowerAlpha.ToString() + "2_AMAFINISH_";
            string mid;
            if (StressDelta > 15)
            {
                mid = "BAD";
            }
            else if (playedQuestions.Count < 6)
            {
                mid = "IGNORE";
            }
            else
            {
                mid = "GOOD";
            }

            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            List<Playing> list = new List<Playing>()
            {
                new Playing(true,NgoEx.TenTalk(prefix + mid + "001",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
                new Playing(true,NgoEx.TenTalk(prefix + mid + "002",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
                new Playing(true,NgoEx.TenTalk(prefix + mid + "003",lang),StatusType.Tension,1,0,"","","",true,SuperchatType.White,false,""),
            };

            if (mid == "BAD" || mid == "IGNORE")
            {
                list.Insert(1, new Playing(false, NgoEx.Kome(prefix + mid, lang)));
            }
            return list;
        }

        public static void StartAMA(ref LiveScenario scenario)
        {
            isAMA = true;
            startTime = DateTime.Now;
            playedQuestions.Clear();

            Live live = new Traverse(scenario).Field(nameof(LiveScenario._Live)).GetValue() as Live;

            //Live live = UnityEngine.Object.FindObjectOfType<Live>();
            wasUncontrollable = live.isUncontrollable;
            live.isUncontrollable = true;
            NeedyMintsMod.log.LogMessage($"Live was {live}");

            // Change comment label
            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            SystemTextType type = (SystemTextType)(int)ModdedSystemTextType.System_AMARead;
            string text = NgoEx.SystemTextFromType(type, lang);
            NeedyMintsMod.log.LogMessage($"AMA systext {text}!");
            live.CommentLabel.text = text;
        }

        public static void FinishAMA(ref LiveScenario scenario)
        {
            isAMA = false;
            Live live = new Traverse(scenario).Field(nameof(LiveScenario._Live)).GetValue() as Live;
            live.isUncontrollable = wasUncontrollable;
            //NeedyMintsMod.log.LogMessage($"AMA ended with {playedQuestions.Count} questions asked and {StressDelta} AMA stress accumulated!");

            List<Playing> amaEndText = GetAMAEnd();
            scenario.playing.InsertRange(1, amaEndText);


            NeedyMintsMod.log.LogMessage($"FinishAMA finishing with {live}...");

            // Change comment label
            LanguageType lang = SingletonMonoBehaviour<Settings>.Instance.CurrentLanguage.Value;
            SystemTextType type = SystemTextType.Sysyem_NotReadComment;
            string text = NgoEx.SystemTextFromType(type, lang);
            NeedyMintsMod.log.LogMessage($"AMA systext {text}!");
            live.CommentLabel.text = text;

            NeedyMintsMod.log.LogMessage("FinishAMA finished");
            
        }
    }
}
