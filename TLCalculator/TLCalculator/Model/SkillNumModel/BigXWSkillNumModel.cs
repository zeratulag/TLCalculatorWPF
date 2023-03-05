using JX3CalculatorShared.Models;
using JX3CalculatorShared.Utils;
using System;
using TLCalculator.Class;
using TLCalculator.Data;
using TLCalculator.Globals;
using TLCalculator.Src;

namespace TLCalculator.Models
{
    /// <summary>
    /// 大心无专用
    /// </summary>
    public class BigXWSkillNumModel : CommonSkillNumModel
    {

        public RestNum Rest; // 剩余技能数，表示一次爆发中没打完，留给常规阶段的技能次数

        public BigXWSkillNumModel(QiXueConfigModel qixue, SkillHasteTable skillhaste,
            AbilitySkillNumItem abilityitem,
            EquipOptionConfigModel equip, BigFMConfigModel bigfm,
            SkillNumModelArg arg) : base(qixue, skillhaste, abilityitem, equip, bigfm, arg)
        {
            if (!IsXW)
            {
                throw new ArgumentException("必须为心无状态下！");
            }
            TJPerTJCast = QiXue.TJPerCast + IsBigXW.ToInt();

            Rest = new RestNum();
        }

        public new void Calc(TLSkillCountItem num)
        {
            CommonCalcBefore();
            CalcGuiFuBullets();
            GetPeriodSkillNum();
            CommonCalcAfter(num);
        }

        public void Calc()
        {
            Calc(Num);
        }

        // 计算鬼斧子弹数
        public void CalcGuiFuBullets()
        {
            Num.LN_GF = 0;

            double bullets = 15 + 9 * QiXue.雷甲三铉.ToInt(); // 雷甲+3发子弹
            var res = GetGuiFuBullets(bullets, Num._LNTotal);

            Num.LN_GF = res.LN_GF; // 强化连弩数
            Num.LN = res.LN; // 常规连弩数
            Rest.FinalBullets = res.RestBullet; // 剩余子弹数
        }

        /// <summary>
        /// 计算大心无期间天绝次数
        /// </summary>
        /// <param name="TJCast">天绝释放次数</param>
        /// <returns>最终天绝次数，留到常规时段的最终次数</returns>
        public (double TJNum, double RestNormalNum) GetBigXWTJ(double TJCast)
        {
            var TJNum = GetTJNum(TJCast);
            var outTJNum = 0; // 表示爆发期没跳完留到非爆发期的原始天绝数
            if (QiXue.积重难返)
            {
                outTJNum = 2;
            }

            TJNum -= outTJNum;
            var NormalFinalNum = outTJNum;
            return (TJNum, NormalFinalNum);
        }

        /// <summary>
        /// 计算大心无期间化血次数（双暴雨模型）
        /// </summary>
        /// <param name="JD"></param>
        /// <param name="BY"></param>
        /// <returns></returns>
        public double GetBigXWHX2(double JD, double BY)
        {
            var HXRes = SJHX.GetHXNum(Num.QJBNum, JD, BY, Time);
            double HX = HXRes.Total;
            double res = Math.Min(16, HX) - 2; // 修正次数
            Num.HX = res;
            Rest.HX_XW = 8 + 2 * AbilityRank; // 心无杨威快照化血数
            return res;
        }

        /// <summary>
        /// 计算大心无期间化血次数（三暴雨模型），此时先打天女再打第三个暴雨
        /// </summary>
        /// <param name="JD"></param>
        /// <param name="BY"></param>
        /// <returns></returns>
        public double GetBigXWHX3(double JD, double BY)
        {
            var time2 = SkillHaste.BY.XWTime + SkillHaste.GCD.XWTime * 2;
            var time1 = Time - time2 - SkillHaste.GCD.XWTime;
            var BY1 = 2 * QiXue.BYPerCast;
            var HXRes1 = SJHX.GetHXNum(Num.QJBNum * time1 / Time, JD - 1, BY1, time1);
            var HXRes2 = SJHX.GetHXNum(Num.QJBNum * time2 / Time, 1, BY - BY1, time2);
            var NumRes1 = Math.Min(16, HXRes1.Total) - 1;
            var NumRes2 = Math.Min(16, HXRes2.Total);
            Num.HX = NumRes1 + NumRes2;
            var penalty = (3 - AbilityRank) * 2; // 模拟手法带来的快照惩罚
            Rest.HX_XW = Math.Max(16 - NumRes2 - penalty - 2, 0);
            return Num.HX;
        }

        public override void GetPeriodSkillNum()
        {
            if (积重诡鉴)
            {
                CalcGJBigXWSkillNum();
            }
            else
            {
                CalcBigXWSkillNum();
            }
        }

        public override void CalcHX(TLSkillCountItem num)
        {
            if (num._BYCast < 3)
            {
                // 双暴雨模型
                num.HX = GetBigXWHX2(num.JD, num.BY);

            }
            else
            {
                // 三暴雨模型
                num.HX = GetBigXWHX3(num.JD, num.BY);
            }
        }


        public void CalcBigXWSkillNum()
        {
            double JDGCD = Num.JD; // 打鸡蛋用的GCD数
            double QQCast = 0; // 千秋开启次数
            if (QiXue.千秋万劫)
            {
                QQCast += 1;
                JDGCD -= 1;
                Num.QQ = QQCast * QQPerCast;
            }

            Num._QQCast = QQCast;

            if (QiXue.积重难返)
            {
                Num._TJCast += 2;
                JDGCD -= 2;
            }

            Num.JD = JDGCD * SkillHaste.GCD.XWTime / SkillHaste.JD.XWTime;

            Num.UpdateBYNum(QiXue.BYPerCast);

            var TJRes = GetBigXWTJ(Num._TJCast);
            double TJ = TJRes.TJNum;
            Num.TJ = TJ;
            Rest.TJ = TJRes.RestNormalNum;

            

            if (QiXue.天风汲雨)
            {
                var TJ_TF = TJPerTJCast * Num._TJCast - 1;
                if (QiXue.积重难返)
                {
                    Rest.TJ_TF = Rest.TJ;
                    TJ_TF -= Rest.TJ_TF;
                }

                Num.TJ_TF = TJ_TF;
            }

        }


        #region 积重诡鉴

        public void CalcGJBigXWSkillNum()
        {
            // 计算诡鉴积重流大心无期间的技能数
            CalcGJTJ();
        }

        protected void CalcGJTJ()
        {
            // 计算天绝数

            var (TJTJ, GJTJ) = GetGJTJ();

            var AllTJ = GJTJ + TJTJ; // 天绝总跳数
            var AllTF = AllTJ - Num._TJRound; // 天风天绝总条数

            Num.TJ_TF = Num.TJ - 1;

            Rest.TJ = AllTJ - Num.TJ;
            Rest.TJ_TF = AllTF - Num.TJ_TF;
        }

        #endregion


    }

    // 剩余次数
    public struct RestNum
    {
        public double FinalBullets; // 剩余的强化子弹数
        public double TJ; // 剩余天绝次数
        public double TJ_TF; // 剩余天风天绝数
        public double HX_XW; // 心无快照化血数
    }
}