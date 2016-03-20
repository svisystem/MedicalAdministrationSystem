﻿#pragma checksum "..\..\..\..\Views\Settings\Security.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "699F9F53EB387EEFD964CFAB9645F16F"
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
using MedicalAdministrationSystem.Views.Settings;
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


namespace MedicalAdministrationSystem.Views.Settings {
    
    
    /// <summary>
    /// Security
    /// </summary>
    public partial class Security : MedicalAdministrationSystem.ViewModels.Utilities.ViewExtender, System.Windows.Markup.IComponentConnector {
        
        
        #line 45 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit currSecurityUser;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit newSecurityUser;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit currSecurityPass;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit newSecurityPass;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal DevExpress.Xpf.Editors.ButtonEdit confSecurityPass;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\..\Views\Settings\Security.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button modify;
        
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
            System.Uri resourceLocater = new System.Uri("/MedicalAdministrationSystem;component/views/settings/security.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\Settings\Security.xaml"
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
            this.currSecurityUser = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 2:
            
            #line 53 "..\..\..\..\Views\Settings\Security.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 3:
            this.newSecurityUser = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 4:
            
            #line 65 "..\..\..\..\Views\Settings\Security.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 5:
            this.currSecurityPass = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 6:
            
            #line 77 "..\..\..\..\Views\Settings\Security.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 7:
            this.newSecurityPass = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 8:
            
            #line 89 "..\..\..\..\Views\Settings\Security.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 9:
            this.confSecurityPass = ((DevExpress.Xpf.Editors.ButtonEdit)(target));
            return;
            case 10:
            
            #line 101 "..\..\..\..\Views\Settings\Security.xaml"
            ((DevExpress.Xpf.Editors.ButtonInfo)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonEditErase);
            
            #line default
            #line hidden
            return;
            case 11:
            this.modify = ((System.Windows.Controls.Button)(target));
            
            #line 105 "..\..\..\..\Views\Settings\Security.xaml"
            this.modify.KeyDown += new System.Windows.Input.KeyEventHandler(this.SecurityDatasChangeWithEnter);
            
            #line default
            #line hidden
            
            #line 105 "..\..\..\..\Views\Settings\Security.xaml"
            this.modify.Click += new System.Windows.RoutedEventHandler(this.SecurityDatasChangeExecute);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
