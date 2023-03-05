using JX3CalculatorShared.Class;
using JX3CalculatorShared.ViewModels;
using TLCalculator.Class;
using TLCalculator.Models;

namespace TLCalculator.Src
{
    public readonly struct DPSCalcShellArg
    {
        public readonly bool SL; // 是否有神力

        public readonly bool BigXW; // 是否为大心无

        public readonly int LongMen; // 龙门飞剑等级

        public readonly int AbilityRank; // 技能水平等级

        public readonly QiXueArg QiXue; // 奇穴选项

        public readonly YZOption YZ; // 腰坠选项

        public readonly BigFMConfigModel BigFM;

        public readonly InitCharacter NoneBigFMInitCharacter; // 不含大附魔的初始人物属性

        public readonly BuffSpecialArg BuffSpecial;

        public readonly BoLiangFenXingArg BoLiang;

        public DPSCalcShellArg(bool bigXw, bool sl, int longMen,
            int abilityRank,
            QiXueArg qiXue, YZOption yz, BigFMConfigModel bigFm,
            BuffSpecialArg buffSpecial,
            InitCharacter ichar, BoLiangFenXingArg boLiang)
        {
            BigXW = bigXw;
            SL = sl;
            LongMen = longMen;
            AbilityRank = abilityRank;
            QiXue = qiXue;
            YZ = yz;
            BigFM = bigFm;
            BuffSpecial = buffSpecial;
            NoneBigFMInitCharacter = ichar;
            BoLiang = boLiang;
        }
    }

    public readonly struct QiXueArg
    {
        public readonly bool 弩击急骤;
        public readonly bool 擘两分星;
        public readonly bool 曙色催寒;
        public readonly bool 杀机断魂;

        public readonly string SkillBaseNumGenre;

        public QiXueArg(bool nx, bool bl, bool ch,  bool sj, string skillBaseNumGenre)
        {
            弩击急骤 = nx;
            擘两分星 = bl;
            曙色催寒 = ch;
            杀机断魂 = sj;
            SkillBaseNumGenre = skillBaseNumGenre;
        }
    }


}