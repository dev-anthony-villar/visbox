using Box.V2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BoxToLocal.BusinessLogic.Interfaces
{
    public interface IBoxFolderService
    {
        // get folderID as folder 
        Task<BoxFolder> GetFolderDetailsByFolderID(string folderId);

        // get folderId as string 
        Task<string> GetFolderIDByFolderNameParentFolderID(string searchFolderName, string parentFolderId);

        // get folder metadata
        Task<BoxFolder> GetFolderMetaByFolderID(string folderId);

        // get folder contents 
        Task<List<BoxItem>> GetFolderItemsByFolderID(string folderId = "0");

        // return list of folder contents 
        Task<List<BoxItem>> GetFolderItemsRecursiveByFolderID(string folderId);


    }


}
