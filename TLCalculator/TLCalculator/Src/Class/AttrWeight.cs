using JX3CalculatorShared.Class;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.Class
{
    public class AttrWeight : AttrWeightBase
    {
        public double Y { get; set; }
        public double Final_Y { get; set; } = double.NaN;

        public AttrWeight(DiamondValueItem item) : base(item)
        {
            Y = item.Y;
        }

        public AttrWeight(string name, string toolTip = "") : base(name, toolTip)
        {
        }

        public override Dictionary<string, double> ToDict()
        {
            var res = base.ToDict();
            res.Add(nameof(Y), Y);
            res.Add(nameof(Final_Y), Final_Y);
            return res;
        }
    }
}