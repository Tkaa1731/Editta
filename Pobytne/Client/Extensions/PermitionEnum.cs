namespace Pobytne.Client.Extensions
{
    public enum PermitionEnum
    {
        Aplication,//.Aplikace, "nastavení aplikace")
        License,//.Licence, "seznam licencí") // seznam
        LoginUser,//LoginUser, "seznam uživatelů aplikace")
        Module,//Modul, "seznam modulů")
        Permition,//Opravneni, "seznam oprávnění")
        PropertiesRecord,//ZaznamyVlastnosti, "seznam účetní spefikace")
        PaymentType,//TypPlatby, "typy plateb")
        Record,//Zaznamy, "seznam záznamů") // seznam
        EvidenceMotion,//PohybyEvidence, "sklad zboží")
        CashMotion,//PohybyPokladna, "vedlejší pokladna")
        Customer,//Uzivatele, "seznam klientů")
        Evidence,//Evidence, "evidence záznamů")
        Pernament,//Permanentky, "permanentky")
        EvidenceSummary,//SouhrnyEvidence, "souhrny evidence")// souhrny
        CashSummary,//SouhrnyPokladny, "souhrny druhů plateb")
        PernamentSummary,//SouhrnyPermanentky, "souhrny permanentky")
        ContractReportSummary,//SouhrnyDohodaZprava, "souhrny dohody - zprávy")
        EvidenceStorageSummary,//SouhrnyEvidenceSklad, "souhrny stavu zásob")
        Contract,//Dohody, "dohody vpp") /// dohody
        ContractPerson,//DohodyOsoby, "dohody - osoby")
        ContractSupplement,//DohodyPrilohy, "dohody - přílohy")
        ContractSeason,//DohodyObdobi, "dohody dle období")
        ContractReport//DohodyZpravy, "dohody - zprávy")
    }
    public enum AccessEnum 
    {
        NoAccess = '0',
        ReadOnly = '1',
        FullAccess = '2'
    }
}
