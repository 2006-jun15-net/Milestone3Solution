using System;
using System.Collections.Generic;
using Lodging.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Linq;
namespace Lodging.IntegrationTests
{
    public static class StaticTestingData
    {
        //Arbitrary number used to limit num
        //of loops during url creation
        private const int IdIterations = StartingId + 1;

        private const int StartingId = 1;

        // Another arbitrary number for limiting
        // length of string generation
        private const int RandomStringLength = 5;

        // Root to be prepended to each url request
        public static string root = "api/";

        //Routes for the app
        public static List<string> Routes = new List<string>()
    {
      "Review",
      "Lodging",
      "Rental"
    };
        /*
         * <summary>
         * returns List<string baseurl+route>
         *         ex: "api/v0.0/" + "review"
         *              baseurl        route
         *</summary> 
         */
        public static List<object[]> BaseUrls()
        {
            var r = new List<object[]>();
            for (int i = 0; i < Routes.Count; i++)
            {
                r.Add(new object[] { root + Routes[i] });
            }
            return r;
        }
        /*
         * <summary>
         * returns List<string baseurl+(xNum of gibberish strings)>
         *         ex: "api/v0.0/" + nonsense
         *              baseurl        route
         *</summary> 
         */
        public static List<object[]> Get404Requests()
        {
            var r = new List<object[]>();
            for (int i = 0; i < IdIterations; i++)
            {
                r.Add(new object[] { root + Utils.GenerateString(RandomStringLength) });
            }
            return r;
        }
        /*
         * <summary>
         * returns List< ex: ["api/v0.0/" + /route/ + id, "api/v0.0/" + route]>
         * Gets a valid object from url and tries to post it as is to the server
         * </summary>
         */
        public static List<object[]> Get409Requests()
        {
            var r = new List<object[]>();

            foreach (var s in Routes)
            {
                for (int i = StartingId; i < IdIterations; i++)
                {
                    r.Add(new object[] { root + s + '/' + i.ToString(), root + s });
                }
            }
            return r;
        }

        public static List<object[]> PutRequests()
        {
            var update = new List<object[]>();
            for (int i = 1; i < IdIterations; i++)
            {
                update.Add(new object[]{ "api/Rental/" + i,  JObject.FromObject(new RentalModel() {
            Status = "available",
            Name = "Baguio House",
            Occupancy = 6,
            Price = 100.00,
            Type = "home"
        })
                });
                update.Add(new object[]{"api/Review/" + i, JObject.FromObject(new ReviewModel(){
          Comment = "this place sucked",
          DateCreated = DateTime.Now,
          Rating = 3,

         })
                });
                update.Add(new object[]{"api/Lodging/" + i, JObject.FromObject(new LodgingModel(){
                  Name = "Updated Lodging name",
          Location = new LocationModel()
          {
            Address = new AddressModel()
            {
              City = "Auburn",
              Country = "United States",
              PostalCode = "13021",
              StateProvince = "NY",
              Street = "30500 Co Rd 383",
            },
            Latitude = "0.00 N",
            Longitude = "123.123 E",
            Locale = "en"
          },
          Rentals = new List<RentalModel>()
          {
            new RentalModel() {
              Name = "Rental Unit 1",
              Occupancy = 1,
              Type = "Cabin",
              Status = "Booked",
              Price = 100.0,
            }
          },
          Reviews = new List<ReviewModel>() {
            new ReviewModel() {
              Comment = "This lodge is fantastic!",
              DateCreated = DateTime.Now,
              Rating = 9,
            }
          }
         })
         });
            }

            return update;
        }


