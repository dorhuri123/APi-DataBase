﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APi_DataBase.Models
{
    public class Likes
    {
       
        [Key, StringLength(15)]
        public string? User_Name { get; set; }
        public int Project_Id { get; set; }
        public DateTime Time { get; set; }
    }
}