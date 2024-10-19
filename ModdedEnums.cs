using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NGO;
using ngov3;


namespace NeedyEnums
{
    public static class Extensions
    {
        /// <summary>
        /// Swaps a <see cref="ModdedStatusType"/> to its vanilla counterpart.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static StatusType Swap(this ModdedStatusType status)
        {
            return (StatusType)(int)status;
        }

        public static EndingType Swap(this ModdedEndingType status)
        {
            return (EndingType)(int)status;
        }

        public static CmdType Swap(this ModdedCmdType status)
        {
            return (CmdType)(int)status;
        }

        public static CommandType Swap(this ModdedCommandType status)
        {
            return (CommandType)(int)status;
        }

        public static SystemTextType Swap(this ModdedSystemTextType status)
        {
            return (SystemTextType)(int)status;
        }

        /// <summary>
        /// Swaps a <see cref="ModdedJineType"/> to its vanilla counterpart.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static JineType Swap(this ModdedJineType jineType)
        {
            return (JineType)(int)jineType;
        }

        /// <summary>
        /// Swaps a <see cref="ModdedTweetType"/> to its vanilla counterpart.
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static TweetType Swap(this ModdedTweetType tweetType)
        {
            return (TweetType)(int)tweetType;
        }

        public static SuperchatType Swap(this ModdedSuperchatType chatType)
        {
            return (SuperchatType)(int)chatType;
        }

        public static ModdedSuperchatType Swap(this SuperchatType chatType)
        {
            return (ModdedSuperchatType)(int)chatType;
        }

        public static ActionType Swap(this ModdedActionType actionType)
        {
            return (ActionType)(int)actionType;
        }

        public static KusoRepType Swap(this ModdedKusoRepType kusoRepType)
        {
            return (KusoRepType)(int)kusoRepType;
        }
        public static List<KusoRepType> Swap(this List<ModdedKusoRepType> kusoRepTypes)
        {
            return kusoRepTypes.Select(k => (k.Swap())).ToList();
        }
    }


    public enum ModdedStatusType
    {
        SleepyAmeCounter = 25,
        OdekakeCounter,
        FollowerPlotFlag,
        OdekakeStressMultiplier,
        OdekakeCountdown,
        AMAStatus,
        AMAStress,
    }
    public enum ModdedEndingType
    {
        Ending_Sleepy = 30,
        Ending_Followers,
    }

    public enum ModdedJineType
    {
        EVENT_COMIKETALERT001 = 1530,
        EVENT_COMIKETALERT002,

        ENDING_SLEEPY_JINE001,
        ENDING_SLEEPY_JINE002,
        ENDING_SLEEPY_JINE003,
        ENDING_SLEEPY_JINE004,
        ENDING_SLEEPY_JINE005,
        ENDING_SLEEPY_JINE006,
        ENDING_SLEEPY_JINE007,
        ENDING_SLEEPY_JINE008,
        ENDING_SLEEPY_JINE009,
        ENDING_SLEEPY_JINE010,

        TOKYO_JINE001,
        TOKYO_JINE002,
        TOKYO_JINE003,
        TOKYO_JINE004,

        FOLLOW_ST_JINE001,
        FOLLOW_ST_JINE002,
        FOLLOW_ST_JINE003,
        FOLLOW_ST_JINE004,
        FOLLOW_ST_JINE005,
        FOLLOW_ST_JINE006,
        FOLLOW_ST_JINE007,
        FOLLOW_ST_JINE008,
        FOLLOW_ST_JINE009,
        FOLLOW_ST_JINE010,

        ODEKAKEPANIC1_POST001,
        ODEKAKEPANIC1_POST002,
        ODEKAKEPANIC1_POST003,

        ODEKAKEPANIC2_POST001,
        ODEKAKEPANIC2_POST002,
        ODEKAKEPANIC2_POST003,
        ODEKAKEPANIC2_POST004,

        ODEKAKEPANIC3_POST001,
        ODEKAKEPANIC3_POST002,
        ODEKAKEPANIC3_POST003,
        ODEKAKEPANIC3_POST004,
        ODEKAKEPANIC3_POST005,

        ODEKAKEPANIC4_POST001,
        ODEKAKEPANIC4_POST002,
        ODEKAKEPANIC4_POST003,
        ODEKAKEPANIC4_POST004,
        ODEKAKEPANIC4_POST005,

