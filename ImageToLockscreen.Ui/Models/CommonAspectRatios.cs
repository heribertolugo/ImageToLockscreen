using ImageToLockscreen.Ui.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace ImageToLockscreen.Ui.Models
{
    internal class CommonAspectRatios
    {
        public static ObservableCollection<AspectRatio> CommonRatios { get; private set; }
        static CommonAspectRatios()
        {
            CommonRatios = new ObservableCollection<AspectRatio>()
            {
                new AspectRatio(ratio: new Ratio(3,2), bounds: new Size(32,32),description: "Camera",
                    sizes: new[] { new Size(2160, 1440), new Size(2560,1700), new Size(3000,2000),
                        new Size(1500,1000) }),
                new AspectRatio(ratio: new Ratio(4,3), bounds: new Size(32,32),description: "SDTV / PC",
                    sizes: new[] { new Size(2048, 1536), new Size(1600,1200), new Size(640,480),
                        new Size(800,600), new Size(1024,768) }),
                new AspectRatio(ratio: new Ratio(5,4), bounds: new Size(32,32),description: "PC",
                    sizes: new[] { new Size(1280,1024) }),
                new AspectRatio(ratio: new Ratio(16,10), bounds: new Size(32,32),description: "Wide PC",
                    sizes: new[] { new Size(1280,800), new Size(1440,900), new Size(1680,1050),
                        new Size(1920,1200), new Size(2560,1600) }),
                new AspectRatio(ratio: new Ratio(16,9), bounds: new Size(32,32),description: "HDTV",
                    sizes: new[] { new Size(640,360), new Size(1280,720), new Size(1536,864),
                        new Size(1600,900), new Size(1920,1080), new Size(2048,1152), new Size(2560,1440),
                        new Size(3840, 2160), new Size(1360,768)}),
                new AspectRatio(ratio: new Ratio(1.85,1), bounds: new Size(32,32),description: "Cinema"),
                new AspectRatio(ratio: new Ratio(2.35,1), bounds: new Size(32,32),description: "Cinemascope"),
                new AspectRatio(ratio: new Ratio(9,16), bounds: new Size(32,32),description: "Mobile Vertical"),
                new AspectRatio(ratio: new Ratio(1,1), bounds: new Size(32,32),description: "Pro",
                    sizes: new[] { new Size(1920,1920) }),
                new AspectRatio(ratio: new Ratio(21,9), bounds: new Size(32,32),description: "UW",
                    sizes: new[] { new Size(3440,1440), new Size(2560,1080) })
            };
        }
    }
}
