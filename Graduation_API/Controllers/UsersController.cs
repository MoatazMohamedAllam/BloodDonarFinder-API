using Graduation_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Graduation_API.Controllers
{
    public class UsersController : ApiController
    {

        private BloodDonor_APIEntities1 _context;

        public UsersController()
        {
            _context = new BloodDonor_APIEntities1();
        }



        [Route("api/users/register")]
        [HttpPost]
        public IHttpActionResult Register(BDF_UserAccount account)
        {

            var result = _context.BDF_UserAccount.Where(e => e.Username == account.Username).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                if (result != null)
                {
                    return Json(new { ErrorID = 1, ErrorMessage = "this username exists in database" });
                }

                var user = new BDF_UserAccount();

                user.Username = account.Username;
                user.Password = account.Password;
                user.Email = account.Email;
                user.RoleID = 2;
                

                _context.BDF_UserAccount.Add(user);

            }
            _context.SaveChanges();
            return Json(new { ErrorID = 2, ErrorMessage = "Created successfully" });
        }


        [Route("api/users/login")]
        [HttpPost]
        public IHttpActionResult LogIn(BDF_UserAccount account)
        {

            bool isAuthorized = _context.BDF_UserAccount.Any(u => u.Username.Equals(account.Username, StringComparison.OrdinalIgnoreCase) && u.Password == account.Password);
            var user_id = _context.BDF_UserAccount.Where(e => e.Username == account.Username && e.Password == account.Password).Select(e => e.AccountID).FirstOrDefault();

            var FirstName = _context.BDF_UserProfile.Where(e=>e.UserProfileID==user_id).Select(e=>e.FirstName).FirstOrDefault();
            var LastName = _context.BDF_UserProfile.Where(e=>e.UserProfileID==user_id).Select(e=>e.LastName).FirstOrDefault();
            var role = _context.BDF_UserAccount.Where(e=>e.AccountID == user_id).Select(e => e.BDF_UserRole.RoleName).FirstOrDefault();

            var profile_ID = _context.BDF_UserProfile.Where(e => e.UserProfileID == user_id).Select(e=>e.UserProfileID).FirstOrDefault();
            var profileImage = _context.UserProfile_Images.Where(e => e.ProfileID == profile_ID).Select(e => e.ImageData).FirstOrDefault();



            bool IsProfileCompleted = _context.BDF_UserProfile.Any(e => e.User_Id == user_id);



            var requestsCount = _context.BDF_BloodRequest.Where(e => e.ReceiverAccount_id == user_id && e.Status != "Accepted").ToList().Count;

            if (isAuthorized == false)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Invalid username or password" });
            }
            else
            {
                if (role=="user")
                {
                    return Json(new { ErrorID = 2, ErrorMessage = "login successfully", RequestsCount = requestsCount , user_id , FirstName , LastName, role, profileImage , IsProfileCompleted });
                }
                else
                {
                    return Json(new { ErrorID = 2, ErrorMessage = "login successfully", role });
                }
            }
        }




        [Route("api/users/GetUserAccountById")]
        [HttpGet]
        public IHttpActionResult GetUserAccountById(int id)
        {

            var user = _context.BDF_UserAccount.Where(e => e.AccountID == id).Select(
                                                    e => new
                                                    {
                                                        e.Username,
                                                        e.Password,
                                                        e.Email
                                                    });
            if (user == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }

            return Json(user, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }





        [Route("api/users/ChangePassword")]
        [HttpPut]
        public IHttpActionResult ChangePassword(int userID ,string oldPassword,string newPassword)
        {
            var IsExisting = _context.BDF_UserAccount.Where(e => e.AccountID == userID).FirstOrDefault();
            var IsAuthorized = _context.BDF_UserAccount.Where(e => e.Password == oldPassword).FirstOrDefault();

            if (IsExisting ==null || IsAuthorized == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var existingPassword = _context.BDF_UserAccount.Where(e =>e.AccountID == userID && e.Password == oldPassword).FirstOrDefault();

                if (existingPassword != null)
                    existingPassword.Password = newPassword;
                    _context.SaveChanges();

                return Json(new { ErrorID = 2, ErrorMessage = "Password Changed Successfully" });

            }
        }


        [Route("api/users/ChangeEmail")]
        [HttpPut]
        public IHttpActionResult ChangeEmail(int userID, string password, string newEmail)
        {
            var IsExisting = _context.BDF_UserAccount.Where(e => e.AccountID == userID).FirstOrDefault();
            var IsAuthorized = _context.BDF_UserAccount.Where(e => e.Password == password).FirstOrDefault();

            if (IsExisting == null || IsAuthorized == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var existingEmail = _context.BDF_UserAccount.Where(e => e.AccountID == userID && e.Password == password).FirstOrDefault();

                if (existingEmail != null)
                    existingEmail.Email = newEmail;
                _context.SaveChanges();

                return Json(new { ErrorID = 2, ErrorMessage = "email Changed Successfully" });

            }
        }



        [Route("api/users/search")]
        [HttpGet]
        public IHttpActionResult Search(int userID, string bloodGroup, string govern_name, string city_name)
        {

            //if(bloodGroup == null || govern_name == null)
            //{

            //}
            var result = _context.BDF_UserProfile.Where(e => e.govern_name == govern_name && e.city_name == city_name && e.BloodGroup == bloodGroup)
                                                                    .Select(e => new { e.UserProfileID,e.FirstName,e.LastName,e.govern_name,e.city_name, e.BloodGroup, e.MobileNo}).ToList();

            var joining_Result_With_ImageTbl = from a in result
                                               join b in _context.UserProfile_Images
                                               on a.UserProfileID equals b.ProfileID
                                               select new { a.UserProfileID, a.FirstName, a.LastName, a.govern_name,
                                                                a.city_name, a.BloodGroup, a.MobileNo , b.ImageData };

            if (result.Count == 0)
            {
                return Json(new ResponseMessage { ErrorID = 1, ErrorMessage = "no items matched" }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            }


            else
            {

                var myprofile = _context.BDF_UserProfile.Where(e => e.UserProfileID == userID).FirstOrDefault();

                var receiversIDs = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == userID).Select(e => e.ReceiverAccount_id).ToList();
                var list_Of_receiversProfiles = new List<BDF_UserProfile>();

                foreach (var item in receiversIDs)
                {
                    var profile = _context.BDF_UserProfile.Where(e => e.UserProfileID == item).FirstOrDefault();
                    list_Of_receiversProfiles.Add(profile);
                    list_Of_receiversProfiles.Select(e => new { IsSent = 0, e.UserProfileID, e.FirstName, e.LastName, e.govern_name, e.city_name, e.BloodGroup, e.MobileNo }).ToList();
                }

                // prevent search for returning me !!!
                list_Of_receiversProfiles.Add(myprofile);

                

                var final = from a in joining_Result_With_ImageTbl
                          join b in list_Of_receiversProfiles
                          on a.UserProfileID equals b.UserProfileID
                          select new { a.UserProfileID, a.FirstName, a.LastName, a.govern_name, a.city_name, a.BloodGroup, a.MobileNo,a.ImageData};

                //exclude users whose receives requests from this user
                var total = joining_Result_With_ImageTbl.Except(final);

                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", Result = total }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });

            }
        }


        [Route("api/users/AskQuestion")]
        [HttpPost]
        public IHttpActionResult AskQuestion(Question questionObj)
        {
            if (questionObj == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var question = new Question()
                {
                    UserID = questionObj.UserID,
                    User_Question = questionObj.User_Question,
                    receiverRole = questionObj.receiverRole,
                    Answer="pending"
                };

                _context.Questions.Add(question);
                _context.SaveChanges();
            }

            return Json(new { ErrorID = 2, ErrorMessage = "Question Sent Successfully" });
        }

        [Route("api/users/GetAllQuestionsAnswer")]
        [HttpGet]
        public IHttpActionResult GetAllQuestionsAnswer(int userID)
        {
            var ISAsking = _context.Questions.Where(e => e.UserID == userID && e.Answer == "pending").FirstOrDefault();

            if (ISAsking == null)
            {
                return Json(new { ErrorID=1,ErrorMessage="Error Occurs"});
            }
            else
            {
               var result = _context.Questions.Where(e=>e.UserID == userID && e.Answer != "pending").Select(e=> new { e.User_Question,e.Answer,e.receiverRole}).ToList();

                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", result });
            }
        }


        [Route("api/users/UploadMedicalReportImage")]
        [HttpPost]
        public IHttpActionResult UploadMedicalReportImage(MedicalReport_Images img)
        {
            var isExisting = _context.MedicalReport_Images.Where(e => e.ProfileID == img.ProfileID).FirstOrDefault();
            if (isExisting != null)
            {
                var existingImg = _context.MedicalReport_Images.Where(e=>e.ProfileID==img.ProfileID).FirstOrDefault();

                existingImg.ImagePath = img.ImagePath;

                _context.SaveChanges();

                return Json(new { ErrorID = 1, ErrorMessage = "updated Successfully" });
            }
            else
            {
              

                var newImage = new MedicalReport_Images()
                {
                    ProfileID = img.ProfileID,
                    ImagePath = img.ImagePath,
                    MedicalReport_Status = "pending"
                };

                _context.MedicalReport_Images.Add(newImage);
                _context.SaveChanges();

                return Json(new { ErrorID = 2, ErrorMessage = "Added Successfully"});
            }
        }
        

    }
}
