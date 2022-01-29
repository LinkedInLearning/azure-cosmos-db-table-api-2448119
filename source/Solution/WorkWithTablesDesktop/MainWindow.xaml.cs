using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WorkWithTablesDesktop {

  #region Comments

  //     The [TableServiceClient] provides synchronous and asynchronous
  //     methods to perform table level operations with Azure Tables
  //     hosted in Azure storage accounts or Azure Cosmos DB table API.

  //     The [TableClient] allows you to interact with Azure Tables
  //     hosted in Azure storage accounts or Azure Cosmos DB table API.

  //     The [TableEntity] A generic dictionary-like Azure.Data.Tables.ITableEntity type
  //     which defines an
  //     arbitrary set of properties on an entity as key-value pairs.

  //     Create a custom [TableEntity] to get strongly typed properties for the entity.

  #endregion Comments

  public partial class MainWindow : Window {

    public MainWindow() {
      InitializeComponent();
      SetupAppSettings();
    }

    private static TableSettings? _tableSettings;

    internal static void SetupAppSettings() {
      // best practice is to remove application secrets (like keys, passwords etc) from code
      // especially when the code is in source control (and visible to many eyes).

      // use the AddUserSecrets (to get access to the local secrets file)

      var configBuilder = new ConfigurationBuilder()
       .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
       .AddJsonFile("appsettings.json")
       .AddUserSecrets<MainWindow>() // this will not work on your computer, unless you choose (Manage User Secrets)!
       .Build();

      _tableSettings = configBuilder.GetSection("TableSettings").Get<TableSettings>();
    }

    private TableServiceClient GetTableServiceClient() {
      //   The [TableServiceClient] provides synchronous and asynchronous
      //   methods to perform table level operations with Azure Tables
      //   hosted in Azure storage accounts or Azure Cosmos DB table API.
      var siteCredential = new TableSharedKeyCredential(accountName: _tableSettings.AccountName,
                                                        accountKey: _tableSettings.DbPrimaryAccessKey);

      var serviceClient = new TableServiceClient(endpoint: new Uri(_tableSettings.EndPoint),
                                                 credential: siteCredential);
      return serviceClient;
    }

    private TableClient GetTableClient() {
      //   The [TableClient] allows you to interact with Azure Tables
      //  hosted in Azure storage accounts or Azure Cosmos DB table API.
      var siteCredential = new TableSharedKeyCredential(accountName: _tableSettings.AccountName,
                                               accountKey: _tableSettings.DbPrimaryAccessKey);
      var tableClient = new TableClient(endpoint: new Uri(_tableSettings.EndPoint),
                                        tableName: _tableSettings.TableName,
                                        credential: siteCredential);

      return tableClient;
    }

    private void RefreshTableNamesButton_Click(object sender, RoutedEventArgs e) {
      BindTableData();
      // foreach (TableItem table in queryTableResults)
      //{
      //  Console.WriteLine("  " + table.Name);
      //}
    }

    private void BindTableData() {
      TableServiceClient serviceClient = GetTableServiceClient();
      Azure.Pageable<TableItem> queryTableResults = serviceClient.Query();

      tableListBox.ItemsSource = queryTableResults;
    }

    private void AddTableButton_Click(object sender, RoutedEventArgs e) {
      TableServiceClient serviceClient = GetTableServiceClient();

      var table = serviceClient.CreateTableIfNotExists(NewTableTextbox.Text);
      BindTableData();
      if (table == null)
      {
        MessageBox.Show($"{NewTableTextbox.Text} already exists in Cosmos DB");
      }
    }

    private void RefreshRowsButton_Click(object sender, RoutedEventArgs e) {
      TableClient tableClient = GetTableClient();
      var filterString = $"BidPrice gt 10.00";
      Azure.Pageable<TableEntity> pageableEntities = tableClient.Query<TableEntity>(filter: filterString);

      // An entity has a set of properties, and each property is a name, typed-value pair.
      // Entities can be strongly-typed or in dictionary form using the TableEntity class.

      foreach (TableEntity row in pageableEntities)
      {
        string output = string.Empty;
        foreach (var key in row.Keys)
        {
          if (key != "odata.etag")
          {
            output = output + $"{key}: {row[key]}, ";
          }
        }
        ReadTableListBox.Items.Add(output);
        output = string.Empty;
      }
    }

    private void RefreshRowsModelButton_Click(object sender, RoutedEventArgs e) {
      // using the custom TradingCardEntity type instead of TableEntity
      // we can access the properties of the row, instead of using dictionary keys
      TableClient tableClient = GetTableClient();
      var filterString = $"BidPrice gt 10.00";
      Azure.Pageable<Models.TradingCardEntity> pageableCards;

      pageableCards = tableClient.Query<Models.TradingCardEntity>(filter: filterString);
      // or  use LinqExpression

      //pageableCards = tableClient.Query<Models.TradingCardEntity>
      //  (filter: (e => e.BidPrice < 10 && e.CardFamily == "Stargazers"));
      CardsDataGrid1.ItemsSource = pageableCards;
      // ReadTableModelListBox.ItemsSource = pageableCards;
    }

    private void AddCardButton_Click(object sender, RoutedEventArgs e) {
      var card = new Models.TradingCardEntity();
      card.PartitionKey = "301";
      card.RowKey = "Frex";
      card.PopularityIndex = 370;
      card.BidPrice = 16.50;
      card.CardFamily = "LiL' Monsters";
      card.Slogan = "Born Leader";
      card.TeamName = "SpookTones";

      TableClient client = GetTableClient();

      //  Replaces the specified table entity of type T, if it exists.
      //  Creates the entity if it does not exist.
      client.UpsertEntity(card);

      UpdateRowsListBox();
    }

    private void UpdateCardButton_Click(object sender, RoutedEventArgs e) {
      var card = new Models.TradingCardEntity();
      card.PartitionKey = "301";
      card.RowKey = "Frex";
      card.BidPrice = new Random().Next(minValue: 19, maxValue: 25);

      TableClient client = GetTableClient();
      client.UpsertEntity(card);

      UpdateRowsListBox();
    }

    private void DeleteCardButton_Click(object sender, RoutedEventArgs e) {
      TableClient client = GetTableClient();
      client.DeleteEntity(partitionKey: "301", rowKey: "Frex");
      UpdateRowsListBox();
    }

    private void UpdateRowsListBox() {
      TableClient client = GetTableClient();
      Azure.Pageable<Models.TradingCardEntity> queryRowResults;
      queryRowResults = client.Query<Models.TradingCardEntity>();
      ShowRowsListBox.ItemsSource = queryRowResults;
    }

    private void UpdateRowsDataGrid() {
      TableClient client = GetTableClient();
      Azure.Pageable<Models.TradingCardEntity> cards;
      cards = client.Query<Models.TradingCardEntity>();
      CardsDataGrid.ItemsSource = CardsDataGrid1.ItemsSource = cards;
    }

    private void GetSingleItemButton_Click(object sender, RoutedEventArgs e) {
      TableClient tableClient = GetTableClient();
      Models.TradingCardEntity card = tableClient.GetEntity<Models.TradingCardEntity>(partitionKey: "102", rowKey: "Yodel");

      SingleItemTextBlock.Text = $"Name: {card.RowKey}, TeamName: {card.TeamName}, BidPrice: {card.BidPrice}";
    }

    private void BatchTransactionsButton_Click(object sender, RoutedEventArgs e) {
      // to batch transactions, build an enumerable of TableTransactionActions.
      // Executing the transaction is accomplished by passing this collection
      // to the SubmitTransaction method on the TableClient.

      TableClient tableClient = GetTableClient();

      var newCard = new Models.TradingCardEntity();
      newCard.PartitionKey = "409";
      newCard.RowKey = "Zoronette";
      newCard.PopularityIndex = 988;
      newCard.BidPrice = 35.13;
      newCard.CardFamily = "StarGazers";
      newCard.Planet = "Planet 998";
      newCard.TeamName = "SuperNovas";

      // batch only works on the same partition!
      var anotherCard = new Models.TradingCardEntity();
      anotherCard.PartitionKey = "409";
      anotherCard.RowKey = "Xanfer";

      anotherCard.UnitsSold = 43;

      // Create a collection of TableTransactionActions and populate it with the actions for each entity.
      var batch = new List<TableTransactionAction>
      {
          new TableTransactionAction(TableTransactionActionType.UpsertReplace, newCard),
         new TableTransactionAction(TableTransactionActionType.UpsertReplace, anotherCard)
      };

      Response<IReadOnlyList<Response>> batchResult = tableClient.SubmitTransaction(batch);
      UpdateRowsDataGrid();
    }

    private void UndoBatchTransactionsButton_Click(object sender, RoutedEventArgs e) {
      TableClient tableClient = GetTableClient();

      var deleteCard = new Models.TradingCardEntity();
      deleteCard.PartitionKey = "409";
      deleteCard.RowKey = "Zoronette";

      var deleteAnotherCard = new Models.TradingCardEntity();
      deleteAnotherCard.PartitionKey = "409";
      deleteAnotherCard.RowKey = "Xanfer";

      var batch = new List<TableTransactionAction>
      {
          new TableTransactionAction(TableTransactionActionType.Delete,deleteCard ),
          new TableTransactionAction(TableTransactionActionType.Delete,deleteAnotherCard )
      };

      Response<IReadOnlyList<Response>> batchResult = tableClient.SubmitTransaction(batch);
      UpdateRowsDataGrid();
    }

    private void CardsDataGrid_AutoGeneratedColumns(object sender, EventArgs e) {
      var grid = sender as DataGrid;

      grid.Columns.FirstOrDefault(x => x.Header.ToString() == "PartitionKey").DisplayIndex = 0;
      grid.Columns.FirstOrDefault(x => x.Header.ToString() == "RowKey").DisplayIndex = 1;
      grid.Columns.FirstOrDefault(x => x.Header.ToString() == "TeamName").DisplayIndex = 2;
    }

    private void BatchTabItem_Loaded(object sender, RoutedEventArgs e) {
      UpdateRowsDataGrid();
    }

    private void TabItem_Loaded(object sender, RoutedEventArgs e) {
      UpdateRowsListBox();
    }
  }
}