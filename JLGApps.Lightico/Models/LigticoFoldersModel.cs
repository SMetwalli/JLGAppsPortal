using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JLGApps.Lightico.Models
{
  


    public class FolderList
    {

        public string Id { get; set; }


        public string Name { get; set; }


        public string Path { get; set; }

        public string Error { get; set; }
    }
    public class Folders
    {
   
        public string Id { get; set; }

    
        public string Name { get; set; }

     
        public string Path { get; set; }

        public string error { get; set; }
    }

  
}
