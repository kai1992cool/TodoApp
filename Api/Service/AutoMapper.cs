using AutoMapper;
using Service.DTO;
using Service.DTO.TodoItem;
using Service.DTO.User;
using Data.Entities;

namespace Service
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<TodoItemDetail, ToDoItem>().ReverseMap();
            CreateMap<ToDoItem, TodoItemSummary>();
            CreateMap<AddTodoItem, ToDoItem>();
            CreateMap<User, UserDetails>();
            CreateMap<LoginRegisterModel, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));

        }
    }
}
