import { Injectable } from '@angular/core';
import { AppService } from '../app.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
@Injectable()
export class VehiclesService {

    constructor(protected _service: AppService) {

    }
    getVehicles(customerId, status): Observable<any> {
        let url = `/api/Vehicles/GetVehiclesWithCustomers?customerId=${customerId}`;
        if (status != -1) {
            url += `&statues=${status}`

        }
        return this._service
            .get(url);
    }

}