        ODEKAKEPANIC5_POST001,
        ODEKAKEPANIC5_POST002,
        ODEKAKEPANIC5_POST003,
        ODEKAKEPANIC5_POST004,
        ODEKAKEPANIC5_POST005,
        ODEKAKEPANIC5_POST006,

        ODEKAKEPANIC6_POST001,
        ODEKAKEPANIC6_POST002,
        ODEKAKEPANIC6_POST003,
        ODEKAKEPANIC6_POST004,
        ODEKAKEPANIC6_POST005,

        ODEKAKEPANIC7_POST001,
        ODEKAKEPANIC7_POST002,
        ODEKAKEPANIC7_POST003,
        ODEKAKEPANIC7_POST004,
        ODEKAKEPANIC7_POST005,

        ODEKAKEPANIC8_POST001,
        ODEKAKEPANIC8_POST002,
        ODEKAKEPANIC8_POST003,
        ODEKAKEPANIC8_POST004,
        ODEKAKEPANIC8_POST005,
        ODEKAKEPANIC8_POST006,
        ODEKAKEPANIC8_POST007,
        ODEKAKEPANIC8_POST008,
        ODEKAKEPANIC8_POST009,
        ODEKAKEPANIC8_POST010,

        EVENT_SONG_DENY001,
        EVENT_SONG_DENY002,
        EVENT_SONG_DENY003,
        EVENT_SONG_DENY004,
        EVENT_SONG_DENY005,
        EVENT_SONG_DENY006,

        FOLLOW_MISTWEET_JINE001,
        FOLLOW_MISTWEET_JINE002,
        FOLLOW_MISTWEET_JINE003,
        FOLLOW_MISTWEET_JINE004,
        FOLLOW_MISTWEET_JINE005,
        FOLLOW_MISTWEET_JINE006,
        FOLLOW_MISTWEET_JINE007,
        FOLLOW_MISTWEET_JINE008,
        FOLLOW_MISTWEET_JINE009,

