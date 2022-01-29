using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Configuration;

public class Program {
  private static TableSettings? _appSettings;

  private static void Main()
  {
    //     The [TableServiceClient] provides synchronous and asynchronous
    //     methods to perform table level operations with Azure Tables hosted in either
    //     Azure storage accounts or Azure Cosmos DB table API.

    //     The [TableClient] allows you to interact with Azure Tables hosted
    //     in either Azure storage accounts or Azure Cosmos DB table API.

    //     The [TableEntity] A generic dictionary-like Azure.Data.Tables.ITableEntity type
    //     which defines an
    //     arbitrary set of properties on an entity as key-value pairs.

    SetupAppSettings();

    TableServiceClient serviceClient = GetServiceClient();
    ShowTables(serviceClient);
    // CreateTable(serviceClient);

    // ReadTableRow(serviceClient);
    Console.ReadLine();
  }

  private static TableServiceClient GetServiceClient()
  {
    var sharedKeyCredentials
      = new TableSharedKeyCredential(accountName: _appSettings.AccountName,
                                                  accountKey: _appSettings.TableKey);
    var serviceClient = new TableServiceClient(endpoint: new Uri(_appSettings.EndPoint),
                                              credential: sharedKeyCredentials);

    return serviceClient;
  }

  internal static void ShowTables(TableServiceClient serviceClient)
  {
    //var filterString = $"TableName eq 'CardSales'";

    Azure.Pageable<TableItem> queryTableResults = serviceClient.Query();

    Console.WriteLine("Table Names:");

    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (TableItem table in queryTableResults)
    {
      Console.WriteLine("  " + table.Name);
    }

    Console.ResetColor();
  }

  internal static void SetupAppSettings()
  {
    // best practice is to not put app secretes (like keys, passwords etc) in code
    // especially when the code is in source control (and visible to many eyes).

    // use the UserSecrets

    ////  var connString = System.Configuration.ConfigurationManager.AppSettings["CosmosTableApi"];

    var configBuilder = new ConfigurationBuilder()
     .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
     .AddJsonFile("appsettings.json")
          .Build();

    _appSettings = configBuilder.GetSection("TableSettings").Get<TableSettings>();
  }

  internal static void CreateTable(TableServiceClient client)
  {
    string tableName = "YearlySales";
    TableItem table = client.CreateTableIfNotExists(tableName);
    Console.WriteLine($"The created table's name is {table.Name}.");
  }

  internal static void ReadTableRow(TableServiceClient serviceClient)
  {
    string endpointUri = _appSettings.EndPoint;

    var credentials = new TableSharedKeyCredential(accountName: _appSettings.AccountName,
                                                   accountKey: _appSettings.TableKey);
    var tableClient = new TableClient(endpoint: new Uri(_appSettings.EndPoint),
                                       tableName: _appSettings.TableName,
                                       credential: credentials);
    var filterString = $"BidPrice gt 10.00";
    Azure.Pageable<TableEntity> queryRowResults = tableClient.Query<TableEntity>(filter: filterString);

    foreach (TableEntity row in queryRowResults)
    {
      Console.WriteLine($"{row.GetString("RowKey")}: \t{row.GetDouble("BidPrice")}");
    }
    // Create the table in the service.
    // tableClient.Create();
  }

  public class TableSettings {
    public string EndPoint { get; set; }
    public string TableKey { get; set; }
    public string TableName { get; set; }
    public string AccountName { get; set; }
  }
}