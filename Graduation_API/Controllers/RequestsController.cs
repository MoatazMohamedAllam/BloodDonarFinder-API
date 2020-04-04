using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Graduation_API.Models;
using Newtonsoft.Json;

namespace Graduation_API.Controllers
{
    public class RequestsController : ApiController
    {

        public DateTime date1;
        public DateTime date2;
        public TimeSpan dateTime;

        private BloodDonor_APIEntities1 _context;

        public RequestsController()
        {
            _context = new BloodDonor_APIEntities1();
        }

        [Route("api/requests/GetAllRecivedRequestsByID")]
        [HttpGet]
        public IHttpActionResult GetAllRecivedRequestsByID(int id)
        {
            var IsIDExisting = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == id).FirstOrDefault();

            var listOfSenders = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == id).Select(e=>e.SenderAccount_id).ToList();




            if (IsIDExisting == null || listOfSenders.Count == 0)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }

            else
            {

                var list_Of_Profiles = new List<BDF_UserProfile>();
                var list_Of_Images = new List<UserProfile_Images>();

                foreach (var item in listOfSenders)
                {
                    var img = _context.UserProfile_Images.Where(e => e.ProfileID == item).FirstOrDefault();
                    list_Of_Images.Add(img);
                    var profile = _context.BDF_UserProfile.Where(e => e.UserProfileID == item).FirstOrDefault();
                    list_Of_Profiles.Add(profile);
                }


                var requests = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == id && e.Status != "Accepted").ToList();

                if (requests.Count == 0)
                    return Json(new { ErrorID = 1, ErrorMessage = "there is no requests(requests accepted!!)" });

                foreach (var item in requests)
                {
                    date1 = item.AppDate.Value;
                    date2 = DateTime.Today;
                    dateTime = date1.Subtract(date2);
                    item.No_Of_Days = (int?)dateTime.TotalDays;
                }

                    var result = (from a in list_Of_Profiles
                                  join b in requests on a.UserProfileID equals b.SenderAccount_id
                                  join i in list_Of_Images on a.UserProfileID equals i.ProfileID
                                  select new { a.FirstName, a.LastName,a.UserProfileID, a.MobileNo, a.BloodGroup, i.ImageData, b.RequestID, b.Status, b.AppDate, b.No_Of_Days }).ToList();


                    //var requestsCount = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == id).ToList().Count;

                    return Json(new { ErrorID = 2, ErrorMessage = "Successfully", Requests = result }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
            }

        

        [Route("api/requests/GetAcceptedRequestsForReceiver")]
        [HttpGet]
        public IHttpActionResult GetAcceptedRequestsForReceiver(int receiverID)
        {
            var IsRecieved = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == receiverID).FirstOrDefault();
            var list_Of_Senders = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == receiverID).Select(e => e.SenderAccount_id).ToList();

            if (IsRecieved == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error OCcurs" });
            }
            else
            {
                var profiles = new List<BDF_UserProfile>();

                foreach (var item in list_Of_Senders)
                {
                    var profile = _context.BDF_UserProfile.Where(e => e.UserProfileID == item).FirstOrDefault();
                    profiles.Add(profile);
                }

                var accepted_Requests = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == receiverID && e.Status == "Accepted").ToList();

                //var appDate = requests.Select(e => e.AppDate).FirstOrDefault();

                foreach (var item in accepted_Requests)
                {
                    date1 = item.AppDate.Value;
                    date2 = DateTime.Today;
                    dateTime = date1.Subtract(date2);
                    item.No_Of_Days = (int?)dateTime.TotalDays;
                }

                var result = from p in profiles
                             join i in _context.UserProfile_Images
                             on p.UserProfileID equals i.ProfileID
                             select new { p.FirstName, p.LastName, p.MobileNo, p.BloodGroup, p.UserProfileID, i.ImageData };

                var total = (from a in result
                              join r in accepted_Requests
                              on a.UserProfileID equals r.SenderAccount_id
                              select new { a.FirstName, a.LastName, a.ImageData , a.MobileNo, a.BloodGroup , a.UserProfileID ,r.RequestID, r.Status, r.AppDate , r.No_Of_Days }).ToList();

                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", Requests = total }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }


        [Route("api/requests/GetAcceptedRequestsForSender")]
        [HttpGet]
        public IHttpActionResult GetAcceptedRequestsForSender(int id)
        {
            var IsExisting = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == id).FirstOrDefault();

            var list_of_recievers = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == id && e.Status == "Accepted").Select(e => e.ReceiverAccount_id).ToList();

            if (IsExisting == null || list_of_recievers.Count == 0)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }

            else
            {

                var accepted_Requests = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == id && e.Status == "Accepted").ToList();

                foreach (var item in accepted_Requests)
                {
                    date1 = item.AppDate.Value;
                    date2 = DateTime.Today;
                    dateTime = date1.Subtract(date2);
                    item.No_Of_Days = (int?)dateTime.TotalDays;
                    
                   
                }

               // var accepted_Requests = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == id && e.Status == "Accepted").ToList();

                


                var profiles = new List<BDF_UserProfile>();

                foreach (var item in list_of_recievers)
                {
                    var profile = _context.BDF_UserProfile.Where(e => e.UserProfileID == item).FirstOrDefault();
                    profiles.Add(profile);
                }


                var result = from p in profiles
                             join i in _context.UserProfile_Images
                             on p.UserProfileID equals i.ProfileID
                             select new { p.FirstName, p.LastName, p.MobileNo, p.BloodGroup, p.UserProfileID, i.ImageData };

                var total = (from a in result
                             join b in accepted_Requests
                             on a.UserProfileID equals b.ReceiverAccount_id
                             select new { a.FirstName, a.LastName,a.ImageData, a.MobileNo, a.UserProfileID,b.RequestID,b.AppDate, b.No_Of_Days }).ToList();


                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", AcceptedRequests = result });
            }

        }



        [Route("api/requests/GetRequestsCountByID")]
        [HttpGet]
        public IHttpActionResult GetRequestsCountByID(int id)
        {
            var IsExisting = _context.BDF_BloodRequest.Where(e=>e.ReceiverAccount_id == id).FirstOrDefault();

            if (IsExisting == null)
            {
                return Json(new { ErrorID = 1 , ErrorMessage = "Error Occurs"});
            }
            else
            {
                var RequestsCount = _context.BDF_BloodRequest.Where(e=>e.ReceiverAccount_id == id && e.Status !="Accepted").ToList().Count;

                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", RequestsCount });
            }

        }






        [Route("api/requests/CreateRequest")]
        [HttpPost]
        public IHttpActionResult CreateRequest(BDF_BloodRequest request)
        {

            var IsSenderExsisting = _context.BDF_UserProfile.Where(e => e.UserProfileID == request.SenderAccount_id ).FirstOrDefault();

            var IsReceiverExisting = _context.BDF_UserProfile.Where(e => e.UserProfileID == request.ReceiverAccount_id).FirstOrDefault();


            if (request == null || IsSenderExsisting == null || IsReceiverExisting == null)
            {
                return Json(new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs" }, new JsonSerializerSettings
                                                                                                                        {
                                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                                        });
            }
           
            else
	        {

                var newRequst = new BDF_BloodRequest()
                {
                    SenderAccount_id = request.SenderAccount_id,
                    ReceiverAccount_id = request.ReceiverAccount_id,
                    Status="pending",
                    AppDate = request.AppDate,
                    ReqDate = DateTime.Today,
                    IsOperationDone=false
                };

                _context.BDF_BloodRequest.Add(newRequst);
                _context.SaveChanges();

                return Json(new ResponseMessage() { ErrorID = 2, ErrorMessage = "Sent Successfully" }, new JsonSerializerSettings
                                                                                                                        {
                                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                                        });
            }
            

        }


        [Route("api/requests/CkeckIsOperationCompleted")]
        [HttpPut]
        public IHttpActionResult CkeckIsOperationCompleted(int requestID)
        {
            var IsExisting = _context.BDF_BloodRequest.Where(e => e.RequestID == requestID).FirstOrDefault();

            if (IsExisting == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var request = _context.BDF_BloodRequest.Where(e => e.RequestID == requestID).FirstOrDefault();

                request.IsOperationDone = true;
                _context.SaveChanges();

                return Json(new { ErrorID = 2, ErrorMessage = "Operation Done " });
            }
        }


        [Route("api/requests/MakeRequestAccepted")]
        [HttpPut]
        public IHttpActionResult MakeRequestAccepted(int requestID)
        {
            var IsExisting = _context.BDF_BloodRequest.Where(e=>e.RequestID==requestID).FirstOrDefault();

            if (IsExisting == null)
            {
                return Json(new ResponseMessage() { ErrorID=1,ErrorMessage="Error Occurs"});
            }
            else
            {
                var existingRequest = _context.BDF_BloodRequest.Where(e=>e.RequestID==requestID).FirstOrDefault();
                    
                existingRequest.Status = "Accepted";
                _context.SaveChanges();

                return Json(new ResponseMessage() { ErrorID = 2, ErrorMessage = "Successfully" }, new JsonSerializerSettings
                                                                                                        {
                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                        });
            }
        }


        [Route("api/requests/MakeRequestReject")]
        [HttpDelete]
        public IHttpActionResult MakeRequestReject(int requestID)
        {
            var IsExisting = _context.BDF_BloodRequest.Where(e => e.RequestID == requestID).FirstOrDefault();

            if (IsExisting == null)
            {
                return Json(new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var existingRequest = _context.BDF_BloodRequest.Where(e => e.RequestID == requestID).FirstOrDefault();
                _context.BDF_BloodRequest.Remove(existingRequest);
                _context.SaveChanges();

                return Json(new ResponseMessage() { ErrorID = 2, ErrorMessage = "Rejected Successfully" },new JsonSerializerSettings
                                                                                                            {
                                                                                                                DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                            });
            }
        }



    }
}
