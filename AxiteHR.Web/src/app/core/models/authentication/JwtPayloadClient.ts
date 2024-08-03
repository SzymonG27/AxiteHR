import { JwtPayload } from "jwt-decode";

export class JwtPayloadClient implements JwtPayload {
	iss?: string;
	sub?: string;
	aud?: string[] | string;
	exp?: number;
	nbf?: number;
	iat?: number;
	jti?: string;

	//custom
	email?: string;
	given_name?: string;
	family_name?: string;
	phoneNumber?: string;
	role?: string | string[];
}