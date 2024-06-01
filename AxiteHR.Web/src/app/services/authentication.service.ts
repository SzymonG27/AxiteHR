import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterRequest } from '../models/authentication/RegisterRequest';
import { Environment } from '../core/environment/environment';
import { ApiPaths } from '../core/environment/ApiPaths';
import { LoginRequest } from '../models/authentication/LoginRequest';
import { LoginResponse } from '../models/authentication/LoginResponse';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  public Register(register: RegisterRequest) : Observable<string> {
    return this.http.post<string>(
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
