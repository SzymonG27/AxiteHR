import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function maxPeriodDifference(
	controlName: string,
	matchingControlName: string,
	maxDifference: number
): ValidatorFn {
	return (formGroup: AbstractControl): ValidationErrors | null => {
		const control = formGroup.get(controlName);
		const matchingControl = formGroup.get(matchingControlName);

		if (!control || !matchingControl) {
			return null;
		}

		const period1 = Number(control.value);
		const period2 = Number(matchingControl.value);

		if (
			isNaN(period1) ||
			period1 < 0 ||
			period1 > 23 ||
			isNaN(period2) ||
			period2 < 0 ||
			period2 > 23
		) {
			return null;
		}

		return period2 - period1 <= maxDifference ? null : { maxPeriodDifference: true };
	};
}
