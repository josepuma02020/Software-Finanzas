import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowErrorModalComponent } from './show-error-modal.component';

describe('ShowErrorModalComponent', () => {
  let component: ShowErrorModalComponent;
  let fixture: ComponentFixture<ShowErrorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowErrorModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShowErrorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
