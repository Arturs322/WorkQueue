using AutoMapper;
using WorkQueue.Application.DTO.WorkItem;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Profiles
{
    public class WorkItemProfile : Profile
    {
        public WorkItemProfile()
        {
            CreateMap<WorkItem, WorkItemDto>();
            CreateMap<WorkItemDto, WorkItem>();
            CreateMap<UpdateWorkItemRequest, WorkItemDto>();
            CreateMap<CreateWorkItemRequest, WorkItem>();
            CreateMap<UpdateWorkItemRequest, WorkItem>();
        }
    }
}