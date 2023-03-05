﻿using MiniExcelLibs.Attributes;
using System.Collections.Generic;

namespace JX3CalculatorShared.Class
{
    public class DamageDerivBase
    {
        public string Name { get; set; }
        public double Base_AP { get; set; } // 基础攻击
        public double Base_OC { get; set; } // 基础破防
        public double PZ { get; set; } // 破招
        public double WS { get; set; } // 无双
        public double CT { get; set; } // 会心
        public double CF { get; set; } // 会效
        public double WP { get; set; } // 武器伤害
        public double Final_AP { get; set; } // 最终攻击
        public double Final_OC { set; get; } // 最终破防
        [ExcelIgnore] public AttrProfitList ProfitList { get; protected set; }
        public string OrderDesc => ProfitList.OrderDesc;

        public AttrWeightBase Weight;
        public string ToolTip => Weight.ToolTip;

        public DamageDerivBase()
        {
        }
        public DamageDerivBase(string name)
        {
            Name = name;
        }

        public DamageDerivBase(DamageDerivBase old)
        {
            Name = old.Name;
            Final_AP = old.Final_AP;
            WP = old.WP;
            PZ = old.PZ;
            WS = old.WS;
            CT = old.CT;
            CF = old.CF;
            Final_OC = old.Final_OC;
            Base_AP = old.Base_AP;
            Base_OC = old.Base_OC;
        }

        public virtual DamageDerivBase Copy()
        {
            return new DamageDerivBase(this);
        }

        #region 方法

        /// <summary>
        /// 按照权重相加，用于合并求出最终收益
        /// </summary>
        /// <param name="other">另一个对象</param>
        /// <param name="w">权重系数（技能频率）</param>
        public virtual void WeightedAdd(DamageDerivBase other, double w = 1.0)
        {
            Final_AP += other.Final_AP * w;
            WP += other.WP * w;
            PZ += other.PZ * w;
            WS += other.WS * w;
            CT += other.CT * w;
            CF += other.CF * w;
            Final_OC += other.Final_OC * w;
            Base_AP += other.Base_AP * w;
            Base_OC += other.Base_OC * w;
        }

        // 根据属性加权求收益
        public virtual void ApplyAttrWeight(AttrWeightBase aw)
        {
            Weight = aw;

            Name = aw.Name;
            Base_AP *= aw.AP;
            Base_OC *= aw.OC;
            CT *= aw.CT;
            CF *= aw.CF;
            WS *= aw.WS;
            PZ *= aw.PZ;
            WP *= aw.WP;
            Final_AP *= aw.Final_AP;
            Final_OC *= aw.Final_OC;
        }


        #endregion


        public List<AttrProfitItem> GetPointAttrDerivListBase()
        {
            var l = new List<AttrProfitItem>
            {
                new AttrProfitItem(nameof(Final_AP), "最终攻击", Final_AP),
                new AttrProfitItem(nameof(Final_OC), "最终破防", Final_AP),
                new AttrProfitItem(nameof(Base_AP), "基础攻击", Base_AP),
                new AttrProfitItem(nameof(Base_OC), "基础破防", Base_OC),
                new AttrProfitItem(nameof(PZ), "破招", PZ),
                new AttrProfitItem(nameof(WS), "无双", WS),
                new AttrProfitItem(nameof(CT), "会心", CT),
                new AttrProfitItem(nameof(CF), "会效", CF),
                new AttrProfitItem(nameof(WP), "武伤", WP)
            };
            return l;
        }

        public List<AttrProfitItem> GetScoreAttrDerivListBase()
        {
            var l = new List<AttrProfitItem>
            {
                new AttrProfitItem(nameof(Base_AP), "攻击", Base_AP),
                new AttrProfitItem(nameof(Base_OC), "破防", Base_OC),
                new AttrProfitItem(nameof(PZ), "破招", PZ),
                new AttrProfitItem(nameof(WS), "无双", WS),
                new AttrProfitItem(nameof(CT), "会心", CT),
                new AttrProfitItem(nameof(CF), "会效", CF),
                new AttrProfitItem(nameof(WP), "武伤", WP)
            };
            return l;
        }

        public virtual List<double> GetValueArr()
        {
            var res = new List<double>(8)
            {
                Base_AP, Base_OC,
                PZ, WS,
                CT, CF,
                //WP
            };
            return res;
        }

        public virtual List<string> GetDescArr()
        {
            var res = new List<string>(8)
            {
                "攻击", "破防",
                "破招", "无双",
                "会心", "会效",
                //"武伤"
            };
            return res;
        }
    }
}