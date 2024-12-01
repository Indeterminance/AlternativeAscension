using NeedyEnums;
using ngov3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternativeAscension
{
    internal class ModdedCommandParams
    {
        public static CmdMaster.Param OdekakeAriakeParam = new CmdMaster.Param()
        {
            ParentAct = "OdekakeAriake",
            Id = "OdekakeAriake",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeAriake").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeAriake").Desc.BodyEN,
            PassingTime = 2,
            StressDelta = 5,
            YamiDelta = 10,
        };

        public static CmdMaster.Param OdekakePanic1Param = new CmdMaster.Param()
        {
            ParentAct = "OdekakePanic",
            Id = "OdekakePanic1",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic1").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic1").Desc.BodyEN,
            PassingTime = 2,
            FavorDelta = 4,
            StressDelta = -10,
            YamiDelta = 2,
        };

        public static CmdMaster.Param OdekakePanic2Param = new CmdMaster.Param()
        {
            ParentAct = "OdekakePanic",
            Id = "OdekakePanic2",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic2").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic2").Desc.BodyEN,
            PassingTime = 2,
            FavorDelta = 2,
            StressDelta = -10,
            YamiDelta = 4,
        };

        public static CmdMaster.Param OdekakePanic3Param = new CmdMaster.Param()
        {
            ParentAct = "OdekakePanic",
            Id = "OdekakePanic3",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic3").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic3").Desc.BodyEN,
            PassingTime = 2,
            StressDelta = -10,
            YamiDelta = 6,
        };

        public static CmdMaster.Param OdekakePanic4Param = new CmdMaster.Param()
        {
            ParentAct = "OdekakePanic",
            Id = "OdekakePanic4",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic4").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakePanic4").Desc.BodyEN,
            PassingTime = 2,
            FavorDelta = -3,
            StressDelta = -10,
            YamiDelta = 9,
        };

        public static CmdMaster.Param OdekakeBreakParam = new CmdMaster.Param()
        {
            ParentAct = "OdekakePanic",
            Id = "OdekakeBreak",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeBreak").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeBreak").Desc.BodyEN,
            PassingTime = 2,
            FavorDelta = -15,
            StressDelta = -12,
            YamiDelta = 15,
        };

        public static CmdMaster.Param OdekakeFuneralParam = new CmdMaster.Param()
        {
            ParentAct = "",
            Id = "OdekakeFuneral",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeFuneral").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OdekakeFuneral").Desc.BodyEN,
            PassingTime = 0,
            FavorDelta = 0,
            StressDelta = 0,
            YamiDelta = 0,
        };

        public static CmdMaster.Param Internet2chStalkParam = new CmdMaster.Param()
        {
            ParentAct = "Internet2ch",
            Id = "Internet2chStalk",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "Internet2chStalk").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "Internet2chStalk").Desc.BodyEN,
            PassingTime = 1,
            FavorDelta = -3,
            StressDelta = 8,
            YamiDelta = 6,
        };

        public static CmdMaster.Param HaishinComiketParam = new CmdMaster.Param()
        {
            ParentAct = "Haishin",
            Id = "HaishinComiket",
            LabelEn = AltAscMod.DATA.Streams.Stream.Find(s => s.AlphaType == ModdedAlphaType.FollowerAlpha.ToString() && s.AlphaLevel == "1").Genre,
            DescEn = "",
            FollowerDelta = 30,
            PassingTime = 1,
            FavorDelta = 6,
            StressDelta = 18,
            YamiDelta = 4,
        };

        public static CmdMaster.Param HaishinAngelWatch = new CmdMaster.Param()
        {
            ParentAct = "Haishin",
            Id = "HaishinAngelWatch",
            LabelEn = AltAscMod.DATA.Streams.Stream.Find(s => s.AlphaType == ModdedAlphaType.FollowerAlpha.ToString() && s.AlphaLevel == "2").Name,
            DescEn = "",
            PassingTime = 0,
            FollowerDelta = 0,
            AttentionDelta = 0,
            StressDelta = 0,
            YamiDelta = 0,
        };

        public static CmdMaster.Param HaishinAngelDeath = new CmdMaster.Param()
        {
            ParentAct = "Haishin",
            Id = "HaishinAngelDeath",
            LabelEn = AltAscMod.DATA.Streams.Stream.Find(s => s.AlphaType == ModdedAlphaType.FollowerAlpha.ToString() && s.AlphaLevel == "3").Name,
            DescEn = "",
            PassingTime = 1,
            FollowerDelta = 0,
            AttentionDelta = 0,
            StressDelta = 0,
            YamiDelta = 0,
        };

        public static CmdMaster.Param HaishinAngelFuneral = new CmdMaster.Param()
        {
            ParentAct = "Haishin",
            Id = "HaishinAngelFuneral",
            LabelEn = AltAscMod.DATA.Streams.Stream.Find(s => s.AlphaType == ModdedAlphaType.FollowerAlpha.ToString() && s.AlphaLevel == "4").Name,
            DescEn = "",
            PassingTime = 1,
            FollowerDelta = 0,
            AttentionDelta = 0,
            StressDelta = 0,
            YamiDelta = 0,
        };

        public static CmdMaster.Param OkusuriDaypassStalkParam = new CmdMaster.Param()
        {
            ParentAct = "OkusuriDaypassStalk",
            Id = "OkusuriDaypassStalk",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OkusuriDaypassStalk").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "OkusuriDaypassStalk").Desc.BodyEN,
            PassingTime = 1,
            StressDelta = 10,
            YamiDelta = 1,
        };

        public static CmdMaster.Param SleepToEternityParam = new CmdMaster.Param()
        {
            ParentAct = "SleepToTomorrow",
            Id = "SleepToEternity",
            LabelEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "SleepToEternity").Name.BodyEN,
            DescEn = AltAscMod.DATA.Commands.Command.Find(s => s.Id == "SleepToEternity").Desc.BodyEN,
            PassingTime = 99,
            StressDelta = -120,
            YamiDelta = 999,
        };
    }

    internal class ModdedActionParams
    {
        public static ActMaster.Param OdekakeAriakeParam = new ActMaster.Param()
        {
            Id = "OdekakeAriake",
            TitleEn = "Ariake"
        };

        public static ActMaster.Param OdekakePanic1Param = new ActMaster.Param()
        {
            Id = "OdekakePanic1",
            TitleEn = "Outside"
        };

        public static ActMaster.Param OdekakePanic2Param = new ActMaster.Param()
        {
            Id = "OdekakePanic2",
            TitleEn = "Out THERE"
        };

        public static ActMaster.Param OdekakePanic3Param = new ActMaster.Param()
        {
            Id = "OdekakePanic3",
            TitleEn = "WHY"
        };

        public static ActMaster.Param OdekakePanic4Param = new ActMaster.Param()
        {
            Id = "OdekakePanic4",
            TitleEn = "!?!?!?!?!?!?!"
        };

        public static ActMaster.Param OdekakeBreakParam = new ActMaster.Param()
        {
            Id = "OdekakeBreak",
            TitleEn = "..."
        };

        public static ActMaster.Param OdekakeFuneralParam = new ActMaster.Param()
        {
            Id = "OdekakeFuneral",
            TitleEn = "The end"
        };

        public static ActMaster.Param SleepToEternityParam = new ActMaster.Param()
        {
            Id = "SleepToEternity",
            TitleEn = "Sleep Forever"
        };

    }
}
