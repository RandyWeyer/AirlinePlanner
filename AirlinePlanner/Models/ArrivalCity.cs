using System.Collections.Generic;
using MySql.Data.MySqlClient;
using AirlinePlanner;
using System;
using Microsoft.AspNetCore.Mvc;

namespace AirlinePlanner.Models
{
    public class ArrivalCity
    {
        private string _arrivalCityName;
        private int _id;

        public ArrivalCity(string arrivalCityName, int id=0)
        {
            _arrivalCityName = arrivalCityName;
            _id = id;
        }

        public string GetArrivalCityName()
        {
            return _arrivalCityName;
        }
        public void SetArrivalCityName(string ArrivalCityName)
        {
            _arrivalCityName = ArrivalCityName;
        }

        public int GetArrivalCityId()
        {
            return _id;
        }


        public override bool Equals(System.Object otherArrivalCity)
        {
          if (!(otherArrivalCity is ArrivalCity))
          {
            return false;
          }
          else
          {
             ArrivalCity newArrivalCity = (ArrivalCity) otherArrivalCity;
             bool idEquality = this.GetArrivalCityId() == newArrivalCity.GetArrivalCityId();
             bool nameEquality = this.GetArrivalCityName() == newArrivalCity.GetArrivalCityName();
             return (idEquality && nameEquality);
           }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO arrival_cities (arrival_city) VALUES (@arrival_city);";

            MySqlParameter arrivalCity = new MySqlParameter();
            arrivalCity.ParameterName = "@arrival_city";
            arrivalCity.Value = this._arrivalCityName;
            cmd.Parameters.Add(arrivalCity);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<ArrivalCity> GetAll()
        {
            List<ArrivalCity> allArrivalCities = new List<ArrivalCity> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM arrival_cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int id = rdr.GetInt32(0);
              string arrivalCityName = rdr.GetString(1);
              ArrivalCity newArrivalCity = new ArrivalCity(arrivalCityName, id);
              allArrivalCities.Add(newArrivalCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allArrivalCities;
        }

        public static ArrivalCity Find(int arrivalId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM arrival_cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = arrivalId;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int id = 0;
            string arrivalCityName = "";

            while(rdr.Read())
            {
              id = rdr.GetInt32(0);
              arrivalCityName = rdr.GetString(1);
            }
            ArrivalCity newArrivalCity = new ArrivalCity(arrivalCityName, id);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newArrivalCity;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM arrival_cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
