using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;

namespace PL.Helper
{
    public static class DocumentSettings
    {
        // Upload
        // Valid For Create/Update File

        public static string UploadFile(IFormFile File, string FolderName)
        {
            // 1.Get Located FolderPath

            // this Way Not Recommended as may users don't have the same Director like me 
            //C:\Route\.Net\.Net\Aliaa\MVC\Session 05\Session-05-ProjectMVC Session 04 with  GenaricRepository\Project MVC\Generic-ProjectMVCSolution\PL\wwwroot\
            // Directory.GetCurrentDirectory() Equals =>   //C:\Route\.Net\.Net\Aliaa\MVC\Session 05\Session-05-ProjectMVC Session 04 with  GenaricRepository\Project MVC\Generic-ProjectMVCSolution\PL
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName); // Recommended


            // 2. Get FileName and Make it Unique
            //File.Name => this means type of file 
            //File.FileName => this means Name of file 
            //string FileName = Guid.NewGuid() + File.FileName;         
            string FileName = $"{Guid.NewGuid()}{File.FileName}";

            //3. Get FilePath[FolderPath + FileName ]

            string FilePath = Path.Combine(FolderPath, FileName);

            //4.Save File As Stream

            // FileMode.CreateNew => If file already Exits it will throw an Exception

            // FileMode.Create => If file already Exits it will Override on it
            //using => to close the stream
            using var FS = new FileStream(FilePath, FileMode.Create);

            File.CopyTo(FS); // To copy the file into the server 

            //5.Retuen FileName

            return FileName;

        }


        // Delete

        public static void DeleteFile(string FileName, string FolderName)
        {
            // 1.Get File Path
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);

            // 2.check if file exists or not 
            if (File.Exists(FilePath))
            {
                // if exists remove it
                File.Delete(FilePath);
            }
        }
    }
}
