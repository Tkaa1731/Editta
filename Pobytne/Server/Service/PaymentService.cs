using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;

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
    }
}
