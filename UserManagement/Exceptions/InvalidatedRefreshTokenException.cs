﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UserManagementService.Exceptions
{
    public class InvalidatedRefreshTokenException : WebApiException
    {
        public InvalidatedRefreshTokenException() : base(HttpStatusCode.Conflict)
        {

        }

        public InvalidatedRefreshTokenException(string message) : base(message, HttpStatusCode.Conflict)
        {

        }

        public InvalidatedRefreshTokenException(string message, Exception inner) : base(message, inner, HttpStatusCode.Conflict)
        {

        }
    }
}
