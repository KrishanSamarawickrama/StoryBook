import { Router } from '@angular/router';
import { AccountService } from './../Security/account.service';
import { PostCreateComponent } from './../Posts/post-create/post-create.component';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {

  constructor(
    public dialog: MatDialog,
    public accountService: AccountService,
    private router: Router) { }

  ngOnInit(): void {
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(PostCreateComponent, {
      width: '50%',
      data: { name: '' },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      this.router.navigate(['/']);
    });
  }

  logOut(): void {
    this.accountService.logOut();
    this.router.navigate(['/login']);
  }

}
