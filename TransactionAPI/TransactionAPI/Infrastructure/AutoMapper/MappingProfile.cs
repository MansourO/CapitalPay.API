using AutoMapper;
using Shared.Models;
using Shared.Models.ViewModels;

namespace TransactionAPI.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PostTransaction, Transaction>()
                .ForMember(d => d.FK_TransactionCategoryId, o => o.MapFrom(s => s.TransactionCategoryId));
        }
    }
}