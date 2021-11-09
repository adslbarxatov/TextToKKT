// ����� ������������

// ��������� ���� ���������� ����������
enum FNDocumentTypes
	{
	// ����� � �����������
	Registration = 0x01,

	// ����� � ����������� ��� ��� 1.2
	Registration_12 = 0x41,

	// ����� �� ��������� ���������� �����������
	RegistrationChange = 0x0B,

	// ����� �� ��������� ���������� ����������� ��� ��� 1.2
	RegistrationChange_12 = 0x4B,

	// �������� �����
	OpenSession = 2,

	// ����� � ������� ��������� ��������
	CurrentState = 21,

	// �������� ���
	Bill = 3,

	// ��� ���������
	CorrectionBill = 31,

	// ���
	Blank = 4,

	// ��� ���������
	CorrectionBlank = 41,

	// �������� �����
	CloseSession = 5,

	// �������� ����������� ������
	CloseFiscalStorage = 6,

	// ������������� ���������
	Confirmation = 7,

	// ������ � ���� ����������
	MarkCodeRequest = 81,

	// ����������� � ���������� ���� ����������
	MarkCodeNotification = 82,

	// ����� �� ������ � ���� ����������
	MarkCodeRequestAnswer = 83,

	// ����� �� ����������� � ���������� ���� ����������
	MarkCodeNotificationAnswer = 84,

	// ����������� ��� ���������
	UnknownType = 255
	};

#define REG_CAUSE(type)		(type == Registration) || (type == RegistrationChange) || \
	(type == Registration_12) || (type == RegistrationChange_12)
#define REREG_CAUSE(type)	(type == RegistrationChange) || (type == RegistrationChange_12)

// ��������� ���� ����� ��
enum FNLifePhases
	{
	// ��������������� �����
	FactoryMode = 0x00,

	// ���������� � ������������
	ReadyForLaunch = 0x01,

	// ���������� �����
	FiscalMode = 0x03,

	// ����� ������, �������� ����������
	AfterFiscalMode = 0x07,

	// ����� ������, ��������� ��������
	ArchiveClosed = 0x0F,

	// ����������� �����
	UnknownMode = -1
	};

// ��������� ���������� ������� � COM-������
enum SendAndReceiveResults
	{
	// �������
	Ok = 0,

	// ������ ������
	WriteError = -11,

	// ������� ������
	ReadTimeout = -21,

	// ������ ������
	ReadError = -22,

	// ������������ ����� ��������� ���������
	AnswerLengthError = -23,

	// ������������ ����������� ����� ��������� ���������
	AnswerCRCError = -24,

	// ������� �����, ���������� ��������� �� ������
	AnswerLogicError = -31
	};

