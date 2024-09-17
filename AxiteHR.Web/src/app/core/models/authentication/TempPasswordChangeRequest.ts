export interface TempPasswordChangeRequest {
    userId: string;
    password: string;
    confirmPassword: string;
}