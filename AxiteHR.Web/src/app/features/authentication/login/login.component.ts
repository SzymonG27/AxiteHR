import { Component } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { LoginRequest } from '../../../models/authentication/LoginRequest';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse, HttpEvent, HttpEventType, HttpHeaderResponse, HttpResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { LoginResponse } from '../../../models/authentication/LoginResponse';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  focusEmail: boolean = false;
  focusPassword: boolean = false;
  showPassword: boolean = false;
  registrationMessage: string | null = null;
  loginModel: LoginRequest = new LoginRequest();

  constructor(private authService: AuthenticationService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route?.queryParams?.subscribe((params: { [x: string]: string; }) => {
      if (params['registered'] === 'true') {
        this.registrationMessage = 'Registration successful! Please log in.';
      }
    });
  }

  login(loginModel: LoginRequest) {
    this.authService.Login(loginModel).subscribe({
      next: (response: LoginResponse) => {
        if (response.IsLoggedSuccessful && response.Token) {
          localStorage.setItem('authToken', response.Token); 
        } else if (!response.IsLoggedSuccessful) {
          console.log(response.ErrorMessage);
        }
      },
      error: (response: HttpErrorResponse) => {
        //ToDo
      }
    });
  }

  onFocus(field: string) {
    if (field === 'email') {
      this.focusEmail = true;
    } else if (field === 'password') {
      this.focusPassword = true;
    }
  }

  onBlur(field: string) {
    if (field === 'email') {
      this.focusEmail = false;
    } else if (field === 'password') {
      this.focusPassword = false;
    }
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
