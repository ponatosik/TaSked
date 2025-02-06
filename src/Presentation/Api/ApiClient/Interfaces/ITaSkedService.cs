namespace TaSked.Api.ApiClient;

public interface ITaSkedService :
	ITaSkedUsers,
	ITaSkedMembers,
	ITaSkedGroups,
	ITaSkedLessons,
	ITaSkedAnnouncements,
	ITaSkedSubjects,
	ITaSkedHomeworks,
	ITaSkedInvitations,
	ITaSkedRegistration;
