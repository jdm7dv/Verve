﻿#pragma checksum "..\..\..\..\Windows\EDMWizard\ChooseDatabaseConnectionUserControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E55A5F23C8CE90A90BD2F1182F11751B50AB84BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ICSharpCode.Data.Core.Common;
using ICSharpCode.Data.Core.DatabaseObjects;
using ICSharpCode.Data.Core.Interfaces;
using ICSharpCode.Data.Core.UI.Helpers;
using ICSharpCode.Data.Core.UI.UserControls;
using ICSharpCode.Data.Core.UI.Windows;
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


namespace ICSharpCode.Data.EDMDesigner.Core.Windows.EDMWizard {
    
    
    /// <summary>
    /// ChooseDatabaseConnectionUserControl
    /// </summary>
    public partial class ChooseDatabaseConnectionUserControl : ICSharpCode.Data.Core.UI.UserControls.WizardUserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 5 "..\..\..\..\Windows\EDMWizard\ChooseDatabaseConnectionUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdConnection;
        
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
            System.Uri resourceLocater = new System.Uri("/ICSharpCode.Data.EDMDesigner.Core;component/windows/edmwizard/choosedatabaseconn" +
                    "ectionusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\EDMWizard\ChooseDatabaseConnectionUserControl.xaml"
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
            this.grdConnection = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            
            #line 21 "..\..\..\..\Windows\EDMWizard\ChooseDatabaseConnectionUserControl.xaml"
            ((System.Windows.Controls.ComboBox)(target)).SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboDatabases_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 28 "..\..\..\..\Windows\EDMWizard\ChooseDatabaseConnectionUserControl.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnNewConnection_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
