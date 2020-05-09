import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutes } from './app.route';
import { AppComponent } from './app.component';
import { AppService } from './app.service';
import { HomeComponent } from './Home/home.component';
import { FormsModule }   from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CustomerService } from './services/customer.service';
import { NavigationBarComponent } from './navmenue/nav.component';
import { VehiclesComponent } from './vehicles/vehicle.component';
import { VehiclesService } from './services/vehicle.service';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavigationBarComponent,
    VehiclesComponent,

   
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(AppRoutes, { useHash: true })
  ],
  providers: [AppService,CustomerService,VehiclesService],
  bootstrap: [AppComponent]
})
export class AppModule { }
