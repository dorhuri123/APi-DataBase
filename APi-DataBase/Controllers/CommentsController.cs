﻿using APi_DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace APi_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly MySqlConnection _connection;

        public CommentsController(MySqlConnection connection)
        {
            _connection = connection;
        }

        [HttpPost("Comment")]
        public IActionResult AddComment([FromBody] Comments comment)
        {
            try
            {
                if(comment.Text != "")
                {
                    _connection.Open();
                    // Insert the new comment into the comments table
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO comments (username, project_id, text, time) VALUES (@username, @projectId, @text, @time)", _connection);
                    //adding the query parameters 
                    cmd.Parameters.AddWithValue("@username", comment.UserName);
                    cmd.Parameters.AddWithValue("@projectId", comment.Project_Id);
                    cmd.Parameters.AddWithValue("@text", comment.Text);
                    cmd.Parameters.AddWithValue("@time", comment.Time);
                    //executing query
                    cmd.ExecuteNonQuery();

                    return Ok();
                }
                else
                {
                    return BadRequest("Empty comment");
                }
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
