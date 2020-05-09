import { Routes } from '@angular/router';
import { HomeComponent } from './Home/home.component';
import { VehiclesComponent } from './vehicles/vehicle.component';
export const AppRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'vehicles', component: VehiclesComponent },
];
