import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class DataBehaviourService {
	constructor () {}

	private registeredSource = new BehaviorSubject<boolean>(false);
	currentRegistered = this.registeredSource.asObservable();
	setRegistered(value: boolean) {
		this.registeredSource.next(value);
	}

	private isTokenExpiredSource = new BehaviorSubject<boolean>(false);
	isTokenExpired = this.isTokenExpiredSource.asObservable();
	setIsTokenExpired(value: boolean) {
		this.isTokenExpiredSource.next(value);
	}

	private newEmployeeCreatedSource = new BehaviorSubject<boolean>(false);
	newEmployeeCreated = this.newEmployeeCreatedSource.asObservable();
	setNewEmployeeCreated(value: boolean) {
		this.newEmployeeCreatedSource.next(value);
	}

	private tempPasswordErrorSource = new BehaviorSubject<string>("");
	tempPasswordError = this.tempPasswordErrorSource.asObservable();
	setTempPasswordError(value: string) {
		this.tempPasswordErrorSource.next(value);
	}

	private tempPasswordSuccessSource = new BehaviorSubject<string>("");
	tempPasswordSuccess = this.tempPasswordSuccessSource.asObservable();
	setTempPasswordSuccess(value: string) {
		this.tempPasswordSuccessSource.next(value);
	}
}