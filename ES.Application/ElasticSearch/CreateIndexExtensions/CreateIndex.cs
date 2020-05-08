using ES.Domain.Enums;
using Nest;

namespace ES.Application.ElasticSearch.CreateIndexExtensions
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
