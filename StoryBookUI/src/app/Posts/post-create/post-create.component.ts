import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PostServiceService } from '../post-service.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.scss']
})
export class PostCreateComponent implements OnInit {

  form: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<PostCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private postService:PostServiceService) {

    this.form = this.fb.group({
      PostContent: ['', { validators: [Validators.required, Validators.minLength(3), Validators.maxLength(500)] }]
    });
    
  }

  ngOnInit(): void {
  }

  post():void{
    this.postService.create(this.form.value).subscribe(res =>{
      this.postService.postUpdated(this.form.value);
      this.dialogRef.close();
    });
  }

  onNoClick(): void {
    this.dialogRef.close();    
  }

}
