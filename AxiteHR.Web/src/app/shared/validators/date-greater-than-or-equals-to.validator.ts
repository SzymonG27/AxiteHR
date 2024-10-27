import { AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';

export function dateGreaterThanOrEqualsTo(
	controlName: string,
	matchingControlName: string
): ValidatorFn {
	return (formGroup: AbstractControl): ValidationErrors | null => {
		const control = formGroup.get(controlName);
		const matchingControl = formGroup.get(matchingControlName);

		if (!control || !matchingControl) {
			return null;
		}

		const startDate = new Date(control.value);
		const endDate = new Date(matchingControl.value);

		if (isNaN(startDate.getTime()) || isNaN(endDate.getTime())) {
			return null;
		}

		return endDate >= startDate ? null : { dateGreaterThanOrEqualsTo: true };
	};
}
