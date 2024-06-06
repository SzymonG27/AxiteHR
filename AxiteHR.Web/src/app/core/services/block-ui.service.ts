import { Injectable } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Injectable({
	providedIn: 'root'
})
export class BlockUIService {
	@BlockUI() blockUI!: NgBlockUI;

	start(message: string = 'Loading...') {
		this.blockUI.start(message);
	}

	stop() {
		this.blockUI.stop();
	}
}
