using JX3CalculatorShared.Data;
using JX3CalculatorShared.ViewModels;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TLCalculator.Class;
using TLCalculator.Data;

namespace TLCalculator.Src
{
    public class MultiZhenFaOptimizer
    {
        // 多重阵法优化器（天罗的更加复杂，因为阵眼影响技能数

        #region 成员

        public static readonly ImmutableArray<ZhenFa> AllZhen; // 阵法集合
        public static readonly int Num; // 阵法个数

        public readonly DPSKernelShell OriginalShell; // 原始hell
        public readonly ZhenFa OriginalZhenFa; // 原始阵法


        public readonly Dictionary<string, MultiZhenRes> Result; // 结论
        public MultiZhenRes[] ResultArr;

        public readonly CalculatorShell[] CalculatorShells;
        public readonly CalculatorShell OriginalCalculatorShell;

        #endregion


        #region 构造

        static MultiZhenFaOptimizer()
        {
            var res = ImmutableArray.CreateBuilder<ZhenFa>();
            foreach (var _ in StaticXFData.DB.ZhenFa.ZhenFa)
            {
                res.Add(_);
            }

            AllZhen = res.ToImmutableArray();
            Num = AllZhen.Length;
        }

        public MultiZhenFaOptimizer(CalculatorShell shell)
        {
            OriginalCalculatorShell = shell;
            CalculatorShells = new CalculatorShell[Num];
            Result = new Dictionary<string, MultiZhenRes>(Num);
            ResultArr = new MultiZhenRes[Num];
        }


        #endregion

        public void Calc()
        {
            GetInputs();
            GetRelative();
            GetRank();
        }


        public void GetInputs()
        {
            for (int i = 0; i < AllZhen.Length; i++)
            {
                var _ = AllZhen[i];
                var newCalculatorShellShell = new CalculatorShell(OriginalCalculatorShell);
                newCalculatorShellShell.Zhen = _;
                newCalculatorShellShell.Calc();
                var resi = new MultiZhenRes(_.ItemName, newCalculatorShellShell.CDPSKernel.FinalDPS);
                Result.Add(_.Name, resi);
            }
        }

        public void GetRelative()
        {
            // 计算阵法相对收益
            var baseline = Result["None"].DPS;

            foreach (var _ in Result.Values)
            {
                _.Relative = _.DPS / baseline;
            }
        }

        public void GetRank()
        {
            var list = Result.Values.OrderByDescending(_ => _.DPS);
            int i = 0;
            foreach (var _ in list)
            {
                ResultArr[i] = _;
                _.Rank = i + 1;
                i++;
            }
        }
    }
}