        ENDING_FOLLOWER_DAY1_JINE001,
        ENDING_FOLLOWER_DAY1_JINE002,
        ENDING_FOLLOWER_DAY1_JINE003,
        ENDING_FOLLOWER_DAY1_JINE004,
        ENDING_FOLLOWER_DAY1_JINE005,
        ENDING_FOLLOWER_DAY1_JINE006,
        ENDING_FOLLOWER_DAY1_JINE007,
        ENDING_FOLLOWER_DAY1_JINE008,
        ENDING_FOLLOWER_DAY1_JINE009,
        ENDING_FOLLOWER_DAY1_JINE010,
        ENDING_FOLLOWER_DAY1_JINE011,
        ENDING_FOLLOWER_DAY1_JINE012,
        ENDING_FOLLOWER_DAY1_JINE013,
        ENDING_FOLLOWER_DAY1_JINE014,
        ENDING_FOLLOWER_DAY1_JINE015,
        ENDING_FOLLOWER_DAY1_JINE016,
        ENDING_FOLLOWER_DAY1_JINE017,
        ENDING_FOLLOWER_DAY1_JINE018,
        ENDING_FOLLOWER_ANGELWATCH_JINE001,
        ENDING_FOLLOWER_ANGELWATCH_JINE002,
        ENDING_FOLLOWER_ANGELWATCH_JINE003,
        ENDING_FOLLOWER_ANGELWATCH_JINE004,
        ENDING_FOLLOWER_ANGELWATCH_JINE005,
        ENDING_FOLLOWER_ANGELWATCH_JINE006,
        ENDING_FOLLOWER_ANGELWATCH_JINE007,
        ENDING_FOLLOWER_ANGELWATCH_JINE008,
        ENDING_FOLLOWER_ANGELWATCH_JINE009,
        ENDING_FOLLOWER_ANGELWATCH_JINE010,
        ENDING_FOLLOWER_ANGELWATCH_JINE011,
        ENDING_FOLLOWER_ANGELWATCH_JINE012,
        ENDING_FOLLOWER_ANGELWATCH_JINE013,
        ENDING_FOLLOWER_ANGELWATCH_JINE014,
        ENDING_FOLLOWER_ANGELWATCH_JINE015,
        ENDING_FOLLOWER_ANGELWATCH_JINE016,
        ENDING_FOLLOWER_ANGELWATCH_JINE017,
        ENDING_FOLLOWER_ANGELWATCH_JINE018,
        ENDING_FOLLOWER_ANGELWATCH_JINE019,
        ENDING_FOLLOWER_ANGELWATCH_JINE020,
        ENDING_FOLLOWER_ANGELWATCH_JINE021,
        ENDING_FOLLOWER_ANGELWATCH_JINE022,
        ENDING_FOLLOWER_ANGELWATCH_JINE023,
        ENDING_FOLLOWER_ANGELWATCH_JINE024,
        ENDING_FOLLOWER_ANGELWATCH_JINE025,
        ENDING_FOLLOWER_ANGELWATCH_JINE026,
        ENDING_FOLLOWER_ANGELWATCH_JINE027,
        ENDING_FOLLOWER_ANGELWATCH_JINE028,
        ENDING_FOLLOWER_ANGELWATCH_JINE029,
        ENDING_FOLLOWER_ANGELWATCH_JINE030,
        ENDING_FOLLOWER_ANGELWATCH_JINE031,
        ENDING_FOLLOWER_ANGELWATCH_JINE032,
        ENDING_FOLLOWER_ANGELWATCH_JINE033,
        ENDING_FOLLOWER_ANGELWATCH_JINE034,
        ENDING_FOLLOWER_ANGELWATCH_JINE035,
        ENDING_FOLLOWER_ANGELWATCH_JINE036,
        ENDING_FOLLOWER_ANGELWATCH_JINE037,
        ENDING_FOLLOWER_ANGELWATCH_JINE038,
        ENDING_FOLLOWER_ANGELWATCH_JINE039,
        ENDING_FOLLOWER_ANGELWATCH_JINE040,
        ENDING_FOLLOWER_ANGELWATCH_JINE041,
        ENDING_FOLLOWER_DAY2_JINE001,
        ENDING_FOLLOWER_DAY2_JINE002,
        ENDING_FOLLOWER_DAY2_JINE003,
        ENDING_FOLLOWER_DAY2_JINE004,
        ENDING_FOLLOWER_DAY2_JINE005,
        ENDING_FOLLOWER_DAY2_JINE006,
        ENDING_FOLLOWER_DAY2_JINE007,
        ENDING_FOLLOWER_DAY2_JINE008,
        ENDING_FOLLOWER_DAY2_JINE009,
        ENDING_FOLLOWER_DAY2_JINE010,
        ENDING_FOLLOWER_DAY2_JINE011,
        ENDING_FOLLOWER_DAY2_JINE012,
        ENDING_FOLLOWER_DAY2_JINE013,
        ENDING_FOLLOWER_DAY2_JINE014,
        ENDING_FOLLOWER_DAY2_JINE015,
        ENDING_FOLLOWER_DAY2_JINE016,
        ENDING_FOLLOWER_DAY2_JINE017,
        ENDING_FOLLOWER_DAY2_JINE018,
        ENDING_FOLLOWER_DAY2_JINE019,
        ENDING_FOLLOWER_DAY2_JINE020,
        ENDING_FOLLOWER_DAY2_JINE021,
        ENDING_FOLLOWER_DAY2_JINE022,
        ENDING_FOLLOWER_DAY2_JINE023,
        ENDING_FOLLOWER_DAY2_JINE024,
        ENDING_FOLLOWER_DAY2_JINE025,
        ENDING_FOLLOWER_DAY2_JINE026,
        ENDING_FOLLOWER_DAY2_JINE027,
        ENDING_FOLLOWER_DAY2_JINE028,
        ENDING_FOLLOWER_DAY2_JINE029,
        ENDING_FOLLOWER_DAY2_JINE030,
        ENDING_FOLLOWER_DAY2_JINE031,
        ENDING_FOLLOWER_DAY2_JINE032,
        ENDING_FOLLOWER_DAY2_JINE033,
        ENDING_FOLLOWER_DAY2_JINE034,
        ENDING_FOLLOWER_DAY2_JINE035,
        ENDING_FOLLOWER_DAY2_JINE036,
        ENDING_FOLLOWER_DAY2_JINE037,
        ENDING_FOLLOWER_DAY2_JINE038,
        ENDING_FOLLOWER_DAY2_JINE039,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE001,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE002,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE003,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE004,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE005,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE006,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE007,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE008,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE009,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE010,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE011,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE012,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE013,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE014,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE015,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE016,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE017,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE018,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE019,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE020,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE021,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE022,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE023,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE024,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE025,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE026,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE027,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE028,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE029,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE030,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE031,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE032,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE033,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE034,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE035,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE036,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE037,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE038,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE039,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE040,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE041,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE042,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE043,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE044,
        ENDING_FOLLOWER_DAY3_LOGIN_JINE045,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE001,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE002,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE003,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE004,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE005,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE006,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE007,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE008,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE009,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE010,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE011,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE012,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE013,
        ENDING_FOLLOWER_DAY3_PRESTREAM_JINE014,
        ENDING_FOLLOWER_DAY4_JINE001,
        ENDING_FOLLOWER_DAY4_JINE002,
        ENDING_FOLLOWER_DAY4_JINE003,
        ENDING_FOLLOWER_DAY4_JINE004,
        ENDING_FOLLOWER_DAY4_JINE005,
        ENDING_FOLLOWER_DAY4_JINE_DELETEPI,
        ENDING_FOLLOWER_DAY4_JINE006,
        ENDING_FOLLOWER_DAY4_JINE007,
    }

