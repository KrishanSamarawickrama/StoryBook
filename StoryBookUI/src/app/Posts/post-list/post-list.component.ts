import { EditorService } from './../../Editors/editor.service';
import { Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { PostServiceService } from '../post-service.service';
import { PostDto } from '../post.modals';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit, OnDestroy {

  dataSource: PostDto[] = [];
  editorsToDisplay: any[] = [];
  selectedEditors: any[] = [];
  searchControl = new FormControl();

  page = 1;
  pageSize = 100;
  noOfRecords = 0;

  subscription: Subscription;

  constructor(
    private postService: PostServiceService,
    private editorService: EditorService) {

    this.subscription = this.postService.postUpdated$.subscribe(res => {
      this.loadData();
    });
    this.searchControl.valueChanges.subscribe(value => {
      if (!value || value === '')
        this.loadData();
      else
        this.editorService.searchByName(value).subscribe(res => {
          this.editorsToDisplay = res;
        });
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(editorId: string | null = null): void {
    this.postService.get(this.page, this.pageSize, editorId).subscribe(res => {
      this.dataSource = res.body;
      this.noOfRecords = res.headers.get('noOfRecords');
    });
  }

  optionSelected(event: MatAutocompleteSelectedEvent): void {
    this.selectedEditors = [];
    this.selectedEditors.push(event.option.value);
    this.searchControl.setValue(event.option.value.editorName);
    this.loadData(event.option.value.editorId);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
