using System.Collections.Immutable;
using System.Linq;
using JX3CalculatorShared.Data;
using JX3CalculatorShared.Utils;
using TLCalculator.Class;

namespace TLCalculator.Data
{
    public class CalcSetting : CalcSettingBase
    {
        public JBPZPanel DefaultJB { get; set; }

        // 以下为生成属性

        public void Parse()
        {
            EssentialQiXues = StringTool.ParseStringList(RawEssentialQiXues).ToImmutableHashSet();
            BannedQiXues = StringTool.ParseStringList(RawBannedQiXues).ToImmutableHashSet();
            EssentialMiJis = StringTool.ParseStringList(RawEssentialMiJis).ToImmutableHashSet();
            QiXueLib = RawQiXueLib.ToImmutableDictionary(_ => _.Key, _ => StringTool.ParseIntList(_.Value).ToArray());
        }
    }
}