    public enum ModdedActionType
    {
        OdekakeTokyo = 43,
        HaishinComiket,
        HaishinAngelWatch,
        HaishinAngelDeath,
        HaishinAngelFuneral,
        OkusuriDaypassStalk,
        OdekakePanic1,
        OdekakePanic2,
        OdekakePanic3,
        OdekakePanic4,
        OdekakeBreak,
        Internet2chStalk,
        OdekakeFuneral
    }

    public enum ModdedCmdType
    {
        OdekakeTokyo = 130,
        HaishinComiket,
        HaishinAngelWatch,
        HaishinAngelDeath,
        HaishinAngelFuneral,
        OkusuriDaypassStalk,
        OdekakePanic1,
        OdekakePanic2,
        OdekakePanic3,
        OdekakePanic4,
        OdekakeBreak,
        Internet2chStalk,
        OdekakeFuneral
    }

    public enum ModdedCommandType
    {
        OdekakeTokyo = 1000,
        OkusuriStalk,
        Internet2chStalk
    }

    public enum ModdedSystemTextType
    {
        System_ComiketStreamUpcoming = 1000,
        System_AMARead,
        Login_BadPassword,
        System_InternetYamero,
        System_RealYamero,
        Poketter_AmeQuit
    }

    public enum ModdedSuperchatType
    {
        AMA_START = 8,
        AMA_END,
        AMA_White,
        AMA_Blue,
        AMA_Cyan,
        AMA_LightGreen,
        AMA_Yellow,
        AMA_Orange,
        AMA_Magenta,
        AMA_Red,
        EVENT_TIMEPASS,
        JINE_INIT,
        JINE_DESTROY,
        JINE_SEND,
        JINE_CHOICE,
        JINE_WAIT,
        EVENT_CREATEDEPAZ,
        EVENT_DOSE,
        EVENT_DELAYFRAME,
        EVENT_MUSICCHANGE,
        EVENT_SHADER,
        EVENT_SHADERWAIT,
        EVENT_MAINPANELCOLOR,
    }

    public enum ModdedTweetType
    {
        TOKYO_TWEET001 = 2001,
        TOKYO_TWEET002,
        DARKNIGHT_TWEET001,
        DARKNIGHT_TWEET002,
        DARKNIGHT_TWEET003,
        DARKNIGHT_TWEET004,
        ODEKAKEPANIC1_TWEET001,
        ODEKAKEPANIC2_TWEET001,
        ODEKAKEPANIC3_TWEET001,
        ODEKAKEPANIC4_TWEET001,
        STALKDISCOVER_TWEET001,
        STALKDISCOVER_TWEET002,
        STALKODEKAKE1_TWEET001,
        STALKODEKAKE1_TWEET002,
        STALKODEKAKE1_TWEET003,
        STALKODEKAKE2_TWEET001,
        STALKODEKAKE2_TWEET002,
        STALKODEKAKE2_TWEET003,
        PREANGELWATCH_TWEET001,
        PREANGELWATCH_TWEET002,
        POSTANGELWATCH_TWEET001,
        POSTANGELWATCH_TWEET002,
        POSTANGELWATCH_TWEET003,
        POSTANGELWATCH_TWEET004,
        POSTANGELWATCH_TWEET005,
        BADPASSWORD_TWEET001,
        BADPASSWORD_TWEET002,
        DEADKANGEL_TWEET001,
        DEADKANGEL_TWEET002,
        DEADKANGEL_TWEET003,
        ANGELFUNERAL_TWEET001,
        ANGELFUNERAL_TWEET002,
    }

