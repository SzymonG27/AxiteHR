import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function greaterThan(from: string, to: string): ValidatorFn {
	return (formGroup: AbstractControl): ValidationErrors | null => {
		const control = formGroup.get(from);
		const matchingControl = formGroup.get(to);

		if (!control || !matchingControl || !control.value || !matchingControl.value) {
			return null;
		}

		const period1 = Number(control.value);
		const period2 = Number(matchingControl.value);

		if (isNaN(period1) || isNaN(period2)) {
			return null;
		}

		return period2 > period1 ? null : { greaterThan: true };
	};
}
