using JX3CalculatorShared.Class;
using JX3CalculatorShared.Models;
using JX3CalculatorShared.Utils;
using Syncfusion.UI.Xaml.Charts;
using System;
using Force.DeepCloner;
using TLCalculator.Class;
using TLCalculator.Data;
using TLCalculator.Globals;
using TLCalculator.Src;

namespace TLCalculator.Models
{
    // 分时段模型
    public partial class CommonSkillNumModel
    {
        #region 成员

        public ShiJiHuaXueModel SJHX { get; private set; }

        public readonly TLSkillCountItem Num; // 技能数对象
        public TLSkillCountItem FinalNum; // 最终技能数对象

        //public readonly bool HasZhen;

        public double QQPerCast; // 单次千秋开启能打出的千秋次数
        public double TJPerTJCast; // 单次释放天绝机关的天绝跳数，常规3心无4
        public double TJPerGJCast; // 诡鉴机关单次变形的天绝跳数，常规3心无6
        public double BY_CWPerBY; // 单跳暴雨触发橙武暴雨数
        public double TJInterval; // 两次天绝伤害之间的间隔，= 9 / 跳数

        public bool 积重诡鉴; // 是否为诡鉴流

        public ShaJiDuanHunArg ShaJiArg; // 杀机断魂作用
        public BoLiangFenXingArg BoLiangArg; // 分星作用

        #endregion

        #region 构造

        public CommonSkillNumModel(QiXueConfigModel qixue, SkillHasteTable skillhaste, AbilitySkillNumItem abilityitem,
            EquipOptionConfigModel equip, BigFMConfigModel bigfm,
            SkillNumModelArg arg)
        {
            QiXue = qixue;
            SkillHaste = skillhaste;
            Equip = equip;
            BigFM = bigfm;
            Num = new TLSkillCountItem(abilityitem, qixue);
            Arg = arg;

            积重诡鉴 = (Genre == XFStaticConst.Genre.积重诡鉴);

            QQPerCast = abilityitem.QQNumPerCast;

            AbilityRank = abilityitem.Rank;

            Time = IsXW ? QiXue.XWDuration : QiXue.NormalDuration;
            Num.ResetTime(Time);

            IsBigXW = IsXW && QiXue.聚精凝神;
            XFPostConstructor();
        }

        public void XFPostConstructor()
        {
            TJPerTJCast = QiXue.TJPerCast + IsBigXW.ToInt();
            TJPerGJCast = QiXue.TJPerCast + IsBigXW.ToInt() * 3;
            TJInterval = 9 / TJPerTJCast;
            BY_CWPerBY = (9.75 + 0.75 * AbilityRank) / 5;

            ShiJiHuaXueModelParam param;
            if (IsBigXW)
            {
                param = new ShiJiHuaXueModelParam(SkillHaste.HX.XWIntervalTime,
                    SkillHaste.JD.XWIntervalTime,
                    SkillHaste.BY.XWIntervalTime);
            }
            else
            {
                param = new ShiJiHuaXueModelParam(SkillHaste.HX.IntervalTime,
                    SkillHaste.JD.IntervalTime,
                    SkillHaste.BY.IntervalTime);
            }

            SJHX = new ShiJiHuaXueModel(param);
        }

        #endregion

        #region 计算

        public void CommonCalcBefore()
        {
            // 在其他技能数尚未确定时的通用计算
            CalcLNTotal(Num);
        }

        public void CommonCalcAfter(TLSkillCountItem num)
        {
            // 在其他技能数已经确定时的通用计算
            CalcHX(num);
            CalcXueYingLiuHen(num);
            CalcPZ(num);
            CalcGF(num);
            CalcShaJiDuanHunArg(num);
        }


        public void CalcLNTotal(TLSkillCountItem num)
        {
            if (QiXue.心机天成)
            {
                num.Apply心机天成();
            }
        }

        public double GetTJNum(double TJCast)
        {
            var basenum = TJPerTJCast * TJCast;
            return basenum;
        }

        public void CalcXueYingLiuHen(TLSkillCountItem num)
        {
            if (QiXue.血影留痕)
            {
                num.Apply血影留痕();
            }
        }

        public void CalcGF(TLSkillCountItem num)
        {
            num.CalcGF();
        }

        /// <summary>
        /// 计算强化连弩次数和普通连弩次数，以剩余子弹数（如果有的话）
        /// </summary>
        /// <param name="Bullets">最终鬼斧子弹数</param>
        /// <param name="LNTotal">连弩总次数</param>
        /// <returns></returns>
        public GuiFuRes GetGuiFuBullets(double Bullets, double LNTotal)
        {
            var LN_GF = Math.Min(LNTotal, Bullets); // 享受了鬼斧子弹加成的连弩数
            var LN = Math.Max(0, LNTotal - LN_GF); //  未享受子弹加成的连弩数
            var RestBullet = Math.Max(0, Bullets - LN_GF); // 剩余子弹数
            var res = new GuiFuRes() {LN = LN, LN_GF = LN_GF, RestBullet = RestBullet};
            return res;
        }

