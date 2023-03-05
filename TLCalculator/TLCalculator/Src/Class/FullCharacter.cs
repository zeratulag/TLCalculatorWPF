using JX3CalculatorShared.Globals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TLCalculator.DB;
using TLCalculator.Globals;

namespace TLCalculator.Class
{
    public partial class FullCharacter
    {
        #region 成员

        // 基础元气，最终元气
        public double Base_Y { get; private set; }
        public double Final_Y { get; private set; }


        // 外功基础攻击，外功最终攻击，外功基础破防，外功最终破防
        public double P_Base_AP { get; private set; }
        public double P_Final_AP { get; private set; }
        public double P_Base_OC { get; private set; }
        public double P_Final_OC { get; private set; }

        // 元气提升
        public double Y_Percent { get; private set; }

        // 外功攻击提升，外功破防提升
        public double P_AP_Percent { get; private set; }
        public double P_OC_Percent { get; private set; }

        // 伤害提高
        public double P_DmgAdd { get; private set; } // 外功伤害提高
        public double M_DmgAdd { get; private set; } // 内功伤害提高

        public double P_Final_OC_Pct => P_Final_OC / StaticConst.fGP.OC; // 面板外功破防

        #endregion

        #region 构造

        /// <summary>
        /// 表示人物完整属性的类
        /// </summary>
        /// <param name="base_y">基础元气</param>
        /// <param name="final_y">最终元气</param>
        /// <param name="base_ap">基础攻击</param>
        /// <param name="final_ap">最终攻击</param>
        /// <param name="base_oc">基础破防</param>
        /// <param name="final_oc">最终破防</param>
        /// <param name="wp">武器伤害</param>
        /// <param name="ct">会心</param>
        /// <param name="cf">会效</param>
        /// <param name="ws">无双</param>
        /// <param name="pz">破招</param>
        /// <param name="hs">加速</param>
        /// <param name="p_base_ap">外功基础攻击</param>
        /// <param name="p_final_ap">外功最终攻击</param>
        /// <param name="p_base_oc">外功基础破防</param>
        /// <param name="p_final_oc">外功最终破防</param>
        /// <param name="y_percent">元气提升</param>
        /// <param name="ap_percent">攻击提升</param>
        /// <param name="oc_percent">破防提升</param>
        /// <param name="p_ap_percent">外功攻击提升</param>
        /// <param name="p_oc_percent">外功破防提升</param>
        /// <param name="ignorea">无视防御A</param>
        /// <param name="p_dmgadd">外功伤害提高</param>
        /// <param name="m_dmgadd">内功伤害提高</param>
        /// <param name="name">名称</param>
        public FullCharacter(double base_y, double final_y, double base_ap, double final_ap, double base_oc,
            double final_oc,
            double wp, double ct, double cf, double ws, double pz, double hs,
            double p_base_ap, double p_final_ap, double p_base_oc, double p_final_oc,
            double y_percent, double ap_percent, double oc_percent,
            double p_ap_percent, double p_oc_percent,
            double ignorea, double p_dmgadd, double m_dmgadd,
            string name = "完整面板")
        {
            Base_Y = base_y;
            Final_Y = final_y;
            Base_AP = base_ap;
            Final_AP = final_ap;
            Base_OC = base_oc;
            Final_OC = final_oc;
            WP = wp;
            CT = ct;
            CF = cf;
            WS = ws;
            PZ = pz;
            HS = hs;
            P_Base_AP = p_base_ap;
            P_Final_AP = p_final_ap;
            P_Base_OC = p_base_oc;
            P_Final_OC = p_final_oc;
            Y_Percent = y_percent;
            AP_Percent = ap_percent;
            OC_Percent = oc_percent;
            P_AP_Percent = p_ap_percent;
            P_OC_Percent = p_oc_percent;
            IgnoreA = ignorea;
            P_DmgAdd = p_dmgadd;
            M_DmgAdd = m_dmgadd;
            Name = name;
            PostConstructor();
        }

