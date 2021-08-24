using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGPS.Shared
{
    public class LayoutList
    {
        public LayoutList(string Link, string Name)
        {
            this.Link = Link;
            this.Name = Name;
        }
        public string Link { get; set; }
        public string Name { get; set; }
    }
}
