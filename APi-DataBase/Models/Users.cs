﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APi_DataBase.Models
{
    public class Users

    {
        [Key, StringLength(15)]
        public string? User_Name { get; set; }
        [StringLength(15)]
        public string? Password { get; set; }
        [StringLength(15)]
        public string? Full_Name { get; set; }
    }
}