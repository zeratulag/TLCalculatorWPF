using JX3CalculatorShared.Models;
using JX3CalculatorShared.Utils;
using System;
using TLCalculator.Class;
using TLCalculator.Data;
using TLCalculator.Src;

namespace TLCalculator.Models
{
    public class NormalSkillNumModel : CommonSkillNumModel
    {
        #region 成员

        public RestNum Rest; // 剩余技能数，表示一次爆发中没打完，留给常规阶段的技能次数
        public const double TIME_240_COEF = 90.5 / 240.0;
        public const double TIME_75_240_COEF = 75.0 / 240.0;

        #endregion

        #region 构造

        public NormalSkillNumModel(QiXueConfigModel qixue, SkillHasteTable skillhaste,
            AbilitySkillNumItem abilityitem,
            EquipOptionConfigModel equip, BigFMConfigModel bigfm,
            SkillNumModelArg arg) :
            base(qixue, skillhaste, abilityitem, equip, bigfm, arg)
        {
            Rest = new RestNum();
            QQPerCast = 3.1 + 0.25 * AbilityRank;
            if (arg.ShortTimeBonus > 0)
            {
                Num.ApplyShortTimeBonus(arg.ShortTimeBonus); // 天罗短时间战斗红利
            }

            if (arg.HasZhen)
            {
                Num.AddJDNumByFreq(10.0 / 195.0); // 有阵4分钟多10个鸡蛋
            }
        }

        public NormalSkillNumModel(QiXueConfigModel qixue, SkillHasteTable skillhaste, AbilitySkillNumItem abilityitem,
            EquipOptionConfigModel equip,
            BigFMConfigModel bigfm,
            SkillNumModelArg arg,
            RestNum rest) : this(qixue, skillhaste, abilityitem, equip, bigfm, arg)
        {
            Rest = rest;
        }

        #endregion

        public (double TJCast, double TJ) GetNormalTJ()
        {
            var TJCast = Num._TJCast * 15 / 20; // 目前天绝cd20秒，数据是15秒 [TODO] 等稳定后修改掉
            TJCast -= TIME_75_240_COEF; // 心无期间留天绝修正（确保最终天绝数量4分钟不超过108）
            var BaseNum = GetTJNum(TJCast);
            return (TJCast, BaseNum + Rest.TJ);
        }

        public GuiFuRes GetGuiFuBullets(RestNum rest)
        {
            var res = GetGuiFuBullets(rest.FinalBullets, Num._LNTotal);
            return res;
        }

