﻿#pragma checksum "..\..\..\..\Teacher\UserControls\ThemeControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1316800F18D684B42B5D58B3172050245108637E116158C857D42217EF1CBCAB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using StudentsControl.Teacher.UserControls;
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


namespace StudentsControl.Teacher.UserControls {
    
    
    /// <summary>
    /// ThemeControl
    /// </summary>
    public partial class ThemeControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView ThemeView;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem AddNode;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem DeleteNode;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
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
            System.Uri resourceLocater = new System.Uri("/StudentsControl;component/teacher/usercontrols/themecontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
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
            this.ThemeView = ((System.Windows.Controls.TreeView)(target));
            
            #line 10 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
            this.ThemeView.PreviewMouseRightButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ThemeView_OnPreviewMouseRightButtonDown);
            
            #line default
            #line hidden
            
            #line 10 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
            this.ThemeView.ContextMenuOpening += new System.Windows.Controls.ContextMenuEventHandler(this.ContextMenu_ContextMenuOpening);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 13 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
            this.AddNode.Click += new System.Windows.RoutedEventHandler(this.AddNode_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.DeleteNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 14 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
            this.DeleteNode.Click += new System.Windows.RoutedEventHandler(this.DeleteNode_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RenameNode = ((System.Windows.Controls.MenuItem)(target));
            
            #line 15 "..\..\..\..\Teacher\UserControls\ThemeControl.xaml"
            this.RenameNode.Click += new System.Windows.RoutedEventHandler(this.RenameNode_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

