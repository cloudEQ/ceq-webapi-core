using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ceq_webapi_core.Model;

namespace ceq_webapi_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController { 

    private readonly IConfiguration _configuration;

    public DepartmentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                          select DepartmentID, DepartmentName from
                          dbo.Departments 
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

            return new JsonResult(table);
        }


        [HttpPost]
        public string Post(Department dep)
        {
            string query = @"
                         insert into dbo.Departments values ('" + dep.DepartmentName + @"')
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

            return "Added Successfully";
        }


        [HttpPut]
        public string Put(Department dep)
        {
            string query = @"
                  update dbo.Departments set DepartmentName = '" + dep.DepartmentName + @"'
                        where DepartmentID = " + dep.DepartmentID + @"
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

            return "Updated Successfully";
        }


        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            string query = @"
                  delete from dbo.Departments 
                  where DepartmentID = " + id;
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
