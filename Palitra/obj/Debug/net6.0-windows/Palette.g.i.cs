﻿#pragma checksum "..\..\..\Palette.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5D418207DAF08C3948662D277510D356CD4DB36B"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Palitra.CustomElements;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace Palitra {
    
    
    /// <summary>
    /// Palette
    /// </summary>
    public partial class Palette : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel RedPanel;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Palitra.CustomElements.NumberBox RedBox;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel GreenPanel;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Palitra.CustomElements.NumberBox GreenBox;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel BluePanel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Palitra.CustomElements.NumberBox BlueBox;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel HexPanel;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Palette.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Palitra.CustomElements.HexBox HexBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Palitra;V1.0.0.0;component/palette.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Palette.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RedPanel = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.RedBox = ((Palitra.CustomElements.NumberBox)(target));
            
            #line 19 "..\..\..\Palette.xaml"
            this.RedBox.AddHandler(System.Windows.Input.CommandManager.PreviewExecutedEvent, new System.Windows.Input.ExecutedRoutedEventHandler(this.RedBox_PreviewExecuted));
            
            #line default
            #line hidden
            return;
            case 3:
            this.GreenPanel = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            this.GreenBox = ((Palitra.CustomElements.NumberBox)(target));
            
            #line 24 "..\..\..\Palette.xaml"
            this.GreenBox.AddHandler(System.Windows.Input.CommandManager.PreviewExecutedEvent, new System.Windows.Input.ExecutedRoutedEventHandler(this.GreenBox_PreviewExecuted));
            
            #line default
            #line hidden
            return;
            case 5:
            this.BluePanel = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 6:
            this.BlueBox = ((Palitra.CustomElements.NumberBox)(target));
            
            #line 29 "..\..\..\Palette.xaml"
            this.BlueBox.AddHandler(System.Windows.Input.CommandManager.PreviewExecutedEvent, new System.Windows.Input.ExecutedRoutedEventHandler(this.BlueBox_PreviewExecuted));
            
            #line default
            #line hidden
            return;
            case 7:
            this.HexPanel = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 8:
            this.HexBox = ((Palitra.CustomElements.HexBox)(target));
            
            #line 34 "..\..\..\Palette.xaml"
            this.HexBox.AddHandler(System.Windows.Input.CommandManager.PreviewExecutedEvent, new System.Windows.Input.ExecutedRoutedEventHandler(this.HexBox_PreviewExecuted));
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

