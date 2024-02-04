using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToLockscreen.Ui.Models
{
    public struct Ratio
    {
        public Ratio(double width, double height) 
        { 
            this.Width = width;
            this.Height = height;
        }
        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}
