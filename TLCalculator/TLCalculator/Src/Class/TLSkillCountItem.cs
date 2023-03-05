using System;
using JX3CalculatorShared.Class;
using JX3CalculatorShared.Common;
using System.Collections.Generic;
using JX3CalculatorShared.Utils;
using TLCalculator.Class;
using TLCalculator.Data;
using TLCalculator.Globals;
using TLCalculator.Models;

namespace TLCalculator.Src
{
    /// <summary>
    /// 描述天罗技能数/频率模型
    /// </summary>
    public class TLSkillCountItem: SkillCountItemBase
    {
        #region 成员

        // Ability 对象中的标准释放时间
        public static readonly Period<TLSkillStandardTime> AbilitySkillTime =
            Globals.XFStaticConst.CurrentHaste.GetTL_SkillStandardTime();

        public readonly bool 积重诡鉴; // 是否为诡鉴流

        public double JD;
        public double BY;
        public double TN;
        public double TJ;
        public double TQ;
        public double _AC;

        public double _LNTotal; // 连弩总次数
        public double _BYCast; // 暴雨释放次数
        public double _TJCast; // 天绝释放次数
        public double _TJRound; // 天绝轮数，心无期间是1轮，常规期间等于天绝释放次数
        public double _TQCast; // 图穷释放次数
        public double _GJCast; // 诡鉴释放次数
        public double _GJExitsWhenTJCast; // 每次天绝释放时，场上存在的平均诡鉴数

        // 以下属性需要结合SkillNumModel计算得到
        public double ZN;
        public double ZN_GF;
        public double LN;
        public double LN_GF;
        public double _QQCast;
        public double QQ;
        public double GF;
        public double HX;

        public double XYLH_TJ;
        public double XYLH_TQ;
        public double TJ_TF;
        public double HX_XW;
        public double PZ;

        public double CW_BY; // 橙武暴雨
        public double CW_JD; // 橙武鸡蛋·神兵

        #endregion

        #region 构造

        public void GetLNTotal(AbilitySkillNumItem item)
        {
            // 基于手法水平获取连弩总次数
            const double LNLastTime = 120.0;
            double loss = 4.0 - item.Rank / 2.0; // 手法越好损失越少
            double freq = (LNLastTime / 1.5 - loss) / LNLastTime;
            var znfreq = 1.3 / LNLastTime;
            _LNTotal = item.Time * freq;
            ZN = item.Time * znfreq;
        }

        public TLSkillCountItem(AbilitySkillNumItem item)
        {
            // 基于输入的技能数据导入
            _XW = item.XW == 1;
            _Time = item.Time;
            _Rank = item.Rank;

            if (_XW)
            {
                _LNTotal = item.LN;
            }
            else
            {
                GetLNTotal(item);
            }

            BY = item.BY;
            JD = item.JD;

            TN = item.TN;
            TJ = item.TJ;
            TQ = item.TQ;
            _AC = item.AC;

            _TQCast = TQ / 3;
            _BYCast = item.BY / 5;

            GetUtilization();
        }

        public TLSkillCountItem(AbilitySkillNumItem item, QiXueConfigModel qiXue) : this(item)
        {
            积重诡鉴 = qiXue.Genre == XFStaticConst.Genre.积重诡鉴;
            // 诡鉴特殊修正
            if (积重诡鉴)
            {
                var penalty = 1 - 0.1 * (3 - item.Rank);
                if (_XW)
                {
                    _TJCast = qiXue.积重难返 ? 3 * penalty : 1;
                    _GJCast = 1 * penalty; // 心无期间1诡鉴
                    _GJExitsWhenTJCast = 2;
                    _TJRound = 1;
                }
                else
                {
                    _TJCast = 3 * penalty; // 常规期间3天绝
                    _GJCast = 2.5 * penalty; // 常规期间
                    _GJExitsWhenTJCast = _GJCast * 2 / _TJCast; // 平均每个天绝转的时候，场上存在的诡鉴个数
                    _TJRound = _TJCast;
                }
            }
            else
            {
                var TJNumCast = 5 + item.XW; // 心无期间多一次天绝（因为这里输入的是未考虑天风的天绝跳数）
                _TJCast = TJ / TJNumCast;
            }
        }

        #endregion

