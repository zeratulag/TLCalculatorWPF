using System;
using System.Collections.Generic;
using NForza.Memoization;

namespace TLCalculator.Src
{
    // 用于求解蚀肌化血触发次数的类
    public class ShiJiHuaXueModel
    {
        #region 成员

        public readonly double HXt;
        public readonly double JDt;
        public readonly double BYt;

        public readonly double Rate; // 触发率
        public readonly double Penalty; // 惩罚系数

        public const double DefaultRate = 0.7;
        public const double DefaultPenalty = 6.0;

        // 应用缓存
        public Func<ShiJiHuaXueInput, ShiJiHuaXueResult> GetHXNumByArg; // 缓存计算结果

        #endregion

        #region 构造

        /// <summary>
        /// 蚀肌化血模型
        /// </summary>
        /// <param name="hxt">化血间隔(2.9375)</param>
        /// <param name="jdt">鸡蛋读条时间(1.4375)</param>
        /// <param name="byt">暴雨伤害间隔(0.4375)</param>
        /// <param name="rate">触发率，当前0.7</param>
        /// <param name="penalty">用于模拟内置cd的惩罚系数(6)，取值在1 - 24之间</param>
        public ShiJiHuaXueModel(double hxt, double jdt, double byt,
            double rate = DefaultRate, double penalty = DefaultPenalty)
        {
            HXt = hxt;
            JDt = jdt;
            BYt = byt;
            Rate = rate;
            Penalty = penalty;

            GetHXNumByArg = FuncExtensions.Memoize<ShiJiHuaXueInput, ShiJiHuaXueResult>(_GetHXNumByArg);
        }

        public ShiJiHuaXueModel(ShiJiHuaXueModelParam param) : this(param.HXt, param.JDt, param.BYt, param.Rate,
            param.Penalty)
        {
        }

        #endregion

        /// <summary>
        /// 计算蚀肌化血触发次数
        /// </summary>
        /// <param name="QJBNum">千机变次数</param>
        /// <param name="JDNum">鸡蛋次数</param>
        /// <param name="BYNum">暴雨次数</param>
        /// <param name="time">总时长（秒）</param>
        /// <returns></returns>
        private ShiJiHuaXueResult _GetHXNum(double QJBNum, double JDNum, double BYNum, double time)
        {
            var LN_HX = QJBNum * Rate;
            var JD_HX = JDNum * Rate * (1 - Penalty * Rate / 24);
            var BY_HX = BYNum * Rate * (1 - Penalty * Rate / 24);
            var JD_total_t = JDNum * JDt;
            var BY_total_t = BYNum * BYt;
            var org_HX = time / HXt; // 如果没有触发，则自然化血数量;
            var org_coef = (JD_total_t + BY_total_t * Math.Pow(1 - Rate, 4)) * Math.Pow(1 - Rate, 2) /
                           (JD_total_t + BY_total_t); // 考虑蚀肌化血的惩罚
            var Nature_HX = org_HX * org_coef;

            var res = new ShiJiHuaXueResult(LN_HX, JD_HX, BY_HX, Nature_HX);
            return res;
        }

        public ShiJiHuaXueResult GetHXNum(double QJBNum, double JDNum, double BYNum, double time)
        {
            var arg = new ShiJiHuaXueInput(QJBNum, JDNum, BYNum, time);
            return GetHXNumByArg(arg);
        }


        private ShiJiHuaXueResult _GetHXNumByArg(ShiJiHuaXueInput arg)
        {
            return _GetHXNum(arg.QJBNum, arg.JDNum, arg.BYNum, arg.Time);
        }
    }


    public readonly struct ShiJiHuaXueInput
    {
        public readonly double QJBNum;
        public readonly double JDNum;
        public readonly double BYNum;
        public readonly double Time;

        public ShiJiHuaXueInput(double qjbNum, double jdNum, double byNum, double time)
        {
            QJBNum = qjbNum;
            JDNum = jdNum;
            BYNum = byNum;
            Time = time;
        }
    }

    public struct ShiJiHuaXueResult
    {
        public readonly double QJB; // 千机变触发的化血数
        public readonly double JD; // 鸡蛋触发的化血数
        public readonly double BY; // 暴雨触发的化血数
        public readonly double Nature; // 自然跳的化血数
        public readonly double Total; // 总化血数

        public ShiJiHuaXueResult(double qjb, double jd, double by, double nature)
        {
            QJB = qjb;
            JD = jd;
            BY = by;
            Nature = nature;
            Total = QJB + JD + BY + Nature;
        }
    }


    public struct ShiJiHuaXueModelParam
    {
        public readonly double HXt;
        public readonly double JDt;
        public readonly double BYt;

        public readonly double Rate; // 触发率
        public readonly double Penalty; // 惩罚系数

        public ShiJiHuaXueModelParam(double hXt, double jDt, double bYt,
            double rate = ShiJiHuaXueModel.DefaultRate, double penalty = ShiJiHuaXueModel.DefaultPenalty)
        {
            HXt = hXt;
            JDt = jDt;
            BYt = bYt;
            Rate = rate;
            Penalty = penalty;
        }
    }

    
    public static class ShiJiHuaXueModelLib
    {
        // 引入求解缓存机制
        public static readonly Dictionary<ShiJiHuaXueModelParam, ShiJiHuaXueModel> Models;

        static ShiJiHuaXueModelLib()
        {
            Models = new Dictionary<ShiJiHuaXueModelParam, ShiJiHuaXueModel>(4);
        }

        public static ShiJiHuaXueModel Get(ShiJiHuaXueModelParam param)
        {
            ShiJiHuaXueModel res;
            if (Models.ContainsKey(param))
            {
                res = Models[param];
            }
            else
            {
                res = new ShiJiHuaXueModel(param);
                Models.Add(param, res);
            }
            return res;
        }

        public static ShiJiHuaXueModel Get(double hxt, double jdt, double byt,
            double rate = 0.7, double penalty = 6)
        {
            var param = new ShiJiHuaXueModelParam(hxt, jdt, byt, rate, penalty);
            return Get(param);
        }
    }
}