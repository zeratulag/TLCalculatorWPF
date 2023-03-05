using TLCalculator.Class;

namespace TLCalculator.Src
{
    public partial class DPSKernel
    {
        #region 方法

        public void FixCTProfit(double ct)
        {
            // 修复会心收益
            FinalProfit = RawDeriv.Copy();
            FinalProfit.FixCT(ct, FinalFChars.Normal.Y_Percent);
            FinalProfit.Name = "单点收益";
            FinalProfitDF = new ProfitDF(FinalProfit);
            FinalScoreProfit = FinalProfitDF.ScoreDeriv;
            FinalScoreProfit.GetScoreAttrDerivList();
        }

        #endregion

        #region 方法

        public void CalcCombatStat()
        {
            // 生成战斗统计
            var normal = new CombatStat(DamageFreqDFs.Normal);
            var xw = new CombatStat(DamageFreqDFs.XW);

            var res = CombatStat.Merge(normal, xw);
            res.Proceed();

            FinalCombatStat = res;
            SimpleFinalCombatStat = FinalCombatStat.ToSimple();
        }

        #endregion
    }
}