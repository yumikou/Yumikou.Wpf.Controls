using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Yumikou.Wpf.Controls
{
    public class BoxShadows : FreezableCollection<BoxShadow>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BoxShadows();
        }
    }
}
