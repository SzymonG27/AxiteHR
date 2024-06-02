import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { LoginRequest } from '../../../models/authentication/LoginRequest';
import { LoginResponse } from '../../../models/authentication/LoginResponse';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  focusEmail: boolean = false;
  focusPassword: boolean = false;
  showPassword: boolean = false;
  loginModel: LoginRequest = new LoginRequest();

  constructor(private authService: AuthenticationService) {}

  login(loginModel: LoginRequest) {
    this.authService.Login(loginModel).subscribe((loginReponse: LoginResponse) => {
      localStorage.setItem('authToken', loginReponse.Token);
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
