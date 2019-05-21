using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace web.util
{
    public static class WorkFile
    {

        /// <summary>
        /// ذخیره فایل در مسیر روت برنامه
        /// </summary>
        /// <param name="file">فایل دریافتی از کلاینت</param>
        /// <param name="FolderName">نام پوشه میتواند به این شکل باشد : Folder/Subfolder/SubOfSubFolder</param>
        /// <param name="FileType">نوع فایل مجاز</param>
        /// <param name="StopExtentions">فرمت های غیر مجاز : png,gif</param>
        /// <param name="_environment">تزریق شده به کنترلر برنامه </param>
        /// <returns></returns>
        internal static async Task<(bool, string)> UploadFileAsync(IFormFile file, string FolderName, FileType fileType, List<string> AllowdExtention, HostingEnvironment _environment)
        {

            try
            {
                if (file != null)
                {
                    string Message = Message = "فایل وارد شده مجاز نمی باشد";

                    //check file Type in Dot net core is Null 
                    //file.ContentType.StartsWith(fileType.ToString()))
                    string Extention = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                    if (!AllowdExtention.Contains(Extention))
                    {
                        return (false, Message);
                    }
                    var uploadsRootFolder = Path.Combine(_environment.WebRootPath, FolderName);
                    if (!Directory.Exists(uploadsRootFolder))
                    {
                        Directory.CreateDirectory(uploadsRootFolder);
                    }
                    string uinqName = Const.Generatetoken();
                    var fileName = $"{uinqName}.{Extention}";

                    var filePath = Path.Combine(uploadsRootFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream).ConfigureAwait(false);
                    }
                    return (true, fileName);
                }
                else
                {
                    return (false, "فایلی دریافت نشد");
                }


            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }



        public static async Task WriteFileAsync(HostingEnvironment env, string data, string FolderName, string FileNameWithextention, bool Apppend)
        {

            var path = Path.Combine(env.WebRootPath, $@"{FolderName}").ToLower();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, FileNameWithextention), Apppend))
            {
                await outputFile.WriteLineAsync(data);
            }


        }



    }

    public enum FileType
    {
        image,
        text,
        audio,
        video,
        application,
        font

    }
}
