export enum ApiPaths {
	//Auth API
	Register = '/Auth/Register',
	Login = '/Auth/Login',
	TempPasswordChange = '/Auth/TempPasswordChange',

	//Company API
	CompanyList = '/Company/List',
	EmployeeList = '/CompanyUser/List',
	EmployeeListCount = '/CompanyUser/Count',
	CompanyCreator = '/CompanyManager/CreateNewCompany',
	EmployeeCreator = '/EmployeeManager/CreateNewEmployee',
	GetCompanyForEmployee = '/Company/GetForEmployee',
	IsUserInCompany = '/CompanyUser/IsInCompany',
	JobStationList = '/CompanyRole/ListAsync',
	JobStationListCount = '/CompanyRole/CountAsync',
	JobStationCreate = '/CompanyRole/CreateAsync',

	//Application API
	NewApplicationCreator = '/Application/CreateNewApplication',
}
