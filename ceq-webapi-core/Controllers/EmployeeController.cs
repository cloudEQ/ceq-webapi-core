using ceq_webapi_core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewRelic.Api.Agent;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ceq_webapi_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]  
        [Trace]
        public JsonResult GetEmployee()
        {
            DataTable table = new DataTable();

            string query = @"
                          select EmployeeID, EmployeeName, Department
                        , MailID, 
                        convert(varchar(10),DOJ, 120) as DOJ
                           from dbo.Employees
                          ";
            SqlDataReader sqlDataReader;
            using (var con = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon")))
            {
                con.Open();
                using (var cmd = new SqlCommand(query, con))
                {
                    sqlDataReader = cmd.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    con.Close();
                }
            }

            return new JsonResult(table);

        }

        [HttpPost]
        public string Post(Employee emp)
        {
            string doj = emp.DOJ.ToString().Split(' ')[0];
            string query = @"
                 insert into dbo.Employees 
                (EmployeeName,
                Department,
                MailID,
                DOJ)
                Values
                (
                '" + emp.EmployeeName + @"'
                ,'" + emp.Department + @"'
                ,'" + emp.MailID + @"'
                ,'" + doj + @"'
                )
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
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return"Added Successfully";
        }

        [HttpPut]
        public string Put(Employee emp)
        {
            DataTable table = new DataTable();
            string doj = emp.DOJ.ToString().Split(' ')[0];
            string query = @"
                update dbo.Employees set 
                EmployeeName = '" + emp.EmployeeName + @"'
                ,Department = '" + emp.Department + @"'
                ,MailID = '" + emp.MailID + @"'
                ,DOJ = '" + doj + @"'
               where EmployeeID = " + emp.EmployeeID + @"
                          ";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return "Updated Successfully";
        }


        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            string query = @"
                  delete from dbo.Employees where EmployeeID = " + id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }

            return "Deleted Successfully";
        }




    }
}
