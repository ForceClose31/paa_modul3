﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PercobaanApi2.Models;

namespace PercobaanApi2.Controllers
{
    public class PersonController : Controller
    {
        private string __constr;
        private readonly IConfiguration _configuration;

        public PersonController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase");
            _configuration = configuration;
        }

        public IActionResult index()
        {
            return View();
        }

        [HttpPost("api/login")]
        public IActionResult LoginUser(string namaUser, string password)
        {
            var context = new PersonContext(_configuration.GetConnectionString("WebApiDatabase"));

            if (context.IsValidUser(namaUser, password))
            {
                return Ok(new { token = context.GenerateJwtToken(namaUser, _configuration) });
            }

            return Unauthorized();
        }

        [Authorize]

        [HttpGet("api/mahasiswa")]

        public ActionResult<Mahasiswa> ListPerson()
        {
            PersonContext context = new PersonContext(this.__constr);
            List<Mahasiswa> ListPerson = context.ListPerson();
            return Ok(ListPerson);
        }

        [HttpPost("api/mahasiswa/create")]
        public IActionResult CreatePerson([FromBody] Mahasiswa person)
        {
            PersonContext context = new PersonContext(this.__constr);
            context.AddPerson(person);
            return Ok("Person added successfully.");
        }

        [HttpPut("api/mahasiswa/update/{id}")]
        public IActionResult UpdatePerson(int id, [FromBody] Mahasiswa person)
        {
            person.id_person = id;
            PersonContext context = new PersonContext(this.__constr);
            context.UpdatePerson(person);
            return Ok("Person updated successfully.");
        }

        [HttpDelete("api/mahasiswa/delete/{id}")]
        public IActionResult DeletePerson(int id)
        {
            PersonContext context = new PersonContext(this.__constr);
            context.DeletePerson(id);
            return Ok("Person deleted successfully.");
        }

    }

}