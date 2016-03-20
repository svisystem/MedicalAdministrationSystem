using System;
using System.Reflection;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public abstract class FormValidate
    {
        protected internal bool Validate(object item)
        {
            Type type = Type.GetType(item.ToString());
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++) if (!(bool)properties[i].GetValue(item)) return false;
            return true;
        }
        protected internal bool Validation(object input, object empty, object valid)
        {
            return (Validate(input) && Validate2(empty) && Validate3(empty, valid));
        }
        private bool Validate2(object item)
        {
            Type type = Type.GetType(item.ToString());
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
                if (!(bool)properties[i].GetValue(item)) return true;
            return false;
        }
        private bool Validate3(object empty, object valid)
        {
            bool[] user = new bool[2];
            Type type1 = Type.GetType(empty.ToString());
            Type type2 = Type.GetType(valid.ToString());
            PropertyInfo[] properties1 = type1.GetProperties();
            PropertyInfo[] properties2 = type2.GetProperties();
            for (int i = 0; i < properties1.Length; i++)
                if (!(bool)properties1[i].GetValue(empty))
                    if ((bool)properties2[i].GetValue(valid)) user[i] = true;
                    else user[i] = false;
                else user[i] = true;
            foreach (bool value in user) if (!value) return false;
            return true;
        }
    }
}
