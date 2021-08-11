import { PostDto, PostCreationDto } from './post.modals';
import { environment } from './../../environments/environment.prod';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PostServiceService {

  private apiURL = environment.apiURL + '/Story';

  private postSource = new Subject<PostDto>();
  postUpdated$ = this.postSource.asObservable();

  constructor(private http: HttpClient) { }

  get(page: number, pageSize: number, editorId: string | null): Observable<any> {
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('recordsPerPage', pageSize.toString());
    params = params.append('editorId', editorId ?? '');
    return this.http.get<PostDto[]>(this.apiURL + '/list', { observe: 'response', params });
  }

  create(actor: PostCreationDto): Observable<any> {
    return this.http.post(this.apiURL, actor);
  }

  listStatData(page: number, pageSize: number): Observable<any> {
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('recordsPerPage', pageSize.toString());
    return this.http.get<any[]>(this.apiURL + '/listStatData', { observe: 'response', params });
  }

  postUpdated(data: PostDto) {
    this.postSource.next(data);
  }



}
