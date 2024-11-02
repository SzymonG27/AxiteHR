import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

export function requiredIfTrue(controlName: string): ValidatorFn {
	return (control: AbstractControl): Record<string, unknown> | null => {
		const formGroup = control.parent as FormGroup;
		if (!formGroup) {
			return null;
		}

		const booleanControlToCompare = formGroup.get(controlName);
		if (booleanControlToCompare?.value === true && !control.value) {
			return { requiredIfTrue: true };
		}

		return null;
	};
}
