using Microsoft.Azure.Cosmos.Table;

namespace GradeHoraria.Models
{
    public class AzureTableStorageUser : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string ETag { get; set; }
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int[] MateriaId { get; set; }
        public int[] CursoId { get; set; }
        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            PartitionKey = properties["PartitionKey"].StringValue;
            RowKey = properties["RowKey"].StringValue;
            Timestamp = properties["Timestamp"].DateTimeOffsetValue.Value;
            ETag = properties["ETag"].StringValue;
            Id = properties["Id"].StringValue;
            Nome = properties["Nome"].StringValue;
            Email = properties["Email"].StringValue;
            MateriaId = Array.ConvertAll(properties["MateriaId"].StringValue.Split(','), int.Parse);
            CursoId = Array.ConvertAll(properties["CursoId"].StringValue.Split(','), int.Parse);
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var properties = new Dictionary<string, EntityProperty>();
            properties.Add("PartitionKey", new EntityProperty(PartitionKey));
            properties.Add("RowKey", new EntityProperty(RowKey));
            properties.Add("Timestamp", new EntityProperty(Timestamp));
            properties.Add("ETag", new EntityProperty(ETag));
            properties.Add("Id", new EntityProperty(Id));
            properties.Add("Nome", new EntityProperty(Nome));
            properties.Add("Email", new EntityProperty(Email));
            properties.Add("MateriaId", new EntityProperty(string.Join(",", MateriaId)));
            properties.Add("CursoId", new EntityProperty(string.Join(",", CursoId)));
            return properties;
        }
    }
}