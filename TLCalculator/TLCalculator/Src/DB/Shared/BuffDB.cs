using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class BuffDB : BuffDBBase
    {

        #region 构造

        public BuffDB(IEnumerable<Buff_dfItem> Buff_df) : base(Buff_df)
        {
        }

        public BuffDB(XFDataLoader xfDataLoader) : this(xfDataLoader.Buff_df)
        {
        }

        public BuffDB() : this(StaticXFData.Data.Buff_df)
        {
        }

        #endregion

    }
}