﻿using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Contracts.Requests
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Username cannot be empty.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password cannot be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
