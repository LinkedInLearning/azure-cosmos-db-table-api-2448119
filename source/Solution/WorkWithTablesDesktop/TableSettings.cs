using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithTablesDesktop {
  public class TableSettings {
    public string EndPoint { get; set; }
    public string DbPrimaryAccessKey { get; set; }
    public string DbSecondaryAccessKey { get; set; }
    public string TableName { get; set; }
    public string AccountName { get; set; }

    public string SecretWord { get; set; } // not really an Azure Table setting :

  }
}
