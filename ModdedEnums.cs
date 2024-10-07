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
    }


    public enum ModdedStatusType
    {
        SleepyAmeCounter = 25,
        OdekakeCounter = 26,
        VisitedComiket = 27,
        FollowerPlotFlag = 28,
        OdekakeStressMultiplier = 29,
        OdekakeCountdown = 30,
        AMAStatus = 31,
    }
    public enum ModdedEndingType
    {
        Ending_Sleepy = 30,
        Ending_Followers = 31,
    }

    public enum ModdedJineType
    {
        ENDING_SLEEPY_JINE001 = 1530,
        ENDING_SLEEPY_JINE002 = 1531,
        ENDING_SLEEPY_JINE003 = 1532,
        ENDING_SLEEPY_JINE004 = 1533,
        ENDING_SLEEPY_JINE005 = 1534,
        ENDING_SLEEPY_JINE006 = 1535,
        ENDING_SLEEPY_JINE007 = 1536,
        ENDING_SLEEPY_JINE008 = 1537,
        ENDING_SLEEPY_JINE009 = 1538,
        ENDING_SLEEPY_JINE010 = 1539,
        TOKYO_JINE001 = 1540,
        TOKYO_JINE002 = 1541,
        TOKYO_JINE003 = 1542,
        TOKYO_JINE004 = 1543,
        FOLLOW_ST_JINE001 = 1544,
        FOLLOW_ST_JINE002 = 1545,
        FOLLOW_ST_JINE003 = 1546,
        FOLLOW_ST_JINE004 = 1547,
        FOLLOW_ST_JINE005 = 1548,
        FOLLOW_ST_JINE006 = 1549,
        FOLLOW_ST_JINE007 = 1550,
        FOLLOW_ST_JINE008 = 1551,
        FOLLOW_ST_JINE009 = 1552,
        FOLLOW_ST_JINE010 = 1553,
        FOLLOW_MISTWEET_JINE001 = 1554,
        FOLLOW_MISTWEET_JINE002 = 1555,
        FOLLOW_MISTWEET_JINE003 = 1556,
        FOLLOW_MISTWEET_JINE004 = 1557,
        FOLLOW_MISTWEET_JINE005 = 1558,
        FOLLOW_MISTWEET_JINE006 = 1559,
        FOLLOW_MISTWEET_JINE007 = 1560,
        FOLLOW_MISTWEET_JINE008 = 1561,
        FOLLOW_MISTWEET_JINE009 = 1562,
        ENDING_FOLLOWER_JINE001 = 1563,
        ENDING_FOLLOWER_JINE002 = 1564,
        ENDING_FOLLOWER_JINE003 = 1565,
        ENDING_FOLLOWER_JINE004 = 1566,
        ENDING_FOLLOWER_JINE005 = 1567,
        ENDING_FOLLOWER_JINE006 = 1568,
        ENDING_FOLLOWER_JINE007 = 1569,
        ENDING_FOLLOWER_JINE008 = 1570,
        ENDING_FOLLOWER_JINE009 = 1571,
        ENDING_FOLLOWER_JINE010 = 1572,
        ENDING_FOLLOWER_JINE011 = 1573,
        ENDING_FOLLOWER_JINE012 = 1574,
        ENDING_FOLLOWER_JINE013 = 1575,
        ENDING_FOLLOWER_JINE014 = 1576,
        ENDING_FOLLOWER_JINE015 = 1577,
        ENDING_FOLLOWER_JINE016 = 1578,
        ENDING_FOLLOWER_JINE017 = 1579,
        ENDING_FOLLOWER_JINE018 = 1580,
        ENDING_FOLLOWER_JINE019 = 1581,
        ENDING_FOLLOWER_JINE020 = 1582,
        ENDING_FOLLOWER_JINE021 = 1583,
        ENDING_FOLLOWER_JINE022 = 1584,
        ENDING_FOLLOWER_JINE023 = 1585,
        ENDING_FOLLOWER_JINE024 = 1586,
        ENDING_FOLLOWER_JINE025 = 1587,
        ENDING_FOLLOWER_JINE026 = 1588,
        ENDING_FOLLOWER_JINE027 = 1589,
        ENDING_FOLLOWER_JINE028 = 1590,
        ENDING_FOLLOWER_JINE029 = 1591,
        ENDING_FOLLOWER_JINE030 = 1592,
        ENDING_FOLLOWER_JINE031 = 1593,
        ENDING_FOLLOWER_JINE032 = 1594,
        ENDING_FOLLOWER_JINE033 = 1595,
        ENDING_FOLLOWER_JINE034 = 1596,
        ENDING_FOLLOWER_JINE035 = 1597,
        ENDING_FOLLOWER_JINE036 = 1598,
        ENDING_FOLLOWER_JINE037 = 1599,
        ENDING_FOLLOWER_JINE038 = 1600,
        ENDING_FOLLOWER_JINE039 = 1601,
        ENDING_FOLLOWER_JINE040 = 1602,
        ENDING_FOLLOWER_JINE041 = 1603,
        ENDING_FOLLOWER_JINE042 = 1604,
        ENDING_FOLLOWER_JINE043 = 1605,
        ENDING_FOLLOWER_JINE044 = 1606,
        ENDING_FOLLOWER_JINE045 = 1607,
        ENDING_FOLLOWER_JINE046 = 1608,
        ENDING_FOLLOWER_JINE047 = 1609,
        ENDING_FOLLOWER_JINE048 = 1610,
        ENDING_FOLLOWER_JINE049 = 1611,
        ENDING_FOLLOWER_JINE050 = 1612,
        ENDING_FOLLOWER_JINE051 = 1613,
        ENDING_FOLLOWER_JINE052 = 1614,
        ENDING_FOLLOWER_JINE053 = 1615,
        ENDING_FOLLOWER_JINE054 = 1616,
        ENDING_FOLLOWER_JINE055 = 1617,
        ENDING_FOLLOWER_JINE056 = 1618,
        ENDING_FOLLOWER_JINE057 = 1619,
        ENDING_FOLLOWER_JINE058 = 1620,
        ENDING_FOLLOWER_JINE059 = 1621,
        ENDING_FOLLOWER_JINE060 = 1622,
        ENDING_FOLLOWER_JINE061 = 1623,
        ENDING_FOLLOWER_JINE062 = 1624,
        ENDING_FOLLOWER_JINE063 = 1625,
        ENDING_FOLLOWER_JINE064 = 1626,
        ENDING_FOLLOWER_JINE065 = 1627,
        ENDING_FOLLOWER_JINE066 = 1628,
        ENDING_FOLLOWER_JINE067 = 1629,
        ENDING_FOLLOWER_JINE068 = 1630,
        ENDING_FOLLOWER_JINE069 = 1631,
        ENDING_FOLLOWER_JINE070 = 1632,
        ENDING_FOLLOWER_JINE071 = 1633,
        ENDING_FOLLOWER_JINE072 = 1634,
        ENDING_FOLLOWER_JINE073 = 1635,
        ENDING_FOLLOWER_JINE074 = 1636,
        ENDING_FOLLOWER_JINE075 = 1637,
        ENDING_FOLLOWER_JINE076 = 1638,
        ENDING_FOLLOWER_JINE077 = 1639,
        ENDING_FOLLOWER_JINE078 = 1640,
        ENDING_FOLLOWER_JINE079 = 1641,
        ENDING_FOLLOWER_JINE080 = 1642,
        ENDING_FOLLOWER_JINE081 = 1643,
        ENDING_FOLLOWER_JINE082 = 1644,
        ENDING_FOLLOWER_JINE083 = 1645,
        ENDING_FOLLOWER_JINE084 = 1646,
        ENDING_FOLLOWER_JINE085 = 1647,
        ENDING_FOLLOWER_JINE086 = 1648,
        ENDING_FOLLOWER_JINE087 = 1649,
        ENDING_FOLLOWER_JINE088 = 1650,
        ENDING_FOLLOWER_JINE089 = 1651,
        ENDING_FOLLOWER_JINE090 = 1652,
        ENDING_FOLLOWER_JINE091 = 1653,
        ENDING_FOLLOWER_JINE092 = 1654,
        ENDING_FOLLOWER_JINE093 = 1655,
        ENDING_FOLLOWER_JINE094 = 1656,
        ENDING_FOLLOWER_JINE095 = 1657,
        ENDING_FOLLOWER_JINE096 = 1658,
        ENDING_FOLLOWER_JINE097 = 1659,
        ENDING_FOLLOWER_JINE098 = 1660,
        ENDING_FOLLOWER_JINE099 = 1661,
        ENDING_FOLLOWER_JINE100 = 1662,
        ENDING_FOLLOWER_JINE101 = 1663,
        ENDING_FOLLOWER_JINE102 = 1664,
        ENDING_FOLLOWER_JINE103 = 1665,
        ENDING_FOLLOWER_JINE104 = 1666,
        ENDING_FOLLOWER_JINE105 = 1667,
        ENDING_FOLLOWER_JINE106 = 1668,
        ENDING_FOLLOWER_JINE107 = 1669,
        ENDING_FOLLOWER_JINE108 = 1670,
        ENDING_FOLLOWER_JINE109 = 1671,
        ENDING_FOLLOWER_JINE110 = 1672,
        ENDING_FOLLOWER_JINE111 = 1673,
        ENDING_FOLLOWER_JINE112 = 1674,
        ENDING_FOLLOWER_JINE113 = 1675,
        ENDING_FOLLOWER_JINE114 = 1676,
        ENDING_FOLLOWER_JINE115 = 1677,
        ENDING_FOLLOWER_JINE116 = 1678,
        ENDING_FOLLOWER_JINE117 = 1679,
        ENDING_FOLLOWER_JINE118 = 1680,
        ENDING_FOLLOWER_JINE119 = 1681,
        ENDING_FOLLOWER_JINE120 = 1682,
        ENDING_FOLLOWER_JINE121 = 1683,
        ENDING_FOLLOWER_JINE122 = 1684,
        ENDING_FOLLOWER_JINE123 = 1685,
        ENDING_FOLLOWER_JINE124 = 1686,
        ENDING_FOLLOWER_JINE125 = 1687,
        ENDING_FOLLOWER_JINE126 = 1688,
        ENDING_FOLLOWER_JINE127 = 1689,
        ENDING_FOLLOWER_JINE128 = 1690,
        ENDING_FOLLOWER_JINE129 = 1691,
        ENDING_FOLLOWER_JINE130 = 1692,
        ENDING_FOLLOWER_JINE131 = 1693,
        ENDING_FOLLOWER_JINE132 = 1694,
        ENDING_FOLLOWER_JINE133 = 1695,
        ENDING_FOLLOWER_JINE134 = 1696,
        ENDING_FOLLOWER_JINE135 = 1697,
        ENDING_FOLLOWER_JINE136 = 1698,
        ENDING_FOLLOWER_JINE137 = 1699,
        ENDING_FOLLOWER_JINE138 = 1700,
        ENDING_FOLLOWER_JINE139 = 1701,
        ENDING_FOLLOWER_JINE140 = 1702,
        ENDING_FOLLOWER_JINE141 = 1703,
        ENDING_FOLLOWER_JINE142 = 1704,
        ENDING_FOLLOWER_JINE143 = 1705,
        ENDING_FOLLOWER_JINE144 = 1706,
        ENDING_FOLLOWER_JINE145 = 1707,
        ENDING_FOLLOWER_JINE146 = 1708,
        ENDING_FOLLOWER_JINE147 = 1709,
        ENDING_FOLLOWER_JINE148 = 1710,
        ENDING_FOLLOWER_JINE149 = 1711,
        ENDING_FOLLOWER_JINE150 = 1712,
        ENDING_FOLLOWER_JINE151 = 1713,
        ENDING_FOLLOWER_JINE152 = 1714,
        ENDING_FOLLOWER_JINE153 = 1715,
        ENDING_FOLLOWER_JINE154 = 1716,
        ENDING_FOLLOWER_JINE155 = 1717,
        ENDING_FOLLOWER_JINE156 = 1718,
        ENDING_FOLLOWER_JINE157 = 1719,
        ENDING_FOLLOWER_JINE158 = 1720,
        ENDING_FOLLOWER_JINE159 = 1721,
        ENDING_FOLLOWER_JINE160 = 1722,
        ENDING_FOLLOWER_JINE161 = 1723,
        ENDING_FOLLOWER_JINE162 = 1724,
        ENDING_FOLLOWER_JINE_DELETEPI = 1725,
        ENDING_FOLLOWER_JINE163 = 1726,
        ENDING_FOLLOWER_JINE164 = 1727,
    }

    public enum ModdedActionType
    {
        OdekakeTokyo = 43,
        HaishinComiket = 44,
        HaishinAngelWatch = 45,
        HaishinAngelDeath = 46,
        HaishinAngelFuneral = 47,
        OkusuriDaypassStalk = 48,
        OdekakePanic1 = 49,
        OdekakePanic2 = 50,
        OdekakePanic3 = 51,
        OdekakePanic4 = 52,
        OdekakeBreak = 53,
        Internet2chStalk = 54,
    }

    public enum ModdedCmdType
    {
        OdekakeTokyo = 130,
        HaishinComiket = 131,
        HaishinAngelWatch = 132,
        HaishinAngelDeath = 133,
        HaishinAngelFuneral = 134,
        OkusuriDaypassStalk = 135,
        OdekakePanic1 = 136,
        OdekakePanic2 = 137,
        OdekakePanic3 = 138,
        OdekakePanic4 = 139,
        OdekakeBreak = 140,
        Internet2chStalk = 141,
    }

    public enum ModdedCommandType
    {
        OdekakeTokyo = 1000,
        OkusuriStalk = 1001,
        Internet2chStalk = 1002
    }

    public enum ModdedSystemTextType
    {
        System_AMARead = 1000,
        Login_BadPassword = 1001,
        System_InternetYamero = 1002,
    }

    public enum ModdedSuperchatType
    {
        AMA_START = 8,
        AMA_END = 9,
        AMA_White = 10,
        AMA_Blue = 11,
        AMA_Cyan = 12,
        AMA_LightGreen = 13,
        AMA_Yellow = 14,
        AMA_Orange = 15,
        AMA_Magenta = 16,
        AMA_Red = 17,
        EVENT_TIMEPASS = 18,
        JINE_INIT = 19,
        JINE_DESTROY = 20,
        JINE_SEND = 21,
        JINE_CHOICE = 22,
        JINE_WAIT = 23,
        EVENT_CREATEDEPAZ = 24,
        EVENT_DOSE = 25,
        EVENT_DELAYFRAME = 26,
        EVENT_MUSICCHANGE = 27,
        EVENT_SHADER = 28,
        EVENT_SHADERWAIT = 29,
        EVENT_MAINPANELCOLOR = 30,
    }

    public enum ModdedTweetType
    {
        TOKYO_TWEET001 = 2001,
        TOKYO_TWEET002 = 2002,
        DARKNIGHT_TWEET001 = 2003,
        DARKNIGHT_TWEET002 = 2004,
        DARKNIGHT_TWEET003 = 2005,
        DARKNIGHT_TWEET004 = 2006,
        ODEKAKEPANIC1_TWEET001 = 2007,
        ODEKAKEPANIC2_TWEET001 = 2008,
        ODEKAKEPANIC3_TWEET001 = 2009,
        ODEKAKEPANIC4_TWEET001 = 2010,
        STALKDISCOVER_TWEET001 = 2011,
        STALKDISCOVER_TWEET002 = 2012,
        STALKODEKAKE1_TWEET001 = 2013,
        STALKODEKAKE1_TWEET002 = 2014,
        STALKODEKAKE1_TWEET003 = 2015,
        STALKODEKAKE2_TWEET001 = 2016,
        STALKODEKAKE2_TWEET002 = 2017,
        STALKODEKAKE2_TWEET003 = 2018,
        PREANGELWATCH_TWEET001 = 2019,
        PREANGELWATCH_TWEET002 = 2020,
        POSTANGELWATCH_TWEET001 = 2021,
        POSTANGELWATCH_TWEET002 = 2022,
        POSTANGELWATCH_TWEET003 = 2023,
        POSTANGELWATCH_TWEET004 = 2024,
        POSTANGELWATCH_TWEET005 = 2025,
        BADPASSWORD_TWEET001 = 2026,
        BADPASSWORD_TWEET002 = 2027,
        DEADKANGEL_TWEET001 = 2028,
        DEADKANGEL_TWEET002 = 2029,
        DEADKANGEL_TWEET003 = 2030,
        ANGELFUNERAL_TWEET001 = 2031,
        ANGELFUNERAL_TWEET002 = 2032,
    }

    public enum ModdedAlphaType
    {
        FollowerAlpha = 13
    }

    public enum ModdedTenCommentType
    {
        FollowerAlpha1_STREAMNAME = 10000,
        FollowerAlpha1_STREAM001 = 10001,
        FollowerAlpha1_STREAM002 = 10002,
        FollowerAlpha1_STREAM003 = 10003,
        FollowerAlpha1_STREAM004 = 10004,
        FollowerAlpha1_STREAM005 = 10005,
        FollowerAlpha1_STREAM006 = 10006,
        FollowerAlpha1_STREAM007 = 10007,
        FollowerAlpha1_STREAM008 = 10008,
        FollowerAlpha1_STREAM009 = 10009,
        FollowerAlpha1_STREAM010 = 10010,
        FollowerAlpha1_STREAM011 = 10011,
        FollowerAlpha1_STREAM012 = 10012,
        FollowerAlpha1_STREAM013 = 10013,
        FollowerAlpha1_RESPONSE001 = 10014,
        FollowerAlpha1_RESPONSE002 = 10015,
        FollowerAlpha1_RESPONSE003 = 10016,
        FollowerAlpha1_RESPONSE004 = 10017,
        FollowerAlpha1_RESPONSE005 = 10018,
        FollowerAlpha2_STREAMNAME = 10019,
        FollowerAlpha2_INTRO001 = 10020,
        FollowerAlpha2_INTRO002 = 10021,
        FollowerAlpha2_INTRO003 = 10022,
        FollowerAlpha2_INTRO004 = 10023,
        FollowerAlpha2_INTRO005 = 10024,
        FollowerAlpha2_INTRO006 = 10025,
        FollowerAlpha2_AMA_Q001_RESPONSE = 10026,
        FollowerAlpha2_AMA_Q002_RESPONSE = 10027,
        FollowerAlpha2_AMA_Q003_RESPONSE = 10028,
        FollowerAlpha2_AMA_Q004_RESPONSE = 10029,
        FollowerAlpha2_AMA_Q005_RESPONSE = 10030,
        FollowerAlpha2_AMA_Q005_RESPONSE_B = 10031,
        FollowerAlpha2_AMA_Q006_RESPONSE = 10032,
        FollowerAlpha2_AMA_Q007_RESPONSE = 10033,
        FollowerAlpha2_AMA_Q007_RESPONSE_B = 10034,
        FollowerAlpha2_AMA_Q008_RESPONSE = 10035,
        FollowerAlpha2_AMA_Q008_RESPONSE_B = 10036,
        FollowerAlpha2_AMA_Q009_RESPONSE = 10037,
        FollowerAlpha2_AMA_Q010_RESPONSE = 10038,
        FollowerAlpha2_AMA_Q010_RESPONSE_B = 10039,
        FollowerAlpha2_AMA_Q011_RESPONSE = 10040,
        FollowerAlpha2_AMA_Q012_RESPONSE = 10041,
        FollowerAlpha2_AMA_Q013_RESPONSE = 10042,
        FollowerAlpha2_AMA_Q013_RESPONSE_B = 10043,
        FollowerAlpha2_AMA_Q014_RESPONSE = 10044,
        FollowerAlpha2_AMA_Q015_RESPONSE = 10045,
        FollowerAlpha2_AMA_Q015_RESPONSE_B = 10046,
        FollowerAlpha2_AMA_Q016_RESPONSE = 10047,
        FollowerAlpha2_AMA_Q016_RESPONSE_B = 10048,
        FollowerAlpha2_AMA_Q017_RESPONSE = 10049,
        FollowerAlpha2_AMA_Q018_RESPONSE = 10050,
        FollowerAlpha2_AMA_Q018_RESPONSE_B = 10051,
        FollowerAlpha2_AMA_Q019_RESPONSE = 10052,
        FollowerAlpha2_AMA_Q020_RESPONSE = 10053,
        FollowerAlpha2_AMA_Q020_RESPONSE_B = 10054,
        FollowerAlpha2_AMA_Q021_RESPONSE = 10055,
        FollowerAlpha2_AMA_Q022_RESPONSE = 10056,
        FollowerAlpha2_AMA_Q023_RESPONSE = 10057,
        FollowerAlpha2_AMA_Q023_RESPONSE_B = 10058,
        FollowerAlpha2_AMA_Q024_RESPONSE = 10059,
        FollowerAlpha2_AMA_Q024_RESPONSE_B = 10060,
        FollowerAlpha2_AMA_Q025_RESPONSE = 10061,
        FollowerAlpha2_AMA_Q026_RESPONSE = 10062,
        FollowerAlpha2_AMA_Q026_RESPONSE_B = 10063,
        FollowerAlpha2_AMA_Q027_RESPONSE = 10064,
        FollowerAlpha2_AMA_Q027_RESPONSE_B = 10065,
        FollowerAlpha2_AMA_Q028_RESPONSE = 10066,
        FollowerAlpha2_AMA_Q029_RESPONSE = 10067,
        FollowerAlpha2_AMA_Q030_RESPONSE = 10068,
        FollowerAlpha2_AMA_Q031_RESPONSE = 10069,
        FollowerAlpha2_AMAFINISH_BAD001 = 10070,
        FollowerAlpha2_AMAFINISH_BAD002 = 10071,
        FollowerAlpha2_AMAFINISH_BAD003 = 10072,
        FollowerAlpha2_AMAFINISH_GOOD001 = 10073,
        FollowerAlpha2_AMAFINISH_GOOD002 = 10074,
        FollowerAlpha2_AMAFINISH_GOOD003 = 10075,
        FollowerAlpha2_AMAFINISH_IGNORE001 = 10076,
        FollowerAlpha2_AMAFINISH_IGNORE002 = 10077,
        FollowerAlpha2_AMAFINISH_IGNORE003 = 10078,
        FollowerAlpha2_GAMING001 = 10079,
        FollowerAlpha2_GAMING002 = 10080,
        FollowerAlpha2_GAMING003 = 10081,
        FollowerAlpha2_GAMING004 = 10082,
        FollowerAlpha2_GAMING005 = 10083,
        FollowerAlpha2_GAMING006 = 10084,
        FollowerAlpha2_GAMING007 = 10085,
        FollowerAlpha2_GAMING008 = 10086,
        FollowerAlpha2_GAMING009 = 10087,
        FollowerAlpha2_GAMING010 = 10088,
        FollowerAlpha2_GAMING011 = 10089,
        FollowerAlpha2_GAMING012 = 10090,
        FollowerAlpha2_GAMING013 = 10091,
        FollowerAlpha2_GAMING014 = 10092,
        FollowerAlpha2_GAMING015 = 10093,
        FollowerAlpha2_GAMING016 = 10094,
        FollowerAlpha2_GAMING017 = 10095,
        FollowerAlpha2_OVERDOSE001 = 10096,
        FollowerAlpha2_OVERDOSE002 = 10097,
        FollowerAlpha2_OVERDOSE003 = 10098,
        FollowerAlpha2_OVERDOSE004 = 10099,
        FollowerAlpha2_OVERDOSE005 = 10100,
        FollowerAlpha2_OVERDOSE006 = 10101,
        FollowerAlpha2_OVERDOSE007 = 10102,
        FollowerAlpha2_OVERDOSE008 = 10103,
        FollowerAlpha2_OVERDOSE009 = 10104,
        FollowerAlpha2_OVERDOSE010 = 10105,
        FollowerAlpha2_OVERDOSE011 = 10106,
        FollowerAlpha2_OVERDOSE012 = 10107,
        FollowerAlpha2_NIGHTMARE001 = 10108,
        FollowerAlpha2_NIGHTMARE002 = 10109,
        FollowerAlpha2_NIGHTMARE003 = 10110,
        FollowerAlpha2_NIGHTMARE004 = 10111,
        FollowerAlpha2_NIGHTMARE005 = 10112,
        FollowerAlpha2_NIGHTMARE006 = 10113,
        FollowerAlpha2_NIGHTMARE007 = 10114,
        FollowerAlpha2_NIGHTMARE008 = 10115,
        FollowerAlpha2_NIGHTMARE009 = 10116,
        FollowerAlpha2_NIGHTMARE010 = 10117,
        FollowerAlpha2_NIGHTMARE011 = 10118,
        FollowerAlpha3_STREAMNAME = 10119,
        FollowerAlpha3_RECENTEVENTS001 = 10120,
        FollowerAlpha3_RECENTEVENTS002 = 10121,
        FollowerAlpha3_RECENTEVENTS003 = 10122,
        FollowerAlpha3_RECENTEVENTS004 = 10123,
        FollowerAlpha3_RECENTEVENTS005 = 10124,
        FollowerAlpha3_RECENTEVENTS006 = 10125,
        FollowerAlpha3_RECENTEVENTS007 = 10126,
        FollowerAlpha3_RECENTEVENTS008 = 10127,
        FollowerAlpha3_RECENTEVENTS009 = 10128,
        FollowerAlpha3_RECENTEVENTS010 = 10129,
        FollowerAlpha3_RECENTEVENTS011 = 10130,
        FollowerAlpha3_RECENTEVENTS012 = 10131,
        FollowerAlpha3_RECENTEVENTS013 = 10132,
        FollowerAlpha3_RECENTEVENTS014 = 10133,
        FollowerAlpha3_RECENTEVENTS015 = 10134,
        FollowerAlpha3_RECENTEVENTS016 = 10135,
        FollowerAlpha3_RECENTEVENTS017 = 10136,
        FollowerAlpha3_RECENTEVENTS018 = 10137,
        FollowerAlpha3_RECENTEVENTS019 = 10138,
        FollowerAlpha3_RECENTEVENTS020 = 10139,
        FollowerAlpha3_RECENTEVENTS021 = 10140,
        FollowerAlpha3_RECENTEVENTS022 = 10141,
        FollowerAlpha3_RECENTEVENTS023 = 10142,
        FollowerAlpha3_RECENTEVENTS024 = 10143,
        FollowerAlpha3_RECENTEVENTS025 = 10144,
        FollowerAlpha3_RECENTEVENTS026 = 10145,
        FollowerAlpha3_RECENTEVENTS027 = 10146,
        FollowerAlpha3_RECENTEVENTS028 = 10147,
        FollowerAlpha3_RECENTEVENTS029 = 10148,
        FollowerAlpha4_STREAMNAME = 10149,
        FollowerAlpha4_FAULT001 = 10150,
        FollowerAlpha4_FAULT002 = 10151,
        FollowerAlpha4_FAULT003 = 10152,
        FollowerAlpha4_FAULT004 = 10153,
        FollowerAlpha4_FAULT005 = 10154,
        FollowerAlpha4_FAULT006 = 10155,
        FollowerAlpha4_FAULT007 = 10156,
        FollowerAlpha4_FAULT008 = 10157,
        FollowerAlpha4_FAULT009 = 10158,
        FollowerAlpha4_FAULT010 = 10159,
        FollowerAlpha4_FAULT011 = 10160,
        FollowerAlpha4_FAULT012 = 10161,
        FollowerAlpha4_FAULT013 = 10162,
        FollowerAlpha4_FAULT014 = 10163,
        FollowerAlpha4_FAULT015 = 10164,
        FollowerAlpha4_FAULT016 = 10165,
        FollowerAlpha4_FAULT017 = 10166,
        FollowerAlpha4_FAULT018 = 10167,
        FollowerAlpha4_FAULT019 = 10168,
        FollowerAlpha4_FAULT020 = 10169,
        FollowerAlpha4_FAULT021 = 10170,
        FollowerAlpha4_FAULT022 = 10171,
        FollowerAlpha4_FAULT023 = 10172,
    }

    public enum ModdedAppType
    {
        PillDaypass_Follower = 91,
        Login_Hacked = 92,
        Follower_taiki = 93,
    }

    public enum ModdedEffectType
    {
        Vengeful = 23
    }
}

