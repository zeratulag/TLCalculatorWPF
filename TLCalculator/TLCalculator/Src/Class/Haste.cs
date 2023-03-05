using JX3CalculatorShared.Class;
using JX3CalculatorShared.Common;
using TLCalculator.Globals;
using static JX3CalculatorShared.Globals.StaticConst;

namespace TLCalculator.Class
{
    public class Haste : HasteBase
    {
        public Haste(double fhs) : base(fhs)
        {
        }

        public Haste(int level) : base(level)
        {
        }

        /// <summary>
        /// 获取一段加速下天罗关键技能的标准时间表，用于计算时间利用率
        /// </summary>
        public Period<TLSkillStandardTime> GetTL_SkillStandardTime()
        {
            int t1hsp = GetT1GCD_HSP(); // 一段加速阈值
            double BY = 0.5, HX = 3;
            var GCD_time = SKT(GCD, t1hsp, 0);
            var BY_time = SKT(BY, t1hsp, 0);
            var HX_time = SKT(HX, t1hsp, 0);
            var BigXW_GCD_time = SKT(GCD, t1hsp, XFStaticConst.XW.ExtraSP);
            var BigXW_BY_time = SKT(BY, t1hsp, XFStaticConst.XW.ExtraSP);
            var BigXW_HX_time = SKT(HX, t1hsp, XFStaticConst.XW.ExtraSP);

            var Normal = new TLSkillStandardTime() { GCD = GCD_time, BY = BY_time, HX = HX_time };
            var BigXW = new TLSkillStandardTime() { GCD = BigXW_GCD_time, BY = BigXW_BY_time, HX = BigXW_HX_time };
            var Res = new Period<TLSkillStandardTime>(Normal, BigXW);
            return Res;
        }
    }

    /// <summary>
    /// 用于描述天罗标准技能加速时间的类
    /// </summary>
    public class TLSkillStandardTime
    {
        public double GCD;
        public double BY; // 暴雨单跳时间
        public double HX;
    }
}