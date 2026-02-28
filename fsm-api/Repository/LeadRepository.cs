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
    }
}