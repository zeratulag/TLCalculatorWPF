﻿using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class ZhenFaDB : ZhenFaDBBase
    {

        #region 构造

        public ZhenFaDB(IDictionary<string, ZhenFa> zhenFaDict, IEnumerable<ZhenFa_dfItem> zhenFaDf) : base(zhenFaDict, zhenFaDf)
        {
        }

        public ZhenFaDB(XFDataLoader xfDataLoader) : this(xfDataLoader.ZhenFa_dict, xfDataLoader.ZhenFa_df)
        {
        }

        public ZhenFaDB() : this(StaticXFData.Data)
        {
        }

        #endregion
    }
}