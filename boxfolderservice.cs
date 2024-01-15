using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Box.V2;
using Box.V2.Models;
using BoxToLocal.BusinessLogic.Interfaces;




namespace BoxToLocal.BusinessLogic.Services
{
    // implementations of Box.com Folder related services 

        /*  Notes
         *  
         *  
         *  box service account's root directory => parentFolderId = 0 . 
         *  
         *  download/file tasks in BoxFileServices.cs
         *  
         *  TO DO
         *  
         *  Add token persistance
         *  
            
         
         */

    //  Folder Services
	public class BoxFolderService : IBoxFolderService
	{
        //  Authenticator 
        private readonly IBoxAuthenticator _boxAuthenticator;
        public BoxFolderService(IBoxAuthenticator boxAuthenticator)
        {
            _boxAuthenticator = boxAuthenticator;
        }

        // Add tasks to be accessible through interface

        // return Folder id as box folder
        public async Task<BoxFolder> GetFolderDetailsByFolderID(string folderId)
        {
            try
            {
                var adminClient = await _boxAuthenticator.GetAdminClientAsync();

                // Fetch the folder details from Box
                BoxFolder folder = await adminClient.FoldersManager.GetInformationAsync(folderId);
                return folder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching folder details: {ex.Message}");
                return null;
            }
        }

        // return FolderID by folder name, and parent folderID box.com root folder for account access is set to 0
        public async Task<string> GetFolderIDByFolderNameParentFolderID(string searchFolderName = "New Folder", string parentFolderId = "0")
        {
            try
            {
                var boxClient = await _boxAuthenticator.GetAdminClientAsync();

                var items = await boxClient.FoldersManager.GetFolderItemsAsync(parentFolderId, 500, 0, new List<string>() { "name" });

                var folder = items.Entries.FirstOrDefault(i => i.Type == "folder" && i.Name == searchFolderName);

                if (folder == null)
                {
                    Console.WriteLine($"Folder '{searchFolderName}' not found in folder with ID {parentFolderId}");
                    return null;
                }

                Console.WriteLine($"Folder '{searchFolderName}' found with ID {folder.Id}");
                return folder.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get folder ID: {ex.Message}");
                return null;
            }
        }

        // return FolderMeta data by FolderID
        public async Task<BoxFolder> GetFolderMetaByFolderID(string folderId)
        {
            try
            {
                var adminClient = await _boxAuthenticator.GetAdminClientAsync();

                // Fetch the folder details from Box
                BoxFolder folder = await adminClient.FoldersManager.GetInformationAsync(folderId);
                return folder;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching folder details: {ex.Message}");
                return null;
            }
        }

        // get folder items from FolderId -- gets both folder and file id's accessible via type
        public async Task<List<BoxItem>> GetFolderItemsByFolderID(string folderId = "0")
        {
            try
            {
                var boxClient = await _boxAuthenticator.GetAdminClientAsync();

                var itemsList = await boxClient.FoldersManager.GetFolderItemsAsync(folderId, 500, 0);

                if (itemsList == null || !itemsList.Entries.Any())
                {
                    Console.WriteLine($"No contents found in folder with ID {folderId}");
                    return new List<BoxItem>();
                }

                Console.WriteLine($"Retrieved contents of folder with ID {folderId}");
                return itemsList.Entries;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get folder contents: {ex.Message}");
                return null;
            }
        }

        // get list of items recursively from FolderID 
        public async Task<List<BoxItem>> GetFolderItemsRecursiveByFolderID(string folderId)
        {
            try
            {
                var boxClient = await _boxAuthenticator.GetAdminClientAsync();
                List<BoxItem> allItems = new List<BoxItem>();
                // get root folderID
                BoxFolder currentFolder = await GetFolderDetailsByFolderID(folderId);
            
                // FolderID not found 
                if (currentFolder == null)
                {
                    Console.WriteLine($"Folder with ID {folderId} not found.");
                    return allItems;
                }

                // Folder Empty -- end recursion
                List<BoxItem> folderContents = await GetFolderItemsByFolderID(folderId);
                if (folderContents == null || folderContents.Count == 0)
                {
                    Console.WriteLine($"Folder '{currentFolder.Name}' is empty or null.");
                    return allItems;
                }

                Console.WriteLine($"\nFolder: {currentFolder.Name} (ID: {folderId})");

                // add item to list add box subfolder items to list 
                foreach (var item in folderContents)
                {
                    allItems.Add(item);

                    if (item is BoxFolder folder)
                    {
                        var subFolderItems = await GetFolderItemsByFolderID(folder.Id);
                        allItems.AddRange(subFolderItems);
                    }
                    else if (item is BoxFile file)
                    {
                        Console.WriteLine($"File: {file.Name} (ID: {file.Id})");
                    }
                }

                return allItems;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get folder contents: {ex.Message}");
                return null;
            }
        }


    }
}
