﻿using ImageToLockscreen.Ui.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageToLockscreen.Ui.Controls
{
    public class SlideViewer : UserControl
    {
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(UserControl));


        private ObservableCollection<object> _items;
        public SlideViewer()
        {
            this._items = new ObservableCollection<object>();
        }

        public Brush BorderColor
        {
            get { return (Brush)base.GetValue(BorderColorProperty); }
            set { base.SetValue(BorderColorProperty, value); }
        }

        public ObservableCollection<object> Items
        {
            get { return this._items; }
            private set { this._items = value; }
        }
    }

    public class SlideViewerItem : PropertyObservable
    {
        //public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("BorderColor", typeof(Brush), typeof(UserControl));

        public Image Image 
        {
            get { return base.GetProperty<Image>(); } 
            set { base.SetProperty(value); }
        }

        public string Text
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }

        public object Value
        {
            get { return base.GetProperty<object>(); }
            set { base.SetProperty(value); }
        }
    }
}
