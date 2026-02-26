using Dapper;
using fsm_api.Common;
using fsm_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace fsm_api.Repository
{
 
    public class JobRepository
    {
        private readonly DapperDataService _dataService;

        public JobRepository()
        {
            _dataService = new DapperDataService();
        }
        public async Task<List<JobsModel>> GetMyJobs()
        {
            var parameters = new DynamicParameters();

            string query = @"SELECT JobId,[JobNumber],CustomerName,MobileNo,d.ServiceName,a.ScheduledOn,
e.AddressLine1+','+e.Area+','+e.City+','+e.[State]+','+e.Pincode as [Address],e.[Latitude],e.[Longitude],a.JobStatus

  FROM [Jobs] as a inner join [Customers] as b on a.[CustomerId]=b.[CustomerId]
  inner join [Leads] as c on c.LeadId=a.LeadId
  inner join [ServiceTypes] as d on d.ServiceTypeId=c.ServiceTypeId
  inner join CustomerAddresses as e on e.CustomerAddressId=c.CustomerAddressId";


            var list = await _dataService.GetAllAsync<JobsModel>(query,parameters);

            return list.ToList();
        }


        public async Task<List<Items>> GetItems()
        {
            var parameters = new DynamicParameters();

            string query = @"SELECT * FROM Items WHERE IsActive=1";


            var list = await _dataService.GetAllAsync<Items>(query, parameters);

            return list.ToList();
        }

        public async Task<int> CreateQuotation(CreateQuotation createQuotation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", createQuotation.JobId);
            parameters.Add("@SubTotal", createQuotation.SubTotal);
            parameters.Add("@DiscountValue", createQuotation.DiscountValue);
            parameters.Add("@CGST", createQuotation.CGST);
            parameters.Add("@SGST", createQuotation.SGST);
            parameters.Add("@TotalAmount", createQuotation.TotalAmount);
            parameters.Add("@Status", createQuotation.Status);
            parameters.Add("@CreatedBy", createQuotation.CreatedBy);
            parameters.Add("@Items", createQuotation.Items);


            return await _dataService.ExecuteAsync("Sp_CreateQuotation", parameters);
        }


        //public async Task<(List<Mst_Scrap_Type>, List<ProductDetailsModel>)> GetProductDetails(int CityId)
        //{
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@CityID", CityId);
        //    using (var multi = await _dataService.QueryMultipleAsync(
        //        "GETProductList",
        //        parameters,
        //        CommandType.StoredProcedure))
        //    {
        //        var scrapTypes = (await multi.ReadAsync<Mst_Scrap_Type>()).AsList();
        //        var scrapCategories = (await multi.ReadAsync<ProductDetailsModel>()).AsList();
        //        return (scrapTypes, scrapCategories);
        //    }
        //}
    }
}