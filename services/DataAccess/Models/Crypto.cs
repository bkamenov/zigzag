using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Crypto
{
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  public required string MongoId { get; set; } // Maps the MongoDB `_id` field

  [BsonElement("id")]
  public required string Id { get; set; } // e.g., "ethereum"

  [BsonElement("symbol")]
  public required string Symbol { get; set; } // e.g., "eth"

  [BsonElement("name")]
  public required string Name { get; set; } // e.g., "Ethereum"

  [BsonElement("image")]
  public required string Image { get; set; } // URL for the crypto image

  [BsonElement("current_price")]
  public required decimal CurrentPrice { get; set; }

  [BsonElement("market_cap")]
  public required decimal MarketCap { get; set; }

  [BsonElement("market_cap_rank")]
  public required int MarketCapRank { get; set; }

  [BsonElement("fully_diluted_valuation")]
  public required decimal FullyDilutedValuation { get; set; }

  [BsonElement("total_volume")]
  public required decimal TotalVolume { get; set; }

  [BsonElement("high_24h")]
  public required decimal High24h { get; set; }

  [BsonElement("low_24h")]
  public required decimal Low24h { get; set; }

  [BsonElement("price_change_24h")]
  public required decimal PriceChange24h { get; set; }

  [BsonElement("price_change_percentage_24h")]
  public required decimal PriceChangePercentage24h { get; set; }

  [BsonElement("market_cap_change_24h")]
  public required decimal MarketCapChange24h { get; set; }

  [BsonElement("market_cap_change_percentage_24h")]
  public required decimal MarketCapChangePercentage24h { get; set; }

  [BsonElement("circulating_supply")]
  public required decimal CirculatingSupply { get; set; }

  [BsonElement("total_supply")]
  public required decimal TotalSupply { get; set; }

  [BsonElement("max_supply")]
  public decimal? MaxSupply { get; set; } // Nullable

  [BsonElement("ath")]
  public required decimal Ath { get; set; }

  [BsonElement("ath_change_percentage")]
  public required decimal AthChangePercentage { get; set; }

  [BsonElement("ath_date")]
  public required DateTime AthDate { get; set; }

  [BsonElement("atl")]
  public required decimal Atl { get; set; }

  [BsonElement("atl_change_percentage")]
  public required decimal AtlChangePercentage { get; set; }

  [BsonElement("atl_date")]
  public required DateTime AtlDate { get; set; }

  [BsonElement("roi")]
  public Roi? Roi { get; set; } // Nullable

  [BsonElement("last_updated")]
  public required DateTime LastUpdated { get; set; }
}

public class Roi
{
  [BsonElement("times")]
  public required decimal Times { get; set; }

  [BsonElement("currency")]
  public required string Currency { get; set; }

  [BsonElement("percentage")]
  public required decimal Percentage { get; set; }
}
