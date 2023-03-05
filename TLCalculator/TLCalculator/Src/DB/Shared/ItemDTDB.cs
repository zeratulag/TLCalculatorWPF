using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class ItemDTDB : ItemDTDBBase
    {

        #region 构造
        public ItemDTDB(IEnumerable<ItemDTItem> itemdata) : base(itemdata)
        {
        }

        public ItemDTDB(XFDataLoader xfDataLoader) : this(xfDataLoader.ItemDT)
        {
        }

        public ItemDTDB() : this(StaticXFData.Data)
        {
        }
        #endregion

    }
}