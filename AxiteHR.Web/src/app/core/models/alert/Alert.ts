export interface Alert {
	id: number;
	message: string;
	type: 'success' | 'error' | 'warning';
	duration: number;
}
