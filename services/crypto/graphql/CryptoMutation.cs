using GraphQL.Types;
using DataAccess.Repositories;
using GraphQL;

public class CryptoMutation : ObjectGraphType
{
  public CryptoMutation(ICryptoRepository cryptoRepository)
  {
    Field<StringGraphType>("removeCrypto")
        .Description("Remove a crypto record by its ID. Requires admin role.")
        .Argument<NonNullGraphType<StringGraphType>>("id", "The ID of the crypto to remove.")
        .Resolve(context =>
        {
          var id = context.GetArgument<string>("id");
          cryptoRepository.RemoveCryptoAsync(id).Wait();
          return $"Crypto with ID {id} removed successfully.";
        })
        .AuthorizeWithPolicy("AdminUser"); ;
  }
}
