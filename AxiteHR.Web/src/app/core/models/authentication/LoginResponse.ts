export interface LoginResponse {
	isLoggedSuccessful: boolean;
	isTempPasswordToChange: boolean;
	errorMessage: string;
	token: string;
	userId: string | null;
}