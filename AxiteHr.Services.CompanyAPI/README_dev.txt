-----Company API-----

Microservice for managing company in application.
HTTPS PORT: 7004


Local ssl certificates for debug. //ToDo WIKI

--AuthAPI
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout axitehr.services.authapi.key -out axitehr.services.authapi.crt -config Path/To/AuthApiOpenSslConfig/openssl.cnf
openssl pkcs12 -export -out axitehr.services.authapi.pfx -inkey axitehr.services.authapi.key -in axitehr.services.authapi.crt -passout pass:Password123

Then copy certificates to AuthAPI/Certs folder

--CompanyAPI
openssl req -newkey rsa:2048 -nodes -keyout axitehr.services.companyapi.key -x509 -days 365 -out axitehr.services.companyapi.crt -subj "/CN=axitehr.services.companyapi"
openssl pkcs12 -export -out axitehr.services.companyapi.pfx -inkey axitehr.services.companyapi.key -in axitehr.services.companyapi.crt -passout pass:Password123

Then run application via docker-compose

To delete old certificates, add line in dockerfile before copy:
RUN rm -f /usr/local/share/ca-certificates/axitehr.services.companyapi.crt