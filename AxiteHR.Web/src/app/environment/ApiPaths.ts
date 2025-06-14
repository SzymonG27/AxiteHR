export enum ApiPaths {
	//Auth API
	Register = '/Auth/Register',
	Login = '/Auth/Login',
	TempPasswordChange = '/Auth/TempPasswordChange',

	//Company API
	CompanyList = '/Company/List',
	CompanyGetCompanyUserId = '/Company/CompanyUserId',
	EmployeeList = '/CompanyUser/List',
	EmployeeListCount = '/CompanyUser/Count',
	CompanyCreator = '/CompanyManager/CreateNewCompany',
	EmployeeCreator = '/EmployeeManager/CreateNewEmployee',
	GetCompanyForEmployee = '/Company/GetForEmployee',
	IsUserInCompany = '/CompanyUser/IsInCompany',
	JobStationList = '/CompanyRole/ListAsync',
	JobStationListCount = '/CompanyRole/CountAsync',
	JobStationCreate = '/CompanyRole/CreateAsync',
	ListEmployeesToAttach = '/CompanyRole/ListEmployeesToAttachAsync',
	CountEmployeesToAttach = '/CompanyRole/CountEmployeesToAttachAsync',
	AttachUserAsync = '/CompanyRole/AttachUserAsync',
	CompanyRoleEmployeeList = '/CompanyRole/ListEmployeesAsync',
	CompanyRoleEmployeeCount = '/CompanyRole/CountEmployeesAsync',

	//Application API
	NewApplicationCreator = '/Application/CreateNewApplication',

	//SignalR API
	NotificationHub = '/Hubs/Notification',
}
