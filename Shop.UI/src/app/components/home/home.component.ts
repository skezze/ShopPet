import { Component, OnDestroy, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Subscription } from 'rxjs';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnDestroy {
  
    private router = inject(Router);
    private cookieService = inject(CookieService);
    private homeService = inject(HomeService);
    private getHelloSubscription?:Subscription;

    public gethello()
    { 
      let token:string = this.cookieService.get('accessToken');
      this.getHelloSubscription = this.homeService.gethello(token).subscribe(
        error => console.log(error));
    }

    ngOnDestroy(): void {
      this.getHelloSubscription?.unsubscribe();
    }

}
