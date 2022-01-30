using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyDesktop.Models {
  internal class MonthlyTotalEntity :TableEntity  {

    public int CardsSold { get; set; }
    public int ActionFiguresSold { get; set; }
    public int ComicsSold { get; set; }
  }
}