        /// <summary>
        /// 从InitCharacter转换
        /// </summary>
        /// <param name="iChar">初始属性</param>
        public FullCharacter(InitCharacter iChar)
        {
            Base_Y = iChar.Y;
            Final_Y = iChar.Y;
            Base_AP = iChar.Base_AP;
            Final_AP = iChar.Final_AP;
            Base_OC = iChar.OC;
            Final_OC = iChar.OC;

            WP = iChar.WP;
            CT = iChar.CT;
            CF = iChar.CF;
            WS = iChar.WS;
            PZ = iChar.PZ;
            HS = iChar.HS;

            P_Base_AP = iChar.P_AP;
            P_Final_AP = iChar.P_AP;
            P_Base_OC = iChar.P_OC;
            P_Final_OC = iChar.P_OC;

            Y_Percent = 0.0;
            AP_Percent = 0.0;
            OC_Percent = 0.0;
            P_AP_Percent = 0.0;
            P_OC_Percent = 0.0;

            IgnoreA = 0.0;
            P_DmgAdd = 0.0;
            M_DmgAdd = 0.0;

            Name = iChar.Name;
            Had_BigFM_jacket = iChar.Had_BigFM_jacket;
            Had_BigFM_hat = iChar.Had_BigFM_hat;
            PostConstructor();
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="old"></param>
        public FullCharacter(FullCharacter old)
        {
            Base_Y = old.Base_Y;
            Final_Y = old.Final_Y;
            Base_AP = old.Base_AP;
            Final_AP = old.Final_AP;
            Base_OC = old.Base_OC;
            Final_OC = old.Final_OC;

            WP = old.WP;
            CT = old.CT;
            CF = old.CF;
            WS = old.WS;
            PZ = old.PZ;
            HS = old.HS;

            P_Base_AP = old.P_Base_AP;
            P_Final_AP = old.P_Final_AP;
            P_Base_OC = old.P_Base_OC;
            P_Final_OC = old.P_Final_OC;

            Y_Percent = old.Y_Percent;
            AP_Percent = old.AP_Percent;
            OC_Percent = old.OC_Percent;
            P_AP_Percent = old.P_AP_Percent;
            P_OC_Percent = old.P_OC_Percent;

            IgnoreA = old.IgnoreA;
            P_DmgAdd = old.P_DmgAdd;
            M_DmgAdd = old.M_DmgAdd;

            Name = old.Name;
            ExtraSP = old.ExtraSP;
            Is_XW = old.Is_XW;
            Has_Special_Buff = old.Has_Special_Buff;

            Had_BigFM_jacket = old.Had_BigFM_jacket;
            Had_BigFM_hat = old.Had_BigFM_hat;

            PostConstructor();
        }

        #endregion


        #region 显示

        public IList<string> GetCatStrList()
        {
            var hadDict = new Dictionary<bool, string>()
            {
                {true, "已"}, {false, "未"}
            };

            var res = new List<string>
            {
                $"{this.Final_Y:F2}({this.Base_Y:F2}) 元气，" +
                $"{this.Final_AP:F2}({this.Base_AP:F2}) 攻击，" +
                $"{this.Final_OC:F2}({this.Base_OC:F2}) 破防，" +
                $"{this.WP:F2} 武器伤害",

                $"{this.CT:P2} 会心，{this.CF:P2} 会效，{this.WS:P2} 无双，" +
                $"{this.HS}({this.HS / StaticConst.fGP.HS + this.ExtraSP / 1024:P2}) 加速，{this.PZ} 破招",
                $"{this.Y_Percent:P2} 元气提升，{this.AP_Percent:P2} 攻击提升，{this.OC_Percent:P2} 破防提升，" +
                $"{this.IgnoreA:P2} 无视防御A，{this.P_DmgAdd:P2} 外功伤害提高，{this.M_DmgAdd:P2} 内功伤害提高",

                $"{this.P_Final_AP:F2}({this.P_Base_AP:F2}) 外功攻击，" +
                $"{this.P_Final_OC:F2}({this.P_Base_OC:F2}) 外功破防。",

                $"{hadDict[this.Is_XW]}处于心无状态，{hadDict[this.Has_Special_Buff]}计算特殊增益",

                $"常规GCD: {this.Normal_GCD:F4} s, 大心无GCD: {this.BigXW_GCD:F4} s"
            };

            return res;
        }

        #endregion

        #region 属性计算_各种属性

        public void Add_WP(double value)
        {
            WP += value;
        }

        public void Add_CT(double value)
        {
            if (Has_Special_Buff)
            {
                throw new ArgumentException("Cannot add CT after has special_buff!");
            }
            else
            {
                CT += value;
            }
        }

        public void Add_CT_Point(double value)
        {
            Add_CT(value / StaticConst.fGP.CT);
        }

        public void Add_CF(double value)
        {
            CF += value;
        }

        public void Add_CF_Point(double value)
        {
            Add_CF(value / StaticConst.fGP.CF);
        }

        public void Add_WS(double value)
        {
            WS += value;
        }

        public void Add_WS_Point(double value)
        {
            Add_WS(value / StaticConst.fGP.WS);
        }

        public void Add_HSP(double value)
        {
            HS += value; // 注意此处加速改变
        }

        public void Add_PZ(double value)
        {
            PZ += value;
        }

        public void Add_Final_AP(double value)
        {
            Final_AP += value;
        }

        public void Add_Base_AP(double value)
        {
            Base_AP += value;
            Add_Final_AP(value * (1 + AP_Percent));
        }

        public void Add_AP_Percent(double value)
        {
            AP_Percent += value;
            Add_Final_AP(value * Base_AP);
        }

        public void Add_Final_OC(double value)
        {
            Final_OC += value;
        }

        public void Add_Base_OC(double value)
        {
            Base_OC += value;
            Add_Final_OC(value * (1 + OC_Percent));
        }

        public void Add_OC_Percent(double value)
        {
            OC_Percent += value;
            Add_Final_OC(value * Base_OC);
        }

        public void Add_S(double value) //身法
        {
            Add_CT_Point(value * XFConsts.CT_PER_S);
        }

        public void Add_Final_Y(double value) // 最终元气
        {
            Final_Y += value;
            Add_Base_AP(value * XFConsts.AP_PER_Y);
            Add_Base_OC(value * XFConsts.OC_PER_Y);

            Add_Final_AP(value * XFConsts.F_AP_PER_Y);
            Add_CT_Point(value * XFConsts.CT_PER_Y);
        }

        public void Add_Base_Y(double value)
        {
            Base_Y += value;
            Add_Final_Y(value * (1 + Y_Percent));
        }

        public void Add_Y_Percent(double value)
        {
            Y_Percent += value;
            Add_Final_Y(value * Base_Y);
        }

        public void Add_IgnoreA(double value)
        {
            IgnoreA += value;
        }

        public void Add_P_DmgAdd(double value)
        {
            P_DmgAdd += value;
        }

        public void Add_M_DmgAdd(double value)
        {
            M_DmgAdd += value;
        }

        public void Add_All_DmgAdd(double value)
        {
            Add_P_DmgAdd(value);
            Add_M_DmgAdd(value);
        }

        public void Add_P_Final_AP(double value)
        {
            P_Final_AP += value;
        }

        public void Add_P_Base_AP(double value)
        {
            P_Base_AP += value;
            Add_P_Final_AP(value * (1 + P_AP_Percent));
        }

        public void Add_P_AP_Percent(double value)
        {
            P_AP_Percent += value;
            Add_P_Final_AP(P_Base_AP * value);
        }

        public void Add_P_Final_OC(double value)
        {
            P_Final_OC += value;
        }

        public void Add_P_Base_OC(double value)
        {
            P_Base_OC += value;
            Add_P_Final_OC(value * (1 + P_OC_Percent));
        }

        public void Add_P_OC_Percent(double value)
        {
            P_OC_Percent += value;
            Add_P_Final_OC(P_Base_OC * value);
        }

        public void Add_L(double value) // 增加力道
        {
            Add_P_Base_AP(value * XFConsts.P_AP_PER_L);
            Add_P_Base_OC(value * XFConsts.P_OC_PER_L);
            // TODO[JY]: 如果是惊羽计算器，需要修改
        }

        public void Add_Base_L(double value)
        {
            Add_L(value);
        }

        public void Add_L_Percent(double value)
        {
            // TODO[JY]: 如果是惊羽的Char，需要修改增加基础力道
        }

        /// <summary>
        /// 增加全属性
        /// </summary>
        /// <param name="value"></param>
        public void Add_All_BasePotent(double value)
        {
            Add_L(value);
            Add_S(value);
            Add_Base_Y(value);
        }


        /// <summary>
        /// 属性修改分派，注意这段代码是由Python程序SwitchTool.py生成的
        /// </summary>
        /// <param name="key">简化后的属性名称</param>
        /// <param name="value">属性值</param>
        protected void _AddSAttr(string key, double value)
        {
            switch (key)
            {
                case "WP":
                    {
                        Add_WP(value);
                        break;
                    }
                case "CT":
                    {
                        Add_CT(value);
                        break;
                    }
                case "CT_Point":
                    {
                        Add_CT_Point(value);
                        break;
                    }
                case "CF":
                    {
                        Add_CF(value);
                        break;
                    }
                case "CF_Point":
                    {
                        Add_CF_Point(value);
                        break;
                    }
                case "WS":
                    {
                        Add_WS(value);
                        break;
                    }
                case "WS_Point":
                    {
                        Add_WS_Point(value);
                        break;
                    }
                case "HSP":
                    {
                        Add_HSP(value);
                        break;
                    }
                case "PZ":
                    {
                        Add_PZ(value);
                        break;
                    }
                case "Final_AP":
                    {
                        Add_Final_AP(value);
                        break;
                    }
                case "Base_AP":
                    {
                        Add_Base_AP(value);
                        break;
                    }
                case "AP_Percent":
                    {
                        Add_AP_Percent(value);
                        break;
                    }
                case "Final_OC":
                    {
                        Add_Final_OC(value);
                        break;
                    }
                case "Base_OC":
                    {
                        Add_Base_OC(value);
                        break;
                    }
                case "OC_Percent":
                    {
                        Add_OC_Percent(value);
                        break;
                    }
                case "S":
                    {
                        Add_S(value);
                        break;
                    }
                case "Final_Y":
                    {
                        Add_Final_Y(value);
                        break;
                    }
                case "Base_Y":
                    {
                        Add_Base_Y(value);
                        break;
                    }
                case "Y_Percent":
                    {
                        Add_Y_Percent(value);
                        break;
                    }
                case "IgnoreA":
                    {
                        Add_IgnoreA(value);
                        break;
                    }
                case "P_DmgAdd":
                    {
                        Add_P_DmgAdd(value);
                        break;
                    }
                case "M_DmgAdd":
                    {
                        Add_M_DmgAdd(value);
                        break;
                    }
                case "All_DmgAdd":
                    {
                        Add_All_DmgAdd(value);
                        break;
                    }
                case "P_Final_AP":
                    {
                        Add_P_Final_AP(value);
                        break;
                    }
                case "P_Base_AP":
                    {
                        Add_P_Base_AP(value);
                        break;
                    }
                case "P_AP_Percent":
                    {
                        Add_P_AP_Percent(value);
                        break;
                    }
                case "P_Final_OC":
                    {
                        Add_P_Final_OC(value);
                        break;
                    }
                case "P_Base_OC":
                    {
                        Add_P_Base_OC(value);
                        break;
                    }
                case "P_OC_Percent":
                    {
                        Add_P_OC_Percent(value);
                        break;
                    }
                case "L":
                    {
                        Add_L(value);
                        break;
                    }
                case "Base_L":
                    {
                        Add_Base_L(value);
                        break;
                    }
                case "L_Percent":
                    {
                        Add_L_Percent(value);
                        break;
                    }
                case "All_BasePotent":
                    {
                        Add_All_BasePotent(value);
                        break;
                    }
                default:
                    {
                        if (!XFDataBase.UselessAttrs.Contains(key))
                        {
                            Trace.WriteLine($"未知的F属性！\t{key}\t{value} ");
                        }

                        break;
                    }
            }
        }

        #endregion
    }
}