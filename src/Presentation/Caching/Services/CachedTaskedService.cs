//using Api.Requests;
//using TaSked.Api.ApiClient;
//using TaSked.Api.Requests;
//using TaSked.Application;
//using TaSked.Domain;

//namespace TaSked.App.Caching;


//public class CachedTaskedService : ITaSkedSevice, CachedRepository<ITaSkedSevice>
//{
//	public Task ActivateInvitation(ActivateInvintationRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<User>> BanMember(Guid groupId, BanMemberRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Homework> ChangeDeadline(ChangeHomeworkDeadlineRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Homework> ChangeDescription(ChangeHomeworkDescriptionRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<GroupDTO> ChangeGroupName(ChangeGroupNameRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Homework> ChangeSourceUrl(ChangeHomeworkSourceUrlRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<UpdateSubjectDTO> ChangeSubjectName(ChangeSubjectNameRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<UpdateSubjectDTO> ChangeSubjectTeacher(ChangeSubjectTeacherRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Lesson> ChangeTime(ChangeLessonTimeRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Homework> ChangeTitle(ChangeHomeworkTitleRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task ClearCache()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Lesson> Create(CreateLessonRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<GroupDTO> CreateGroup(CreateGroupRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Homework> CreateHomework(CreateHomeworkRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Invitation> CreateInvitation(CreateInvintationRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Report> CreateReport(CreateReportRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<SubjectDTO> CreateSubject(CreateSubjectRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<User> CurrentUser()
//	{
//		throw new NotImplementedException();
//	}

//	public Task Delete(DeleteLessonRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task DeleteGroup()
//	{
//		throw new NotImplementedException();
//	}

//	public Task DeleteHomework()
//	{
//		throw new NotImplementedException();
//	}

//	public Task DeleteSubject(DeleteSubjectRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<User>> DemoteMember(Guid groupId, DemoteMemberRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task ExpireInvitation(ExpireInvintationRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<Lesson>> Get(DateTime? from = null, DateTime? to = null)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<Homework>> GetAllHomework()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<Invitation>> GetAllInvitation()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<Report>> GetAllReports()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<SubjectDTO>> GetAllSubjects()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<Lesson>> GetBySubject(Guid SubjectId)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<GroupDTO> GetGroupById(Guid GroupId)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<Invitation> GetInvitationById(Guid InvitationId)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<User>> GetMembers(Guid groupId)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<User> GetUserById(Guid id)
//	{
//		throw new NotImplementedException();
//	}

//	public Task LeaveGroup()
//	{
//		throw new NotImplementedException();
//	}

//	public Task<List<User>> PromoteMember(Guid groupId, PromoteMemberRequest request)
//	{
//		throw new NotImplementedException();
//	}

//	public Task<string> RegisterAnonymous(CreateUserTokenRequest request)
//	{
//		throw new NotImplementedException();
//	}
//}
