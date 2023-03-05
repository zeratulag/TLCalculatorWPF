using TLCalculator.Src;

namespace TLCalculator.Models
{
    public readonly struct EventTriggerArg
    {

        public readonly bool SL; // 是否有神力
        public readonly bool SY; // 是否有伤腰大附魔

        public readonly bool BigFM_SHOES_120; // 是否有120级的伤鞋大附魔
        public readonly bool BigXW; // 是否为大心无

        public readonly int LongMen; // 龙门飞剑等级
        public readonly int AbilityRank; // 技能水平等级

        public readonly double PiaoHuangCover; // 飘黄Buff覆盖率


        public readonly QiXueArg QiXue;
        public readonly BoLiangFenXingArg BoLiang;
        

        public EventTriggerArg(bool sl, bool sy,
            bool bigFM_SHOES_120, bool bigXW,
            int longMen,
            int abilityRank,
            double piaoHuangCover,
            QiXueArg qiXue, BoLiangFenXingArg boLiang)
        {
            SL = sl;
            SY = sy;
            BigFM_SHOES_120 = bigFM_SHOES_120;
            BigXW = bigXW;
            LongMen = longMen;
            AbilityRank = abilityRank;
            PiaoHuangCover = piaoHuangCover;
            QiXue = qiXue;
            BoLiang = boLiang;
        }

        public EventTriggerArg(DPSCalcShellArg arg)
        {
            SY = arg.BigFM.Belt != null; // 是否有伤腰大附魔
            BigFM_SHOES_120 = arg.BigFM.Shoes?.DLCLevel == 120; // 是否有120级伤鞋子大附魔
            SL = arg.SL;
            BigXW = arg.BigXW;
            LongMen = arg.LongMen;
            AbilityRank = arg.AbilityRank;
            PiaoHuangCover = arg.BuffSpecial.PiaoHuangCover;
            QiXue = arg.QiXue;
            BoLiang = arg.BoLiang;
        }

    }


    public readonly struct BoLiangFenXingArg
    {
        public readonly bool Activated; // 是否有此奇穴
        public readonly double Under70; // “神机值”低于70点时，造成内功伤害增加20% 覆盖率
        public readonly double Above30; // “神机值”高于30点时，“陷阱技”造成伤害增加10%。 覆盖率

        public BoLiangFenXingArg(bool activated = false, double under70 = 0, double above30 = 0)
        {
            Activated = activated;
            Under70 = under70;
            Above30 = above30;
        }
    }

}