import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EntidadesComponent } from './entidades.component';

describe('EntidadesComponent', () => {
  let component: EntidadesComponent;
  let fixture: ComponentFixture<EntidadesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EntidadesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntidadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
