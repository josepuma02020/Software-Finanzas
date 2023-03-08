import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class footerComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