    public enum ModdedKusoRepType
    {
        TOKYO_TWEET001_KUSO001 = 1000,
        TOKYO_TWEET001_KUSO002,
        TOKYO_TWEET001_KUSO003,
        TOKYO_TWEET001_KUSO004,
        STALKODEKAKE2_TWEET001_KUSO001,
        STALKODEKAKE2_TWEET001_KUSO002,
        STALKODEKAKE2_TWEET001_KUSO003,
        STALKODEKAKE2_TWEET003_KUSO001,
        STALKODEKAKE2_TWEET003_KUSO002,
        STALKODEKAKE2_TWEET003_KUSO003,
        STALKODEKAKE2_TWEET003_KUSO004,
        STALKODEKAKE2_TWEET003_KUSO005,
        STALKODEKAKE2_TWEET003_KUSO006,
        STALKODEKAKE2_TWEET003_KUSO007,
        PREANGELWATCH_TWEET001_KUSO001,
        PREANGELWATCH_TWEET001_KUSO002,
        PREANGELWATCH_TWEET001_KUSO003,
        PREANGELWATCH_TWEET001_KUSO004,
        PREANGELWATCH_TWEET001_KUSO005,
        POSTANGELWATCH_TWEET001_KUSO001,
        POSTANGELWATCH_TWEET001_KUSO002,
        POSTANGELWATCH_TWEET001_KUSO003,
        POSTANGELWATCH_TWEET001_KUSO004,
        POSTANGELWATCH_TWEET002_KUSO001,
        POSTANGELWATCH_TWEET002_KUSO002,
        POSTANGELWATCH_TWEET002_KUSO003,
        POSTANGELWATCH_TWEET003_KUSO001,
        POSTANGELWATCH_TWEET003_KUSO002,
        POSTANGELWATCH_TWEET003_KUSO003,
        POSTANGELWATCH_TWEET003_KUSO004,
        POSTANGELWATCH_TWEET003_KUSO005,
        BADPASSWORD_TWEET001_KUSO001,
        BADPASSWORD_TWEET001_KUSO002,
        BADPASSWORD_TWEET001_KUSO003,
        BADPASSWORD_TWEET001_KUSO004,
        DEADKANGEL_TWEET001_KUSO001,
        DEADKANGEL_TWEET001_KUSO002,
        DEADKANGEL_TWEET001_KUSO003,
        DEADKANGEL_TWEET001_KUSO004,
        DEADKANGEL_TWEET001_KUSO005,
        DEADKANGEL_TWEET001_KUSO006,
        DEADKANGEL_TWEET001_KUSO007,
        DEADKANGEL_TWEET002_KUSO001,
        DEADKANGEL_TWEET002_KUSO002,
        DEADKANGEL_TWEET002_KUSO003,
        DEADKANGEL_TWEET002_KUSO004,
        DEADKANGEL_TWEET002_KUSO005,
        DEADKANGEL_TWEET003_KUSO001,
        DEADKANGEL_TWEET003_KUSO002,
        DEADKANGEL_TWEET003_KUSO003,
    }

    public enum ModdedAlphaType
    {
        FollowerAlpha = 13
    }

