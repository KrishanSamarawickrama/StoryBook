import { UsersDto } from './../account.modals';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { PageEvent } from '@angular/material/paginator';

export interface PeriodicElement {
  name: string;
  position: number;
  weight: number;
  symbol: string;
}

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  displayedColumns: string[] = ['UserId', 'FirstName', 'LastName', 'EmailAddress', 'UserRole', 'IsEditor', 'IsBanned'];
  dataSource: any[] = [];

  page = 1;
  pageSize = 10;
  noOfRecords = 0;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.accountService.getUsers(this.page, this.pageSize).subscribe(res => {
      this.dataSource = res.body;
      this.noOfRecords = res.headers.get('noOfRecords');
    })
  }

  makeWriter(userId: string): void {
    this.accountService.makeWriter(userId).subscribe(res => {
      this.loadData();
    });
  }

  unMakeWriter(userId: string): void {
    this.accountService.removeWriter(userId).subscribe(res => {
      this.loadData();
    });
  }

  banUser(userId: string): void {
    this.accountService.banUser(userId).subscribe(res => {
      this.loadData();
    });
  }

  unBanUser(userId: string): void {
    this.accountService.unBanUser(userId).subscribe(res => {
      this.loadData();
    });
  }

  updatePagination(page: PageEvent): void {
    this.page = page.pageIndex + 1;
    this.pageSize = page.pageSize;
    this.loadData();
  }

}
