﻿#pragma checksum "..\..\MainWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0F6606A277906FC6F728E0F82B426CB997F98357CD313FA0F7C3884458AD7B23"
//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

using Quest_Song_Exporter;
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


namespace Quest_Song_Exporter {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sr;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtbox;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxs;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtboxd;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox box;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox index;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox auto;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox Quest;
        
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
            System.Uri resourceLocater = new System.Uri("/Quest Song Exporter;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainWindow.xaml"
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
            
            #line 11 "..\..\MainWindow.xaml"
            ((Quest_Song_Exporter.MainWindow)(target)).MouseMove += new System.Windows.Input.MouseEventHandler(this.Drag);
            
            #line default
            #line hidden
            return;
            case 2:
            this.sr = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\MainWindow.xaml"
            this.sr.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 19 "..\..\MainWindow.xaml"
            this.sr.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 19 "..\..\MainWindow.xaml"
            this.sr.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtbox = ((System.Windows.Controls.TextBox)(target));
            
            #line 20 "..\..\MainWindow.xaml"
            this.txtbox.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 20 "..\..\MainWindow.xaml"
            this.txtbox.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 21 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 22 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 22 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 22 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtboxs = ((System.Windows.Controls.TextBox)(target));
            
            #line 23 "..\..\MainWindow.xaml"
            this.txtboxs.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 23 "..\..\MainWindow.xaml"
            this.txtboxs.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtboxd = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\MainWindow.xaml"
            this.txtboxd.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 24 "..\..\MainWindow.xaml"
            this.txtboxd.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 8:
            this.box = ((System.Windows.Controls.CheckBox)(target));
            
            #line 25 "..\..\MainWindow.xaml"
            this.box.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 25 "..\..\MainWindow.xaml"
            this.box.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 9:
            this.index = ((System.Windows.Controls.CheckBox)(target));
            
            #line 26 "..\..\MainWindow.xaml"
            this.index.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 26 "..\..\MainWindow.xaml"
            this.index.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 27 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.AccessText)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 27 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.AccessText)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 28 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 28 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 28 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Close);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 29 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 29 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 29 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Mini);
            
            #line default
            #line hidden
            return;
            case 13:
            this.auto = ((System.Windows.Controls.CheckBox)(target));
            
            #line 39 "..\..\MainWindow.xaml"
            this.auto.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 39 "..\..\MainWindow.xaml"
            this.auto.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 39 "..\..\MainWindow.xaml"
            this.auto.Click += new System.Windows.RoutedEventHandler(this.Auto);
            
            #line default
            #line hidden
            return;
            case 14:
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 40 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Backup);
            
            #line default
            #line hidden
            return;
            case 15:
            this.Quest = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\MainWindow.xaml"
            this.Quest.GotFocus += new System.Windows.RoutedEventHandler(this.ClearText);
            
            #line default
            #line hidden
            
            #line 41 "..\..\MainWindow.xaml"
            this.Quest.MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 41 "..\..\MainWindow.xaml"
            this.Quest.MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 42 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.noDrag);
            
            #line default
            #line hidden
            
            #line 42 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.doDrag);
            
            #line default
            #line hidden
            
            #line 42 "..\..\MainWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Restore);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

