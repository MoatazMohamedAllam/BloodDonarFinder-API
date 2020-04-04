using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Graduation_API.Models;
namespace Graduation_API.Controllers
{
    public class DoctorsController : ApiController
    {

        private BloodDonor_APIEntities1  _context;

        public DoctorsController()
        {
            _context = new BloodDonor_APIEntities1();
        }


        [Route("api/doctors/GetAllQuestionByDoctor")]
        [HttpGet]
        public IHttpActionResult GetAllQuestionByDoctor(string receiverRole)
        {
            var result = _context.Questions.Where(e => e.receiverRole == receiverRole && e.Answer == null).Select(e => new { e.QuestionID, e.User_Question }).ToList();

            if (receiverRole == null || receiverRole !="doctor")
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


        [Route("api/doctors/GetAllMedicalReportImages")]
        [HttpGet]
        public IHttpActionResult GetAllMedicalReportImages()
        {
            var IsCheckedBefore = _context.MedicalReport_Images.Where(e => e.MedicalReport_Status != null).FirstOrDefault();
            var AllImages =_context.MedicalReport_Images.Select(e => new { e.ImageID, e.ImagePath }).ToList();

            if (AllImages.Count == 0)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else if (IsCheckedBefore != null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs,this item checked before" });
            }
            else
            {
                return Json(new { ErrorID = 2, ErrorMessage = "Successfully" ,AllImages});

            }
        }



        [Route("api/Doctors/CheckMedicalReport")]
        [HttpPut]
        public IHttpActionResult CheckMedicalReport(int imgID,string medicalStatus)
        {
            var IsExisting = _context.MedicalReport_Images.Where(e => e.ImageID == imgID).FirstOrDefault();

            if (IsExisting == null)
            {
                return Json(new { ErrorId = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                var medicalImg = _context.MedicalReport_Images.Where(e => e.ImageID == imgID).FirstOrDefault();

                medicalImg.MedicalReport_Status = medicalStatus;
                _context.SaveChanges();

                return Json(new { ErrorID = 2, ErrorMessage = "Checked Successfully" });

            }
        }

        //[Route("api/doctors/ReplayForQuestion")]
        //[HttpPut]
        //public IHttpActionResult ReplayForQuestion(int userID,string answer)
        //{
        //    var IsUerAsked =_context.Questions.Where(e => e.UserID == userID).FirstOrDefault();

        //    if (IsUerAsked == null)
        //    {
        //        return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
        //    }
        //    else
        //    {
        //        var question =_context.Questions.Where(e => e.UserID == userID).FirstOrDefault();
        //        question.Answer = answer;
        //        _context.SaveChanges();
        //    }
        //    return Json(new { ErrorID = 2, ErrorMessage = "Your Replay Sent Successfully" });
        //}
    }
}
