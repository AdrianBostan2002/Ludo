import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WinningModalComponent } from './winning-modal.component';

describe('WinningModalComponent', () => {
  let component: WinningModalComponent;
  let fixture: ComponentFixture<WinningModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WinningModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WinningModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
