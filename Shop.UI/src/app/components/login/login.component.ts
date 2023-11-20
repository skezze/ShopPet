import { Component, OnDestroy, inject } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { LoginService } from '../../services/login.service';
import { JsonPipe } from '@angular/common';
import { UserView } from '../../models/ViewModels/UserView';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnDestroy {
  
  private router = inject(Router);
  private loginservice = inject(LoginService);
  private cookieService = inject(CookieService);
  private loginSubscription?:Subscription;
  
  
  public goToRegisterComponent(){
    this.router.navigate(['/register']);
  }
  public login()
  { 
    this.loginSubscription = this.loginservice.login(JSON.stringify(new UserView("string","string"))).subscribe(
      data => {
        let accesString = data;
        this.cookieService.set('accessToken', accesString.toString())
      },
      error => console.log(error)
    );
  }

  ngOnDestroy(): void {
    this.loginSubscription?.unsubscribe();
  }

}

