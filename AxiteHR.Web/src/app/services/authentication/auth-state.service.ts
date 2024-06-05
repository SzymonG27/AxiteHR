import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthDictionary } from '../../shared/dictionary/AuthDictionary';

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

	private hasToken(): boolean {
		return !!localStorage.getItem(AuthDictionary.Token);
	}
	
	get isLoggedIn() {
		return this.loggedIn.asObservable();
	}

	setLoggedIn(isLoggedIn: boolean) {
		this.loggedIn.next(isLoggedIn);
	}
}
