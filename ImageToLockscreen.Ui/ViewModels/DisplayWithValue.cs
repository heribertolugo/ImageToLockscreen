using ImageToLockscreen.Ui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class DisplayWithValue : PropertyObservable
    {
        public DisplayWithValue() { }
        public DisplayWithValue(string display, object value)
        {
            Display = display;
            Value = value;
        }
        public string Display
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public object Value
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
