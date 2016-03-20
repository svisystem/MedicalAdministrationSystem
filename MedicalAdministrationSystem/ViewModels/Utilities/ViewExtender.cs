﻿using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using System.Windows;
using System.Windows.Controls;

namespace MedicalAdministrationSystem.ViewModels.Utilities
{
    public class ViewExtender : UserControl
    {
        protected internal object validatorClass;
        protected internal Button button;
        protected internal ValidationEventArgs lastState;
        protected internal virtual void ForceBinding(object sender, ValidationEventArgs e)
        {
            if (lastState == null || lastState.Value == null || !lastState.Value.Equals(e.Value))
            {
                lastState = e;
                (sender as dynamic).EditValue = e.Value;
            }
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
        }
        protected internal void TextChanged(object sender, RoutedEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            button.IsEnabled = (validatorClass as FormValidate).Validate(validatorClass);
            (sender as dynamic).DoValidate();
        }
        protected internal string GetSenderName(object sender)
        {
            return sender.GetType().GetProperty("Name").GetValue(sender as dynamic); ;
        }
        protected internal void ButtonEditErase(object sender, RoutedEventArgs e)
        {
            var parent = LayoutHelper.FindParentObject<ButtonEdit>(sender as DependencyObject);
            parent.Clear();
        }
        protected internal void ComboBoxEditErase(object sender, RoutedEventArgs e)
        {
            var parent = LayoutHelper.FindParentObject<ComboBoxEdit>(sender as DependencyObject);
            parent.Clear();
        }
        protected internal void DateEditErase(object sender, RoutedEventArgs e)
        {
            var parent = LayoutHelper.FindParentObject<DateEdit>(sender as DependencyObject);
            parent.Clear();
        }
        protected internal void MaskedNotNullValidateForString(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
            ForceBinding(sender, e);
        }
        protected internal void MaskedNullEnabledValidateForString(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            if (string.IsNullOrEmpty(e.Value as string)) e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBinding(sender, e);
        }
        protected internal void MaskedNullEnabledValidateForNumber(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            if (e.Value == null) e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBinding(sender, e);
        }
        protected internal void NonMaskedNullEnabledValidateForString(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése nem kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBinding(sender, e);
        }
        protected internal void NonMaskedNotNullValidateForString(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, false, null);
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A mező kitöltése kötelező", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            }
            ForceBinding(sender, e);
        }
        protected internal void UserNameValidate(object sender, ValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("A felhasználónevet nem lehet üresen hagyni", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
            else
            {
                validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
                e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            }
            ForceBinding(sender, e);
        }
        protected internal void BirthNameValidate(object sender, ValidationEventArgs e)
        {
            validatorClass.GetType().GetProperty(GetSenderName(sender)).SetValue(validatorClass, true, null);
            if (string.IsNullOrEmpty(e.Value as string))
                e.SetError("Születési név megadására csak bizonyos esetekben van szüség", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Information);
            else e.SetError("A mező tartalma megfelelő", DevExpress.XtraEditors.DXErrorProvider.ErrorType.User1);
            ForceBinding(sender, e);
        }
        
    }
}