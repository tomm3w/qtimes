﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QTimes.api.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}