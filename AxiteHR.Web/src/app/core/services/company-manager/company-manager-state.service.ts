import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { JobStationState } from '../../models/company-manager/job-station/JobStationState';
import { StoredState } from '../../models/state/StoredState';

@Injectable({
	providedIn: 'root',
})
export class CompanyManagerStateService {
	private readonly JOB_STATION_MANAGER_STORAGE_KEY = 'JobStationManagerState';
	private readonly TTL_MS = 5 * 60 * 1000;

	private jobStationManagerStateSubject = new BehaviorSubject<JobStationState | null>(
		this.loadJobStationManagerState()
	);

	jobStationManagerState$ = this.jobStationManagerStateSubject.asObservable();

	setStateJobStationManager(state: JobStationState): void {
		this.clearJobStationManager();

		const expiresAt = Date.now() + this.TTL_MS;

		const toStore: StoredState<JobStationState> = {
			data: state,
			expiresAt: expiresAt,
		};

		localStorage.setItem(this.JOB_STATION_MANAGER_STORAGE_KEY, JSON.stringify(toStore));
		this.jobStationManagerStateSubject.next(state);
	}

	getStateJobStationManager(): JobStationState | null {
		return this.jobStationManagerStateSubject.getValue();
	}

	clearJobStationManager(): void {
		localStorage.removeItem(this.JOB_STATION_MANAGER_STORAGE_KEY);
		this.jobStationManagerStateSubject.next(null);
	}

	private loadJobStationManagerState(): JobStationState | null {
		const raw = localStorage.getItem(this.JOB_STATION_MANAGER_STORAGE_KEY);
		if (!raw) return null;

		try {
			const parsed: StoredState<JobStationState> = JSON.parse(raw);

			if (Date.now() > parsed.expiresAt) {
				this.clearJobStationManager();
				return null;
			}

			return parsed.data;
		} catch {
			this.clearJobStationManager();
			return null;
		}
	}
}
