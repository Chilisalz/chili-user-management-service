﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UserManagementService.Exceptions
{
    public class SecretQuestionNotFoundException : WebApiException
    {
        public SecretQuestionNotFoundException() : base(HttpStatusCode.NotFound)
        {

        }

        public SecretQuestionNotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }
        public SecretQuestionNotFoundException(string message, Exception inner) : base(message, inner, HttpStatusCode.NotFound) { }
    }
}
