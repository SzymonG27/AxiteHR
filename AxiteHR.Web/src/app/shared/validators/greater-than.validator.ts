import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

export function greaterThan(controlName: string): ValidatorFn {
	return (control: AbstractControl): Record<string, unknown> | null => {
		const formGroup = control.parent as FormGroup;
		if (!formGroup) {
			return null;
		}

		const controlToCompare = formGroup.get(controlName);
		if (!control || !controlToCompare) {
			return null;
		}

		const period1 = Number(controlToCompare.value);
		const period2 = Number(control.value);

		if (isNaN(period1) || isNaN(period2)) {
			return null;
		}

		return period2 > period1 ? null : { greaterThan: true };
	};
}
