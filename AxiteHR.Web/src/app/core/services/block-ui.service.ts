import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { first, map } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class BlockUIService {
	constructor(private translate: TranslateService) { }

	@BlockUI() blockUI!: NgBlockUI;

	start() {
		this.translate.get('Global_Loading').pipe(
			first(),
			map((translation: string) => translation)
		)
		.subscribe(message => this.blockUI.start(message));
	}

	stop() {
		this.blockUI.stop();
	}
}
