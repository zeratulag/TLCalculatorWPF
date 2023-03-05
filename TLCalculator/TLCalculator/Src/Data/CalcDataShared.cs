using JX3CalculatorShared.Data;
using TLCalculator.Class;

namespace TLCalculator.Data
{
    /// <summary>
    /// 用于存储导出与导入
    /// </summary>
    public class CalcData : CalcDataBase
    {
        #region 成员

        public JBPZPanel JBPz;

        #endregion

        public CalcData() : base()
        {
        }

        public CalcData(CalcSetting setting)
        {
            SkillMiJiConfig = setting.SkillMiJiConfig;
            QiXueConfig = setting.QiXueLib[setting.DefaultQiXue];
            JBPz = setting.DefaultJB;
            JBPz.Parse();
        }

        public static CalcData GetSample()
        {
            var res = new CalcData(StaticXFData.Data.Setting);
            return res;
        }
    }
}