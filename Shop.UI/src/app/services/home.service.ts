import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Domain } from '../domain';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  
  private httpclient = inject(HttpClient);
  private domain = inject(Domain);

  constructor() { }

  public gethello(token:string): Observable<any>{
    let headerAuthParameter: string = 'Bearer ' + token;
    let httpHeaders: HttpHeaders = new HttpHeaders().set('Authorization', headerAuthParameter)
      .set('accept', 'application/json').set('Content-Type', 'application/json');
      return this.httpclient.get<any>(`${this.domain.ApiUrl}/Home/GetHello`, {headers: httpHeaders});
  }
}

