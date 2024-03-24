using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Struct
{
    public enum EPermition
    {
		[Display(Name = "RoleUzivatele")]
        UserRole = 0,

        [Display(Name = "Aplikace")]
        Aplication = 1,

		[Display(Name = "Licence")]
		License = 2,

		[Display(Name = "Modul")]
        Module = 3,

		[Display(Name = "LoginUser")]
        LoginUser = 4,

		[Display(Name = "Opravneni")]
        Permition = 5,

		[Display(Name = "ZaznamyVlastnosti")]
        RecordAttribute = 6,

		[Display(Name = "TypPlatby")]
        PaymentType = 7,

		[Display(Name = "Uzivatele")]
        Client = 9,

		[Display(Name = "Zaznamy")]
        Record = 10,

		[Display(Name = "PohybyEvidence")]
        EvidenceTransport = 11,

		[Display(Name = "PohybyPokladna")]
        CashTransport = 12,

		[Display(Name = "VyberUzivatele")]
        ClientSelect = 13,

		[Display(Name = "Evidence")]
        Evidence = 14,

		[Display(Name = "Permanentky")]
        SeasonTicket = 15,

		[Display(Name = "PermanentkyPlatba")]
        SeasonTicketPayment = 16,

		[Display(Name = "PermanentkyEvidence")]
        SeasonTicketEvidence = 17,

		[Display(Name = "EvidenceCinnost")]
        ActivityEvidence = 18,

		[Display(Name = "SouhrnyEvidence")]
        EvidenceSummary = 19,

		[Display(Name = "SouhrnyPokladny")]
        CashSummary = 20,

		[Display(Name = "SouhrnyEvidenceSklad")]
        EvidenceStockSummary = 22,

		[Display(Name = "SouhrnyPermanentky")]
        SeasonTicketSummary = 23,

		[Display(Name = "SouhrnyDohodaZprava")]
        ContractReportSummary = 24,

		[Display(Name = "SouhrnyCinnost")]
        ActivitySummary = 25,

		[Display(Name = "Dohody")]
        Contract = 29,

		[Display(Name = "DohodyOsoby")]
        ContractClient = 30,

		[Display(Name = "DohodyPrilohy")]
        ContractSupplement = 31,

		[Display(Name = "DohodyObdobi")]
        ContractSeason = 32,

		[Display(Name = "DohodyZpravy")]
        ContractReport = 33,

		[Display(Name = "OSPOD")]
        OSPOD = 34, 

		[Display(Name = "DohodyOsobyPlan")]
        ContractClientPlan = 35,

		[Display(Name = "DohodyDalsiJevy")]
        ContractOther = 36,

		[Display(Name = "DalsiJevy")]
        Other = 37
    }
    public enum EAccess 
    {
        NoAccess = '0',
        ReadOnly = '1',
        FullAccess = '2'
    }
}
