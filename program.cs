using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using BoxToLocal.Infrastructure;
using BoxToLocal.BusinessLogic.Services;
using BoxToLocal.ServiceIntegrations.BoxService;
using BoxToLocal.BusinessLogic.Interfaces;
using System;
using System.Threading.Tasks;
using Box.V2.JWTAuth;
using Box.V2;
using Box.V2.Models;

namespace Import_Pelican_Test_Dir
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            // Build configuration
            var configuration = new ConfigurationBuilder()
                //  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

            // Add config files 
            var serviceProvider = new ServiceCollection()
                .Configure<BoxServiceConfig>(configSection =>
                {
                    configuration.GetSection("BoxServiceConfig").Bind(configSection);
                })
                .Configure<ProxyConfig>(configSection =>
                {
                    configuration.GetSection("ProxyConfig").Bind(configSection);
                })

            // add services 
                .AddTransient<IBoxAuthenticator, BoxAuthenticator>()
                .AddTransient<IBoxFolderService, BoxFolderService>()
                
                .BuildServiceProvider();

            // Resolve Services 
            var boxAuthenticator = serviceProvider.GetService<IBoxAuthenticator>();
            var boxFolderService = serviceProvider.GetService<IBoxFolderService>();


            // add implementations 

            await TestBoxAuthentication(boxAuthenticator);

            await ListDirContentsRecursive(boxFolderService, "2023", "0");







        }


        // implementation auth test 
        private static async Task TestBoxAuthentication(IBoxAuthenticator boxAuthenticator)
        {
            try
            {
                BoxClient boxClient = await boxAuthenticator.GetAdminClientAsync();

                // Fetch the current user's information to test the authentication
                BoxUser boxCurrentUser = await boxClient.UsersManager.GetCurrentUserInformationAsync();

                Console.WriteLine($"ID: {boxCurrentUser.Id}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication failed: {ex.Message}");
            }
        }

        // implementation printlistfile 
        private static async Task ListDirContentsRecursive(IBoxFolderService boxFolderService, string folderName,string folderId)
        {
            try
            {
                var searchFolderID = await boxFolderService.GetFolderIDByFolderNameParentFolderID(folderName, folderId);

                await boxFolderService.GetFolderItemsRecursiveByFolderID(searchFolderID);
                
                // Process and print the items
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to list directory contents: {ex.Message}");
            }
        }

    }

    
}
