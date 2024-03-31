using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Service
{
    public class PaymentService(PaymentTable paymentTable)
    {
        private readonly PaymentTable _paymentTable = paymentTable;

        public async Task<IEnumerable<Payment>> GetByModule(int moduleId)
        {
            return await _paymentTable.GetPayments(moduleId);
        }
		public async Task<int> GetCount(int ModuleId)
		{
			var conditions = new{ ModuleId };
			return await _paymentTable.GetCount(conditions);
		}
		public async Task<Payment> GetPaymentById(int id)
        {
            return await _paymentTable.GetById(id);
        }
        //---------------------------- InsUpDel-------------------------------
        public async Task<Payment?> Update(Payment updatePayment)
        {            
            //SET Server time
            updatePayment.CreationDate = DateTime.Now;

            var rows = await _paymentTable.Update(updatePayment);
            if(rows > 0)
                return await GetPaymentById(updatePayment.Id);
            return null;
        }
        public async Task<Payment?> Insert(Payment insertPayment)
        {
            //SET Server time
            insertPayment.CreationDate = DateTime.Now;

            var id = await _paymentTable.Insert(insertPayment);
            if (id.HasValue)
                return await GetPaymentById(id.Value);
            return null;
        }
        public async Task<int> Delete(int id)
        {
			//Kontrola na existenci navazujících tabulek
			var errors = await _paymentTable.IsDeletable(id);
			if (errors.Any())
				throw new Exception($"Pro typ platby ID:{id},který se pokoušíte smazat existuje platný záznam v tabulce {errors.First().Error}");

			return await _paymentTable.Delete(id);
        }
    }
}
