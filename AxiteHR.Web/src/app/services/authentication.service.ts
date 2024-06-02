import { HttpClient, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../models/authentication/RegisterRequest';
import { Environment } from '../core/environment/Environment';
import { ApiPaths } from '../core/environment/ApiPaths';
import { LoginRequest } from '../models/authentication/LoginRequest';
import { LoginResponse } from '../models/authentication/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  public Register(register: RegisterRequest) : Observable<HttpEvent<any>> {
    return this.http.post<HttpEvent<any>>(
      `${Environment.authApiBaseUrl}${ApiPaths.Register}`,
      register
    );
  }

  public Login(login: LoginRequest) : Observable<LoginResponse> {
    return this.http.post<LoginResponse>(
      `${Environment.authApiBaseUrl}${ApiPaths.Login}`,
      login
    );
  }
}
