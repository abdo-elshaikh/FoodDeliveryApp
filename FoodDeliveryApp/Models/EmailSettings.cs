﻿using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class EmailSettings
    {
        public int Id { get; set; }

        
        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public bool EnableSsl { get; set; }

        
        public string SenderEmail { get; set; }

        public string SenderName { get; set; }

        
        public string Username { get; set; }

        
        public string Password { get; set; }
    }
}
