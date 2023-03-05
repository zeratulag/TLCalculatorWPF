using JX3CalculatorShared.Common;
using JX3CalculatorShared.Globals;
using JX3CalculatorShared.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TLCalculator.Globals;
using static JX3CalculatorShared.Globals.StaticConst;

namespace TLCalculator.Class
{
    public partial class InitCharacter : AbsViewModel, ICatsable
    {
        #region 成员

        // 元气,
        // 外功攻击, 外功破防,
        public double Y { get; set; }
        public double P_AP { get; set; } = XFConsts.Default_P_AP;
        public double P_OC { get; set; } = XFConsts.Default_P_OC;

        #endregion

        #region 构造

        /// <summary>
        /// 表示人物初始属性（没有任何增益）的类
        /// </summary>
        /// <param name="y">元气</param>
        /// <param name="base_ap">基础攻击</param>
        /// <param name="final_ap">最终攻击</param>
        /// <param name="wp">武伤</param>
        /// <param name="ct">会心</param>
        /// <param name="cf">会效</param>
        /// <param name="ws">无双</param>
        /// <param name="pz">破招</param>
        /// <param name="oc">破防</param>
        /// <param name="hs">加速</param>
        /// <param name="p_ap">外功攻击</param>
        /// <param name="p_oc">外功破防</param>
        /// <param name="had_BigFM_hat">是否带了伤帽</param>
        /// <param name="had_BigFM_jacket">是否带了伤衣</param>
        /// <param name="name">名称</param>
        public InitCharacter(double y, double base_ap, double final_ap, double wp,
            double ct, double cf, double ws,
            double pz, double oc, double hs,
            bool had_BigFM_hat = false, bool had_BigFM_jacket = false,
            double p_ap = XFConsts.Default_P_AP, double p_oc = XFConsts.Default_P_OC,
            string name = "") : this()
        {
            Y = y;
            Base_AP = base_ap;
            Final_AP = final_ap;
            WP = wp;

            CT = ct;
            CF = cf;
            WS = ws;

            PZ = pz;
            OC = oc;
            HS = hs;

            P_AP = p_ap;
            P_OC = p_oc;

            Had_BigFM_hat = had_BigFM_hat;
            Had_BigFM_jacket = had_BigFM_jacket;
            Name = name;

            PostConstructor();
        }

        public InitCharacter(InitCharacter old) : base()
        {
            Y = old.Y;
            Base_AP = old.Base_AP;
            Final_AP = old.Final_AP;
            WP = old.WP;

            CT = old.CT;
            CF = old.CF;
            WS = old.WS;

            PZ = old.PZ;
            OC = old.OC;
            HS = old.HS;

            P_AP = old.P_AP;
            P_OC = old.P_OC;

            Had_BigFM_jacket = old.Had_BigFM_jacket;
            Had_BigFM_hat = old.Had_BigFM_hat;
            Name = old.Name;

            PostConstructor();
        }

        #endregion

        /// <summary>
        /// 从导入的JB面板更新
        /// </summary>
        /// <param name="panel"></param>
        protected void _UpdateFromJBPanel(JBPZPanel panel)
        {
            Y = panel.Spunk;
            Base_AP = panel.PoisonAttackPowerBase;
            Final_AP = panel.PoisonAttackPower;
            WP = panel.MeleeWeaponDamage + panel.MeleeWeaponDamageRand / 2;

            CT = panel.PhysicsCriticalStrikeRate;
            CF = panel.PhysicsCriticalDamagePowerPercent;
            WS = panel.StrainPercent;

            PZ = panel.SurplusValue;
            OC = Math.Round(panel.PoisonOvercomePercent * fGP.OC);
            HS = Math.Round(panel.HastePercent * fGP.HS);

            P_AP = XFConsts.Default_P_AP;
            P_OC = XFConsts.Default_P_OC;

            Had_BigFM_jacket = panel.EquipList.Had_BigFM_jacket;
            Had_BigFM_hat = panel.EquipList.Had_BigFM_hat;
            Name = panel.Title;
        }


