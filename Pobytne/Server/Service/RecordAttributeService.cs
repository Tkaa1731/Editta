using Pobytne.Data.Tables;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Service
{
    public class RecordAttributeService(RecordAttributeTable attributeTable)
    {
        private readonly RecordAttributeTable _attributeTable = attributeTable;
        public async Task<IEnumerable<RecordAttribute>> GetAttibutesByModule(int moduleId)
        {
            return await _attributeTable.GetAttributesByModule(moduleId);
        }
        public async Task<RecordAttribute?> Update(RecordAttribute updateAttribute)
        {
            //SET Server time
            updateAttribute.CreationDate = DateTime.Now;

            var rows = await _attributeTable.Update(updateAttribute);
            if (rows > 0)
                return await _attributeTable.GetById(updateAttribute.Id);
            return null;
        }
        public async Task<RecordAttribute?> Insert(RecordAttribute insertAttribute)
        {
            //SET Server time
            insertAttribute.CreationDate = DateTime.Now;

            var id = await _attributeTable.Insert(insertAttribute);
            if (id.HasValue)
                return await _attributeTable.GetById(id.Value);
            return null;
        }
        public async Task<int> Delete(int id)
        {
            var errors = await _attributeTable.IsDeletable(id);
            if (errors.Any())
                throw new Exception($"Pro vlastnost záznamu {id},kterou se pokoušíte smazat existuje platný záznam v tabulce {errors.First().Error}");

            return await _attributeTable.Delete(id);
        }
    }
}
