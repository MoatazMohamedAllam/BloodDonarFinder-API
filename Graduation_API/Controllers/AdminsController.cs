using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Graduation_API.Models;

namespace Graduation_API.Controllers
{
    
    public class AdminsController : ApiController
    {
        private BloodDonor_APIEntities1 _context;
        public AdminsController()
        {
            _context = new BloodDonor_APIEntities1();
        }


       
        [Route("api/admins/GetAllUsers")]
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            //var AcountsIDs = _context.BDF_UserAccount.Where(e => e.RoleID == 2).Select(e=>e.AccountID).ToList();
            //var list_Of_Profiles = new List<BDF_UserProfile>();

            var profiles = _context.BDF_UserProfile.ToList();

            //foreach (var item in AcountsIDs)
            //{
            //        var profile = _context.BDF_UserProfile.Where(e => e.User_Id == item).FirstOrDefault();
            //        list_Of_Profiles.Add(profile);
            //}

            var result = from p in profiles
                         join i in _context.UserProfile_Images
                         on p.UserProfileID equals i.ProfileID
                         select new { p.FirstName, p.LastName, p.MobileNo, p.BloodGroup, p.UserProfileID,p.city_name,p.govern_name, i.ImageData };

            //var result = from p in list_Of_Profiles
            //             join i in _context.UserProfile_Images
            //             on p.UserProfileID equals i.ProfileID
            //             select new { p.FirstName, p.LastName, p.MobileNo, p.BloodGroup, p.UserProfileID, p.city_name, p.govern_name, i.ImageData };

            var finalResult = result.Select(e => new { e.UserProfileID,e.FirstName, e.LastName, e.BloodGroup, e.MobileNo, e.city_name, e.govern_name,e.ImageData}).ToList();
            if (finalResult.Count == 0)
            { 
                return Json(new { ErrorID = 1, ErrorMessage = "There is no users" });
            }
            else
            {
                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", Result = finalResult });
            }
        }

        [Route("api/admins/DeleteUserByID")]
        [HttpDelete]
        public IHttpActionResult DeleteUserByID(int userID)
        {
            var IsAccountExisting =_context.BDF_UserAccount.Where(e => e.AccountID == userID).FirstOrDefault();
            //var IsProfileExisting =_context.BDF_UserProfile.Where(e => e.UserProfileID == userID).FirstOrDefault();

            if (IsAccountExisting == null )
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var existingUserAccount = _context.BDF_UserAccount.Where(e => e.AccountID == userID).FirstOrDefault();
                var existingUserProfile = _context.BDF_UserProfile.Where(e => e.UserProfileID == userID).FirstOrDefault();

                var requests = _context.BDF_BloodRequest.Where(e => e.SenderAccount_id == userID || e.ReceiverAccount_id == userID).ToList();
                var questions = _context.Questions.Where(e => e.UserID == userID).ToList(); ;
                var ProfileImg = _context.UserProfile_Images.Where(e => e.ProfileID == userID).FirstOrDefault();
                var MedicalReportImgs = _context.MedicalReport_Images.Where(e => e.ProfileID == userID).ToList();

                _context.BDF_UserAccount.Remove(existingUserAccount);

                if (requests.Count != 0)
                    _context.BDF_BloodRequest.RemoveRange(requests);

                if (questions.Count != 0)
                    _context.Questions.RemoveRange(questions);

                if (ProfileImg != null)
                    _context.UserProfile_Images.Remove(ProfileImg);

                if (MedicalReportImgs.Count != 0)
                    _context.MedicalReport_Images.RemoveRange(MedicalReportImgs);

                if (existingUserProfile != null)
                    _context.BDF_UserProfile.Remove(existingUserProfile);
               
                _context.SaveChanges();

                return Json(new { ErrorID=2,ErrorMessage="Deleted Successfully"});
            }
        }





        [Route("api/admins/GetAllQuestionByAdmin")]
        [HttpGet]
        public IHttpActionResult GetAllQuestionByAdmin(string receiverRole)
        {
            var result = _context.Questions.Where(e => e.receiverRole == receiverRole && e.Answer == null).Select(e => new { e.QuestionID, e.User_Question }).ToList();

            if (receiverRole == null || receiverRole != "admin")
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else if (result.Count == 0)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "there is no questions or you are replay for it!!" });
            }
            else
            {
                return Json(new { ErrorID = 2, ErrorMessage = "Successfully", result });
            }
        }


        [Route("api/admins/ReplayForQuestion")]
        [HttpPut]
        public IHttpActionResult ReplayForQuestion(int questionID, string answer)
        {
            var IsUerAsked = _context.Questions.Where(e => e.QuestionID == questionID).FirstOrDefault();

            if (IsUerAsked == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var question = _context.Questions.Where(e => e.UserID == questionID).FirstOrDefault();
                question.Answer = answer;
                _context.SaveChanges();
            }
            return Json(new { ErrorID = 2, ErrorMessage = "Your Replay Sent Successfully" });
        }

    }
}
