export interface UserCredentials {
    UserId: string;
    Password: string;
}

export interface AuthResponse {
    token: string;
    expiration: Date;
}

export interface UserCreationDto {
    UserId: string;
    Password: string;
    FirstName: string;
    LastName: string;
    EmailAddress: string;
}

export interface UsersDto {
    UserId: string;
    FirstName: string;
    LastName: string;
    EmailAddress: string;
    UserRole: any;
    IsEditor: string;
    IsBanned: string;
}

