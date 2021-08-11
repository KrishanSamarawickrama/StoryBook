import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { EditorService } from '../editor.service';

@Component({
  selector: 'app-editor-list',
  templateUrl: './editor-list.component.html',
  styleUrls: ['./editor-list.component.scss']
})
export class EditorListComponent implements OnInit {

  displayedColumns: string[] = ['editorId', 'editorName', 'email', 'isFlowing'];
  dataSource: any[] = [];

  page = 1;
  pageSize = 10;
  noOfRecords = 0;

  constructor(private editorService:EditorService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.editorService.get(this.page, this.pageSize).subscribe(res => {
      this.dataSource = res.body;
      this.noOfRecords = res.headers.get('noOfRecords');
    })
  }

  followWriter(userId: string): void {
    this.editorService.followWriter(userId).subscribe(res => {
      this.loadData();
    });
  }

  unFollowWriter(userId: string): void {
    this.editorService.unFollowWriter(userId).subscribe(res => {
      this.loadData();
    });
  }

  updatePagination(page: PageEvent): void {
    this.page = page.pageIndex + 1;
    this.pageSize = page.pageSize;
    this.loadData();
  }

}
