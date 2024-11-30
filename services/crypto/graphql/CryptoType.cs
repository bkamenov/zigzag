using GraphQL.Types;

public class CryptoType : ObjectGraphType<Crypto>
{
  public CryptoType()
  {
    Field(c => c.Id).Description("The ID of the crypto, e.g., 'ethereum'.");
    Field(c => c.Symbol).Description("The symbol of the crypto, e.g., 'eth'.");
    Field(c => c.Name).Description("The name of the crypto, e.g., 'Ethereum'.");
    Field(c => c.Image).Description("The image URL of the crypto.");
    Field(c => c.CurrentPrice).Description("The current price of the crypto.");
    Field(c => c.MarketCap).Description("The market capitalization of the crypto.");
    Field(c => c.MarketCapRank).Description("The market cap rank of the crypto.");
    Field(c => c.FullyDilutedValuation).Description("The fully diluted valuation of the crypto.");
    Field(c => c.TotalVolume).Description("The total volume of the crypto.");
    Field(c => c.High24h).Description("The highest price in the last 24 hours.");
    Field(c => c.Low24h).Description("The lowest price in the last 24 hours.");
    Field(c => c.PriceChange24h).Description("The price change in the last 24 hours.");
    Field(c => c.PriceChangePercentage24h).Description("The price change percentage in the last 24 hours.");
    Field(c => c.MarketCapChange24h).Description("The market capitalization change in the last 24 hours.");
    Field(c => c.MarketCapChangePercentage24h).Description("The market cap change percentage in the last 24 hours.");
    Field(c => c.CirculatingSupply).Description("The circulating supply of the crypto.");
    Field(c => c.TotalSupply).Description("The total supply of the crypto.");
    Field(c => c.MaxSupply, nullable: true).Description("The maximum supply of the crypto, if available.");
    Field(c => c.Ath).Description("The all-time high price of the crypto.");
    Field(c => c.AthChangePercentage).Description("The percentage change from the all-time high price.");
    Field(c => c.AthDate).Description("The date of the all-time high price.");
    Field(c => c.Atl).Description("The all-time low price of the crypto.");
    Field(c => c.AtlChangePercentage).Description("The percentage change from the all-time low price.");
    Field(c => c.AtlDate).Description("The date of the all-time low price.");

    // Updated usage of FieldBuilder for nested type
    Field<RoiType>("roi")
        .Description("The return on investment data for the crypto, if available.")
        .Resolve(context => context.Source.Roi);

    Field(c => c.LastUpdated).Description("The last updated timestamp for the crypto data.");
  }
}

public class RoiType : ObjectGraphType<Roi>
{
  public RoiType()
  {
    Field(r => r.Times).Description("The ROI multiple times value.");
    Field(r => r.Currency).Description("The currency for the ROI calculation.");
    Field(r => r.Percentage).Description("The ROI percentage value.");
  }
}
