# ZigZag Interview Task

This is a solution for the **ZigZag interview task**. It solves a problem of generating a JWT token, create a GraphQL API accessible to authorized users, and a ReactJS UI client with simple login and API data visualization using pagination.

## Prerequisites

1. Install MongoDB on port 27017.
2. Under the **'local'** database create following collections:
  - **cryptos** - import *'data.json'* here;
  - **users** - import *'users.json'* here;
  - **tokens** - a collection to hold active JWT tokens. No data imports needed;

## Backend services (.NET Core)

Folder **./services** contains a .NET solution file **'Services.sln'**.

The solution contains following project:

1. **auth** - Webapi microservice with **login** and **logout** HTTP POST actions. The **login** action generates a JWT-token, while the **logout** action revokes all JWT-tokens for the currently logged user. It also cleans up expired JWT tokens in the background. The microservice uses different **appsettings.{Environment}.json** for each **Environment** - Local, Development and Production to use specific SSL certificates, endpoints and JWT settings.
2. **crypto** - Webapi microservice exposing all the crypto data with GraphQL. It exposes following queries and mutations:
  - **getCryptosCount(searchFilter: string)** - a query to obtain the count of all crypto currencies filtered by a name search filter (if filter is null or empty, all records are counted).
  - **getCryptos(searchFilter: string, page: int, pageCount: int)** - a query to list all crypto currencies filtered by a name search filter (if filter is null or empty, all records are listed), for the specified page (if ommited, default to page 1) with max pageCount records listed (if omitted defaults to 10). The query is available for authorized regular users (with role 'user').
  - **removeCrypto(id: string)** - a mutation to remove a crypto by its id (e.g. 'binancecoin', 'ethereuim', etc.). The mutation is available for authorized administrator users (with role 'admin').
3. **ApiAccess** - Classlib to hold shared API-access related (authentication and authorization) DTO's, JWT related interfaces and services, SSL certificate helpers used accross all services.
4. **DataAccess** - Classlib to hold shared data-related repositories + database context, interfaces and services.

## Frontend crypto-client (ReactJS in Typescript + Redux toolkit)

The **crypto-client** application is a single page application using routing for the **login** and **main** pages. Depending on the environment (Local, Development, Production), which is invoked during the build process:
```shell
# Uses local endpoints for 'auth' and 'crypto' microservices
npm build:local
```
```shell
# Uses development endpoints for 'auth' and 'crypto' microservices
npm build:dev
```
```shell
# Uses production endpoints for 'auth' and 'crypto' microservices
npm build:prod
```
The application present the login page and after a successfull login it redirects to the main page which renders all the cryptos available using a pagination. A searchfield filters the crypto currencies by name and adjusts the pagination respectively. 

