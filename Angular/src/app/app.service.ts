import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable()
export class AppService {


    _baseURL = 'http://localhost:5000';
   
  httpOptions;
  constructor(protected _http: HttpClient) {
    this.httpOptions = {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${localStorage.getItem('dasaToken')}`
      })
    };
  }



  get(nameURL): Observable<any> {
   let url = this._baseURL + nameURL
    return this._http
      .get(`${url}`, this.httpOptions);

  }
  post(item, nameURL): Observable<any> {
    let url = this._baseURL + nameURL
    return this._http
      .post(`${url}`, item, this.httpOptions);
  }
  auth(item, nameURL): Observable<any> {
    let url = this._baseURL + nameURL
    return this._http
      .post(`${url}`, item);
  }
  upload(formData, nameURL): Observable<any> {
    let url = this._baseURL + nameURL
    const req = new HttpRequest('POST', url, formData, this.httpOptions);
    return this._http
      .request(req);
  }
}

