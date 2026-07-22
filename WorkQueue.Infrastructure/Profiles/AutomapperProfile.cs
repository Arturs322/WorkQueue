using AutoMapper;
using WorkQueue.Application.DTO.WorkItem;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Profiles
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<WorkItem, WorkItemDto>();
            CreateMap<WorkItemDto, WorkItem>();
            CreateMap<UpdateWorkItemRequest, WorkItemDto>();
            CreateMap<CreateWorkItemRequest, WorkItem>();
            CreateMap<UpdateWorkItemRequest, WorkItem>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
        }
    }
}