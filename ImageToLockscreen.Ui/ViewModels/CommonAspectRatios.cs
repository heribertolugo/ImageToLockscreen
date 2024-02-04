using ImageToLockscreen.Ui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageToLockscreen.Ui.ViewModels
{
    internal class CommonAspectRatios
    {
        private ObservableCollection<AspectRatio> _commonRatios;

        public CommonAspectRatios()
        {

        }

        private void init()
        {
            this._commonRatios = new ObservableCollection<AspectRatio>()
            {
                new AspectRatio(new Ratio(3,2), new Size(32,32), "Camera"),
            };
        }
    }
}
