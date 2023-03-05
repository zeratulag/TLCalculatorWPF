using JX3CalculatorShared.Data;


namespace TLCalculator.Data
{
    public class AbilitySkillNumItem : AbilitySkillNumItemBase
    {
        public double LN { get; set; }
        public double JD { get; set; }
        public double TN { get; set; }
        public double BY { get; set; }
        public double TJ { get; set; }
        public double TQ { get; set; }
        public double AC { get; set; } // 暗藏释放
        public double QQNumPerCast => 3.1 + Rank * 0.3;
    }

    public class SkillInfoItem : SkillInfoItemBase
    {
        public double M_AP_Coef { get; set; } = 0;
        public double P_AP_Coef { get; set; } = 0;
        public double IgnoreB_M { get; set; } = 0;
        public double IgnoreB_P { get; set; } = 0;

    }

    public class DiamondValueItem : DiamondValueItemBase
    {
        // 五行石镶嵌数值
        public int Y { get; set; }
    }

}