using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fsm_api.Models
{

    public class RefreshTokens
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string DeviceId { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }

    }
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
    public class Mst_Settings
    {
        public int SettingsId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class Mst_Users
    {
        public int UserId { get; set; }
        public Int64 MobileNumber { get; set; }
        public int CityId { get; set; }
    }
    public class Mst_City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityIcon { get; set; }
    }
}