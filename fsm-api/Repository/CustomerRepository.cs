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

    public class CustomerRepository
    {
        private readonly DapperDataService _dataService;

        public CustomerRepository()
        {
            _dataService = new DapperDataService();
        }
        public async Task<CustomerSearchResponse> SearchCustomer(string mobileNo)
        {
            var param = new DynamicParameters();
            param.Add("@MobileNo", mobileNo);

            var result = await _dataService.GetAllAsync<dynamic>(
                "Sp_SearchCustomerByMobile",
                param
            );

            var list = result.ToList();

            if (!list.Any())
            {
                return new CustomerSearchResponse
                {
                    IsNewCustomer = true
                };
            }

            var first = list.First();

            var response = new CustomerSearchResponse
            {
                IsNewCustomer = false,
                CustomerId = first.CustomerId,
                CustomerName = first.CustomerName,
                MobileNo = first.MobileNo,
                WhatsappNo = first.WhatsappNo,
                EmailId = first.EmailId,
                Addresses = list
                    .Where(x => x.CustomerAddressId != null)
                    .Select(x => new CustomerAddressModel
                    {
                        CustomerAddressId = x.CustomerAddressId,
                        AddressType = x.AddressType,
                        AddressLine1 = x.AddressLine1,
                        Area = x.Area,
                        City = x.City,
                        State = x.State,
                        Pincode = x.Pincode,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude,
                        IsDefault = x.IsDefault
                    })
                    .ToList()
            };

            return response;
        }
    }
}