﻿#pragma checksum "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0D091470AAFC0FEA07545B15A7EF082854E73C15"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Debugger.AddIn.Visualizers.Graph;
using ICSharpCode.Core.Presentation;
using Microsoft.Windows.Themes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Debugger.AddIn.Visualizers.GridVisualizer {
    
    
    /// <summary>
    /// GridVisualizerWindow
    /// </summary>
    public partial class GridVisualizerWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtExpression;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInspect;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbColumns;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView listView;
        
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
            System.Uri resourceLocater = new System.Uri("/Debugger.AddIn;component/visualizers/gridvisualizer/gridvisualizerwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
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
            this.txtExpression = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.btnInspect = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\Visualizers\GridVisualizer\GridVisualizerWindow.xaml"
            this.btnInspect.Click += new System.Windows.RoutedEventHandler(this.btnInspect_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.cmbColumns = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.listView = ((System.Windows.Controls.ListView)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
