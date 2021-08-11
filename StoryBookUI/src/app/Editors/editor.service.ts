import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { EditorDto } from './editor.modals';

@Injectable({
  providedIn: 'root'
})
export class EditorService {

  private apiURL = environment.apiURL + '/Editor';

  constructor(private http: HttpClient) { }

  get(page: number, pageSize: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('recordsPerPage', pageSize.toString());
    return this.http.get<EditorDto[]>(this.apiURL + '/list', { observe: 'response', params });
  }

  followWriter(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/followWriter', JSON.stringify(userId), { headers });
  }

  unFollowWriter(userId: string): Observable<any> {
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(this.apiURL + '/unFollowWriter', JSON.stringify(userId), { headers });
  }

  searchByName(name:string): Observable<any[]>{
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post<any[]>(`${this.apiURL}/searchByName`,JSON.stringify(name),{headers});
  }

}
