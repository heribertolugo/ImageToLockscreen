using ImageToLockscreen.Ui.Core;

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
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
        public object Value
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
    }
}
