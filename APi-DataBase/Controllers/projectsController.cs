﻿using Microsoft.AspNetCore.Mvc;
using APi_DataBase.Models;
using System.Data;
using MySqlConnector;

namespace APi_DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly MySqlConnection _connection;

        public ProjectsController(MySqlConnection connection)
        {
            _connection = connection;
        }

        // GET: api/projects
        [HttpGet("{startIndex}")]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjects(int startIndex)
        {
            var projects = new List<Projects>();

            try
            {
                _connection.Open();

                var command = new MySqlCommand(@"
                    SELECT p.*, COUNT(l.Project_Id) AS Likes_Count,
                        (SELECT COUNT(*) FROM Comments WHERE Project_Id = p.Id) AS Comments_Count
                    FROM Projects p
                    LEFT JOIN Likes l ON l.Project_Id = p.Id
                    GROUP BY p.Id
                    ORDER BY p.created_timestamp DESC
                    LIMIT @startIndex, 50", _connection);

                command.Parameters.AddWithValue("@startIndex", startIndex);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var project = new Projects
                        {
                            Id = reader.GetInt32("Id"),
                            Platform = reader.GetString("Platform"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Created_Timestamp = reader.GetDateTime("created_timestamp"),
                            Homepage_Url = reader.GetString("Homepage_Url"),
                            Repository_Url = reader.GetString("Repository_Url"),
                            Language = reader.GetString("Language"),
                            Repository_Id = reader.GetInt32("Repository_Id"),
                            Likes_Count = reader.GetInt32("Likes_Count"),
                            Comments_Count = reader.GetInt32("Comments_Count")
                        };
                        projects.Add(project);
                    }
                }
                
                return Ok(projects);
            }
            catch (MySqlException)
            {
                return BadRequest();
            }
        }

        // GET: api/projects
        [HttpGet("{startIndex}, {numVersions}")]
        public async Task<ActionResult<IEnumerable<Projects>>> GetProjectsVersions(int startIndex, int numVersions)
        {
            var projects = new List<Projects>();

            try
            {
                _connection.Open();

                var command = new MySqlCommand("SELECT p.* " +
                "FROM Projects p JOIN Repositories r " +
                "ON p.Repository_Id = r.Id JOIN " +
                "(SELECT Project_Id, COUNT(*) AS num_versions " +
                "FROM Versions GROUP BY Project_Id) AS v ON p.Id = v.Project_Id " +
                "WHERE v.num_versions > @numVersions AND r.Forks_count > (SELECT AVG(Forks_count) " +
                "FROM Repositories) LIMIT @startIndex,50", _connection);

                command.Parameters.AddWithValue("@startIndex", startIndex);
                command.Parameters.AddWithValue("@numVersions", numVersions);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var project = new Projects
                        {
                            Id = reader.GetInt32("Id"),
                            Platform = reader.GetString("Platform"),
                            Name = reader.GetString("Name"),
                            Description = reader.GetString("Description"),
                            Created_Timestamp = reader.GetDateTime("created_timestamp"),
                            Homepage_Url = reader.GetString("Homepage_Url"),
                            Repository_Url = reader.GetString("Repository_Url"),
                            Language = reader.GetString("Language"),
                            Repository_Id = reader.GetInt32("Repository_Id"),
                        };
                        projects.Add(project);
                    }
                }

                return Ok(projects);
            }
            catch (MySqlException)
            {
                return BadRequest();
            }
        }
    }
}
