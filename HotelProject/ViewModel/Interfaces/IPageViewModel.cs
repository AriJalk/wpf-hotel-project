using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelProject.ViewModel
{
    public interface IPageViewModel
    {
        string Name { get; }

        bool ShowButton { get; set; }

        void Refresh();
        void Dispose();
    }
}
