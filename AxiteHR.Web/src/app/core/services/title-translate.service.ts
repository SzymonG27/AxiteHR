import { Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TranslateService } from '@ngx-translate/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { filter, map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class TitleService {
	constructor(
		private titleService: Title,
		private translate: TranslateService,
		private router: Router,
		private activatedRoute: ActivatedRoute
	) {}

	init() {
		this.router.events
			.pipe(
				filter(event => event instanceof NavigationEnd),
				map(() => {
					let route = this.activatedRoute.firstChild;
					while (route && route.firstChild) {
						route = route.firstChild;
					}
					return route;
				}),
				switchMap(route => {
					if (route && route.snapshot.data['title']) {
						return this.translate.stream(route.snapshot.data['title']);
					}
					return of(this.translate.instant('defaultTitle'));
				})
			)
			.subscribe((title: string) => {
				this.titleService.setTitle(title);
			});
	}
}