        public void CalcGuiFuBullets()
        {
            var res = GetGuiFuBullets(Rest);
            Num.LN = res.LN;
            Num.LN_GF = res.LN_GF;
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


        public override void GetPeriodSkillNum()
        {
            if (积重诡鉴)
            {
                CalcGJNormalSkillNum();
            }
            else
            {
                CalcNormalSkillNum();
            }
        }


        /// <summary>
        /// 计算常规状态下的技能数
        /// </summary>
        public void CalcNormalSkillNum()
        {
            double JDGCD = Num.JD; // 打鸡蛋用的GCD数
            double QQCast = 0; // 千秋开启次数
            if (QiXue.千秋万劫)
            {
                QQCast += 2;
                Num.QQ = QQCast * QQPerCast;
            }

            Num._QQCast = QQCast;

            var TJRes = GetNormalTJ();
            var newTJCast = TJRes.TJCast;
            JDGCD += Num._TJCast - newTJCast;
            Num._TJCast = newTJCast;
            Num.TJ = TJRes.TJ;

            if (QiXue.天风汲雨)
            {
                Num.TJ_TF = Num._TJCast * (QiXue.TJPerCast - 1) + Rest.TJ_TF;
            }

            bool JiZhongLackEnergy = (QiXue.积重难返) && !Arg.HasZhen; // 是否缺神机

            JDGCD += (QiXue.积重难返.ToInt() - JiZhongLackEnergy.ToInt()) * TIME_240_COEF;

            Num.TQ -= 5 * TIME_240_COEF;
            Num._TQCast = Num.TQ / 3;

            var BYTime = SkillHaste.BY.Time; // 完整释放暴雨所需时间
            var BYCast = Num._BYCast;
            BYCast -= ((QiXue.积重难返).ToInt() + (QiXue.积重难返 && !Arg.HasZhen).ToInt()) * TIME_75_240_COEF; // 积重和缺乏神机的影响
            var BYQQCast = (QiXue.千秋万劫).ToInt() * TIME_75_240_COEF; // 千秋影响暴雨释放

            BYCast -= BYQQCast; // 240秒少5发暴雨，由于开鬼斧
            Num._BYCast = BYCast; // 用于释放鬼斧和千秋的补偿

            var BYQQTime = BYQQCast * BYTime; // 由于开千秋导致暴雨少打的时间，将会转换为鸡蛋的时间

            var GCD = SkillHaste.GCD.Time;
            var JDt = SkillHaste.JD.Time;
            var JDTotalTime = JDGCD * GCD; // 可以用来打鸡蛋的时间
            var JDTimeCost = (4 - BYTime * TIME_75_240_COEF) + (QQCast * GCD - BYQQTime); // 用于支付开鬼斧的时间+支付开千秋的时间
            JDTotalTime -= JDTimeCost;

            if (WP.IsBigCW)
            {
                // 如果有大橙武，则每240秒多释放1.6发暴雨
                var extraBYCast = 2.0 * (AbilityRank + 1) / 5 * TIME_75_240_COEF;
                var extraBYCastTime = extraBYCast * BYTime;
                Num._BYCast += extraBYCast;
                JDTotalTime -= extraBYCastTime;
            }

            Num.UpdateBYNum(QiXue.BYPerCast);

            var OriginJD = JDTotalTime / JDt; // 初始鸡蛋数
            var OriginTN = Num.TN - 1;

            Num.HX_XW = Rest.HX_XW;
            var SJHXRes = SolveNormalSJHX(Num.QJBNum, Num.BY, OriginJD, OriginTN, Rest.HX_XW, Time, JDt);
            Num.Update(SJHXRes);

        }


        /// <summary>
        /// 获得常规阶段化血数量（不包括快照化血）
        /// </summary>
        /// <param name="QJB">千机变攻击次数</param>
        /// <param name="BY">鸡蛋数</param>
        /// <param name="JD">暴雨数</param>
        /// <param name="RestHX_XW">从心无期间快照的化血次数</param>
        /// <param name="time">时间（秒）</param>
        /// <returns>化血次数</returns>
        public double GetNormalHX(double QJB, double BY, double JD, double RestHX_XW, double time)
        {
            var res = SJHX.GetHXNum(QJB, JD, BY, time);
            return (res.Total - RestHX_XW);
        }

        // 基于化血模型进行迭代，修正鸡蛋数和天女数
        public (double JD, double TN) UpdateNormalJDTN(double QJB, double BY, double JD, double TN, double RestHX_XW,
            double time, double JDt)
        {
            var HX = GetNormalHX(QJB, BY, JD, RestHX_XW, time); // 计算化血跳数
            var newTN = HX / 14.5; // 为了满足化血数，实际上要补多少天女
            var JDTotalTime = JD * JDt + (TN - newTN); // 如果标准天女比实际多，多出来的部分时间留作打鸡蛋用
            var newJD = JDTotalTime / JDt;
            return (newJD, newTN);
        }

        /// <summary>
        /// 求解常规阶段鸡蛋和天女数量平衡点
        /// </summary>
        /// <param name="QJB">千机变攻击次数</param>
        /// <param name="BY">暴雨数</param>
        /// <param name="JD">初始鸡蛋数</param>
        /// <param name="TN">初始天女数</param>
        /// <param name="RestHX_XW">从心无期间快照的化血次数</param>
        /// <param name="time">时间（秒）</param>
        /// <param name="JDt">鸡蛋读条时间</param>
        /// <returns></returns>
        public NormalSJHXResult SolveNormalSJHX(double QJB, double BY, double JD, double TN, double RestHX_XW,
            double time, double JDt)
        {
            int step = 0;
            double delta;
            do
            {
                (var newJD, var newTN) = UpdateNormalJDTN(QJB, BY, JD, TN, RestHX_XW, time, JDt);
                step++;
                delta = Math.Abs(newTN - TN);
                JD = newJD;
                TN = newTN;
            } while (delta > 0.01);

            var HX = GetNormalHX(QJB, BY, JD, RestHX_XW, time);
            var res = new NormalSJHXResult() { JD = JD, TN = TN, HX = HX, Step = step };
            return res;
        }


        #region 积重诡鉴
        public void CalcGJNormalSkillNum()
        {
            CalcGJTJ();
            CalcHX(Num);
        }

        protected void CalcGJTJ()
        {
            // 计算天绝数

            var (TJTJ, GJTJ) = GetGJTJ();

            var AllTJ = GJTJ + TJTJ; // 天绝总跳数
            var AllTF = AllTJ - Num._TJRound; // 天风天绝总跳数

            Num.TJ = AllTJ + Rest.TJ;
            Num.TJ_TF = AllTF + Rest.TJ_TF;
        }


        public override void CalcHX(TLSkillCountItem num)
        {
            num.HX_XW = Rest.HX_XW;
            var SJHXRes = SolveNormalSJHX(num.QJBNum, num.BY, num.JD, num.TN, Rest.HX_XW, Time, SkillHaste.JD.Time);
            num.Update(SJHXRes);
        }

        #endregion
    }





    public struct NormalSJHXResult
    {
        public double JD; // 鸡蛋数
        public double TN; // 天女数
        public double HX; // 常规化血数
        public int Step; // 迭代步数
    }
}