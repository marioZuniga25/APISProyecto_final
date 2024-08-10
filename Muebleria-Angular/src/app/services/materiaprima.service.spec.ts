import { TestBed } from '@angular/core/testing';

import { MateriaprimaService } from './materiaprima.service';

describe('MateriaprimaService', () => {
  let service: MateriaprimaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MateriaprimaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
