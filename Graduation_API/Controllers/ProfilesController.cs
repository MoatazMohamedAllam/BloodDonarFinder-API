using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Graduation_API.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.IO;
using System.Web;
using System.Threading.Tasks;

namespace Graduation_API.Controllers
{
    public class ProfilesController : ApiController
    {
        private BloodDonor_APIEntities1 _context;

        public ProfilesController()
        {
            _context = new BloodDonor_APIEntities1();
        }





        [Route("api/profiles/CreateProfile")]
        [HttpPost]
        public IHttpActionResult CreateProfile(BDF_UserProfile profile)
        {

            
            if (profile == null)
            {
                return Json(new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs" }, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });
            }

            else
            {

                //var IsExisting = _context.BDF_UserProfile.Where(e => e.FirstName == profile.FirstName && e.LastName == profile.LastName).FirstOrDefault();

                //if (IsExisting != null)
                //    return Json(new ResponseMessage { ErrorID = 1, ErrorMessage = "firstName and lastname existing in database !!" }, new JsonSerializerSettings
                //                                                                                                                        {
                //                                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                //                                                                                                                        });
                

                var new_Profile = new BDF_UserProfile()
                {
                    User_Id = profile.User_Id,
                    //UserProfileID=profile.User_Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    DOB = profile.DOB,
                    Gender = profile.Gender,
                    BloodGroup = profile.BloodGroup,
                    MobileNo = profile.MobileNo,
                    govern_name = profile.govern_name,
                    city_name = profile.city_name,
                };

                new_Profile.UserProfileID = profile.User_Id;

                //SaveImgURL(profile.ImageURL, new_Profile.UserProfileID);

                _context.BDF_UserProfile.Add(new_Profile);
                _context.SaveChanges();

                var img = new UserProfile_Images();
                img.ProfileID = profile.User_Id;

                _context.UserProfile_Images.Add(img);
                _context.SaveChanges();

                return Json(new ResponseMessage { ErrorID = 2, ErrorMessage = "Added Successfully", UserID = new_Profile.User_Id });

            }
        }


        [Route("api/profiles/UploadProfileImage")]
        [HttpPost]
        public IHttpActionResult UploadProfileImage(UserProfile_Images img)
        {
            var IsProfileExisting = _context.BDF_UserProfile.Where(e => e.UserProfileID == img.ProfileID).FirstOrDefault();
            var IsExisting = _context.UserProfile_Images.Where(e => e.ProfileID == img.ProfileID).FirstOrDefault();
            if (IsProfileExisting == null)
            {
                return Json(new { ErrorID = 1, ErrorMessage = "Error Occurs" });
            }
            else
            {
                if (IsExisting != null)
                {
                    var existingImg = _context.UserProfile_Images.Where(e => e.ProfileID == img.ProfileID).FirstOrDefault();

                    existingImg.ImageData = img.ImageData;
                    _context.SaveChanges();

                    return Json(new { ErrorID = 2, ErrorMessage = "Updated Successfully" });
                }
                else
                {
                    var newImage = new UserProfile_Images()
                    {
                        ProfileID = img.ProfileID,
                        ImageData = img.ImageData,
                    };

                    _context.UserProfile_Images.Add(newImage);
                    _context.SaveChanges();

                    return Json(new { ErrorID = 2, ErrorMessage = "Added Successfully" });
                }
            }
           
        }



        [Route("api/profiles/GetProfileById")]
        [HttpGet]
        public IHttpActionResult GetProfileById(int id)
        {

            var imageProfile = _context.UserProfile_Images.Where(e => e.ProfileID == id).Select(e => e.ImageData).FirstOrDefault();
            var medicalStatus = _context.MedicalReport_Images.Where(e => e.ProfileID == id).Select(e => e.MedicalReport_Status).FirstOrDefault();

            var No_Of_Donations = _context.BDF_BloodRequest.Where(e=>e.ReceiverAccount_id == id && e.IsOperationDone == true).ToList().Count;

            var user = _context.BDF_UserProfile.Where(e => e.UserProfileID == id).Select(
                                                    e => new {
                                                        e.FirstName,
                                                        e.LastName,
                                                        e.DOB,
                                                        e.MobileNo,
                                                        imageProfile,
                                                        medicalStatus,
                                                        No_Of_Donations,
                                                        e.Weight,
                                                        e.Gender,
                                                        e.BloodGroup,
                                                        e.govern_name,
                                                        e.city_name,
                                                        e.User_Id
                                                    }).FirstOrDefault();
            if (user == null)
            {
                return Json(new ResponseMessage { ErrorID = 1, ErrorMessage = "Error Occurs" }, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            }

            return  Json(user ,new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }





        [Route("api/profiles/UpdateUserProfile")]
        [HttpPut]
        public IHttpActionResult UpdateUserProfile(int id, [FromBody]BDF_UserProfile profile)
        {

            if (!ModelState.IsValid)
            {
                return Json(new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs" }, new JsonSerializerSettings
                                                                                                                        {
                                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                                        });
            }


            var existingProfile = _context.BDF_UserProfile.Where(e => e.UserProfileID == id).FirstOrDefault();

       

            if (existingProfile != null)
            {
                existingProfile.FirstName = profile.FirstName;
                existingProfile.LastName = profile.LastName;
                existingProfile.Weight = profile.Weight;
                existingProfile.MobileNo = profile.MobileNo;
                existingProfile.govern_name = profile.govern_name;
                existingProfile.city_name = profile.city_name;
                
                _context.SaveChanges();
            }
            else
            {
                return Json(new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs" }, new JsonSerializerSettings
                                                                                                                        {
                                                                                                                            DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                                        });
            }

            return Json(new ResponseMessage() { ErrorID = 2, ErrorMessage = "Updated Successfully" }, new JsonSerializerSettings
                                                                                                                            {
                                                                                                                                DefaultValueHandling = DefaultValueHandling.Ignore
                                                                                                                            });
        }



    }
}

