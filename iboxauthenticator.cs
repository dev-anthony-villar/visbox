using Box.V2;
using System;
using System.Threading.Tasks;

namespace BoxToLocal.BusinessLogic.Interfaces
{
    public interface IBoxAuthenticator
    {
        // add methods 
        Task<BoxClient> GetAdminClientAsync();

    }


}
