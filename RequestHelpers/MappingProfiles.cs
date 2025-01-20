using System;
using AutoMapper;
using BackendService.DTOs;
using BackendService.DTOs.AuthorizedUser;
using BackendService.DTOs.AuthorizedUSer;
using BackendService.DTOs.CheckboxOption;
using BackendService.DTOs.FormResponseObject;
using BackendService.DTOs.FormTemplate;
using BackendService.DTOs.User;
using BackendService.Entities;

namespace BackendService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, User_DTO>()
            .ForMember(dest => dest.UserType,
                            opt => opt.MapFrom(src => src.UserType.ToString()))
            .ForMember(dest => dest.AccountStatus,
                            opt => opt.MapFrom(src => src.AccountStatus.ToString()));

        CreateMap<CreateUser_DTO, User>();
        CreateMap<Topic, Topic_DTO>().ReverseMap();
        CreateMap<Topic, CreateTopic_DTO>().ReverseMap();
        CreateMap<CreateFormTemplateTag_DTO, FormTemplateTag>();

        CreateMap<UpdateUser_DTO, User>()
            .ForMember(dest => dest.UserType, opts =>
            {
                opts.MapFrom((src, dest) => src.UserType != null ? (Role)Enum.Parse(typeof(Role), src.UserType) : dest.UserType);
            })
            .ForMember(dest => dest.AccountStatus, opts =>
            {
                opts.MapFrom((src, dest) => src.AccountStatus != null ? (Status)Enum.Parse(typeof(Status), src.AccountStatus) : dest.AccountStatus);
            })
            .ForAllMembers(options => options.Condition((src, dest, srcValue) => srcValue != null));
        CreateMap<CreateUserTag_DTO, UserTag>();
        CreateMap<UserTag, UserTag_DTO>();



        CreateMap<FormTemplate, FormTemplate_DTO>()
            .ForMember(dest => dest.AccessControl,
                            opt => opt.MapFrom(src => src.AccessControl.ToString()));
        CreateMap<CreateFormTemplate_DTO, FormTemplate>()
            .ForMember(dest => dest.AccessControl,
                            opt => opt.MapFrom(src => (Access)Enum.Parse(typeof(Access), src.AccessControl))).ForMember(ft => ft.Topic, opts => opts.Ignore());
        CreateMap<CreateBlock_DTO, Block>().ForMember(b => b.BlockType, opts => opts.MapFrom(src => (InputType)Enum.Parse(typeof(InputType), src.BlockType)));
        CreateMap<CreateQuestion_DTO, Question>().ForMember(q => q.Type, opts => opts.MapFrom(src => (QuestionType)Enum.Parse(typeof(QuestionType), src.Type)));

        CreateMap<CreateBlockResponse_DTO, BlockResponse>().ForMember(b => b.BlockType, opts =>
        {
            opts.MapFrom((src, dest) => src.BlockType != null ? (InputType)Enum.Parse(typeof(InputType), src.BlockType) : dest.BlockType);
        });
        CreateMap<UpdateBlockResponse_DTO, BlockResponse>().ForMember(b => b.BlockType, opts =>
        {
            opts.MapFrom((src, dest) => src.BlockType != null ? (InputType)Enum.Parse(typeof(InputType), src.BlockType) : dest.BlockType);
        });
        CreateMap<BlockResponse, BlockResponse_DTO>().ForMember(dest => dest.BlockType, opts => opts.MapFrom(src => src.BlockType.ToString())); ;
        CreateMap<FormResponseObject, FormResponseObject_DTO>().ForMember(fro => fro.Title, opt => opt.MapFrom(src => src.ParentTemplate.Title));
        CreateMap<CreateFormResponseObject_DTO, FormResponseObject>();
        CreateMap<UpdateFormResponseObject_DTO, FormResponseObject>();


        CreateMap<FormTemplateTag, FormTemplateTag_DTO>().ForMember(ftt => ftt.TagName, opt => opt.MapFrom(src => src.UserTag.TagName));

        CreateMap<UpdateFormTemplate_DTO, FormTemplate>().ForMember(f => f.AccessControl, opts =>
        {
            opts.MapFrom((src, dest) => src.AccessControl != null ? (Access)Enum.Parse(typeof(Access), src.AccessControl) : dest.AccessControl);
        }).ForAllMembers(options => options.Condition((src, dest, srcValue) => srcValue != null));
        CreateMap<Topic_DTO, Topic>();

        CreateMap<UpdateBlock_DTO, Block>().ForMember(b => b.BlockType, opts =>
        {
            opts.MapFrom((src, dest) => src.BlockType != null ? (InputType)Enum.Parse(typeof(InputType), src.BlockType) : dest.BlockType);
        }).ForMember(b => b.Id, opt => opt.Ignore()).ForAllMembers(options => options.Condition((src, dest, srcValue) => srcValue != null));
        CreateMap<UpdateQuestion_DTO, Question>().ForMember(q => q.Type, opts =>
        {
            opts.MapFrom((src, dest) => src.Type != null ? (QuestionType)Enum.Parse(typeof(QuestionType), src.Type) : dest.Type);
        }).ForAllMembers(options => options.Condition((src, dest, srcValue) => srcValue != null));

        //-------------------------------------------------------------------------------

        CreateMap<Like, Like_DTO>().ForMember(dest => dest.NormalizedName, opts => opts.MapFrom(src => src.User.NormalizedName));
        CreateMap<Block, Block_DTO>().ForMember(dest => dest.BlockType, opts => opts.MapFrom(src => src.BlockType.ToString()));
        CreateMap<Question, Question_DTO>().ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.Type.ToString()));
        //-------------------------------------------------------------------------------
        CreateMap<CheckboxOption, CheckboxOption_DTO>();
        CreateMap<CreateCheckboxOption_DTO, CheckboxOption>();
        CreateMap<UpdateCheckboxOption_DTO, CheckboxOption>().ForMember(co => co.Id, opt => opt.Ignore());
        CreateMap<AuthorizedUser, AuthorizedUser_DTO>();
        CreateMap<CreateAuthorizedUser_DTO, AuthorizedUser>();
        CreateMap<UpdateAuthorizedUser_DTO, AuthorizedUser>();


        CreateMap<Comment, Comment_DTO>();

        CreateMap<FormTemplate, FormTemplateIndex_DTO>()
            .ForMember(fti => fti.FormTemplateId, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(fti => fti.Description, opt => opt.MapFrom(src => PropExtractor.CollectTextProperties(src.Description)))
            .ForMember(fti => fti.AuthorFullName, opt => opt.MapFrom(src => src.Author.NormalizedName))
            .ForMember(dest => dest.AccessControl,
                            opt => opt.MapFrom(src => src.AccessControl.ToString()));
        CreateMap<User, UserIndex_DTO>().ForMember(ui => ui.UserId, opt => opt.MapFrom(s => s.UserId.ToString()));
        CreateMap<FormResponseObject, FormResponseObjectIndex_DTO>()
            .ForMember(fro => fro.Title, opt => opt.MapFrom(src => src.ParentTemplate.Title))
            .ForMember(fro => fro.RespondentFullName, opt => opt.MapFrom(src => src.Respondent.NormalizedName))
            .ForMember(fro => fro.RespondentEmail, opt => opt.MapFrom(src => src.Respondent.Email));
    }
}



