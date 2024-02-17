namespace Pobytne.Shared.Struct
{
    public enum EPermition
    {
        UserRole = 0,//RoleUzivatele = 1
		Aplication = 1,//Aplikace = 2
		License = 2,//Licence = 3
		Module = 3,//Modul = 4
		LoginUser = 4,//LoginUser = 5
		Permition = 5,//Opravneni = 6
		PropertiesRecord = 6,//ZaznamyVlastnosti = 7
		PaymentType = 7,//TypPlatby = 8
        Client = 9,//Uzivatele = 10
		Record = 10,//Zaznamy = 11
		EvidenceTransport = 11,//PohybyEvidence = 12
		CashTransport = 12,//PohybyPokladna = 13
		ClientSelect = 13,//VyberUzivatele = 14
		Evidence = 14,//Evidence = 15
		SeasonTicket = 15,//Permanentky = 16
		SeasonTicketPayment = 16,//PermanentkyPlatba = 17
		SeasonTicketEvidence = 17,//PermanentkyEvidence = 18
		ActivityEvidence = 18,//EvidenceCinnost = 19
		EvidenceSummary = 19,//SouhrnyEvidence = 20
		CashSummary = 20,//SouhrnyPokladny = 21
		EvidenceStorageSummary = 22,//SouhrnyEvidenceSklad = 23
		SeasonTicketSummary = 23,//SouhrnyPermanentky = 24
        ContractReportSummary = 24,//SouhrnyDohodaZprava = 25
		ActivitySummary = 25,//SouhrnyCinnost = 26
		Contract = 29,//Dohody = 30
        ContractClient = 30,//DohodyOsoby = 31
		ContractSupplement = 31,//DohodyPrilohy = 32
		ContractSeason = 32,//DohodyObdobi = 33
		ContractReport = 33,//DohodyZpravy = 34
		OSPOD = 34, //OSPOD = 35
		ContractClientPlan = 35,//DohodyOsobyPlan = 36
		ContractOther = 36,//DohodyDalsiJevy = 37
		Other = 37//DalsiJevy = 38
	}
    public enum EAccess 
    {
        NoAccess = '0',
        ReadOnly = '1',
        FullAccess = '2'
    }
}
