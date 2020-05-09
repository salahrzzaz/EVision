import { Injectable } from '@angular/core';
import { AppService } from '../app.service';
import { Observable } from 'rxjs';
@Injectable()
export class CustomerService  {
constructor(private _service :AppService){

}
    getCustomers(): Observable<any> {
        let url = '/api/Customers';
        return this._service
            .get(url);
    }
}

