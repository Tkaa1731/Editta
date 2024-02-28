using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
    public enum EPermition
    {
		[Display(Name = "RoleUzivatele")]
        UserRole = 0,//RoleUzivatele = 1

        [Display(Name = "Aplikace")]
        Aplication = 1,//Aplikace = 2

		[Display(Name = "Licence")]
		License = 2,//Licence = 3

		[Display(Name = "Modul")]
        Module = 3,//Modul = 4

		[Display(Name = "LoginUser")]
        LoginUser = 4,//LoginUser = 5

		[Display(Name = "Opravneni")]
        Permition = 5,//Opravneni = 6

		[Display(Name = "ZaznamyVlastnosti")]
        PropertiesRecord = 6,//ZaznamyVlastnosti = 7

		[Display(Name = "TypPlatby")]
        PaymentType = 7,//TypPlatby = 8

		[Display(Name = "Uzivatele")]
        Client = 9,//Uzivatele = 10

		[Display(Name = "Zaznamy")]
        Record = 10,//Zaznamy = 11

		[Display(Name = "PohybyEvidence")]
        EvidenceTransport = 11,//PohybyEvidence = 12

		[Display(Name = "PohybyPokladna")]
        CashTransport = 12,//PohybyPokladna = 13

		[Display(Name = "VyberUzivatele")]
        ClientSelect = 13,//VyberUzivatele = 14

		[Display(Name = "Evidence")]
        Evidence = 14,//Evidence = 15

		[Display(Name = "Permanentky")]
        SeasonTicket = 15,//Permanentky = 16

		[Display(Name = "PermanentkyPlatba")]
        SeasonTicketPayment = 16,//PermanentkyPlatba = 17

		[Display(Name = "PermanentkyEvidence")]
        SeasonTicketEvidence = 17,//PermanentkyEvidence = 18

		[Display(Name = "EvidenceCinnost")]
        ActivityEvidence = 18,//EvidenceCinnost = 19

		[Display(Name = "SouhrnyEvidence")]
        EvidenceSummary = 19,//SouhrnyEvidence = 20

		[Display(Name = "SouhrnyPokladny")]
        CashSummary = 20,//SouhrnyPokladny = 21

		[Display(Name = "SouhrnyEvidenceSklad")]
        EvidenceStorageSummary = 22,//SouhrnyEvidenceSklad = 23

		[Display(Name = "SouhrnyPermanentky")]
        SeasonTicketSummary = 23,//SouhrnyPermanentky = 24

		[Display(Name = "SouhrnyDohodaZprava")]
        ContractReportSummary = 24,//SouhrnyDohodaZprava = 25

		[Display(Name = "SouhrnyCinnost")]
        ActivitySummary = 25,//SouhrnyCinnost = 26

		[Display(Name = "Dohody")]
        Contract = 29,//Dohody = 30

		[Display(Name = "DohodyOsoby")]
        ContractClient = 30,//DohodyOsoby = 31

		[Display(Name = "DohodyPrilohy")]
        ContractSupplement = 31,//DohodyPrilohy = 32

		[Display(Name = "DohodyObdobi")]
        ContractSeason = 32,//DohodyObdobi = 33

		[Display(Name = "DohodyZpravy")]
        ContractReport = 33,//DohodyZpravy = 34

		[Display(Name = "OSPOD")]
        OSPOD = 34, //OSPOD = 35

		[Display(Name = "DohodyOsobyPlan")]
        ContractClientPlan = 35,//DohodyOsobyPlan = 36

		[Display(Name = "DohodyDalsiJevy")]
        ContractOther = 36,//DohodyDalsiJevy = 37

		[Display(Name = "DalsiJevy")]
        Other = 37//DalsiJevy = 38
    }
    public enum EAccess 
    {
        NoAccess = '0',
        ReadOnly = '1',
        FullAccess = '2'
    }
}
