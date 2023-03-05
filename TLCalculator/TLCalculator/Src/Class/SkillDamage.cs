using JX3CalculatorShared.Common;
using JX3CalculatorShared.Globals;
using System;
using TLCalculator.Globals;

namespace TLCalculator.Class
{
    public partial class SkillDamage
    {
        #region 成员

        public double OrgMagicDmg { get; set; } = 0; // 原始内功伤害（固定部分+内功攻击部分）
        public double StdMagicDmg { get; set; } // 基准内功伤害（原始内功+破招）

        // 目标防御
        public double FinalMDef { get; set; } // 等价内功最终防御

        public double ClosingMDef { get; set; }

        public double BearMDef { get; set; } // 承伤率

        // 各类增伤
        public double ParaMOC { get; set; } // 内功破防增伤

        public double ParaMYS { get; set; } // 内功易伤

        public double ParaMDmgAdd { get; set; } // 内功伤害提高

        // 实际伤害
        public double RealMagicDmg { get; set; } // 实际外功伤害

        // 会心会效

        public double ExpectMagicDmg { get; set; } // 实际内功伤害期望


        #endregion

        #region 方法

        // 计算伤害
        public void GetDamage()
        {
            GetStdDamage();
            GetDef();
            GetParams();
            GetRealDamage();
            GetETable();
            GetFinalDmg();
        }


        // 计算基准伤害
        public void GetStdDamage()
        {
            OrgMagicDmg = Data.Info.Fixed_Dmg + Data.M_APCoef * FChar.Final_AP;
            OrgPhysicsDmg = Data.WPCoef * FChar.WP + Data.P_APCoef * FChar.P_Final_AP;
            if (Type == SkillDataTypeEnum.PZ)
            {
                OrgPDmg = Math.Max(0, FChar.PZ) * XFStaticConst.fGP.XPZ;
            }

            StdPhysicsDmg = OrgPhysicsDmg;
            StdMagicDmg = OrgMagicDmg + OrgPDmg;
        }

        #endregion

        // 计算最终防御
        public void GetDef()
        {
            FinalPDef = Math.Max(CTarget.Final_PDef - CTarget.Base_PDef * Data.IgnoreB_P, 0);
            FinalMDef = Math.Max(CTarget.Final_MDef - CTarget.Base_MDef * Data.IgnoreB_M, 0);

            ClosingPDef = FinalPDef * Math.Max(0, 1 - FChar.IgnoreA);
            ClosingMDef = FinalMDef * Math.Max(0, 1 - FChar.IgnoreA);

            BearPDef = CTarget.GetBearDef(ClosingPDef); // 最终外功承伤率
            BearMDef = CTarget.GetBearDef(ClosingMDef); // 最终内功承伤率

        }

        // 计算各类增伤
        public void GetParams()
        {
            ParaPOC = 1 + FChar.P_Final_OC_Pct;
            ParaMOC = 1 + FChar.Final_OC_Pct;
            ParaPYS = 1 + CTarget.P_YS;
            ParaMYS = 1 + CTarget.M_YS;

            ParaPDmgAdd = 1 + FChar.P_DmgAdd + Data.AddDmg;
            ParaMDmgAdd = 1 + FChar.M_DmgAdd + Data.AddDmg;
            ParaWS = 1 + FChar.WS;
            ParaNPC = 1 + FChar.NPC_Coef;

            ParaLevelCrush = DamageTool.LevelCrushCoef(CTarget.Level);
        }

        // 计算实际伤害
        public void GetRealDamage()
        {
            var OtherParas = ParaWS * ParaNPC * ParaLevelCrush; // 除了破防之外的其他系数之积
            RealPhysicsDmg = StdPhysicsDmg * BearPDef * ParaPOC * ParaPYS * ParaPDmgAdd * OtherParas;
            RealMagicDmg = StdMagicDmg * BearMDef * ParaMOC * ParaMYS * ParaMDmgAdd * OtherParas;
            RealDmg = RealPhysicsDmg + RealMagicDmg;
        }

        // 计算圆桌期望

        // 计算最终期望伤害
        public void GetFinalDmg()
        {
            ExpectPhysicsDmg = RealPhysicsDmg * Expect;
            ExpectMagicDmg = RealMagicDmg * Expect;
            FinalEDamage = ExpectMagicDmg + ExpectPhysicsDmg;
        }

        // 获取相对伤害
        public void CalcRelativeDamage(double baseline)
        {
            RelativeDamage = FinalEDamage / baseline * 100;
        }

        // 计算属性收益（求导）
        public DamageDeriv CalcDeriv()
        {
            var res = new DamageDeriv(Name);
            res.Final_AP = Data.M_APCoef > 0 ? Data.M_APCoef * ExpectMagicDmg / OrgMagicDmg : 0;
            res.WP = Data.WPCoef > 0 ? Data.WPCoef * ExpectPhysicsDmg / OrgPhysicsDmg : 0;

            res.PZ = Data.Info.IsP ? XFStaticConst.fGP.XPZ * ExpectMagicDmg / OrgPDmg : 0;

            res.Final_OC = ExpectMagicDmg / ParaMOC / XFStaticConst.fGP.OC;
            res.WS = FinalEDamage / ParaWS / XFStaticConst.fGP.WS;

            res.CF = CF < 3 ? FinalEDamage / Expect * CT / XFStaticConst.fGP.CF : 0;
            res.CT = CT < 1 ? FinalEDamage / Expect * (CF - 1) / XFStaticConst.fGP.CT : 0;

            res.Base_OC = res.Final_OC * (1 + FChar.OC_Percent);
            res.Base_AP = res.Final_AP * (1 + FChar.AP_Percent);

            res.GetFinal_Y();
            res.GetBase_Y(FChar.Y_Percent);

            return res;
        }

    }
}