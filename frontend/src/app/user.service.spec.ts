import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserService, User } from './user.service';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService]
    });
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should register a user', () => {
    const mockUser: User = {
      name: 'Test User',
      email: 'test@example.com',
      program: 'Test Program',
      interests: 'Testing',
      availability: 'Anytime'
    };

    service.register(mockUser).subscribe(response => {
      expect(response).toBeTruthy();
    });

    const req = httpMock.expectOne('https://your-api-url/api/users');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(mockUser);
    req.flush({ success: true });
  });

  it('should get users', () => {
    const mockUsers: User[] = [
      { name: 'User1', email: 'u1@example.com', program: 'P1', interests: 'I1', availability: 'A1' }
    ];

    service.getUsers().subscribe(users => {
      expect(users.length).toBe(1);
      expect(users).toEqual(mockUsers);
    });

    const req = httpMock.expectOne('https://your-api-url/api/users');
    expect(req.request.method).toBe('GET');
    req.flush(mockUsers);
  });
});