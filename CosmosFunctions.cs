

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using FinApp.Data;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;

namespace FinApp
{
    public static class GetAll
    {
        [FunctionName("GetAll")]
        
        public static async Task<IEnumerable<Customers>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Get")] HttpRequest req)
        {
            //log.Info("C# HTTP trigger function to get all data from Cosmos DB"); TraceWriter log

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            return await Respository.GetItemsAsync("Customers");
        }
    }
    public static class GetSingle
    {
        [FunctionName("GetSingle")]
        [Obsolete]
        public static async Task<   Customers> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Get/{email}/{password}")] HttpRequest req, TraceWriter log, string email, string password)
        {
            log.Info("C# HTTP trigger function to get a single data from Cosmos DB");

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            var custdetails = await Respository.GetItemsAsync("Customers");
            //var custdetails1 = custdetails.Where(e => e.Email == email && e.Password == password);
            //Customers e = custdetails1.FirstOrDefault<Customers>();
            //if (e.Email != null && e.Email != string.Empty && e.Password!=null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //if(e.Password!=null)
            //{
            //    return true;
            //}
            // return false;
            var employees = await Respository.GetItemsAsync(d => d.Email == email && d.Password == password, "Customers");
            Customers employee = new Customers();

            foreach (var emp in employees)
            {
                employee = emp;
                break;
            }
            return employee;
        }
    }

    public static class CreateOrUpdate
    {
        [FunctionName("CreateOrUpdate")]
        [Obsolete]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "post", "put", Route = "CreateOrUpdate")] HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function to create a record into Cosmos DB");
            try
            {
                IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var employee = JsonConvert.DeserializeObject<Customers>(requestBody);
                if (req.Method == "POST")
                {
                    //employee.Name = null;
                    await Respository.CreateItemAsync(employee, "Customers");
                    
                }
                else
                {
                    await Respository.UpdateItemAsync(employee.Name, employee, "Customers");
                }
                return true;
            }
            catch
            {
                log.Info("Error occured while creating a record into Cosmos DB");
                return false;
            }

        }
    }

    public static class Delete
    {
        [FunctionName("Delete")]
        [Obsolete]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Delete/{name}/{cityName}")] HttpRequest req, TraceWriter log, string name, string cityName)
        {
            log.Info("C# HTTP trigger function to delete a record from Cosmos DB");

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            try
            {
                await Respository.DeleteItemAsync(name, "Customers", cityName);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }




    public static class GetBalance
    {
        [FunctionName("GetBalance")]
        [Obsolete]
        public static async Task<int> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetBalance/{email}/{password}")] HttpRequest req, TraceWriter log, string email, string password)
        {
            log.Info("C# HTTP trigger function to get a single data from Cosmos DB");

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            var custdetails = await Respository.GetItemsAsync("Customers");
           
            var employees = await Respository.GetItemsAsync(d => d.Email == email && d.Password == password, "Customers");
            Customers employee = new Customers();

            foreach (var emp in employees)
            {
                employee = emp;
                break;
            }
            return employee.Balance;
        }
    }



    public static class Deposit
    {
        [FunctionName("Deposit")]
        [Obsolete]
        public static async Task<Customers> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Get/{depositamt}/{email}/{password}")] HttpRequest req, TraceWriter log, string email, string password,int depositamt)
        {
            log.Info("C# HTTP trigger function to get a single data from Cosmos DB");

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            var custdetails = await Respository.GetItemsAsync("Customers");
           
            var employees = await Respository.GetItemsAsync(d => d.Email == email && d.Password == password, "Customers");
            Customers employee = new Customers();

            foreach (var emp in employees)
            {
                employee = emp;
                break;
            }
            employee.Balance = employee.Balance + depositamt;
            await Respository.UpdateItemAsync(employee.Name, employee, "Customers");
            return employee;
            
        }
    }



    public static class Withdraw
    {
        [FunctionName("Withdraw")]
        [Obsolete]
        public static async Task<bool> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "withdraw/{withdrawamt}/{email}/{password}")] HttpRequest req, TraceWriter log, string email, string password, int withdrawamt)
        {
            log.Info("C# HTTP trigger function to get a single data from Cosmos DB");

            IDocumentRepository<Customers> Respository = new DocumentRepository<Customers>();
            var custdetails = await Respository.GetItemsAsync("Customers");

            var employees = await Respository.GetItemsAsync(d => d.Email == email && d.Password == password, "Customers");
            Customers employee = new Customers();

            foreach (var emp in employees)
            {
                employee = emp;
                break;
            }
           if(employee.Balance<=withdrawamt)
            {
                employee.Balance -= withdrawamt;
                return true;
            }
           else
            {
                return false;
            }
           
        }
    }
}
