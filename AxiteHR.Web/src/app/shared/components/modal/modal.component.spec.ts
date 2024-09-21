import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalComponent } from './modal.component';
import { By } from '@angular/platform-browser';
import { AnimationEvent } from '@angular/animations';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('ModalComponent', () => {
	let component: ModalComponent;
	let fixture: ComponentFixture<ModalComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ModalComponent, BrowserAnimationsModule],
		}).compileComponents();
	});

	beforeEach(() => {
		fixture = TestBed.createComponent(ModalComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('Should create the component', () => {
		expect(component).toBeTruthy();
	});

	it('Should be not visible by default', () => {
		expect(component.isVisible).toBeFalse();
	});

	it('Should show modal when isOpen is set to true', () => {
		component.isOpen = true;
		component.ngOnChanges();
		expect(component.isVisible).toBeTrue();
	});

	it('Should emit closeModalEmitter after calling CloseModal()', () => {
		spyOn(component.closeModalEmitter, 'emit');
		component.closeModal();
		expect(component.closeModalEmitter.emit).toHaveBeenCalled();
	});

	it('Should emit submitModalEmitter after calling submitForm()', () => {
		spyOn(component.submitModalEmitter, 'emit');
		component.submitForm();
		expect(component.submitModalEmitter.emit).toHaveBeenCalled();
	});

	it('Should set isVisible to false after the shutdown animation completes', () => {
		const event = { toState: 'closed' } as AnimationEvent;
		component.isVisible = true;
		component.onAnimationDone(event);
		expect(component.isVisible).toBeFalse();
	});

	it('Should not change isVisible after the opening animation completes', () => {
		const event = { toState: 'open' } as AnimationEvent;
		component.isVisible = true;
		component.onAnimationDone(event);
		expect(component.isVisible).toBeTrue();
	});

	it('Should display the title passed in @Input', () => {
		const title = 'Testowy Tytuł';
		component.title = title;
		fixture.detectChanges();
		const titleElement = fixture.debugElement.query(By.css('h2')).nativeElement;
		expect(titleElement.textContent).toContain(title);
	});

	it('Should display buttons with appropriate titles', () => {
		component.closeButtonTitle = 'Zamknij';
		component.submitButtonTitle = 'Wyślij';
		component.isForm = true;
		fixture.detectChanges();
		const buttons = fixture.debugElement.queryAll(By.css('button'));
		expect(buttons[0].nativeElement.textContent).toContain('X');
		expect(buttons[1].nativeElement.textContent).toContain('Zamknij');
		expect(buttons[2].nativeElement.textContent).toContain('Wyślij');
	});

	it('Should not show the submit button when isForm is false', () => {
		component.isForm = false;
		fixture.detectChanges();
		const buttons = fixture.debugElement.queryAll(By.css('button'));
		expect(buttons.length).toBe(2);
		expect(buttons[0].nativeElement.textContent).toContain('X');
		expect(buttons[1].nativeElement.textContent).toContain('Close');
	});
});
