import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

export function mustMatch(controlName: string): ValidatorFn {
	return (control: AbstractControl): Record<string, unknown> | null => {
		const formGroup = control.parent as FormGroup;
		if (!formGroup) {
			return null;
		}
		const controlToCompare = formGroup.get(controlName);
		if (control.value !== controlToCompare?.value) {
			return { mustMatch: true };
		}
		return null;
	};
}
