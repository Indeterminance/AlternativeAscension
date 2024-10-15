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
using System.Runtime.CompilerServices;

namespace NeedyMintsOverdose
{
    internal static partial class MintyOverdosePatches
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Compares a value against its modded and vanilla <see cref="Enum"/>, and returns a <see cref="Tuple"/>
        /// containing both the <see cref="bool"/> representing its modded status as well as the <see cref="int"/>
        /// value contained by the host enum.
        /// </summary>
        /// <param name="baseType">The vanilla enum of the value.</param>
        /// <param name="moddedType">The modded enum counterpart of the value.</param>
        /// <param name="val">A modded or vanilla value to compare against.</param>
        /// <returns></returns>
        public static Tuple<bool, int> CheckModdedPrefix(Type baseType, Type moddedType, object val)
        {
            bool isVanilla = Enum.IsDefined(baseType, (int)val);
            bool isModded = Enum.IsDefined(moddedType, (int)val);


            if (isVanilla && isModded) NeedyMintsMod.log.LogError($"ID conflict between {baseType} and {moddedType} ID {val}");
            if (!isVanilla && !isModded) NeedyMintsMod.log.LogError($"ID {val} doesn't exist in either {baseType} or {moddedType}");
            if (isVanilla && !isModded)
            {
                return new Tuple<bool, int>(false, (int)val);
            }
            return new Tuple<bool, int>(true, (int)val);
        }

        [Serializable]
        public class ModdedSlotData : SlotData
        {
            public int sleepCount;
        }

        private static ModdedSlotData LoadModdedSlotByES3(string fileName)
        {
            return new ModdedSlotData
            {
                jineHistory = ES3.Load<List<JineData>>("JINEHISTORY", fileName),
                poketterHistory = ES3.Load<List<TweetData>>("POKETTERHISTORY", fileName),
                eventsHistory = ES3.Load<List<string>>("EVENTHISTORY", fileName),
                dayActionHistory = ES3.Load<List<string>>("DAYACTIONHISTORY", fileName, new List<string>()),
                loop = (int)ES3.Load("LOOPCOUNT", fileName),
                midokumushi = (int)ES3.Load("MIDOKUCOUNT", fileName),
                psycheCount = ES3.Load<int>("PSYCHECOUNT", fileName, 0),
                havingNetas = ES3.Load<List<AlphaLevel>>("HAVINGNETAS", fileName),
                usedNetas = ES3.Load<List<AlphaLevel>>("USEDNETAS", fileName),
                isJuncho = ES3.Load<bool>("ISJUNCHO", fileName),
                isHearTrauma = ES3.Load<bool>("ISHEARTRAUMA", fileName),
                trauma = ES3.Load<JineType>("TRAUMA", fileName),
                firstDate = ES3.Load<CmdType>("FIRSTDATE", fileName, CmdType.None),
                isHappaOK = ES3.Load<bool>("ISHAPPAOK", fileName),
                isHorror = ES3.Load<bool>("ISHORROR", fileName),
                isGedatsu = ES3.Load<bool>("ISGEDATSU", fileName),
                beforeWristCut = ES3.Load<bool>("BEFOREWRISTCUT", fileName, false),
                isWristCut = ES3.Load<bool>("ISWRISTCUT", fileName, false),
                isHakkyo = ES3.Load<bool>("ISHAKKYO", fileName, false),
                wishlist = ES3.Load<int>("WISHLIST", fileName, 0),
                loveDiary = ES3.Load<int>("LOVEDIARY", fileName, 0),
                isShurokued = ES3.Load<bool>("ISSHUROKUED", fileName, false),
                kyuusiCount = ES3.Load<int>("KYUUSICOUNT", fileName, 0),
                isOpenGinga = ES3.Load<bool>("ISOPENGINGA", fileName, false),
                is150mil = ES3.Load<bool>("IS150MIL", fileName, false),
                is300mil = ES3.Load<bool>("IS300MIL", fileName, false),
                is500mil = ES3.Load<bool>("IS500MIL", fileName, false),
                stats = ES3.Load<List<Status>>("STATUS", fileName),
                sleepCount = ES3.Load<int>("SLEEPCOUNT", fileName, 0)
            };
        }

        public static bool CheckTokyoAvailable()
        {
            int day = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
            int dayPart = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayPart);
            int followers = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.Follower);
            int plotflag = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(ModdedStatusType.FollowerPlotFlag.Swap());

            return ((day >= 14 && day <= 16) && dayPart != 2 && followers >= 250000 && plotflag == (int)FollowerPlotFlagValues.None);
        }

        public static void BumpDayMax()
        {
            int today = SingletonMonoBehaviour<StatusManager>.Instance.GetStatus(StatusType.DayIndex);
            SingletonMonoBehaviour<StatusManager>.Instance.UpdateStatusMaxToNumber(StatusType.DayIndex, today + 1);
        }

        public static Playing SuperPlaying(bool isJimaku, string nakami, StatusType difftype = StatusType.Tension,
            int delta = 1, int AdditionalTension = 0, string henji = "", string henjiAnim = "",
            string animation = "", bool isLoopAnim = true, SuperchatType color = SuperchatType.White,
            bool showAnti = false, string antiComment = "")
        {
            Playing play = new Playing()
            {
                isJimaku = isJimaku,
                nakami = nakami,
                diffStatusType = difftype,
                delta = delta,
                additionalTension = AdditionalTension,
                henji = henji,
                henjiAnim = henjiAnim,
                animation = animation,
                isLoopAnim = isLoopAnim,
                color = color,
                showAnti = showAnti,
                antiComment = antiComment,
                inout = "",
                startReading = false,
                obi = ChanceEffectType.None,
                isSetting = false,
                isYohaku = false,
                ongaku = SoundType.None,
            };
            return play;
        }

        public enum FollowerPlotFlagValues
        {
            None = 0,
            VisitedComiket = 1,
            PostComiket = 2,
            PostDepaz = 3,
            StalkReveal = 4,
            OdekakeBreak = 5,
            AngelWatch = 6,
            BadPassword = 7,
            AngelDeath = 8,
            FinalOdekake = 9,
            AngelFuneral = 10,
        }
    }
}
