import { ApolloClient, DefaultOptions, InMemoryCache, gql } from '@apollo/client';
import { injectable } from 'inversify';
import endpoints from '../endpoints';

@injectable()
class CryptoService {
  private client: ApolloClient<any>;

  constructor() {
    const defaultOptions: DefaultOptions = {
      query: {
        fetchPolicy: 'network-only', // Always fetch fresh data for queries
      },
      mutate: {
        fetchPolicy: 'no-cache', // No caching for mutations
      },
    };

    this.client = new ApolloClient({
      uri: endpoints.GRAPHQL,
      cache: new InMemoryCache(),
      defaultOptions
    });
  }

  private getAuthHeaders(token: string | null) {
    return token ? { Authorization: `Bearer ${token}` } : {};
  }

  async getCryptosCount(search: string, token: string | null): Promise<number> {
    const result = await this.client.query({
      query: gql`
        query {
          getCryptosCount(search: "${search}")
        }
      `,
      context: {
        headers: this.getAuthHeaders(token),
      },
    });
    return result.data.getCryptosCount;
  }

  async getCryptos(search: string, page: number, pageCount: number, token: string | null): Promise<any[]> {
    const result = await this.client.query({
      query: gql`
        query {
          getCryptos(search: "${search}", page: ${page}, pageCount: ${pageCount}) {
            id
            name
            currentPrice
            marketCap
          }
        }
      `,
      context: {
        headers: this.getAuthHeaders(token),
      },
    });
    return result.data.getCryptos;
  }

  async removeCrypto(id: string, token: string | null): Promise<void> {
    await this.client.mutate({
      mutation: gql`
        mutation {
          removeCrypto(id: "${id}")
        }
      `,
      context: {
        headers: this.getAuthHeaders(token),
      },
    });
  }
}

export default CryptoService;
