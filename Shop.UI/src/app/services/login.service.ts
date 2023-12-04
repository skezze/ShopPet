import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Domain } from '../domain';
import { UserView } from '../models/ViewModels/UserView';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private httpclient = inject(HttpClient);
  private domain = inject(Domain);

  constructor() { }

  public login(userView:UserView): Observable<any>{
    let httpHeaders: HttpHeaders = new HttpHeaders().set('accept', 'application/json').set('Content-Type', 'application/json');

    return this.httpclient.post<any>(`${this.domain.ApiUrl}/User/CreateToken`,userView,{headers: httpHeaders});
  }
}
