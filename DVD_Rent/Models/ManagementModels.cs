using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVD_Rent.Models
{
    public class ManagementModels
    {
        public class Customer
        {
            public int id { get; set; }
            public string img_url { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string address { get; set; }
            public string email { get; set; }
            public string onrent { get; set; }
            public string endedrent { get; set; }
            public bool active { get; set; }
        }
        public class Customer<T>
        {
            public List<T> t_cust { get; set; }
        }
        public class Admin
        {
            public int id { get; set; }
            public int user_id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
            public string store { get; set; }
            public string position { get; set; }
            public bool active { get; set; }
            public string createdDate { get; set; }
            public string updatedDate { get; set; }
            public string pictureURL { get; set; }
            public string email { get; set; }
        }
        public class Admin<T>
        {
            public List<T> t_admin { get; set; }

        }
        public class AddAdmin
        {
            public string store { get; set; }
            public string storeID { get; set; }
            public string position { get; set; }
            public string positionID { get; set; }
        }
        public class AddAdmin<T>
        {
            public List<T> listDDLStore { get; set; }
            public List<T> listDDLPosition { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string address { get; set; }
            public string address2 { get; set; }
            public string gender { get; set; }
            public string picture_URL { get; set; }
            public string password { get; set; }
            public string confirmpassword { get; set; }
            public string store { get; set; }
            public string position { get; set; }
        }

        public class Movie
        {
            public int id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string ReleaseYear { get; set; }
            public string Duration { get; set; }
            public string AgeRating { get; set; }
            public string UpdatedDate { get; set; }
            public string PictureURL { get; set; }
            public string TrailerURL { get; set; }
        }
        public class Movie<T>
        {
            public List<T> t_movie { get; set; }

        }

        public class AddModifyMovie
        {
            public string AgeRating { get; set; }
            public string AgeRatingID { get; set; }
            public string Genre { get; set; }
            public string GenreID { get; set; }
        }

        public class AddModifyMovie<T>
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ReleaseYear { get; set; }
            public string Duration { get; set; }
            public string[] Genre { get; set; }
            public string[] TambahGenre { get; set; }
            public string[] KurangGenre { get; set; }
            public string AgeRating { get; set; }
            public List<T> listDDLAgeRating { get; set; }
            public List<T> listGenre { get; set; }
            public string PictureURL { get; set; }
            public string TrailerURL { get; set; }
        }


        public class AddModifyInventory
        {
            public string Movie { get; set; }
            public string MovieID { get; set; }
            public string Store { get; set; }
            public string StoreID { get; set; }
            public string StatusMovie { get; set; }
            public string StatusMovieID { get; set; }
        }

        public class AddModifyInventory<T>
        {
            public List<T> listDDLMovie { get; set; }
            public List<T> listDDLStore { get; set; }
            public int stock { get; set; }
            public int onrent { get; set; }
            public int available { get; set; }
            public List<T> listDDLStatus { get; set; }
        }
    }
}