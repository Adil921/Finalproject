﻿namespace Banking.Control.Panel.Model.Model.Account

{
    public class LoginResponseModel
    {
        public string?  Email { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }

        public string Role { get; set; }
    }
}