        /// <summary>
        /// 从另外一个面板更新
        /// </summary>
        /// <param name="ichar"></param>
        protected void _UpdateFromIChar(InitCharacter ichar)
        {
            Y = ichar.Y;
            Base_AP = ichar.Base_AP;
            Final_AP = ichar.Final_AP;
            WP = ichar.WP;

            CT = ichar.CT;
            CF = ichar.CF;
            WS = ichar.WS;

            PZ = ichar.PZ;
            OC = ichar.OC;
            HS = ichar.HS;

            P_AP = ichar.P_AP;
            P_OC = ichar.P_OC;

            Had_BigFM_jacket = ichar.Had_BigFM_jacket;
            Had_BigFM_hat = ichar.Had_BigFM_hat;
            Name = ichar.Name;
        }


        #region 显示

        public IList<string> GetCatStrList()
        {
            var res = new List<string>
            {
                "人物初始属性：",
                $"{Y:F0} 元气，{Base_AP:F0} 基础攻击，{Final_AP:F0} 最终攻击，{WP:F1} 武器伤害，{PZ:F0} 破招",

                $"{CT:P2} 会心，{CF:P2} 会效，{WS:P2} 无双，" +
                $"{OC:F0}({OC / fGP.OC:P2}) 破防，{HS:F0}({HS / fGP.HS:P2}) 加速",

                $"{P_AP:F0} 外功攻击，{P_OC:F0} 外功防御"
            };

            res.Add("");
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
            CT += value;
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
            HS += value;
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
            Add_Final_AP(value);
        }

        public void Add_OC(double value)
        {
            OC += value;
        }

        public void Add_S(double value) //身法
        {
            Add_CT_Point(value * XFConsts.CT_PER_S);
        }

        public void Add_Y(double value) // 最终元气
        {
            Y += value;
            Add_Base_AP(value * XFConsts.AP_PER_Y);
            Add_OC(value * XFConsts.OC_PER_Y);

            Add_Final_AP(value * XFConsts.F_AP_PER_Y);
            Add_CT_Point(value * XFConsts.CT_PER_Y);
        }

        public void Add_P_AP(double value)
        {
            P_AP += value;
        }


        public void Add_P_OC(double value)
        {
            P_OC += value;
        }

        public void Add_L(double value) // 增加力道
        {
            Add_P_AP(value * XFConsts.P_AP_PER_L);
            Add_P_OC(value * XFConsts.P_OC_PER_L);
        }

        /// <summary>
        /// 增加全属性
        /// </summary>
        /// <param name="value"></param>
        public void Add_All_BasePotent(double value)
        {
            Add_L(value);
            Add_S(value);
            Add_Y(value);
        }

        #endregion

        #region 属性计算

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
                case "OC":
                case "Base_OC":
                    {
                        Add_OC(value);
                        break;
                    }
                case "S":
                    {
                        Add_S(value);
                        break;
                    }
                case "Y":
                case "Base_Y":
                    {
                        Add_Y(value);
                        break;
                    }
                case "P_AP":
                case "P_Base_AP":
                    {
                        Add_P_AP(value);
                        break;
                    }
                case "P_OC":
                case "P_Base_OC":
                    {
                        Add_P_OC(value);
                        break;
                    }
                case "L":
                    {
                        Add_L(value);
                        break;
                    }
                case "All_BasePotent":
                    {
                        Add_All_BasePotent(value);
                        break;
                    }
                default:
                    {
                        Trace.WriteLine($"未知的I属性！ {key}:{value} ");
                        break;
                    }
            }
        }

        #endregion


        #region 示例样例

        public static InitCharacter GetSample(bool cat = false)
        {
            var I_C = new InitCharacter(4271, 13882, 21356, 1310.5, 0.5213, 1.8659, 0.4706, 3530, 9532, 441,
                had_BigFM_hat: false, had_BigFM_jacket: false, name: "初始实例面板");
            if (cat)
            {
                I_C.Cat();
            }

            return I_C;
        }

        #endregion
    }
}