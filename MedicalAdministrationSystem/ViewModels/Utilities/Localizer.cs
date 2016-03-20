using DevExpress.Xpf.Editors;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    class Localizer : EditorLocalizer
    {
        protected override void PopulateStringTable()
        {
            base.PopulateStringTable();
            AddString(EditorStringId.MaskIncomplete, "A mező tartalma nem megfelelő");
        }
    }
}
