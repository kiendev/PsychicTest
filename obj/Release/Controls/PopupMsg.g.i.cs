﻿#pragma checksum "C:\Users\kiennt45\Downloads\PsychicTest\PsychicTest\Controls\PopupMsg.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D74E96705DC855D821F58F37AB7A991A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PsychicTest.Controls {
    
    
    public partial class PopupMsg : System.Windows.Controls.UserControl {
        
        internal System.Windows.Media.Animation.Storyboard AnimationHide;
        
        internal System.Windows.Media.Animation.Storyboard AnimationShow;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock TxtTitle;
        
        internal System.Windows.Controls.Image ImgWarning;
        
        internal System.Windows.Controls.TextBlock TxtWarning;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/PsychicTest;component/Controls/PopupMsg.xaml", System.UriKind.Relative));
            this.AnimationHide = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimationHide")));
            this.AnimationShow = ((System.Windows.Media.Animation.Storyboard)(this.FindName("AnimationShow")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TxtTitle = ((System.Windows.Controls.TextBlock)(this.FindName("TxtTitle")));
            this.ImgWarning = ((System.Windows.Controls.Image)(this.FindName("ImgWarning")));
            this.TxtWarning = ((System.Windows.Controls.TextBlock)(this.FindName("TxtWarning")));
        }
    }
}

