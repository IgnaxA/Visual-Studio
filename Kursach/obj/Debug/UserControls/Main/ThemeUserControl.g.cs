﻿#pragma checksum "..\..\..\..\UserControls\Main\ThemeUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C0421E0693D3594D898C49BE1D1A4077BEC04A27645FF7DA6E5DC33361DA16E2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Kursach.UserControls.Main;
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


namespace Kursach.UserControls.Main {
    
    
    /// <summary>
    /// ThemeUserControl
    /// </summary>
    public partial class ThemeUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ThemeUserControlGrid;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView ThemesView;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem AddNode;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeleteNode;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem RenameNode;
        
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
            System.Uri resourceLocater = new System.Uri("/Kursach;component/usercontrols/main/themeusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
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
            this.ThemeUserControlGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ThemesView = ((System.Windows.Controls.TreeView)(target));
            
            #line 10 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
            this.ThemesView.PreviewMouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ThemesView_OnPreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
            this.ThemesView.ContextMenuOpening += new System.Windows.Controls.ContextMenuEventHandler(this.ThemesView_ContextMenuOpening);
            
            #line default
            #line hidden
            return;
            case 3:
            this.AddNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 13 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
            this.AddNode.Click += new System.Windows.RoutedEventHandler(this.AddNode_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.DeleteNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 14 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
            this.DeleteNode.Click += new System.Windows.RoutedEventHandler(this.DeleteNode_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RenameNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 15 "..\..\..\..\UserControls\Main\ThemeUserControl.xaml"
            this.RenameNode.Click += new System.Windows.RoutedEventHandler(this.RenameNode_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
