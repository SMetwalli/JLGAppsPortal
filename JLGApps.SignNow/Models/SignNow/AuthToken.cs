﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JLGApps.SignNow.Models.SignNow
{
    public class AuthToken
    {
       
            public string access_token { get; set; }
            public string refresh_token { get; set; }

            public int expires_in { get; set; }
   
    }
}
