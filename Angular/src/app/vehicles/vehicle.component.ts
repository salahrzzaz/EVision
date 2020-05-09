import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { VehiclesService } from '../services/vehicle.service';
import { CustomerService } from '../services/customer.service';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'vehicle',
  templateUrl: 'vehicle.template.html'
})
// tslint:disable-next-line:component-class-suffix
export class VehiclesComponent {
  Vehicles: any;
  Customers: any;

  customerId = 0;
  status = -1;
  constructor(
     private _vehiclesService: VehiclesService,
    private _customerService: CustomerService) {
      this.getCustomers();
      this.getvehicles();
  }


  getCustomers() {
    this._customerService.getCustomers().subscribe(res => {
      this.Customers = res.response;

    });
  }

  getvehicles() {
    console.log(this.status);
    this._vehiclesService.getVehicles(this.customerId,this.status).subscribe(res => {
      this.Vehicles = res.response;

    });
  }






}
