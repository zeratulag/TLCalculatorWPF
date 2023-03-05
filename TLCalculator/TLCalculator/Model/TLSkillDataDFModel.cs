using JX3CalculatorShared.Class;
using JX3CalculatorShared.Common;
using JX3CalculatorShared.Data;
using JX3CalculatorShared.Utils;
using TLCalculator.Class;
using TLCalculator.Data;
using TLCalculator.Src;
using TLCalculator.Utils;

namespace TLCalculator.Models
{
    public class TLSkillDataDFModel: IModel
    {
        /// <summary>
        /// 用于求解天罗特殊奇穴（分星，杀机）加成的类
        /// </summary>
        public readonly Period<SkillDataDF> SkillDFs; // 分为不同阶段的技能数据
        public readonly SkillNumModel SkillNum; // 技能数模型
        public readonly int AbilityRank;
        public readonly QiXueArg QiXue;
        public readonly BoLiangFenXingArg BoLiang;
        public readonly Period<ShaJiDuanHunArg> ShaJiDuanHun; // 杀机断魂参数（区分常规和心无状态）

        public TLCoverDF CoverDF;

        public static readonly Recipe BL_TJ = StaticXFData.GetRecipe("QXTrig_BoLiangFenXing_TianJue"); // 擘两分星30+(天绝)
        public static readonly Recipe BL_TQ = StaticXFData.GetRecipe("QXTrig_BoLiangFenXing_TuQiong"); // 擘两分星30+(图穷)

        public static readonly SkillModifier SJ_TJ4QJB = StaticXFData.GetSkillModifier("QXTrig_ShaJiDuanHun_TJ4QJB"); // 杀机断魂1：每个天绝+10%千机变及破招
        public static readonly SkillModifier SJ_AC4TJ = StaticXFData.GetSkillModifier("QXTrig_ShaJiDuanHun_AC4TJ"); // 杀机断魂2：每个暗藏+20%天绝
        public static readonly SkillModifier SJ_QJB4TQ = StaticXFData.GetSkillModifier("QXTrig_ShaJiDuanHun_QJB4TQ"); // 杀机断魂1：每个千机变+30%图穷

        public TLSkillDataDFModel(Period<SkillDataDF> skillDFs, SkillNumModel skillNum, QiXueArg qixue,
            BoLiangFenXingArg boLiang,
            Period<ShaJiDuanHunArg> shaJiDuanHun)
        {
            SkillDFs = skillDFs;
            QiXue = qixue;
            SkillNum = skillNum;
            AbilityRank = skillNum.XfAbility.Rank;
            ShaJiDuanHun = shaJiDuanHun;

            CoverDF = new TLCoverDF();
            BoLiang = boLiang;
        }

        public void Calc()
        {
            CalcBoLiangFenXing();
            CalcShaJiDuanHun();
        }

        public void CalcBoLiangFenXing()
        {
            // 计算 擘两分星
            if (QiXue.擘两分星)
            {
                ApplyBL30();
            }
        }

        protected void ApplyBL30()
        {
            // 擘两分星30+效果
            double above30 = BoLiang.Above30;
            const string bl3 = "BL3";
            CoverDF.SetCover(bl3, above30, above30);

            var TJ = BL_TJ.Emit(above30);
            var TQ = BL_TQ.Emit(above30);
            SkillDFs.ApplyRecipe(TJ, TJ);
            SkillDFs.ApplyRecipe(TQ, TQ);
        }


        #region 杀机断魂

        public void CalcShaJiDuanHun() 
        {
            if (QiXue.杀机断魂)
            {
                ApplySJ_TJ4QJB();
                ApplySJ_AC4TJ();
                ApplySJ_QJB4TQ();
            }
        }

        /// <summary>
        /// 通用应用杀机断魂效果
        /// </summary>
        /// <param name="sm">技能修饰</param>
        /// <param name="normalCover">常规覆盖率</param>
        /// <param name="xwCover">心无覆盖率</param>
        private void ApplySJModifier(SkillModifier sm, double normalCover, double xwCover)
        {
            var normal = sm.Emit(normalCover);
            var xw = sm.Emit(xwCover);
            var key = sm.Name.RemovePrefix("QXTrig_ShaJiDuanHun_");
            CoverDF.SetCover(key, normalCover, xwCover);
            SkillDFs.ApplySkillModifier(normal, xw);
        }

        protected void ApplySJ_TJ4QJB()
        { // 1:每个天绝+10%千机变
            ApplySJModifier(SJ_TJ4QJB, ShaJiDuanHun.Normal.TJ4QJB, ShaJiDuanHun.XW.TJ4QJB);
        }

        protected void ApplySJ_AC4TJ()
        { // 2:每个暗藏+20%天绝
            ApplySJModifier(SJ_AC4TJ, ShaJiDuanHun.Normal.AC4TJ, ShaJiDuanHun.XW.AC4TJ);
        }

        protected void ApplySJ_QJB4TQ()
        { // 3:每个千机变+30%图穷
            ApplySJModifier(SJ_QJB4TQ, ShaJiDuanHun.Normal.QJB4TQ, ShaJiDuanHun.XW.QJB4TQ);
        }

        #endregion
    }


    public class ShaJiDuanHunArg
    {
        public readonly double TJ4QJB;
        public readonly double AC4TJ;
        public readonly double QJB4TQ;

        public ShaJiDuanHunArg()
        {
            TJ4QJB = 0.0;
            AC4TJ = 0.0;
            QJB4TQ = 0.0;
        }

        public ShaJiDuanHunArg(double tJ4QJB, double aC4TJ, double qJB4TQ)
        {
            TJ4QJB = tJ4QJB;
            AC4TJ = aC4TJ;
            QJB4TQ = qJB4TQ;
        }

    }
}