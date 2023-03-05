using JX3CalculatorShared.Class;
using JX3CalculatorShared.Data;
using TLCalculator.DB;
using TLCalculator.Globals;


namespace TLCalculator.Data
{
    // 心法数据库
    public static class StaticXFData
    {
        public static readonly XFDataLoader Data;
        public static readonly XFDataBase DB;

        static StaticXFData()
        {
            Data = new XFDataLoader(XFAppStatic.DATA_PATH,
                XFAppStatic.OUTPUT_PATH,
                XFAppStatic.ZHENFA_PATH,
                autoLoad: true);
            DB = new XFDataBase();
        }

        public static Recipe GetRecipe(string name) => DB.Recipe[name];

        public static SkillModifier GetSkillModifier(string name) => DB.SkillModifier[name];

        public static Buff GetExtraTriggerBuff(string name) => DB.Buff.Buff_ExtraTrigger[name];
    }
}