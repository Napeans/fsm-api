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
    public class LeadRepository
    {
        private readonly DapperDataService _dataService;

        public LeadRepository()
        {
            _dataService = new DapperDataService();
        }

        public async Task<List<LeadModel>> GetLeads()
        {
            return (await _dataService.GetAllAsync<LeadModel>("Sp_GetLeads")).ToList();
        }

        public async Task DeleteLead(int leadId)
        {
            var param = new DynamicParameters();
            param.Add("@LeadId", leadId);
            await _dataService.ExecuteAsync("Sp_DeleteLead", param);
        }

        public async Task ConvertToJob(int leadId)
        {
            var param = new DynamicParameters();
            param.Add("@LeadId", leadId);
            await _dataService.ExecuteAsync("Sp_ConvertLeadToJob", param);
        }

        public async Task<int> CreateLead(LeadCreateRequest request)
        {
            try
            {
                var param = new DynamicParameters();

                // Core Lead Details
                param.Add("@ServiceTypeId", request.ServiceTypeId);
                param.Add("@ScheduledOn", request.ScheduledOn);
                param.Add("@Remarks", request.Remarks);
                param.Add("@CreatedBy", CommonMentods.UserId);

                // Customer Details (Scenarios 1 & 3)
                param.Add("@CustomerId", request.CustomerId);
                param.Add("@CustomerName", request.CustomerName);
                param.Add("@MobileNo", request.MobileNo);
                param.Add("@WhatsappNo", request.WhatsappNo);
                param.Add("@EmailId", request.EmailId);

                // Address Details (Scenarios 1 & 3)
                param.Add("@CustomerAddressId", request.CustomerAddressId ?? 0);
                param.Add("@AddressType", request.Addresse?.AddressType);
                param.Add("@AddressLine1", request.Addresse?.AddressLine1);
                param.Add("@Area", request.Addresse?.Area);
                param.Add("@City", request.Addresse?.City);
                param.Add("@State", request.Addresse?.State);
                param.Add("@Pincode", request.Addresse?.Pincode);
                param.Add("@Latitude", request.Addresse?.Latitude);
                param.Add("@Longitude", request.Addresse?.Longitude);
                param.Add("@GoogleMapLink", request.Addresse?.GoogleMapLink);
                param.Add("@CustomerGST", request.CustomerGST);

                // Execute and return the new LeadId
                return await _dataService.ExecuteScalarAsync<int>("sp_CreateLead", param);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}