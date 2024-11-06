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
using Extensions;

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
            public bool isLove;
            public bool isLoveLoop;
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
            AngelFuneral = 9,
        }

        public static Array GetBothEndings()
        {
            //NeedyMintsMod.log.LogMessage($"GetBothEndings!");
            object[] endings = new object[] { };
            foreach (EndingType end in Enum.GetValues(typeof(EndingType)))
            {
                endings = endings.Append(end).ToArray();
            }
            foreach (ModdedEndingType end in Enum.GetValues(typeof(ModdedEndingType)))
            {
                endings = endings.Append(end.Swap()).ToArray();
            }
            //foreach (object obj in endings)
            //{
            //    NeedyMintsMod.log.LogMessage($"Ending : {obj}");
            //}
            return endings;
        }

        public static string[] GetBothEndingNames()
        {
            List<string> strings = Enum.GetNames(typeof(EndingType)).ToList();
            strings.AddRange(Enum.GetNames(typeof(ModdedEndingType)));
            return strings.ToArray();
        }

        public static void EndingBlockMaker(Transform parent, int endingsPerRow)
        {
            HorizontalLayoutGroup hgroup = parent.GetComponent<HorizontalLayoutGroup>();

            Vector2 vals = (parent.GetChild(0).transform as RectTransform).sizeDelta;
            RectOffset roff = hgroup.padding;

            UnityEngine.Object.DestroyImmediate(hgroup);
            GridLayoutGroup grid = parent.AddComponent<GridLayoutGroup>();
            grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = endingsPerRow;
            grid.spacing = new Vector2(8, 8);
            grid.childAlignment = TextAnchor.MiddleCenter;
            grid.cellSize = vals;
        }
    }
}
