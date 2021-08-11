import { ParseWebApiErrors } from 'src/app/Util/util.methods';
import { AuthResponse } from './../account.modals';
import { AccountService } from './../account.service';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  form: FormGroup;
  errors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private accountService: AccountService) {

    this.form = this.fb.group({
      UserId: ['', { validators: [Validators.required, Validators.maxLength(12)] }],
      Password: ['', { validators: [Validators.required] }],
      FirstName: ['', { validators: [Validators.required] }],
      LastName: ['', { validators: [Validators.required] }],
      EmailAddress: ['', { validators: [Validators.required, Validators.email] }],
    });

  }

  ngOnInit(): void {
  }

  register(){
    if (this.form.valid) {
      this.accountService.create(this.form.value).subscribe((res: AuthResponse) => {
        this.accountService.saveToken(res);
        this.router.navigate(['/']);
      }, error => this.errors = ParseWebApiErrors(error))
    }
  }

}
