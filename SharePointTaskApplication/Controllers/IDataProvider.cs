using System.Collections.Generic;

namespace SharePointTaskApplication.Controllers
{
    public interface IDataProvider
    {
        void ChangeApproveStatus(int personId, bool approvalStatus);
        void DeletePerson(int personId);
        void UpdatePerson(PersonData person);
        List<PersonData> GetData();
    }
}