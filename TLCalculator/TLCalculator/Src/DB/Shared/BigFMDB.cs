using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class BigFMDB : BigFMDBBase
    {

        #region 构造
        public BigFMDB(IEnumerable<BigFMItem> itemdata, IEnumerable<BottomsFMItem> bottomsdata) : base(itemdata, bottomsdata)
        {
        }

        public BigFMDB(XFDataLoader xfDataLoader) : this(xfDataLoader.BigFM, xfDataLoader.BottomsFM)
        {
        }

        public BigFMDB() : this(StaticXFData.Data)
        {
        }

        #endregion

    }
}