using JX3CalculatorShared.Common;
using TLCalculator.Globals;
using TLCalculator.Src;

namespace TLCalculator.Models
{
    public partial class QiXueConfigModel : IModel
    {
        #region 成员

        // Tags

        // 是否拥有如下奇穴
        public bool 千机之威 { get; private set; }
        public bool 心机天成 { get; private set; }
        public bool 积重难返 { get; private set; }
        public bool 千秋万劫 { get; private set; }
        public bool 雷甲三铉 { get; private set; }
        public bool 血影留痕 { get; private set; }
        public bool 天风汲雨 { get; private set; }
        public bool 杀机断魂 { get; private set; }
        public bool 弩击急骤 { get; private set; }
        public bool 曙色催寒 { get; private set; }
        public bool 擘两分星 { get; private set; }
        public bool 诡鉴冥微 { get; private set; }


        public double TJPerCast; // 天绝每次释放的跳数

        public double BYPerCast; // 暴雨一次释放的跳数

        // 流派

        public string Genre; // 实际流派
        public string SkillBaseNumGenre; // 基础技能数

        public QiXueArg Arg; 

        #endregion

        #region 构建


        #endregion

        public override void Calc()
        {
            GetRecipes();
            GetTags();
            GetXW();
            GetNums();
            GetSkillEffects();
            GetSkillEvents();
            GetSelfBuffNames();
            GetIsSupport();
            GetGenre();
            ExportArg();
        }


        public void GetTags()
        {
            秋风散影 = Has(nameof(秋风散影));
            千机之威 = Has(nameof(千机之威));
            聚精凝神 = Has(nameof(聚精凝神));
            心机天成 = Has(nameof(心机天成));
            积重难返 = Has(nameof(积重难返));
            千秋万劫 = Has(nameof(千秋万劫));
            雷甲三铉 = Has(nameof(雷甲三铉));
            血影留痕 = Has(nameof(血影留痕));
            天风汲雨 = Has(nameof(天风汲雨));
            杀机断魂 = Has(nameof(杀机断魂));
            弩击急骤 = Has(nameof(弩击急骤));
            曙色催寒 = Has(nameof(曙色催寒));
            擘两分星 = Has(nameof(擘两分星));
            诡鉴冥微 = Has(nameof(诡鉴冥微));
        }


        public void GetNums()
        {
            TJPerCast = 天风汲雨 ? 3 : 5;
            BYPerCast = 5;
        }

        /// <summary>
        /// 获取当前流派
        /// </summary>
        protected void GetGenre()
        {
            var res = XFStaticConst.Genre.回肠雷甲;
            if (诡鉴冥微 && 积重难返)
            {
                res = XFStaticConst.Genre.积重诡鉴;
            }

            SkillBaseNumGenre = res;
            Genre = res;

            if (千秋万劫 && 积重难返)
            {
                Genre = XFStaticConst.Genre.积重千秋;
            }
        }

        public QiXueArg ExportArg()
        {
            Arg = new QiXueArg(弩击急骤, 擘两分星, 曙色催寒, 杀机断魂, SkillBaseNumGenre);
            return Arg;
        }

    }

}