    public enum ModdedTenCommentType
    {
        FollowerAlpha1_STREAMNAME = 10000,
        FollowerAlpha1_STREAM001,
        FollowerAlpha1_STREAM002,
        FollowerAlpha1_STREAM003,
        FollowerAlpha1_STREAM004,
        FollowerAlpha1_STREAM005,
        FollowerAlpha1_STREAM006,
        FollowerAlpha1_STREAM007,
        FollowerAlpha1_STREAM008,
        FollowerAlpha1_STREAM009,
        FollowerAlpha1_STREAM010,
        FollowerAlpha1_STREAM011,
        FollowerAlpha1_STREAM012,
        FollowerAlpha1_STREAM013,
        FollowerAlpha1_RESPONSE001,
        FollowerAlpha1_RESPONSE002,
        FollowerAlpha1_RESPONSE003,
        FollowerAlpha1_RESPONSE004,
        FollowerAlpha1_RESPONSE005,
        FollowerAlpha2_STREAMNAME,
        FollowerAlpha2_INTRO001,
        FollowerAlpha2_INTRO002,
        FollowerAlpha2_INTRO003,
        FollowerAlpha2_INTRO004,
        FollowerAlpha2_INTRO005,
        FollowerAlpha2_INTRO006,
        FollowerAlpha2_AMA_Q001_RESPONSE,
        FollowerAlpha2_AMA_Q002_RESPONSE,
        FollowerAlpha2_AMA_Q003_RESPONSE,
        FollowerAlpha2_AMA_Q004_RESPONSE,
        FollowerAlpha2_AMA_Q005_RESPONSE,
        FollowerAlpha2_AMA_Q005_RESPONSE_B,
        FollowerAlpha2_AMA_Q006_RESPONSE,
        FollowerAlpha2_AMA_Q007_RESPONSE,
        FollowerAlpha2_AMA_Q007_RESPONSE_B,
        FollowerAlpha2_AMA_Q008_RESPONSE,
        FollowerAlpha2_AMA_Q008_RESPONSE_B,
        FollowerAlpha2_AMA_Q009_RESPONSE,
        FollowerAlpha2_AMA_Q010_RESPONSE,
        FollowerAlpha2_AMA_Q010_RESPONSE_B,
        FollowerAlpha2_AMA_Q011_RESPONSE,
        FollowerAlpha2_AMA_Q012_RESPONSE,
        FollowerAlpha2_AMA_Q013_RESPONSE,
        FollowerAlpha2_AMA_Q013_RESPONSE_B,
        FollowerAlpha2_AMA_Q014_RESPONSE,
        FollowerAlpha2_AMA_Q015_RESPONSE,
        FollowerAlpha2_AMA_Q015_RESPONSE_B,
        FollowerAlpha2_AMA_Q016_RESPONSE,
        FollowerAlpha2_AMA_Q016_RESPONSE_B,
        FollowerAlpha2_AMA_Q017_RESPONSE,
        FollowerAlpha2_AMA_Q018_RESPONSE,
        FollowerAlpha2_AMA_Q018_RESPONSE_B,
        FollowerAlpha2_AMA_Q019_RESPONSE,
        FollowerAlpha2_AMA_Q020_RESPONSE,
        FollowerAlpha2_AMA_Q020_RESPONSE_B,
        FollowerAlpha2_AMA_Q021_RESPONSE,
        FollowerAlpha2_AMA_Q022_RESPONSE,
        FollowerAlpha2_AMA_Q023_RESPONSE,
        FollowerAlpha2_AMA_Q023_RESPONSE_B,
        FollowerAlpha2_AMA_Q024_RESPONSE,
        FollowerAlpha2_AMA_Q024_RESPONSE_B,
        FollowerAlpha2_AMA_Q025_RESPONSE,
        FollowerAlpha2_AMA_Q026_RESPONSE,
        FollowerAlpha2_AMA_Q026_RESPONSE_B,
        FollowerAlpha2_AMA_Q027_RESPONSE,
        FollowerAlpha2_AMA_Q027_RESPONSE_B,
        FollowerAlpha2_AMA_Q028_RESPONSE,
        FollowerAlpha2_AMA_Q029_RESPONSE,
        FollowerAlpha2_AMA_Q030_RESPONSE,
        FollowerAlpha2_AMA_Q031_RESPONSE,
        FollowerAlpha2_AMAFINISH_BAD001,
        FollowerAlpha2_AMAFINISH_BAD002,
        FollowerAlpha2_AMAFINISH_BAD003,
        FollowerAlpha2_AMAFINISH_GOOD001,
        FollowerAlpha2_AMAFINISH_GOOD002,
        FollowerAlpha2_AMAFINISH_GOOD003,
        FollowerAlpha2_AMAFINISH_IGNORE001,
        FollowerAlpha2_AMAFINISH_IGNORE002,
        FollowerAlpha2_AMAFINISH_IGNORE003,
        FollowerAlpha2_GAMING001,
        FollowerAlpha2_GAMING002,
        FollowerAlpha2_GAMING003,
        FollowerAlpha2_GAMING004,
        FollowerAlpha2_GAMING005,
        FollowerAlpha2_GAMING006,
        FollowerAlpha2_GAMING007,
        FollowerAlpha2_GAMING008,
        FollowerAlpha2_GAMING009,
        FollowerAlpha2_GAMING010,
        FollowerAlpha2_GAMING011,
        FollowerAlpha2_GAMING012,
        FollowerAlpha2_GAMING013,
        FollowerAlpha2_GAMING014,
        FollowerAlpha2_GAMING015,
        FollowerAlpha2_GAMING016,
        FollowerAlpha2_GAMING017,
        FollowerAlpha2_OVERDOSE001,
        FollowerAlpha2_OVERDOSE002,
        FollowerAlpha2_OVERDOSE003,
        FollowerAlpha2_OVERDOSE004,
        FollowerAlpha2_OVERDOSE005,
        FollowerAlpha2_OVERDOSE006,
        FollowerAlpha2_OVERDOSE007,
        FollowerAlpha2_OVERDOSE008,
        FollowerAlpha2_OVERDOSE009,
        FollowerAlpha2_OVERDOSE010,
        FollowerAlpha2_OVERDOSE011,
        FollowerAlpha2_OVERDOSE012,
        FollowerAlpha2_NIGHTMARE001,
        FollowerAlpha2_NIGHTMARE002,
        FollowerAlpha2_NIGHTMARE003,
        FollowerAlpha2_NIGHTMARE004,
        FollowerAlpha2_NIGHTMARE005,
        FollowerAlpha2_NIGHTMARE006,
        FollowerAlpha2_NIGHTMARE007,
        FollowerAlpha2_NIGHTMARE008,
        FollowerAlpha2_NIGHTMARE009,
        FollowerAlpha2_NIGHTMARE010,
        FollowerAlpha2_NIGHTMARE011,
        FollowerAlpha3_STREAMNAME,
        FollowerAlpha3_RECENTEVENTS001,
        FollowerAlpha3_RECENTEVENTS002,
        FollowerAlpha3_RECENTEVENTS003,
        FollowerAlpha3_RECENTEVENTS004,
        FollowerAlpha3_RECENTEVENTS005,
        FollowerAlpha3_RECENTEVENTS006,
        FollowerAlpha3_RECENTEVENTS007,
        FollowerAlpha3_RECENTEVENTS008,
        FollowerAlpha3_RECENTEVENTS009,
        FollowerAlpha3_RECENTEVENTS010,
        FollowerAlpha3_RECENTEVENTS011,
        FollowerAlpha3_RECENTEVENTS012,
        FollowerAlpha3_RECENTEVENTS013,
        FollowerAlpha3_RECENTEVENTS014,
        FollowerAlpha3_RECENTEVENTS015,
        FollowerAlpha3_RECENTEVENTS016,
        FollowerAlpha3_RECENTEVENTS017,
        FollowerAlpha3_RECENTEVENTS018,
        FollowerAlpha3_RECENTEVENTS019,
        FollowerAlpha3_RECENTEVENTS020,
        FollowerAlpha3_RECENTEVENTS021,
        FollowerAlpha3_RECENTEVENTS022,
        FollowerAlpha3_RECENTEVENTS023,
        FollowerAlpha3_RECENTEVENTS024,
        FollowerAlpha3_RECENTEVENTS025,
        FollowerAlpha3_RECENTEVENTS026,
        FollowerAlpha3_RECENTEVENTS027,
        FollowerAlpha3_RECENTEVENTS028,
        FollowerAlpha3_RECENTEVENTS029,
        FollowerAlpha4_STREAMNAME,
        FollowerAlpha4_FAULT001,
        FollowerAlpha4_FAULT002,
        FollowerAlpha4_FAULT003,
        FollowerAlpha4_FAULT004,
        FollowerAlpha4_FAULT005,
        FollowerAlpha4_FAULT006,
        FollowerAlpha4_FAULT007,
        FollowerAlpha4_FAULT008,
        FollowerAlpha4_FAULT009,
        FollowerAlpha4_FAULT010,
        FollowerAlpha4_FAULT011,
        FollowerAlpha4_FAULT012,
        FollowerAlpha4_FAULT013,
        FollowerAlpha4_FAULT014,
        FollowerAlpha4_FAULT015,
        FollowerAlpha4_FAULT016,
        FollowerAlpha4_FAULT017,
        FollowerAlpha4_FAULT018,
        FollowerAlpha4_FAULT019,
        FollowerAlpha4_FAULT020,
        FollowerAlpha4_FAULT021,
        FollowerAlpha4_FAULT022,
        FollowerAlpha4_FAULT023,
    }

    public enum ModdedAppType
    {
        PillDaypass_Follower = 1000,
        Login_Hacked,
        Follower_taiki,
        AltPoketter
    }

    public enum ModdedEffectType
    {
        Vengeful = 23,
        Hazy
    }
}

