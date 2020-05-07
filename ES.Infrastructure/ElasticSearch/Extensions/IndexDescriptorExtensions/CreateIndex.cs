using ES.Domain.Enums;
using Nest;

namespace ES.Infrastructure.ElasticSearch.Extensions
{
    public static class CreateIndex
    {
        public static CreateIndexDescriptor CreateIndexSettingAndMapping(this CreateIndexDescriptor indexDescriptor, IndexType type)
        {
            switch(type)
            {
                case IndexType.Management:
                    indexDescriptor.CreateManagementIndexSettingAndMapping();
                    break;
                case IndexType.Property:
                    indexDescriptor.CreateManagementIndexSettingAndMapping();
                    break;
            }

            return indexDescriptor;
        }
    }
}
