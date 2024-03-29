﻿using System;
using System.Net;

namespace UserManagementService.Exceptions
{
    public class UserNotFoundException : WebApiException
    {
        public UserNotFoundException() : base(HttpStatusCode.NotFound)
        {

        }

        public UserNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {

        }

        public UserNotFoundException(string message, Exception inner) : base(message, inner, HttpStatusCode.NotFound)
        {

        }
    }
}
