using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithTablesDesktop.Models {
  public class TradingCardEntity : ITableEntity {

    public double BidPrice { get; set; }
    public string CardFamily { get; set; }
    public string Planet { get; set; }
    public int PopularityIndex { get; set; }
    public string Slogan { get; set; }
    public int UnitsSold { get; set; }
    public string TeamName { get; set; }
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
  }


}
