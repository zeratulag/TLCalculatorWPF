using System.Collections.Generic;
using System.Collections.Immutable;

namespace TLCalculator.Class
{
    public partial class SkillHasteTable
    {
        /// <summary>
        /// 一些已经固定的Item，不需要重复计算
        /// </summary>

        #region 成员


        public readonly HasteTableItem PZ;

        public readonly HasteTableItem JD;
        public readonly HasteTableItem BY;

        public readonly HasteTableItem HX;
        public readonly HasteTableItem HX_Small;


        #endregion

        #region 构造

        public SkillHasteTable(SkillDataDF df)
        {
            SkillDF = df;
            GCD = HasteTableItem.GetGCDItem();
            PZ = new HasteTableItem(df.Data[nameof(PZ)]);

            JD = new HasteTableItem(df.Data[nameof(JD)]);
            BY = new HasteTableItem(df.Data[nameof(BY)]);

            HX = new HasteTableItem(df.Data[nameof(HX)]);
            HX_Small = new HasteTableItem(df.Data[nameof(HX_Small)]);

            var dict = new Dictionary<string, HasteTableItem>()
            {
                {nameof(GCD), GCD}, {nameof(PZ), PZ},
                {nameof(JD), JD}, {nameof(BY), BY},
                {nameof(HX), HX}, {nameof(HX_Small), HX_Small},
            };
            Dict = dict.ToImmutableDictionary();
        }

        #endregion
    }
}