        /// <summary>
        /// 转化为字典
        /// </summary>
        /// <returns></returns>
        public new Dictionary<string, double> ToDict()
        {
            var res = new Dictionary<string, double>()
            {
                {nameof(JD), JD}, {nameof(BY), BY}, {nameof(TN), TN}, {nameof(TJ), TJ},
                {nameof(TQ), TQ}, {nameof(_AC), _AC},

                {nameof(_LNTotal), _LNTotal}, {nameof(_BYCast), _BYCast}, {nameof(_TQCast), _TQCast},
                {nameof(_TJCast), _TJCast}, {nameof(_GJCast), _GJCast},
                {nameof(_GJExitsWhenTJCast), _GJExitsWhenTJCast}, {nameof(_TJRound), _TJRound},

                {nameof(ZN), ZN}, {nameof(ZN_GF), ZN_GF}, {nameof(LN), LN}, {nameof(LN_GF), LN_GF},
                {nameof(_QQCast), _QQCast}, {nameof(QQ), QQ}, {nameof(GF), GF},
                {nameof(HX), HX}, {nameof(HX_XW), HX_XW},
                {nameof(XYLH_TJ), XYLH_TJ}, {nameof(XYLH_TQ), XYLH_TQ}, {nameof(TJ_TF), TJ_TF},
                {nameof(PZ), PZ},
                {nameof(CW_BY), CW_BY}, {nameof(CW_JD), CW_JD}
            };

            return res;
        }

        public SkillNumDict ToSkillNumDict()
        {
            var dict = ToDict();
            return new SkillNumDict(dict, _Time);
        }

        public SkillFreqDict ToSkillFreqDict()
        {
            var dict = ToDict();
            return new SkillFreqDict(dict, _Time);
        }

        public void GetUtilization()
        {
            var stdtime = _XW ? AbilitySkillTime.XW : AbilitySkillTime.Normal;
            var GCDNum = JD + TN + _TQCast + _TJCast;
            _UTime = GCDNum * stdtime.GCD + BY * stdtime.BY;
            _URate = _UTime / _Time;
        }

        public new void ResetTime(double newTime)
        {
            // 重设定时间（当新增字段时候别忘了增加）

            if (Math.Abs(newTime - _Time) < 1e-5) return;
            var k = newTime / _Time;

            JD *= k;
            BY *= k;
            TN *= k;
            TJ *= k;
            TQ *= k;

            ZN *= k;
            _AC *= k;

            _LNTotal *= k;
            _BYCast *= k;
            _TJCast *= k;
            _TQCast *= k;
            _GJCast *= k;

            CW_JD *= k;
            CW_BY *= k;
        }

        // 更新暴雨释放次数
        public void UpdateBYNum(double BYPerCast)
        {
            BY = _BYCast * BYPerCast;
        }



        /// <summary>
        /// 应用短时间红利，使得鸡蛋，暴雨技能数目增加
        /// </summary>
        /// <param name="k">提升系数</param>
        public void ApplyShortTimeBonus(double k)
        {
            JD *= (1 + k);
            BY *= (1 + k);
            _BYCast *= (1 + k);
        }

        public void AddJDNumByFreq(double freq)
        {
            //根据频率增加鸡蛋数量
            JD += _Time * freq;
        }


        /// <summary>
        /// 计算千机变总次数，用于计算蚀肌化血触发
        /// </summary>
        /// <returns></returns>
        public double QJBNum => _LNTotal + ZN + ZN_GF;

        // 计算罡风数
        public void CalcGF()
        {
            double gf = JD + TN + _BYCast;
            if (积重诡鉴)
            {
                gf += _TQCast + (_TJCast + _QQCast);
            }
            else
            {
                gf += _TQCast * 1.5 + (_TJCast + _QQCast);
            }

            GF = gf;
        }


        /// <summary>
        /// 同步蚀肌化血计算结果
        /// </summary>
        /// <param name="result"></param>
        public void Update(NormalSJHXResult result)
        {
            JD = result.JD;
            HX = result.HX;
            TN = result.TN;
        }


        public void Apply血影留痕()
        {
            // 应用血影留痕
            XYLH_TQ = TQ;
            XYLH_TJ = TJ;
        }

        public void Apply心机天成()
        {
            _LNTotal *= 1.5;
        }

        public void CalcPZ()
        {
            double zhuNeng = _BYCast + _TJCast + _AC;
            var pz = zhuNeng / 3.0;
            if (_XW)
            {
                pz += _BYCast; // 心无期间暴雨附送破招
            }
            PZ = pz;
        }
    }
}