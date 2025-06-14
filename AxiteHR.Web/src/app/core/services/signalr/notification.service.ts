import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Environment } from '../../../environment/Environment';
import { ApiPaths } from '../../../environment/ApiPaths';
import { JWTTokenService } from '../authentication/jwttoken.service';

@Injectable({
	providedIn: 'root',
})
export class NotificationService {
	private hubConnection: signalR.HubConnection | null = null;

	constructor(private jwtTokenService: JWTTokenService) {}

	public startConnection(userCompanyId: string) {
		this.hubConnection = new signalR.HubConnectionBuilder()
			.withUrl(
				`${Environment.gatewayApiUrl}${ApiPaths.NotificationHub}?userCompanyId=${userCompanyId}`,
				{
					transport: signalR.HttpTransportType.WebSockets,
					withCredentials: true,
					accessTokenFactory: () => {
						return this.jwtTokenService.getToken();
					},
				}
			)
			.withAutomaticReconnect()
			.build();

		this.hubConnection
			.start()
			.catch(err => console.log('Error with connection to SignalR: ', err));
	}

	public stopConnection(): void {
		if (this.hubConnection) {
			this.hubConnection
				.stop()
				.catch(err => console.log('Error while stopping connection to signalr: ', err));
		}
	}

	public addNotificationListener() {
		if (!this.hubConnection) {
			return;
		}

		this.hubConnection.on(
			'ReceiveNotification',
			(notification: { header: string; message: string }) => {
				//ToDo notification on navbar
			}
		);
	}

	public sendMessage(userCompanyId: string, header: string, message: string) {
		if (!this.hubConnection) {
			return;
		}

		this.hubConnection.invoke('SendMessage', userCompanyId, header, message);
	}
}
