export class LoginResponse {
	isLoggedSuccessful: boolean = false;
	isTempPasswordToChange: boolean = false;
	errorMessage: string = '';
	token: string = '';
	userId: string | null = null;
}