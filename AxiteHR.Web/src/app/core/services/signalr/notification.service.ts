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
			.withUrl(`${Environment.gatewayApiUrl}${ApiPaths.NotificationHub}`, {
				transport: signalR.HttpTransportType.WebSockets,
				withCredentials: true,
				accessTokenFactory: () => {
					return this.jwtTokenService.getToken();
				},
			})
			.withAutomaticReconnect()
			.build();

		this.hubConnection
			.start() //ToDo console log delete
			.then(() => console.log('Connection active with SignalR'))
			.catch(err => console.log('Error with connection to SignalR: ', err));
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

	public sendMessage(userId: string, header: string, message: string) {
		if (!this.hubConnection) {
			return;
		}

		this.hubConnection.invoke('SendMessage', userId, header, message);
	}
}
