using JX3CalculatorShared.Class;
using TLCalculator.ViewModels;


namespace TLCalculator.Class
{
    public class FightTimeSummary : FightTimeSummaryBase
    {
        #region 成员
        public double TLShortTimeBonus = 0.0; // 天罗短时间战斗红利（指木桩起手读鬼斧）
        public FightTimeSummaryViewModel _VM { get; private set; } = null;

        #endregion

        #region 构造

        public FightTimeSummary() : base()
        {
        }

        public void AttachVM(FightTimeSummaryViewModel vm)
        {
            _VM = vm;
        }

        public FightTimeSummary(FightTimeSummaryViewModel vm) : this()
        {
            AttachVM(vm);
        }

        /// <summary>
        /// 用于描述战斗时间的类
        /// </summary>
        /// <param name="totalTime">总战斗时间（s）</param>
        /// <param name="xwCD">心无CD</param>
        /// <param name="xwDuration">心无持续时间</param>
        public FightTimeSummary(double totalTime, double xwCD = 90.0, double xwDuration = 15.0) : base(totalTime, xwCD,
            xwDuration)
        {
        }

        /// <summary>
        /// 仅仅改变心无CD
        /// </summary>
        /// <param name="xwCD"></param>
        public new void UpdateXWCD(double xwCD)
        {
            base.UpdateXWCD(xwCD);
            if (_VM != null)
            {
                _VM.XWCD = XWCD;
            }
        }

        /// <summary>
        /// 计算天罗短时间战斗红利系数
        /// </summary>
        public void GetTLShortTimeBonus()
        {
            if (IsShort)
            {
                var normalDuration = XWCD - XWDuration; // 常规持续时间,75
                const double nXW = 3;
                const double GFTime = 4;
                var cost = 240.0 / (nXW * XWCD) * GFTime * nXW; // 理论上需要支付的时间
                var save = cost - GFTime * (nXW - 1);
                TLShortTimeBonus = save / ShortItem.NormalTime;
            }
            else
            {
                TLShortTimeBonus = 0.0;
            }
        }


        public new void Update(double totalTime, double xwCD = 90.0, double xwDuration = 15.0, bool isShort = false)
        {
            base.Update(totalTime, xwCD, xwDuration, isShort);
            UpdateXWCD(xwCD);
            GetTLShortTimeBonus();
        }

        public void Update(FightTimeSummaryViewModel vm)
        {
            Update(vm.TotalTime, vm.XWCD, vm.XWDuration, vm.FightOption.ShortFight);
        }

        #endregion
    }
}