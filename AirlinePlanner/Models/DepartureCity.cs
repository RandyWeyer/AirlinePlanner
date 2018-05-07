using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AirlinePlanner.Models
{
    public class DepartureCity
    {
        private string _departureCityType;
        private int _id;

        public DepartureCity(string DepartureCityType, int Id = 0)
        {
            _departureCityType = DepartureCityType;
            _id = Id;
        }

        public int GetId()
        {
          return _id;
        }

        public string GetDepartureCityType()
        {
          return _departureCityType;
        }

        public static List<DepartureCity> GetAll()
        {
            List<DepartureCity> allDepartureCity = new List<DepartureCity> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departure_cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int departureCityId = rdr.GetInt32(0);
              string departureCityType = rdr.GetString(1);
              DepartureCity newDepartureCity = new DepartureCity(departureCityType, departureCityId);
              allDepartureCity.Add(newDepartureCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allDepartureCity;
        }

        public static DepartureCity Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM departure_cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int departureCityId = 0;
            string departureCityType = "";

            while(rdr.Read())
            {
              departureCityId = rdr.GetInt32(0);
              departureCityType = rdr.GetString(1);
            }
            DepartureCity newDepartureCity = new DepartureCity(departureCityType, departureCityId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newDepartureCity;
        }
    }
}
