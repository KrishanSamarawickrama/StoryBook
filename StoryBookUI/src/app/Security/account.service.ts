import { AuthResponse, UserCredentials, UserCreationDto, UsersDto } from './account.modals';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly apiURL = environment.apiURL + '/Accounts';
  private readonly tokenKey = "token-key";
  private readonly expirationTokenKey = 'token-expiration';
  private readonly roleFelid = 'userRole';

  constructor(private http: HttpClient) { }

  getUsers(page: number, recordsPerPage: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('recordsPerPage', recordsPerPage.toString());
    return this.http.get<UsersDto[]>(`${this.apiURL}/list`, { observe: 'response', params });
  }

  login(dto: UserCredentials): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.apiURL + '/login', dto);
  }

  logOut(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expirationTokenKey);
  }

  create(dto: UserCreationDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(this.apiURL + '/create', dto);
  }

  makeWriter(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/makeWriter', JSON.stringify(userId), { headers });
  }

  removeWriter(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/removeWriter', JSON.stringify(userId), { headers });
  }

  banUser(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/banUser', JSON.stringify(userId), { headers });
  }

  unBanUser(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/unBanUser', JSON.stringify(userId), { headers });
  }

  saveToken(res: AuthResponse) {
    localStorage.setItem(this.tokenKey, res.token);
    localStorage.setItem(this.expirationTokenKey, res.expiration.toString());
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRole(): string {
    return this.getFieldFromJwt(this.roleFelid);
  }

  getFieldFromJwt(field: string): string {
    const token = localStorage.getItem(this.tokenKey);
    if (!token) { return ''; }
    const dataToken = JSON.parse(atob(token.split('.')[1]));
    return dataToken[field];
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    if (!token) {
      return false;
    }
    const expiration: string = localStorage.getItem(this.expirationTokenKey) ?? '';
    const expirationdate = new Date(expiration);
    if (expirationdate <= new Date()) {
      this.logOut();
      return false;
    }
    return true;
  }

}
