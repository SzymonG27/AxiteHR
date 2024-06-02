import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { RegisterRequest } from '../../../models/authentication/RegisterRequest';
import { FormControl, FormGroup, FormsModule, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { mustMatch } from '../../../shared/validators/password-match.validator';

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

  showPassword: boolean = false;
  registerForm: FormGroup;
  registerModel: RegisterRequest = new RegisterRequest();

  private strongPassRegex: RegExp = /^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d).{8,}$/;

  constructor(private authService: AuthenticationService) {
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

  register(registerModel: RegisterRequest) {
    this.authService.Register(registerModel).subscribe((response: string) => {
      console.log(response);
    });
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
