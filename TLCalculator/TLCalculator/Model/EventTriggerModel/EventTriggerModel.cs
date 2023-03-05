using JX3CalculatorShared.Class;
using JX3CalculatorShared.Utils;
using System;
using System.Collections.Generic;
using JX3CalculatorShared.Data;
using TLCalculator.Data;

namespace TLCalculator.Models
{
    /// <summary>
    /// 用于计算特殊触发BUFF的模型
    /// </summary>
    public partial class EventTriggerModel
    {
        #region 成员

        public static readonly Buff NX = StaticXFData.GetExtraTriggerBuff("NuXin"); // 弩心
        public static readonly Buff CH = StaticXFData.GetExtraTriggerBuff("CuiHan"); // 催寒
        public static readonly Buff BL = StaticXFData.GetExtraTriggerBuff("BoLiangFenXing70-"); // 擘两分星70-

        #endregion

        public void Calc()
        {
            BuffCover.Reset();
            CalcSkillCTDF();
            CalcBigFMBelt();
            CalcSLBuff();
            CalcLongMenBuff();
            CalcBL();
            // 需要先计算所有加会心会效的BUFF后再计算弩心催寒

            CalcNXBuff();
            CalcCHBuff();
            BuffedFChars.Normal.Has_Special_Buff = true;
            BuffedFChars.XW.Has_Special_Buff = true;

            CalcBigFM_SHOES_120();
        }

        public void CalcNXBuff()
        {
            // 计算弩心BUFF
            if (!Arg.QiXue.弩击急骤) return;
            var names = new HashSet<string>();

            foreach (var KVP in SkillNum.CTSkillEvents)
            {
                if (KVP.Key.StartsWith("NX_"))
                {
                    names.AddRange(KVP.Value.TriggerSkillNames);
                }
            }

            var normMeanCT = SkillFreqCTDFs.Normal.GetMeanCT(names);
            var xwMeanCt = SkillFreqCTDFs.XW.GetMeanCT(names);

            const string key = nameof(NX);
            var normal = GetNXCover(normMeanCT);
            var xw = GetNXCover(xwMeanCt);
            ApplyCoverBuff(NX, key, normal, xw);
        }

        /// <summary>
        /// 计算弩心覆盖率
        /// </summary>
        /// <returns></returns>
        public double GetNXCover((double SumFreq, double MeanCT) MeanCTRes)
        {
            var res = SkillNum.CTSkillEvents["NX_LN"].BuffCoverRate(MeanCTRes.SumFreq, MeanCTRes.MeanCT);
            return res;
        }


        // 计算催寒BUFF
        public void CalcCHBuff()
        {
            if (!Arg.QiXue.曙色催寒) return;
            const string key = nameof(CH);
            var names = SkillNum.CTSkillEvents[key].TriggerSkillNames;

            var normRes = SkillFreqCTDFs.Normal.GetMeanCT(names).MeanCT;
            var xwRes = SkillFreqCTDFs.XW.GetMeanCT(names).MeanCT;

            var normal = GetCHCover(normRes);
            var xw = GetCHCover(xwRes);

            ApplyCoverBuff(CH, key, normal, xw);
        }

        /// <summary>
        /// 计算催寒覆盖率
        /// </summary>
        /// <param name="MeanCT">可以触发催寒技能的平均会心率</param>
        /// <returns>覆盖率</returns>
        public static double GetCHCover(double MeanCT)
        {
            var res = 1 - Math.Pow(1 - MeanCT, 2);
            return res;
        }


        public void CalcBL()
        {
            // 计算 擘两分星
            if (Arg.QiXue.擘两分星)
            {
                ApplyBL70();
            }
        }

        protected void ApplyBL70()
        {
            // 擘两分星70-效果
            double under70 = Arg.BoLiang.Under70;

            const string bl7 = "BL7";

            BuffCover.SetCover(bl7, under70, under70);

            var bl = BL.Emit(under70, 1);
            AddPeriodBaseBuff(bl, bl);
        }

    }
}