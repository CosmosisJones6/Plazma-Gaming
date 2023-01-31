using WebApi.ModelLayer;

namespace WebApi.DataAccessLayer
{
    public interface IMemberDataAccess
    {
        public bool CreateMember(Member member);
        public Member FindMemberFromId(int memberId);
        public IEnumerable<Member> GetAllMembers();
        public bool UpdateMember(Member member);
        public bool DeleteMember(int memberId);
    }
}
