﻿namespace APi_DataBase.Models
{
    public class Comments
    {
        //attributes that needed for comments
        public int Id { get; set; }
        public string? UserName { get; set; }
        public int Project_Id { get; set; }
        public string? Text { get; set; }
        public DateTime? Time { get; set; }
    }
}
