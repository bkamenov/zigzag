import { injectable } from 'inversify';
import axios from 'axios';
import endpoints from '../endpoints';

@injectable()
class AuthService {
  private readonly tokenKey = 'jwtToken';

  saveToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  loadToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  clearToken() {
    localStorage.removeItem(this.tokenKey);
  }

  async login(username: string, password: string): Promise<string> {
    const response = await axios.post(endpoints.AUTHORIZATION, { username, password });
    return response.data.accessToken;
  }

  async getLoginInfo(token: string) {
    const response = await axios.get(endpoints.INFO, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  }

  async logout(token: string) {
    await axios.post(endpoints.REVOKE, null, {
      headers: { Authorization: `Bearer ${token}` },
    });
  }
}

export default AuthService;
