﻿#pragma checksum "..\..\ThreadWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "03ABB96A81A5E668F9A2A125BBB44EEE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using GUI;
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


namespace GUI {
    
    
    /// <summary>
    /// ThreadWindow
    /// </summary>
    public partial class ThreadWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\ThreadWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView postsTV;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\ThreadWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button subForumBtn;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\ThreadWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addReplyBtn;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\ThreadWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deletePostBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/GUI;component/threadwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ThreadWindow.xaml"
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
            this.postsTV = ((System.Windows.Controls.TreeView)(target));
            
            #line 10 "..\..\ThreadWindow.xaml"
            this.postsTV.Loaded += new System.Windows.RoutedEventHandler(this.postsTV_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.subForumBtn = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\ThreadWindow.xaml"
            this.subForumBtn.Click += new System.Windows.RoutedEventHandler(this.subForumBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.addReplyBtn = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\ThreadWindow.xaml"
            this.addReplyBtn.Click += new System.Windows.RoutedEventHandler(this.addReplyBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.deletePostBtn = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\ThreadWindow.xaml"
            this.deletePostBtn.Click += new System.Windows.RoutedEventHandler(this.deletePostBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