        /*
        * <summary>
        * Returns a list of urls to each route with an id x IdIterations times
        *\/base/route/id
        * </summary>
        */
        public static List<object[]> GetRequests()
        {
            var _ = new List<object[]>();
            _.AddRange(BaseUrls());
            var range = _.Count;
            for (int z = 0; z < range; z++)
            {
                for (int i = 1; i < IdIterations; i++)
                {
                    _.Add(new object[] { _[z][0] + "/" + i });
                }
            }
            return _;
        }
        /*
        * <summary>
        * Returns a list of urls to each route with an id x IdIterations times
        *\/base/route/id
        * </summary>
        */
        public static List<object[]> DeleteRequests()
        {
            var _ = new List<object[]>();
            foreach (var s in BaseUrls())
            {
                for (int i = StartingId; i < IdIterations; i++)
                {
                    _.Add(new object[] { s[0] + "/" + i });
                }
            }
            return _;
        }
        /*
        * <summary>
        * Returns a list of valid post url and damaged json string as payload
        * </summary>
        */
        public static List<object[]> Post422Requests()
        {
            var _ = new List<object[]>();
            _.AddRange(PostRequests());
            foreach (var z in _)
            {
                z[1] = z[1].ToString() + Utils.GenerateString(RandomStringLength);
            }
            return _;
        }
        /*
        * <summary>
        * Returns a list of [valid post urls, post content]
        * </summary>
        */
        public static List<object[]> PostRequests()
        {
            return new List<object[]>
       {
        new object[] { "/api/Review", JObject.FromObject(new ReviewModel(){
            Comment = "I love it here",
            Rating = 10,
            DateCreated = DateTime.Now,
        }) },
        new object[] { "/api/Rental", JObject.FromObject(new RentalModel() {
            Status = "available",
            Name = "Baguio House",
            Occupancy = 6,
            Price = 100.00,
            Type = "home"
        }) },
        new object[] { "/api/Lodging", JObject.FromObject(new LodgingModel()
        {
          Name = "Quiet Forest Lodge",
          Location = new LocationModel()
          {
            Address = new AddressModel()
            {
              City = "Buena Vista",
              Country = "United States",
              PostalCode = "81211",
              StateProvince = "CO",
              Street = "30500 Co Rd 383",
            },
            Latitude = "0.00 N",
            Longitude = "123.123 E",
            Locale = "en"
          },
          Rentals = new List<RentalModel>()
          {
            new RentalModel() {
              Name = "Rental Unit 1",
              Occupancy = 1,
              Type = "Cabin",
              Status = "Booked",
              Price = 0.0,
            }
          },
          Reviews = new List<ReviewModel>() {
            new ReviewModel() {
              Comment = "This lodge is fantastic!",
              DateCreated = DateTime.Now,
              Rating = 9,
            }
          }
        }) }
      };
        }
    }
    /*
    * <summary>
    * Returns a list of [valid put urls, put content]
    * </summary>
    */
    public static List<object[]> PutRequests()
    {
      var update = new List<object[]>();
      for (int i = 1; i < IdIterations; i++)
      {
        update.Add(new object[]{ "api/Rental/" + i,  JObject.FromObject(new RentalModel() {
            Status = "available",
            Name = "Baguio House",
            Occupancy = 6,
            Price = 100.00,
            Type = "home"
        })
                });
        update.Add(new object[]{"api/Review/" + i, JObject.FromObject(new ReviewModel(){
          Comment = "this place sucked",
          DateCreated = DateTime.Now,
          Rating = 3,

         })
                });
        update.Add(new object[]{"api/Lodging/" + i, JObject.FromObject(new LodgingModel(){
                  Name = "Updated Lodging name",
          Location = new LocationModel()
          {
            Address = new AddressModel()
            {
              City = "Auburn",
              Country = "United States",
              PostalCode = "13021",
              StateProvince = "NY",
              Street = "30500 Co Rd 383",
            },
            Latitude = "0.00 N",
            Longitude = "123.123 E",
            Locale = "en"
          },
          Rentals = new List<RentalModel>()
          {
            new RentalModel() {
              Name = "Rental Unit 1",
              Occupancy = 1,
              Type = "Cabin",
              Status = "Booked",
              Price = 100.0,
            }
          },
          Reviews = new List<ReviewModel>() {
            new ReviewModel() {
              Comment = "This lodge is fantastic!",
              DateCreated = DateTime.Now,
              Rating = 9,
            }
          }
         })
         });
      }

      return update;
    }
  }
}

