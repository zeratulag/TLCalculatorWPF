using JX3CalculatorShared.ViewModels;
using TLCalculator.Src;

namespace TLCalculator.ViewModels
{
    public class OptimizationViewModel : OptimizationViewModelBase
    {
        public void UpdateSource(CalculatorShell shell)
        {
            if (IsChecked)
            {
                OptimizationDescSource = shell.DpsKernelOp.BestProportion.Desc;
                MultiZhenResSource = shell.MultiZhenDPSOp.ResultArr;
            }
            else
            {
                OptimizationDescSource = "";
                MultiZhenResSource = null;
            }

            Refresh();
        }
    }
}