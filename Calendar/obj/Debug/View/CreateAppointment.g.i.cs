﻿#pragma checksum "..\..\..\View\CreateAppointment.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DD7DEE6DB728E7F8D50034799410CD21E4C004222EF6B060F7F602D10A5D0A35"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Calendar.View;
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


namespace Calendar.View {
    
    
    /// <summary>
    /// CreateAppointment
    /// </summary>
    public partial class CreateAppointment : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxTitle;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TextBoxDescription;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker DatePickerDateOfEvent;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxInitialHour;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxInitialMinute;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxFinalHour;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ComboBoxFinalMinute;
        
        #line default
        #line hidden
        
        
        #line 195 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonCreate;
        
        #line default
        #line hidden
        
        
        #line 196 "..\..\..\View\CreateAppointment.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonCancel;
        
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
            System.Uri resourceLocater = new System.Uri("/Calendar;component/view/createappointment.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\CreateAppointment.xaml"
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
            this.TextBoxTitle = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.TextBoxDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.DatePickerDateOfEvent = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 4:
            this.ComboBoxInitialHour = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.ComboBoxInitialMinute = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.ComboBoxFinalHour = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.ComboBoxFinalMinute = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 8:
            this.ButtonCreate = ((System.Windows.Controls.Button)(target));
            
            #line 195 "..\..\..\View\CreateAppointment.xaml"
            this.ButtonCreate.Click += new System.Windows.RoutedEventHandler(this.ButtonCreate_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ButtonCancel = ((System.Windows.Controls.Button)(target));
            
            #line 196 "..\..\..\View\CreateAppointment.xaml"
            this.ButtonCancel.Click += new System.Windows.RoutedEventHandler(this.ButtonCancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

