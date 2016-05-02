using DevExpress.Xpf.Editors;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class EmptyComboBoxEdit : ComboBoxEdit
    {
        protected override bool CanShowPopupCore()
        {
            return this.ItemsProvider != null;
        }
    }
}