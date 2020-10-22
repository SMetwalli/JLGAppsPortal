using JLGProcessPortal.Models.SignNow;
using JLGProcessPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGProcessPortal.Controllers.Vendors
{
    public interface IAssureSign
    {
        List<TemplateInfo> GetTemplates();
        string GetTemplateInfo();


    }

    public interface ISignNow
    {
        List<TemplateInfo> GetTemplates(SignNowAuth authentication, string folderName, string folderId);
        string GetTemplateInfo(SignNowAuth authentication, string folderId);

        FolderList GetFolders(SignNowAuth authentication);
    }

}
