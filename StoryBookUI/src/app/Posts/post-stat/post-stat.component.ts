import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { PostServiceService } from '../post-service.service';

@Component({
  selector: 'app-post-stat',
  templateUrl: './post-stat.component.html',
  styleUrls: ['./post-stat.component.scss']
})
export class PostStatComponent implements OnInit {

  displayedColumns: string[] = ['Date', 'SingleVowelCount', 'PairVowelCount', 'TotalWordCount'];
  dataSource: any[] = [];

  page = 1;
  pageSize = 10;
  noOfRecords = 0;

  constructor(private postService: PostServiceService) { }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.postService.listStatData(this.page, this.pageSize).subscribe(res => {
      this.dataSource = res.body;
      this.noOfRecords = res.headers.get('noOfRecords');
    })
  }

  updatePagination(page: PageEvent): void {
    this.page = page.pageIndex + 1;
    this.pageSize = page.pageSize;
    this.loadData();
  }

}