        /// <summary>
        /// 计算触发事件
        /// </summary>
        public void CalcEvents()
        {
            GetBasicSkillFreq();
            GetBasicEventHitFreq();
            GetBasicSLCover();
            FinalNum = Num;
            FinalSkillFreq = BasicSkillFreq;
            GetBigCWFreq();
            GetBigFMFreq();
            GetPiaoHuangFreq();
            GetLMJF();
        }


        public void GetBigCWFreq()
        {
            // 计算橙武技能频率
            if (!WP.IsBigCW) return;

            FinalNum = Num.ShallowClone();

            const string keyBY = "CW_BY";
            var CWInterval = SkillEvents[keyBY].MeanTriggerInterval(BasicEventsHitFreq[keyBY]); // 平均触发间隔

            double JDt;
            double BYt;
            if (IsBigXW)
            {
                JDt = SkillHaste.JD.XWTime;
                BYt = SkillHaste.BY.XWTime;
            }
            else
            {
                JDt = SkillHaste.JD.Time;
                BYt = SkillHaste.BY.Time;
            }

            const string JDkey = "JD";
            const string BYKey = "BY";

            const double extraBYCastTotal = 2.0; // 一定要打出2个暴雨
            var OrgBYFreq = BasicSkillFreq["_BYCast"];
            var time = SkillEvents[keyBY].Time; // 6秒内本来应该打的暴雨数
            var extraBYCast = (extraBYCastTotal - OrgBYFreq * time) / (1 - BYt * OrgBYFreq); // 需要支出的额外暴雨数

            var Total_Interval_Time = CWInterval + extraBYCast * BYt; // 触发之后打2发暴雨
            var After_BY_Cast_Freq = (OrgBYFreq * CWInterval + extraBYCast) / Total_Interval_Time; // 修正后的暴雨频率
            var Before_JD_Time = BasicSkillFreq[JDkey] * Total_Interval_Time * JDt; // 在这段时间内，打鸡蛋用的时间
            var After_JD_Time = Before_JD_Time - extraBYCast * BYt; // 打暴雨所用时间全由鸡蛋支付
            var After_JD_Freq = After_JD_Time / Before_JD_Time * BasicSkillFreq[JDkey]; // 修正后的鸡蛋频率

            FinalNum._BYCast = FinalNum.GetNumByFreq(After_BY_Cast_Freq);
            FinalNum.UpdateBYNum(QiXue.BYPerCast);

            FinalNum.JD = FinalNum.GetNumByFreq(After_JD_Freq);
            var bounsJD = 7.0;
            if (Arg.HasZhen) bounsJD -= 1.0;
            FinalNum.AddJDNumByFreq(bounsJD / 195.0);

            var CW_BY_Num = (extraBYCastTotal * 2 * QiXue.BYPerCast + AbilityRank * 0.5); // 每次橙武触发可以打出的特效暴雨数

            var CW_BY_Freq = CW_BY_Num / Total_Interval_Time;
            //var CW_BY = 1 / CWInterval * BY_CWPerBY * QiXue.BYPerCast;
            const string keyJD = "CW_JD";
            var CW_JD = SkillEvents[keyJD].TriggerFreq(FinalNum.JD / FinalNum._Time);
            //FinalSkillFreq.AddByFreq(keyBY, CW_BY);
            //FinalSkillFreq.AddByFreq(keyJD, CW_JD);

            FinalNum.CW_BY = FinalNum.GetNumByFreq(CW_BY_Freq);
            FinalNum.CW_JD = FinalNum.GetNumByFreq(CW_JD);

            CalcAfterCW();

            FinalSkillFreq = FinalNum.ToSkillFreqDict();
            GetFinalEventHitFreq();
        }



        public virtual void CalcHX(TLSkillCountItem num)
        {
        }

        public virtual void CalcAfterCW()
        {
            // 当拥有橙武时，对技能数的修正配平
            CalcHX(FinalNum);
            CommonCalcAfter(FinalNum);
        }

        public virtual void GetPeriodSkillNum()
        {

        }


        #endregion

        public void CalcPZ(TLSkillCountItem num)
        {
            // 计算破招
            num.CalcPZ();
        }


        /// <summary>
        /// 计算天绝伤害次数
        /// </summary>
        /// <returns>天绝机关伤害次数，诡鉴天绝伤害次数</returns>
        public (double TJTJ, double GJTJ) GetGJTJ()
        {
            var tj = Num._TJCast * TJPerTJCast;
            double gj = Num._GJExitsWhenTJCast * TJPerGJCast * Num._TJRound;

            return (tj, gj);
        }

        public virtual void CalcShaJiDuanHunArg(TLSkillCountItem num)
        {
            // 天绝加千机变
            var tj = num.TJ * TJInterval;
            var tj4qjb = tj / num._Time;

            // 暗藏加天绝
            var ac4tj = 3 * (0.85 + 0.05 * AbilityRank);

            // 千机变加图穷 TODO: 考虑三弩流
            var qjb4tq = 1;

            ShaJiArg = new ShaJiDuanHunArg(tj4qjb, ac4tj, qjb4tq);
        }


    }

    public struct GuiFuRes
    {
        public double LN; // 常规连弩次数
        public double LN_GF; // 鬼斧连弩次数
        public double RestBullet; // 剩余子弹数
    }
}