﻿-----Company API----- TODO README in each microservice

Microservice for managing company in application.
HTTPS PORT: 7004


Local ssl certificates for debug. //ToDo WIKI

--AuthAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.authapi.key -out axitehr.services.authapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.authapi.pfx -inkey axitehr.services.authapi.key -in axitehr.services.authapi.crt -passout pass:Password123

Then copy certificates to AuthAPI/Certs folder

--CompanyAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.companyapi.key -out axitehr.services.companyapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.companyapi.pfx -inkey axitehr.services.companyapi.key -in axitehr.services.companyapi.crt -passout pass:Password123

Then copy certificates to CompanyAPI/Certs folder

--ApplicationAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.applicationapi.key -out axitehr.services.applicationapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.applicationapi.pfx -inkey axitehr.services.applicationapi.key -in axitehr.services.applicationapi.crt -passout pass:Password123

--SignalRAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.signalrapi.key -out axitehr.services.signalrapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.signalrapi.pfx -inkey axitehr.services.signalrapi.key -in axitehr.services.signalrapi.crt -passout pass:Password123

--InvoiceRAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.invoiceapi.key -out axitehr.services.invoiceapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.invoiceapi.pfx -inkey axitehr.services.invoiceapi.key -in axitehr.services.invoiceapi.crt -passout pass:Password123

Then copy certificates to ApplicationAPI/Certs folder

Install all .crt in the CA cert folder on local PC

Then run application via docker-compose

To delete old certificates, add line in dockerfile before copy:
RUN rm -f /usr/local/share/ca-certificates/axitehr.services.companyapi.crt