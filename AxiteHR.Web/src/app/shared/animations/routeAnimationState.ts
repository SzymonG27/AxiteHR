import { animate, style, transition, trigger } from '@angular/animations';

export const routeAnimationState = trigger('routeAnimationTrigger', [
	transition(':enter', [
		// Ustawiamy początkową pozycję poza ekranem z lewej strony
		style({ transform: 'translateX(-100%)' }),
		// Animacja przesunięcia do pozycji początkowej
		animate('300ms ease-in', style({ transform: 'translateX(0%)' })),
	]),
	transition(':leave', [
		// Animacja przesunięcia poza ekran w prawą stronę
		animate('300ms ease-out', style({ transform: 'translateX(100%)' })),
	]),
]);
