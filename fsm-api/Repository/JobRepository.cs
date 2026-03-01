using Dapper;
using fsm_api.Common;
using fsm_api.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
            string query = @"Sp_GetQuotationItems";


            var list = await _dataService.GetAllAsync<QuotationItem>(query, parameters);

            return list.ToList();
        }


        public async Task<List<JobMediaResponseModel>> GetJobMedia(int JobId)
        {

            var result= await getJobMediaData(JobId);
            var mediaList = new List<JobMediaResponseModel>();

            foreach (var item in result)
            {
                byte[] imageBytes = item.MediaData;

                string base64String = Convert.ToBase64String(imageBytes);

                mediaList.Add(new JobMediaResponseModel
                {
                    MediaId = item.MediaId,
                    Base64Image = base64String,
                    Flag=item.Flag
                });
            }

            return mediaList;
        }
        public async Task<List<JobMediaModel>> getJobMediaData(int JobId) {

            var parameters = new DynamicParameters();
            parameters.Add("@JobId", JobId);
            string query = @"SELECT MediaId, MediaData,Flag FROM JobMedia WHERE JobId = @JobId";


            var list = await _dataService.GetAllAsync<JobMediaModel>(query, parameters);

            var result = list.ToList();

            return result;
        }

        public async Task<List<JobMediaModel>> getClientDetails(int JobId)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@JobId", JobId);
            string query = @"SELECT MediaId, MediaData,Flag FROM JobMedia WHERE JobId = @JobId";


            var list = await _dataService.GetAllAsync<JobMediaModel>(query, parameters);

            var result = list.ToList();

            return result;
        }


        public async Task<List<ClientDetails>> GetClientDetails()
        {

            var parameters = new DynamicParameters();
            string query = @"select top 1 * from ClientDetails";


            var list = await _dataService.GetAllAsync<ClientDetails>(query, parameters);

            var result = list.ToList();

            return result;
        }
        public async Task<int> CreateQuotation(CreateQuotation createQuotation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", createQuotation.JobId);
            parameters.Add("@DiscountValue", createQuotation.DiscountValue);
            parameters.Add("@CreatedBy", CommonMentods.UserId);
            parameters.Add("@Status", createQuotation.Status);
            parameters.Add("@Items", createQuotation.Items);


            return await _dataService.ExecuteAsync("Sp_CreateQuotation", parameters);
        }
        public async Task<int> DeleteJobMedia(int MediaId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MediaId", MediaId);

            return await _dataService.ExecuteAsync("delete from [JobMedia] where MediaId=@MediaId", parameters);
        }
        public async Task<int> SaveJobJobMedia(JobMediaModel jobMediaModel)
        {
            var table = new DataTable();
            table.Columns.Add("JobId", typeof(int));
            table.Columns.Add("MediaData", typeof(byte[]));
            table.Columns.Add("UploadedBy", typeof(int));

            foreach (var base64Image in jobMediaModel.MediaDatas)
            {
                if (string.IsNullOrEmpty(base64Image))
                    continue;

                var cleanBase64 = base64Image.Contains(",")
                    ? base64Image.Substring(base64Image.IndexOf(",") + 1)
                    : base64Image;

                byte[] imageBytes = Convert.FromBase64String(cleanBase64);

                table.Rows.Add(jobMediaModel.JobId, imageBytes, CommonMentods.UserId);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@JobMediaList", table.AsTableValuedParameter("JobMediaType"));
            parameters.Add("@Flag", jobMediaModel.Flag);
            parameters.Add("@Comments", jobMediaModel.Comments??"");
            parameters.Add("@SignedBy", jobMediaModel.SignedBy??"");
            parameters.Add("@Latitude", jobMediaModel.Latitude);
            parameters.Add("@Longitude", jobMediaModel.Longitude);
            parameters.Add("@Discount", jobMediaModel.Discount);
            parameters.Add("@QuotationId", jobMediaModel.QuotationId);
            parameters.Add("@UserId", CommonMentods.UserId);
            return await _dataService.ExecuteAsync(
                "InsertJobMediaBulk",
                parameters);
        }
        public async Task<int> AddOrRemoveQuotationItems(QuotationItemsModel quotationItemsModel)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@QuotationId", quotationItemsModel.QuotationId);
            parameters.Add("@QuotationItemId", quotationItemsModel.QuotationItemId);
            parameters.Add("@ItemId", quotationItemsModel.ItemId);
            parameters.Add("@Quantity", quotationItemsModel.Quantity);
            parameters.Add("@Flag", quotationItemsModel.Flag);
            parameters.Add("@UserID", CommonMentods.UserId);

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


        public async Task<(EstimateModel, List<EstimateItem>)> GetInvoiceData(int JobId,bool IsEstimate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@JobId", JobId);
            parameters.Add("@IsEstimate", IsEstimate);
            using (var multi = await _dataService.QueryMultipleAsync(
                "Sp_GetInvoiceData",
                parameters,
                CommandType.StoredProcedure))
            {
                var estimate = (await multi.ReadAsync<EstimateModel>()).FirstOrDefault();
                var item = (await multi.ReadAsync<EstimateItem>()).AsList();
                return (estimate, item);
            }
        }
    }
}