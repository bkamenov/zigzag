using GraphQL.Types;

public class CryptoSchema : Schema
{
  public CryptoSchema(IServiceProvider serviceProvider) : base(serviceProvider)
  {
    Query = serviceProvider.GetRequiredService<CryptoQuery>();
    Mutation = serviceProvider.GetRequiredService<CryptoMutation>();
  }
}
