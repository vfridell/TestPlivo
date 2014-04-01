using System;
using System.Collections.Generic;
using System.Reflection;
using RestSharp;
using Plivo.API;

namespace TestPlivo
{
    class Program
    {
        static void Main(string[] args)
        {
            string auth_id = "MAYZI0ZDG2ZWFJZDVIYZ";  // obtained from Plivo account dashboard
            string auth_token = "MDVhOWZiMWMzNDUwZGI3YzE1ZDNkNGIyNmQ5MmVl";  // obtained from Plivo account dashboard

            // Creating the Plivo Client
            RestAPI plivo = new RestAPI(auth_id, auth_token);

            // Making a Call
            string from_number = "15025551234";
            string to_number = "15022967010";


            IRestResponse<Call> response = plivo.make_call(new Dictionary<string, string>() {
                { "from", from_number },
                { "to", to_number }, 
                { "answer_url", "http://www.bellarmine.edu/" }, 
                { "answer_method", "GET" },
                { "caller_name", "PhoneGame" }
            });

            // The "Outbound call" API response has four properties -
            // message, request_uuid, error, and api_id.
            // error - contains the error response sent back from the server.
            if (response.Data != null)
            {
                PropertyInfo[] properties = response.Data.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                    Console.WriteLine("{0}: {1}", property.Name, property.GetValue(response.Data, null));

                IRestResponse<Record> recordResponse = plivo.record(new Dictionary<string, string>() 
                { 
                    {"call_uuid", response.Data.request_uuid}
                });
                Console.Write("Recording URL: {0}", recordResponse.Data.url);
            }
            else
            {
                // ErrorMessage - contains error related to network failure.
                Console.WriteLine(response.ErrorMessage);
            }
            Console.Read();
        }
    }
}
