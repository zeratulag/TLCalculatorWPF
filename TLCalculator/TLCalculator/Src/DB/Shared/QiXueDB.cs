using JX3CalculatorShared.Data;
using JX3CalculatorShared.DB;
using System.Collections.Generic;
using TLCalculator.Data;

namespace TLCalculator.DB
{
    public class QiXueDB : QiXueDBBase
    {

        #region 构造
        public QiXueDB(IDictionary<string, QiXueItem> qixuedict) : base(qixuedict)
        {
        }

        public QiXueDB(XFDataLoader xfDataLoader) : this(xfDataLoader.QiXue)
        {

        }

        public QiXueDB() : this(StaticXFData.Data)
        {

        }

        #endregion

    }
}