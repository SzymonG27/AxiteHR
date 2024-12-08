import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

@Component({
	selector: 'app-list-filter',
	imports: [CommonModule, TranslateModule],
	templateUrl: './list-filter.component.html',
	styleUrl: './list-filter.component.css',
})
export class ListFilterComponent {
	isFilterVisible = false;

	@Output() search = new EventEmitter<void>();
	@Output() clear = new EventEmitter<void>();

	toggleFilter() {
		this.isFilterVisible = !this.isFilterVisible;
	}

	onSearch() {
		this.search.emit();
	}

	onClear() {
		this.clear.emit();
	}
}
