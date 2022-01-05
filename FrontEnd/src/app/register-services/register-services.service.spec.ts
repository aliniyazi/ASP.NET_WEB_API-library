import { TestBed } from '@angular/core/testing';

import { RegisterServicesService } from './register-services.service';

describe('RegisterServicesService', () => {
  let service: RegisterServicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RegisterServicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
