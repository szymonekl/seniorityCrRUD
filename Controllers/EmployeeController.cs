using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using my_new_app.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace my_new_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    select EmployeeID, EmployeeName,Department,
                    convert(varchar(10),DateOfJoining,120) as DateOfJoining
                    ,PhotoFileName
                    from dbo.Employee";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult(table);

        }



        [HttpPost]
        public JsonResult Post(Employee emp)

        {
            string query = @"INSERT INTO dbo.Employee
            (EmployeeName,Department,DateOfJoining,PhotoFileName)
            values (
            '" + emp.EmployeeName + @"',
            '" + emp.Department + @"',
            '" + emp.DateOfJoining + @"',
            '" + emp.PhotoFileName + @"'
            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Added seuccessfully");

        }
        [HttpPut]
        public JsonResult Put(Employee emp)

        {
            string query = @"UPDATE dbo.Employee SET  
            EmployeeName = '" + emp.EmployeeName + @"',
            Department = '" + emp.Department + @"',
            DateOfJoining = '" + emp.DateOfJoining + @"',
            PhotoFileName = '" + emp.PhotoFileName + @"'
            WHERE EmployeeId=" + emp.EmployeeId + @"

                   ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("change seuccessfully");

        }

        [HttpDelete]
        public JsonResult Delete(Employee emp)

        {
            string query = @"DELETE FROM dbo.Employee WHERE EmployeeId=" + emp.EmployeeId + @"

                   ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Deleted seuccessfully");

        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SeveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/"+filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

    }
}
