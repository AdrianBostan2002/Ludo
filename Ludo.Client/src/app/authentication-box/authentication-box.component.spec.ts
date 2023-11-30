import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthenticationBoxComponent } from './authentication-box.component';

describe('AuthenticationBoxComponent', () => {
  let component: AuthenticationBoxComponent;
  let fixture: ComponentFixture<AuthenticationBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuthenticationBoxComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthenticationBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
