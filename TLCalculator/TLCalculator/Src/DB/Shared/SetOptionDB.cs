using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class SetOptionDB : SetOptionDBBase
    {

        #region 构造
        public SetOptionDB(IEnumerable<SetOptionItem> itemdata) : base(itemdata)
        {
        }

        public SetOptionDB(XFDataLoader xfDataLoader) : this(xfDataLoader.SetOption.Values)
        {
        }

        public SetOptionDB() : this(StaticXFData.Data)
        {
        }

        #endregion

    }
}