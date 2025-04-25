using AutoMapper;
using TaskManager.DTOs;
using TaskManager.Models;

namespace TaskManager.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Goal mappings
            CreateMap<Goal, GoalDto>();
            CreateMap<CreateGoalDto, Goal>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GoalStatus.NotStarted))
                .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateGoalDto, Goal>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Reminder mappings
            CreateMap<Reminder, ReminderDto>();
            CreateMap<CreateReminderDto, Reminder>()
                .ForMember(dest => dest.IsSent, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateReminderDto, Reminder>();
        }
    }
} 