using JX3CalculatorShared.ViewModels;
using TLCalculator.Globals;

namespace TLCalculator.ViewModels
{
    public class AboutDialogViewModel : AboutDialogViewModelBase
    {


        public AboutDialogViewModel()
        {
            URLDict = XFAppStatic.URLDict;
            MainName = "天罗诡道计算器Pro v" + XFAppStatic.AppVersion.ToString() + " \"Nirvana\" Release";
            Description = "Desc";

            BuildDateTime = XFAppStatic.BuildDateTime.ToString("G");
            LastPatchTime = XFAppStatic.LastPatchTime.ToString("d");
            LastPatchURL = XFAppStatic.LastPatchURL;
            GameDLC = XFAppStatic.GameVersion;
            ThanksTo = "影白，JX3BOX。";

            GitUrl = URLDict["Git"];
            JBUrl = URLDict["JB"];
            TMUrl = URLDict["TM"];

        }
    }
}