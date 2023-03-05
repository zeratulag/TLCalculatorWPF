using JX3CalculatorShared.Class;
using JX3CalculatorShared.ViewModels;
using System.Collections.Generic;
using System.Collections.Immutable;
using TLCalculator.Data;
using TLCalculator.DB;


namespace TLCalculator.ViewModels
{
    public class ItemDTConfigViewModel : ItemDTConfigViewModelBase
    {

        #region 构造

        public ItemDTConfigViewModel(ItemDTDB db)
        {
            ItemDTSlotDict = db.TypeData.ToImmutableDictionary(
                kvp => kvp.Key,
                kvp => new ItemDTSlotViewModel(kvp.Value));
            Data = ItemDTSlotDict.Values.ToImmutableArray();
            ValidItemDts = new List<ItemDT>(8);

            ItemsSource = ItemDTSlotDict.Values.ToImmutableArray();

            ExtendInputNames(nameof(IsChecked));
            AttachDependVMsOutputChanged();
            PostConstructor();
            PropertyChanged += OnIsCheckedChanged;
            _Update();
        }

        public ItemDTConfigViewModel() : this(StaticXFData.DB.ItemDT)
        {

        }

        #endregion

    }
}