using GraphQL.Types;
using DataAccess.Repositories;
using GraphQL;

public class CryptoQuery : ObjectGraphType
{
  public CryptoQuery(ICryptoRepository cryptoRepository)
  {
    // Define getCryptos query
    Field<ListGraphType<CryptoType>>("getCryptos")
        .Description("Retrieve a list of cryptos with optional search and pagination.")
        .Argument<StringGraphType>("search", "Optional search term to filter cryptos by name.")
        .Argument<IntGraphType>("page", "The page number to retrieve, starting from 1.")
        .Argument<IntGraphType>("pageCount", "The number of items per page.")
        .ResolveAsync(async context =>
        {
          var search = context.GetArgument<string>("search");
          var page = context.GetArgument<int?>("page") ?? 1; // Default to page 1
          var pageCount = context.GetArgument<int?>("pageCount") ?? 10; // Default to 10 items per page
          return await cryptoRepository.GetCryptosAsync(search, page, pageCount);
        })
        .AuthorizeWithPolicy("RegularUser");

    // Define getCryptosCount query
    Field<IntGraphType>("getCryptosCount")
        .Description("Retrieve the total count of cryptos that match the optional search term.")
        .Argument<StringGraphType>("search", "Optional search term to filter cryptos by name.")
        .ResolveAsync(async context =>
        {
          var search = context.GetArgument<string>("search");
          return await cryptoRepository.GetCryptosCountAsync(search);
        })
        .AuthorizeWithPolicy("RegularUser");
  }
}
