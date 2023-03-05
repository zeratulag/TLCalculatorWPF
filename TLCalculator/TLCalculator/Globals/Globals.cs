using JX3CalculatorShared.Class;
using JX3CalculatorShared.Globals;
using JX3CalculatorShared.Utils;
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using TLCalculator.Class;

// ReSharper disable InconsistentNaming

namespace TLCalculator.Globals
{
    public static class XFAppStatic
    // 存储一些计算器设置常量（发布版本）
    {
        public const string DATA_FOLDER = AppStatic.DATA_FOLDER;
        public const string DATA_PATH = DATA_FOLDER + "TL_Data.xlsx";
        public const string OUTPUT_PATH = DATA_FOLDER + "TL_Output.xlsx";
        public const string ZHENFA_PATH = DATA_FOLDER + "TL_Zhenfa.json";
        public const string SETTING_PATH = DATA_FOLDER + "TL_Setting.json";
        public const string AT_PATH = DATA_FOLDER + "TM_Ats.xlsx";

        public const string XinFa = "天罗诡道";
        public const string XinFaTag = "TL";

        public static readonly string TypePrefix;

        public const string FileExtension = "tld"; // 扩展名
        public static readonly string FileFilter; // 打开方式的Filter;

        public const string GameVersion = "横刀断浪";


        public const string JX3BOXURL = @"https://www.jx3box.com/bps/49354"; // JX3BOX主页
        public const string GitHubURL = @"https://github.com/zeratulag/TLCalculatorWPF"; // GitHub主页
        public const string XFTutorialURL = @"https://www.jx3box.com/bps/46326"; // 当前版本天罗攻略
        public const string TMTutorialURL = @"https://www.jx3box.com/bps/21041"; // 个人作品合集

        public static readonly ImmutableDictionary<string, string> URLDict; // URL 字典，方便Cmd调用

        public static readonly AppMetaInfo CurrentAppMeta;
        public static readonly Version AppVersion;
        public static readonly string MainTitle;
        public static readonly DateTime BuildDateTime; // 构建时间
        public static readonly DateTime LastPatchTime; // 最新技改时间
        public const string LastPatchURL = @"https://jx3.xoyo.com/show-2466-5610-1.html"; // 最新技改内容

        static XFAppStatic()
        {
            LastPatchTime = new DateTime(2023, 02, 20); // 最新技改时间

            AppVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var version = AppVersion.ToString();
            var dateTimestr = ImportTool.ReadAllTextFromResource(AppStatic.BUILD_PATH, Encoding.Default);
            BuildDateTime = DateTime.Parse(dateTimestr);

            CurrentAppMeta =
                new AppMetaInfo(AppVersion, XinFa, GameVersion, StaticConst.CurrentLevel, LastPatchTime);

            MainTitle = $"天罗诡道计算器 Pro（v{version} 公测版）";
            FileFilter = $"计算方案 (*.{FileExtension})|*.{FileExtension}"; // 打开方式的Filter

            var urlB = ImmutableDictionary.CreateBuilder<string, string>();
            urlB.Add("JB", JX3BOXURL);
            urlB.Add("Git", GitHubURL);
            urlB.Add("WB", AppStatic.SinaWBURL);
            urlB.Add("XF", XFTutorialURL);
            urlB.Add("TM", TMTutorialURL);
            URLDict = urlB.ToImmutable();

            switch (XinFaTag)
            {
                // 攻击和破防统一去除前缀
                case "JY":
                    {
                        TypePrefix = "P_";
                        break;
                    }

                case "TL":
                    {
                        TypePrefix = "M_";
                        break;
                    }
            }
        }
    }

    public static class XFConsts
    {
        // 和心法有关的常数

        public const double ChannelIntervalToAPFactor = 12.0; // 内功为12，外功为10
        public const DamageTypeEnum CurrentDamnagType = DamageTypeEnum.Magic; // 当前心法伤害类型

        public const double AP_PER_Y = 0.18; // 元气加基础攻击
        public const double OC_PER_Y = 0.3; // 元气加基础破防
        public const double P_AP_PER_L = 0.15; // 力道加外功攻击
        public const double P_OC_PER_L = 0.3; // 力道加外功破防
        public const double CT_PER_S = 0.64; // 身法加外功会心
        public const double Default_P_AP = 6.0; // 默认满级外功攻击
        public const double Default_P_OC = 12.0; // 默认满级外功破防

        // 唐门_内功_机关

        public const double
            F_AP_PER_Y = 1792.0 / StaticConst.G_KILO; // 元气增加1.75内功AP，SPUNK_TO_POISON_ATTACK_POWER_COF 1792

        public const double CT_PER_Y = 584.0 / StaticConst.G_KILO; // 元气0.57外功会心，SPUNK_TO_PHYSICS_CRITICAL_STRIKE_COF 584

        public const double NPC_Coef = 0.0 / StaticConst.G_KILO; // DST_NPC_DAMAGE_COEFFICIENT


        public static AttrWeight PointWeight = new AttrWeight("单点属性", "提高一点属性增加的DPS")
        { AP = 1, Y = 1, WP = 1, Final_AP = 1, Final_OC = 1, Final_Y = 1 };

        public static AttrWeight ScoreWeight = new AttrWeight("同分属性", "提高等分属性增加的DPS")
        { AP = 1257.0 / 2340.0, Y = 524.0 / 2340.0, WP = 1571.0 / 2340.0 };
    }


    public class XFGlobalParams : GlobalParams
    {
        public readonly double XPZ = 0; // 心法破招系数

        public XFGlobalParams(int level) : base(level)
        {
            XPZ = PZ * (1 + XFStaticConst.XinFaPZCoef);
            Dict.Add(nameof(XPZ), XPZ);
        }
    }

    public static class XFStaticConst
    // 存储一些游戏常量，此类的成员将会被using static 直接访问
    {
        public static readonly XFGlobalParams fGP;
        public const double XinFaPZCoef = (0.6 - 1); // 心法破招系数

        public static readonly Haste CurrentHaste;

        public static class XW
        {
            public const double CT = 1500.0 / 10000.0;
            public const double CF = 300.0 / 1024.0;
            public const int ExtraSP = 204;
            public const double AP_Percent = 512.0 / 1024.0; // 天罗专用
        }

        public static class Genre
        {
            // 流派命名
            public const string 回肠雷甲 = "回肠雷甲";
            public const string 积重诡鉴 = "积重诡鉴";
            public const string 积重千秋 = "积重千秋";
        }

        static XFStaticConst()
        {
            fGP = new XFGlobalParams(StaticConst.CurrentLevel);
            CurrentHaste = new Haste(level: StaticConst.CurrentLevel);
        }
    }


}