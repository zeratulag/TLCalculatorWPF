using System;
using JX3CalculatorShared.Common;
using TLCalculator.Class;
using TLCalculator.Models;

namespace TLCalculator.Src
{
    public partial class CalculatorShell
    {
        public TLSkillDataDFModel SkillDfModel;
        public BoLiangFenXingArg BoLiangArg;

        protected void CalcTLSkillDataDFModel()
        {
            var normalSJ = SkillNum.Normal.ShaJiArg;
            var xwSJ = SkillNum.XW.ShaJiArg;
            var shaJiDuanHun = new Period<ShaJiDuanHunArg>(normalSJ, xwSJ);
            SkillDfModel = new TLSkillDataDFModel(SkillDFs, SkillNum, QiXue.Arg, BoLiangArg, shaJiDuanHun);
            SkillDfModel.Calc();
        }

        public void GetDPSKernelShell()
        {
            var LM = Equip.WP.IsLongMen ? Equip.WP.Value : 0; // 龙门飞剑等级
            var arg = new DPSCalcShellArg(QiXue.聚精凝神, Equip.SL, LM, AbilityItem.Rank, QiXue.Arg, Equip.YZ, BigFM,
                Buffs.BuffSpecial, InitInput.NoneBigFMInitCharacter, BoLiangArg);
            KernelShell = new DPSKernelShell(FullCharGroup.ZhenBuffed, CTarget, SkillDFs, SkillNum, FightTime, arg);
        }

        protected void CalcBoLiangFenXingArg()
        {
            double under70 = 0.7 + 0.05 * (AbilityItem.Rank - 3);
            double above30 = 0.85 + 0.05 * (AbilityItem.Rank - 3);
            if (Zhen.IsOwn)
            {
                under70 -= 0.1;
                above30 += 0.1;
            }
            under70 = Math.Min(1, under70);
            above30 = Math.Min(1, above30);
            BoLiangArg = new BoLiangFenXingArg(QiXue.擘两分星, under70, above30);
        }
    }
}