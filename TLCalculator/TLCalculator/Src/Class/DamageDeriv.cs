using JX3CalculatorShared.Class;
using System.Collections.Generic;
using TLCalculator.Globals;

namespace TLCalculator.Class
{
    public partial class DamageDeriv : DamageDerivBase
    {
        public double Base_Y { get; set; } // 基础元气
        public double Final_Y { get; set; } // 最终元气


        #region 构造

        // 复制构造
        public DamageDeriv(DamageDeriv old) : base(old)
        {
            Final_Y = old.Final_Y;
            Base_Y = old.Base_Y;
        }

        // 计算最终元气收益
        public static double GetFinal_Y(double base_ap, double final_ap, double ct, double base_oc)
        {
            double res = 0;
            res += base_ap * XFConsts.AP_PER_Y;
            res += base_oc * XFConsts.OC_PER_Y;
            res += final_ap * XFConsts.F_AP_PER_Y;
            res += ct * XFConsts.CT_PER_Y;
            return res;
        }

        public void GetFinal_Y()
        {
            Final_Y = GetFinal_Y(Base_AP, Final_AP, CT, Base_OC);
        }

        public void GetBase_Y(double y_Percent)
        {
            Base_Y = Final_Y * (1 + y_Percent);
        }


        // 修复会心收益
        public void FixCT(double ct)
        {
            CT = ct;
            GetFinal_Y();
        }

        public void FixCT(double ct, double y_Percent)
        {
            FixCT(ct);
            GetBase_Y(y_Percent);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 按照权重相加，用于合并求出最终收益
        /// </summary>
        /// <param name="other">另一个对象</param>
        /// <param name="w">权重系数（技能频率）</param>
        public void WeightedAdd(DamageDeriv other, double w = 1.0)
        {
            base.WeightedAdd(other, w);
            Final_Y += other.Final_Y * w;
            Base_Y += other.Base_Y * w;
        }

        public new void ApplyAttrWeight(AttrWeight aw)
        {
            // 根据属性加权求收益
            base.ApplyAttrWeight(aw);
            Weight = aw;
            Base_Y *= aw.Y;
            Final_Y *= aw.Final_Y;
        }

        // 单点收益
        public AttrProfitList GetPointAttrDerivList()
        {
            var l = GetPointAttrDerivListBase();
            l.Add(new AttrProfitItem(nameof(Base_Y), "基础元气", Base_Y));
            l.Add(new AttrProfitItem(nameof(Final_Y), "最终元气", Final_Y));

            var res = new AttrProfitList(l);
            res.Proceed();
            ProfitList = res;
            return res;
        }

        // 单分收益
        public AttrProfitList GetScoreAttrDerivList()
        {
            var l = GetScoreAttrDerivListBase();
            l.Add(new AttrProfitItem(nameof(Base_Y), "元气", Base_Y));

            var res = new AttrProfitList(l);
            res.Proceed();
            ProfitList = res;
            return res;
        }

        public override List<double> GetValueArr()
        {
            var res = base.GetValueArr();
            res.Insert(2, Base_Y);
            return res;
        }

        public override List<string> GetDescArr()
        {
            var res = base.GetDescArr();
            res.Insert(2, "元气");
            return res;
        }

        #endregion
    }
}