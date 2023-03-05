using JX3CalculatorShared.Class;
using TLCalculator.Globals;

namespace TLCalculator.Class
{
    public class TLCoverDF : CoverDFBase
    {
        public TLCoverDF(): base()
        {
            // 天罗特殊奇穴增益覆盖率
            if (XFAppStatic.XinFaTag == "TL")
            {
                var BL3 = new CoverItem("BL3", "擘两分星30+");
                var TJ4QJB = new CoverItem("TJ4QJB", "杀机断魂_天绝增益千机变");
                var AC4TJ = new CoverItem("AC4TJ", "杀机断魂_暗藏增益天绝");
                var QJB4TQ = new CoverItem("QJB4TQ", "杀机断魂_千机变增益图穷");

                Data.Add(nameof(BL3), BL3);
                Data.Add(nameof(TJ4QJB), TJ4QJB);
                Data.Add(nameof(AC4TJ), AC4TJ);
                Data.Add(nameof(QJB4TQ), QJB4TQ);
            }
        }
    }
}