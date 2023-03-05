using System.Collections.Generic;
using System.Collections.Immutable;

namespace TLCalculator.DB
{
    public partial class AttrWeightDB
    {
        #region 成员

        public static readonly ImmutableDictionary<string, string> AttrNameDict = new Dictionary<string, string>()
        {
            {"AP", "基础攻击"}, {"OC", "基础破防"}, {"Y", "基础元气"},
            {"CT", "会心"}, {"CF", "会效"},
            {"PZ", "破招"}, {"WS", "无双"},
            {"Final_AP", "最终攻击"}, {"Final_OC", "最终破防"}, {"Final_Y", "最终元气"},
            {"Base_AP", "基础攻击"}, {"Base_OC", "基础破防"}, {"Base_Y", "基础元气"}
        }.ToImmutableDictionary();

        #endregion
    }
}