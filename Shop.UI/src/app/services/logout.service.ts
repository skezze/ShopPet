import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Domain } from '../domain';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LogoutService {

  private router = inject(Router);
  private cookieService = inject(CookieService);

  constructor() { }

  public logout(credentialsJson:string){
    this.cookieService.delete('accessToken');
    this.router.navigate(['/home']);
  }
}
