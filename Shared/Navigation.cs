using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorGPS.Shared
{
    public class Navigation
    {
        #region Constructor
        public Navigation() =>
            PointNavigationList = new List<NavigationList>();
        #endregion

        /// <summary>
        /// Descrição da primeira marcação
        /// </summary>
        public string StartingPointText { get; set; }

        /// <summary>
        /// Inicio da Marcação do GPS
        /// </summary>
        public NavigationList StartingPoint
        {
            get => (!Equals(PointNavigationList, null) && PointNavigationList.Count() > 0) ? PointNavigationList.FirstOrDefault() : null;
        }

        /// <summary>
        /// Lista de Marcações
        /// </summary>
        public List<NavigationList> PointNavigationList { get; set; }

        /// <summary>
        /// Descrição da ultima marcação
        /// </summary>
        public string EndPointText { get; set; }

        /// <summary>
        /// Ultima marcação do GPS
        /// </summary>
        public NavigationList EndPoint
        {
            get => (!Equals(PointNavigationList, null) && PointNavigationList.Count() > 0) ? PointNavigationList.LastOrDefault() : null;
        }
    }
}
