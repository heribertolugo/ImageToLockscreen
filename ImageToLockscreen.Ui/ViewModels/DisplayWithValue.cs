using ImageToLockscreen.Ui.Core;

namespace ImageToLockscreen.Ui.ViewModels
{
    public class DisplayWithValue : PropertyObservable
    {
        public DisplayWithValue() { }
        public DisplayWithValue(string display, BackgroundFillImageOption value, bool isVisible)
        {
            this.Display = display;
            this.Value = value;
            this.IsVisible = isVisible;
        }
        public string Display
        {
            get { return base.GetProperty<string>(); }
            set { base.SetProperty(value); }
        }
        public BackgroundFillImageOption Value
        {
            get { return base.GetProperty<BackgroundFillImageOption>(); }
            set { base.SetProperty(value); }
        }
        public bool IsVisible
        {
            get { return base.GetProperty<bool>(getDefault:() => true); }
            set { base.SetProperty(value); }
        }
    }
}
