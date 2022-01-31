using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace LegacyDesktop {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      SetupAppSettings();
      ShowTableData();

    }

    private void ShowTableData() {
      CloudTable table = GetTableRef("MonthlyTotals");
     //  var query = new TableQuery();
      var query = new TableQuery<Models.MonthlyTotalEntity>();
      var totals = table.ExecuteQuery(query).ToList();
      TotalsDataGrid.ItemsSource = totals;
    }

    private static CloudTable GetTableRef(string tableName) {
      var account = CloudStorageAccount.Parse(_storageSettings.LegacyConnectionString);
      var client = account.CreateCloudTableClient();
      return client.GetTableReference(tableName);
    }

    private static StorageSettings? _storageSettings;
    internal static void SetupAppSettings() {


      var configBuilder = new ConfigurationBuilder()
       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
       .AddJsonFile("appsettings.json")
       .AddUserSecrets<MainWindow>() // this will not work on your computer, unless you choose (Manage User Secrets)!
       .Build();

      _storageSettings = configBuilder.GetSection("StorageSettings").Get<StorageSettings>();
    }
  }
}
