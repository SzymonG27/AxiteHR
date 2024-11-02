import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

export function requiredIfFalse(controlName: string): ValidatorFn {
	return (control: AbstractControl): Record<string, unknown> | null => {
		const formGroup = control.parent as FormGroup;
		if (!formGroup) {
			return null;
		}

		const booleanControlToCompare = formGroup.get(controlName);
		if (
			booleanControlToCompare?.value === false &&
			(control.value === undefined || control.value === null)
		) {
			return { requiredIfFalse: true };
		}

		return null;
	};
}
