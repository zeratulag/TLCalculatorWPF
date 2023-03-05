using Minimod.PrettyPrint;
using System.Linq;
using TLCalculator.Class;
using TLCalculator.Data;

namespace TLCalculator.Src
{
    public class ConsoleMainProgram
    {
        public static void ConsoleMain()
        {
            var args = new string[] { "0" };
            ConsoleMain(args);
        }

        public static void ConsoleMain(string[] args)
        {

#if DEBUG
            var tlData = StaticXFData.Data;
            var tlDB = StaticXFData.DB;


            var tmp = InitCharacter.GetSample().ToFullCharacter();

            var data = StaticXFData.DB.ZhenFa.Data.Skip(1).First().Value.Buff;
            var tmps = data.SCharAttrs.Others.PrettyPrint();
            var strs = data.SCharAttrs.ToNamed().Desc;
            var re0 = StaticXFData.DB.Recipe.Data.Values.First();
            var re1 = re0.Emit(0.4);
            var guard = 0;

#endif
        }
    }
}
