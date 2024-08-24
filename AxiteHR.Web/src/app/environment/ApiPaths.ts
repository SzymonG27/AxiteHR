export enum ApiPaths {
    //Auth API
    Register = "/Auth/Register",
    Login = "/Auth/Login",
    TempPasswordChange = "/Auth/TempPasswordChange",

    //Company API
    CompanyList = "/Company/List",
    EmployeeList = "/Company/CompanyUserList",
    EmployeeListCount = "/Company/CompanyUsersCount",
    CompanyCreator = "/CompanyManager/CreateNewCompany",
    EmployeeCreator = "/CompanyManager/CreateNewEmployee"
}