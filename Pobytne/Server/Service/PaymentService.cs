using DocumentFormat.OpenXml.Office2010.Excel;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;

namespace Pobytne.Server.Service
{
    public class PaymentService
    {
        private readonly PaymentTable _paymentTable;
        public PaymentService(PaymentTable paymentTable) => _paymentTable = paymentTable;
        public async Task<IEnumerable<Payment>> GetByModule(int moduleId)
        {
            return await _paymentTable.GetPayments(moduleId);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<int> Update(Payment updatePayment)
        {
            return await _paymentTable.Update(updatePayment);
        }
        public async Task<int?> Insert(Payment insertPayment)
        {
            return await _paymentTable.Insert(insertPayment);
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _paymentTable.IsDeletable(id);//TODO: KDE VSUDE?
			if (errors.Any())
				throw new Exception($"Pro typ platby ID:{id},který se pokoušíte smazat existuje platný záznam v tabulce {errors}");

			return await _paymentTable.Delete(id);
        }
    }
}
