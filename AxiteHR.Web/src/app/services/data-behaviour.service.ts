import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class DataBehaviourService {
	private registeredSource = new BehaviorSubject<boolean>(false);
	currentRegistered = this.registeredSource.asObservable();

	setRegistered(value: boolean) {
		this.registeredSource.next(value);
	}
}