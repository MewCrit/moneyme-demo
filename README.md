
# MONEY ME QUOTE CALCULATOR


A Simple loan calculator

### System Requirements

Running it Manually requires
 - .NET 8
 - MSSQL Studio
 - Node js latest

Running it Automatically only requires Docker
    - Ubuntu Docker Daemon
    - Windows Docker Desktp


## Run Automatically

 -  If you have Docker installed in your machine run the following command this will spin up all images MSSQL with its database and tables, Web API and the UI

    ```shell
        ./init.ps1
    ```


## Run Manually

### Backend 

- Create the database using the SQL scripts in your MSSQL studio.
    ```
        .
        └── moneyme/
            └── backend/
                └── db/
                    ├── 1_init.sql
                    ├── 2_crud.sql
                    └── 3_populate.sql
    ```

- Run the project using dotnet cli commands inside the directory of MoneyMe.API.

  ```shell
    dotnet build
    dotnet run
  ```

### Frontend

- Install all dependencies in the main directory type below in your terminal.

```shell
 npm install
```
- Run the code the main directory type below in your terminal.

```shell
npm run dev
```



<br/>

## Tools used to develop

### IDE Tools
- DBeaver
- VSCode
- Docker Desktop on Windows

### Languages / Frameworks
- C#
- Type Script
- .NET 8
- Next JS 14

### Testing
- Nunit
- Fluent Assertion



<br/>

## UI is Built with Next JS 14


### UI components with SHADCN UI

Beautifully designed components that you can copy and paste into your apps. Accessible. Customizable. Open Source.
This is NOT a component library. It's a collection of re-usable components that you can copy and paste into your apps.

https://ui.shadcn.com/

