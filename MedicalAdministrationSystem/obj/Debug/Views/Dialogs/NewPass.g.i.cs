﻿#pragma checksum "..\..\..\..\Views\Dialogs\NewPass.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A63CA96C291596E59652495D0CFE78EE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DevExpress.Core;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.ConditionalFormatting;
using DevExpress.Xpf.Core.DataSources;
using DevExpress.Xpf.Core.Serialization;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.DataPager;
using DevExpress.Xpf.Editors.DateNavigator;
using DevExpress.Xpf.Editors.ExpressionEditor;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Flyout;
using DevExpress.Xpf.Editors.Popups;
using DevExpress.Xpf.Editors.Popups.Calendar;
using DevExpress.Xpf.Editors.RangeControl;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Editors.Settings.Extension;
using DevExpress.Xpf.Editors.Themes;
using DevExpress.Xpf.Editors.Validation;
using DevExpress.Xpf.LayoutControl;
using MedicalAdministrationSystem.ViewModels.Utilities;
using MedicalAdministrationSystem.Views.Dialogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MedicalAdministrationSystem.Views.Dialogs {
    
    
    /// <summary>
    /// NewPass
    /// </summary>
    public partial class NewPass : MedicalAdministrationSystem.ViewModels.Utilities.ViewExtender, System.Windows.Markup.IComponentConnector {
        
        
        #line 44 "..\..\..\..\Views\Dialogs\NewPass.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit newPass;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Views\Dialogs\NewPass.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit confPass;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MedicalAdministrationSystem;component/views/dialogs/newpass.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Dialogs\NewPass.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.newPass = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 2:
            
            #line 53 "..\..\..\..\Views\Dialogs\NewPass.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 3:
            this.confPass = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 4:
            
            #line 66 "..\..\..\..\Views\Dialogs\NewPass.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

