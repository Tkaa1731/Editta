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
        public async Task<int> Delete(int it)
        {
            return await _paymentTable.Delete(it);
        }
    }
}
