import { AccountService } from './../account.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AuthResponse } from '../account.modals';
import { Router } from '@angular/router';
import { ParseWebApiErrors } from 'src/app/Util/util.methods';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  errors: string[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private accountService: AccountService
  ) {
    this.form = this.fb.group({
      UserId: ['', { validators: [Validators.required, Validators.maxLength(12)] }],
      Password: ['', { validators: [Validators.required] }]      
    });
  }

  ngOnInit(): void {
  }

  login() {
    if (this.form.valid) {
      this.accountService.login(this.form.value).subscribe((res: AuthResponse) => {
        this.accountService.saveToken(res);
        this.router.navigate(['/']);
      }, error => this.errors = ParseWebApiErrors(error))
    }
  }

}
