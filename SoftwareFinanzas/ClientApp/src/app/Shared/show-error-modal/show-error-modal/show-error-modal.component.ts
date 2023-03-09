import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-show-error-modal',
  templateUrl: './show-error-modal.component.html',
  styleUrls: ['./show-error-modal.component.css']
})
export class ShowErrorModalComponent implements OnInit {
  data: any;
  constructor(public matDialogRef: MatDialogRef<ShowErrorModalComponent>,
    @Inject(MAT_DIALOG_DATA) private _data: any) {
    this.data = _data;
  }

  ngOnInit(): void {
  }

}