// �������������� TLV-����
enum TLVTags
	{
	// ����������� ����

	// ���� ������ � ������� ��������������� ���������� �������� (1001)
	AutomaticFlag = 0x03E9,

	#define	PROC_AUTOMATICFLAG(src,dest,type)\
		case AutomaticFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ��� ����������� � ��������\r\n", dest);\
			break;

	// ���� ����������� ������ (1002)
	AutonomousFlag = 0x03EA,

	#define PROC_AUTONOMOUSFLAG(src,dest,type)\
		case AutonomousFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  ���������� ����� (��� ���)\r\n" : "%s  ����� �������� ������\r\n", dest);\
			break;

	// E-mail/������� ���������� (1008)
	ClientAddress = 0x03F0,

	#define PROC_CLIENTADDRESS(src,dest)\
		case ClientAddress:\
			sprintf (dest, "%s  E-mail ��� ������� ����������: %s\r\n", dest, src);\
			break;

	// ����� �������� (1009)
	RegistrationAddress = 0x03F1,

	#define PROC_REGISTRATIONADDRESS(src,dest,type,preproc)\
		case RegistrationAddress:\
			if (REG_CAUSE (type))\
				{\
				preproc;\
				sprintf (dest, "%s  ����� �������: %s\r\n", dest, src);\
				}\
			break;

	// ���� � ����� ��������� (1012)
	DocumentDateTime = 0x03F4,

	// ��������� ����� ��� (1013)
	RegisterSerialNumber = 0x03F5,

	// ��� ��� (1017)
	OFD_INN = 0x03F9,

	// ��� ������������ (1018)
	UserINN = 0x03FA,

	#define PROC_USERINN	"%s  ��� ������������: %s\r\n"

	// ����� ������� (1020)
	OperationSumma = 0x03FC,

	// ��� ������� (1021)
	CashierName = 0x03FD,

	#define PROC_CASHIERNAME(src,dest,preproc)\
		case CashierName:\
			preproc;\
			sprintf (dest, "%s  ������: %s\r\n", dest, src);\
			break;

	// ���������� ������ (1023)
	GoodsCount = 0x03FF,

	// ������������ �������� ������� (1030)
	GoodName = 0x0406,

	// ����� ��������� (1031)
	RealCashValue = 0x0407,

	// ����� �������� (���������) (1036)
	TerminalNumber = 0x040C,

	#define PROC_TERMINALNUMBER(src,dest,type)\
		case TerminalNumber:\
			if (REG_CAUSE (type))\
				sprintf (dest, "%s  ����� ��������: %s\r\n", dest, src);\
			break;

	// ��������������� ����� ��� (1037)
	RegistrationNumber = 0x040D,

	#define PROC_REGISTRATIONNUMBER	"%s  ��������������� ����� ���: %s\r\n"

	// ����� �����, � ������� ������ �������� (1038)
	SessionNumber = 0x040E,

	#define PROC_SESSIONNUMBER	"%s  ����� �����: %u\r\n"

	// ����� ����������� ��������� (1040)
	FiscalDocumentNumber = 0x0410,

	// ��������� ����� �� (1041)
	FNSerialNumber = 0x0411,

	// ��������� �������� ������� (1043)
	ItemResult = 0x0413,
		
	// ��� ������������ (1048)
	UserName = 0x0418,

	#define PROC_USERNAME	"%s  ������������: %s\r\n"

	// ������� ������� (1054)
	OperationType = 0x041E,

	// ���������� ��� (1055)
	AppliedTaxSystem = 0x041F,

	// ���� ���������� (1056)
	EncryptionFlag = 0x0420,

	#define PROC_ENCRYPTIONFLAG(src,dest,type)\
		case EncryptionFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ���������� ���������� ������\r\n", dest);\
			break;

	// �������� �������� ������� (1059)
	PaymentObject = 0x0423,

	// ����� ����� ��� (1060)
	TaxServiceAddress = 0x0424,
		
	// ����� ������ ��������������� (1062)
	TaxFlags = 0x0426,

	#define PROC_TAXFLAGS(src,dest)	\
		case TaxFlags:\
		case AppliedTaxSystem:\
			sprintf (dest, "%s  ���������������: %s\r\n", dest, GetTaxFlags (src));\
			break;

	// ���������� ������� ��������� (1077)
	FiscalSignature = 0x0435,

	// ���������� ������� ��������� (���������, 1078)
	FiscalConfirmation = 0x0436,

	// ��������� ������� ������ (1079)
	ItemCost = 0x0437,

	// ����� ������������ (1081)
	ElectronicCashValue = 0x0439,

	// ���������� ������������ ���������� (1097)
	UnsentDocumentsCount = 0x0449,

	// ���������� ������������ ���������� (1098)
	FirstUnsentDocumentDate = 0x044A,

	// ������� ��������������� (1101)
	RegistrationChangeCause = 0x044D,

	#define PROC_REGISTRATIONCHANGECAUSE(src,dest,type)\
		case RegistrationChangeCause:\
			if (REREG_CAUSE (type))\
				sprintf (dest, "%s  ������� ���������������: %s\r\n", dest, GetRegistrationChangeCause (src));\
			break;

	// ���� ������ � ���� �������� (1108)
	InternetFlag = 0x0454,

	#define PROC_INTERNETFLAG(src,dest,type)\
		case InternetFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ������ � ���� ��������\r\n", dest);\
			break;

	// ���� ������ ����� (1109)
	ServiceFlag = 0x0455,

	#define PROC_SERVICEFLAG(src,dest,type)\
		case ServiceFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  ����� �����\r\n" : "%s  ����� �������\r\n", dest);\
			break;

	// ���� ������������ ��� (1110)
	BlankFlag = 0x0456,

	#define PROC_BLANKFLAG(src,dest,type)\
		case BlankFlag:\
			if (REG_CAUSE (type))\
				sprintf (dest, src ? "%s  ����� ������� ���\r\n": "%s  ����� �������� �����\r\n", dest);\
			break;

	// ��� �������� ������� (��� ����������) (1162)
	GoodCode = 0x048A,

	// ����� �������� (1187)
	RegistrationPlace = 0x04A3,

	#define PROC_REGISTRATIONPLACE(src,dest,type,preproc)\
		case RegistrationPlace:\
			if (REG_CAUSE (type))\
				{\
				preproc;\
				sprintf (dest, "%s  ����� �������: %s\r\n", dest, src);\
				}\
			break;

	// ���� �������� ��� (1193)
	GamesFlag = 0x04A9,

	#define PROC_GAMESFLAG(src,dest,type)\
		case GamesFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ������������ ��� ���������� �������� ���\r\n", dest);\
			break;

	// ������ ��� (1199)
	NDS = 0x04AF,

	// ��� ������� (1203)
	CashierINN = 0x04B3,

	#define PROC_CASHIERINN(src,dest)\
		case CashierINN:\
			sprintf (dest, "%s  ��� �������: %s\r\n", dest, src);\
			break;

	// ���� ����������� ������� (1207)
	ExciseFlag = 0x04B7,

	#define PROC_EXCISEFLAG(src,dest,type)\
		case ExciseFlag:\
			if ((REG_CAUSE (type)) && src)\
				sprintf (dest, "%s  ������������ ��� ������� ����������� �������\r\n", dest);\
			break;

	// ������ ������� ���������� ���������� (1209)
	FFDVersion = 0x04B9,

	#define PROC_FFDVERSION(src,dest)\
		case FFDVersion:\
			sprintf (dest, "%s  ������ ���: %s\r\n", dest, GetFFDVersion (src));\
			break;

	// ������� �������� ������� (1212)
	ResultObject = 0x04BC,

	// ������� ������� ������� (1214)
	ResultMethod = 0x04BE,

	// ���� ������ ������� ��

	// ����� � �����������
	FNA_Registration = Registration + 100,

	// ����� �� ��������� ���������� �����������
	FNA_RegistrationChange = RegistrationChange + 100,

	// �������� �����
	FNA_OpenSession = OpenSession + 100,

	// ����� � ������� ��������� ��������
	FNA_CurrentState = CurrentState + 100,

	// �������� ���
	FNA_Bill = Bill + 100,

	// ��� ���������
	FNA_CorrectionBill = CorrectionBill + 100,

	// ���
	FNA_Blank = Blank + 100,

	// ��� ���������
	FNA_CorrectionBlank = CorrectionBlank + 100,

	// �������� �����
	FNA_CloseSession = CloseSession + 100,

	// �������� ����������� ������
	FNA_CloseFiscalStorage = CloseFiscalStorage + 100,

	// ������������� ���������
	FNA_Confirmation = Confirmation + 100,

	// ���������� �������� (���������� �����, 65001)
	FNA_AutonomousArray = 0xFDE9,

	// ���������� �������� (����� �������� ������, 65002)
	FNA_OFDArray = 0xFDEA,

	// ���������� �������� (���������� �����, ��� 1.1, 65011)
	FNA_AutonomousArray_1_1 = 0xFDF3,

	// ���������� �������� (����� �������� ������, ��� 1.1, 65012)
	FNA_OFDArray_1_1 = 0xFDF4,

	// ���������� �������� (���������� �����, ��� 1.2, 65021)
	FNA_AutonomousArray_1_2 = 0xFDFD,

	// ���������� �������� (����� �������� ������, ��� 1.2, 65022)
	FNA_OFDArray_1_2 = 0xFDFE,
		
	// ���, ���������� � ���������� ������ �� ��� ����������
	ReadingFailure = 9999,

	// �� ���������
	FNASignature = 301,

	// ��������� �������� ������ �����������
	FNARegistrationProtocol = 304,

	// ��������� �������� ������ �������� ������
	FNAClosingProtocol = 305,
	};
