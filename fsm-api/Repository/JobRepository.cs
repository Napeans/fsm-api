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
            parameters.Add("@UserID", CommonMentods.UserId);

            var list = await _dataService.GetAllAsync<JobsModel>("Sp_GetMyJobs", parameters);

            return list.ToList();
        }


        public async Task<List<Items>> GetItems()
        {
            var parameters = new DynamicParameters();

            string query = @"SELECT * FROM Items WHERE IsActive=1";


            var list = await _dataService.GetAllAsync<Items>(query, parameters);

            return list.ToList();
        }
        public async Task<List<QuotationItem>> GetQuotationItems(int QuotationId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@QuotationId", QuotationId);
            string query = @"select a.QuotationItemId,a.ItemId,b.ItemName,a.UnitPrice,a.Quantity,a.TotalPrice from [QuotationItems] 
as a inner join [Items] as b on a.ItemId=b.ItemId
WHERE QuotationId=@QuotationId
";


            var list = await _dataService.GetAllAsync<QuotationItem>(query, parameters);

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
        public async Task<int> AddOrRemoveQuotationItems(QuotationItemsModel quotationItemsModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@QuotationId", quotationItemsModel.QuotationId);
            parameters.Add("@QuotationItemId", quotationItemsModel.QuotationItemId);
            parameters.Add("@ItemId", quotationItemsModel.ItemId);
            parameters.Add("@Quantity", quotationItemsModel.Quantity);
            parameters.Add("@Flag", quotationItemsModel.Flag);


            return await _dataService.ExecuteAsync("Sp_AddOrRemoveQuotationItems", parameters);
        }

        

        public async Task<int> UpdateSatus(UpdateStatusModel updateStatusModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", CommonMentods.UserId);
            parameters.Add("@JobId", updateStatusModel.JobId);
            parameters.Add("@Status", updateStatusModel.Status);
            parameters.Add("@Latitude", updateStatusModel.Latitude);
            parameters.Add("@Longitude", updateStatusModel.Longitude);


            return await _dataService.ExecuteAsync("Sp_UpdateStatus", parameters);
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