import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { RegisterRequest } from '../../../models/authentication/RegisterRequest';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { mustMatch } from '../../../shared/validators/password-match.validator';
import { HttpErrorResponse, HttpEvent, HttpStatusCode } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  focusEmail: boolean = false;
  focusPassword: boolean = false;
  focusUserName: boolean = false;
  focusFirstName: boolean = false;
  focusLastName: boolean = false;
  errorMessage: string | null = null;

  showPassword: boolean = false;
  registerForm: FormGroup;
  registerModel: RegisterRequest = new RegisterRequest();

  private strongPassRegex: RegExp = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*]).{8,}$/;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.registerForm = new FormGroup({
      Email: new FormControl(this.registerModel.Email, {
        validators: [Validators.required, Validators.email]
      }),
      UserName: new FormControl(this.registerModel.UserName, {
        validators: [Validators.required, Validators.minLength(5)]
      }),
      FirstName: new FormControl(this.registerModel.FirstName, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      LastName: new FormControl(this.registerModel.LastName, {
        validators: [Validators.required, Validators.minLength(2)]
      }),
      UserPassword: new FormControl(this.registerModel.UserPassword, {
        validators: [Validators.required, Validators.minLength(8), Validators.pattern(this.strongPassRegex)]
      }),
      UserPasswordRepeated: new FormControl(this.registerModel.UserPasswordRepeated, {
        validators: [Validators.required, Validators.minLength(8), Validators.pattern(this.strongPassRegex), mustMatch('UserPassword', 'UserPasswordRepeated')]
      })
    });
  }

  register() {
    if (this.registerForm.valid) {
      this.registerModel = this.registerForm.value;
      this.authService.Register(this.registerModel).subscribe({
        next: (response: HttpEvent<any>) => {
          console.log(response);
          this.router.navigate(['/Login'], { queryParams: { registered: 'true' } });
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error occurred:', error);
          if (error.status === HttpStatusCode.BadRequest && error.error && error.error.value) {
            this.errorMessage = error.error.value.errorMessage;
          } else {
            this.errorMessage = 'An unexpected error occurred. Please try again.';
          }
        }
      });
    }
  }

  onFocus(field: string) {
    switch (field) {
      case 'email':
        this.focusEmail = true;
        return;
      case 'password':
        this.focusPassword = true;
        return;
      case 'userName':
        this.focusUserName = true;
        return;
      case 'firstName':
        this.focusFirstName = true;
        return;
      case 'lastName':
        this.focusLastName = true;
        return;
    }
  }

  onBlur(field: string) {
    switch (field) {
      case 'email':
        this.focusEmail = false;
        return;
      case 'password':
        this.focusPassword = false;
        return;
      case 'userName':
        this.focusUserName = false;
        return;
      case 'firstName':
        this.focusFirstName = false;
        return;
      case 'lastName':
        this.focusLastName = false;
        return;
    }
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
