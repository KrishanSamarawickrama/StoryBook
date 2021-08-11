import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-authorized-view',
  templateUrl: './authorized-view.component.html',
  styleUrls: ['./authorized-view.component.scss']
})
export class AuthorizedViewComponent implements OnInit {

  @Input() userRole: string[] = [];

  constructor(private accountService:AccountService) { }

  ngOnInit(): void {
  }

  public IsAuthorized() : boolean{

    if(this.userRole.length === 0){
      return this.accountService.isAuthenticated();
    }

    return this.userRole.filter(x => x === this.accountService.getRole())?.length > 0   ;
  }

}
