using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Graduation_API.Models;

namespace Graduation_API.Controllers
{
    public class FileUploadingController : ApiController
    {


        private BloodDonor_APIEntities1 _context;

        public FileUploadingController()
        {
            _context = new BloodDonor_APIEntities1();
        }


        //[Route("api/fileuploading/UploadFile")]
        //[HttpPost]
        //public async Task<string> UploadFile(int userProfileID)
        //{
        //    var ctx = HttpContext.Current;
        //    var root = ctx.Server.MapPath("~/Images/ProfileImages/");
        //    var provider = new MultipartFormDataStreamProvider(root);

        //    try
        //    {

        //       // Request.Content.IsMimeMultipartContent();
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        foreach (var file in provider.FileData)
        //        {
        //            var name = file.Headers.ContentDisposition.FileName;

        //            name = name.Trim('"');

        //            var localFileName = file.LocalFileName;
        //            var filePath = Path.Combine(root, name);

        //            //File.Move(localFileName,filePath);

        //            SaveFilePathSQLServerEF(userProfileID, localFileName, filePath);

        //            if (File.Exists(localFileName))
        //                File.Delete(localFileName);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return $"Error:{e.Message}";
        //    }
        //    return "file Uploaded!";
        //}


        //[Route("api/fileuploading/UploadFile")]
        //[HttpPost]
        //public async Task<string> UploadFile([FromBody]int userID)
        //{
        //    var ctx = HttpContext.Current;
        //    //int description = HttpContext.Current.Request.Form["userID"];
        //    var root = ctx.Server.MapPath("~/Images");
        //    var provider =
        //        new MultipartFormDataStreamProvider(root);


        //    try
        //    {
        //        await Request.Content
        //            .ReadAsMultipartAsync(provider);


        //        foreach (var file in provider.FileData)
        //        {
        //            var name = file.Headers
        //                .ContentDisposition
        //                .FileName;

        //            // remove double quotes from string.
        //            name = name.Trim('"');

        //            var localFileName = file.LocalFileName;
        //            var filePath = Path.Combine(
        //                root, "ProfileImages", name);

        //        //File.Move(localFileName, filePath);



        //        SaveFileBinarySQLServerEF(userID, localFileName, name);


        //            if (File.Exists(localFileName))
        //                File.Delete(localFileName);


        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return $"Error: {e.Message}";
        //    }
        //    return "file uploaded!";
        //}


        //private void SaveFileBinarySQLServerEF(int userProfileID,string localFile, string fileName)
        //{
        //    // 1) Get file binary
        //    byte[] fileBytes;
        //    using (var fs = new FileStream(
        //        localFile, FileMode.Open, FileAccess.Read))
        //    {
        //        fileBytes = new byte[fs.Length];
        //        fs.Read(
        //            fileBytes, 0, Convert.ToInt32(fs.Length));
        //    }

        //    // 2) Create a Files object
        //    var img = new UserProfile_Images()
        //    {
        //        ProfileID = userProfileID,
        //        ImageData = fileBytes,
        //        ImageName=fileName,
        //        ImageSize=fileBytes.Length
        //    };

        //    var IsUserHasImg = _context.UserProfile_Images.Where(e => e.ProfileID == img.ProfileID).FirstOrDefault();


        //    // 3) Add and save it in database
        //    if (IsUserHasImg == null)
        //        _context.UserProfile_Images.Add(img);
        //        _context.SaveChanges();





        //}

        //public IHttpActionResult GetImage(byte[] data)
        //{
        //    MemoryStream ms = new MemoryStream(data);
        //    return File();
        //return File(ms.ToArray(), "image/png");
        //}


        //public void SaveFilePathSQLServerEF(int userProfileID,string localFile, string filePath)
        //{
        //    //move file to folder
        //    File.Move(localFile, filePath);

        //    //create userProfile_image object
        //    var profileImg = new UserProfile_Images()
        //    {
        //        ImageData = filePath,
        //        ProfileID=userProfileID
        //    };

        //    _context.UserProfile_Images.Add(profileImg);
        //    _context.SaveChanges();
        //}



        //[Route("api/fileuploading/UpdateFile")]
        //[HttpPost]
        //public async Task<string> UpdateFile(int userID)
        //{
        //    var ctx = HttpContext.Current;
        //    var root = ctx.Server.MapPath("~/Images");
        //    var provider =
        //        new MultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        await Request.Content
        //            .ReadAsMultipartAsync(provider);

        //        foreach (var file in provider.FileData)
        //        {
        //            var name = file.Headers
        //                .ContentDisposition
        //                .FileName;

        //            // remove double quotes from string.
        //            name = name.Trim('"');

        //            var localFileName = file.LocalFileName;
        //            var filePath = Path.Combine(
        //                root, "ProfileImages", name);

        //            //File.Move(localFileName, filePath);



        //            UpdateFileBinarySQLServerEF(userID, localFileName, name);


        //            if (File.Exists(localFileName))
        //                File.Delete(localFileName);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return $"Error: {e.Message}";
        //    }

        //    return "File uploaded!";
        //}


        //private void UpdateFileBinarySQLServerEF(int userProfileID, string localFile, string fileName)
        //{
        //    // 1) Get file binary
        //    byte[] fileBytes;
        //    using (var fs = new FileStream(
        //        localFile, FileMode.Open, FileAccess.Read))
        //    {
        //        fileBytes = new byte[fs.Length];
        //        fs.Read(
        //            fileBytes, 0, Convert.ToInt32(fs.Length));
        //    }



        //    var existingImg = _context.UserProfile_Images.Where(e => e.ProfileID == userProfileID).FirstOrDefault();

        //    if (existingImg != null)
        //    {
        //        existingImg.ImageName = fileName;
        //        existingImg.ImageData = fileBytes;
        //        existingImg.ImageSize = fileBytes.Length;
        //    }

        //    _context.SaveChanges();

        //}




        //[Route("api/profiles/Upload")]
        //[HttpPost]
        //public async Task<string> Upload(int userProfileID)
        //{

        //        var request = HttpContext.Current.Request;
        //        var description = request.Form["description"];
        //        var photo = request.Files["photo"];

        //        var root = HttpContext.Current.Server.MapPath("~/Images" + photo.FileName);

        //        var provider = new MultipartFormDataStreamProvider(root);

        //        try
        //        {
        //            await Request.Content.ReadAsMultipartAsync(provider);

        //            foreach (var file in provider.FileData)
        //            {
        //                var name = file.Headers.ContentDisposition.FileName;

        //                // remove double quotes from string.
        //                name = name.Trim('"');

        //                var localFileName = file.LocalFileName;
        //                var filePath = Path.Combine(root, "ProfileImages", name);

        //                //File.Move(localFileName, filePath);



        //                SaveFileBinarySQLServerEF(userProfileID, localFileName, name);


        //                if (File.Exists(localFileName))
        //                    File.Delete(localFileName);
        //            }




        //        //var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images/ProfileImages/");

        //        //var multiFormDataStreamProvider = new MultipartFormDataStreamProvider(fileuploadPath);

        //        //await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);

        //        //string uploadingFileName = multiFormDataStreamProvider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

        //        //var UserProfile_Img = new UserProfile_Images()
        //        //{
        //        //    ImagePath=uploadingFileName,
        //        //    ProfileID=userProfileID
        //        //};

        //        //_context.UserProfile_Images.Add(UserProfile_Img);
        //        //_context.SaveChanges();

        //    }
        //    catch (Exception e)
        //    {
        //        return $"Error:{e.Message }";
        //    }


        //    return "file uploaded!";
        //}

        //private void SaveFileBinarySQLServerEF(int userProfileID, string localFile, string fileName)
        //{
        //    // 1) Get file binary
        //    byte[] fileBytes;
        //    using (var fs = new FileStream(
        //        localFile, FileMode.Open, FileAccess.Read))
        //    {
        //        fileBytes = new byte[fs.Length];
        //        fs.Read(
        //            fileBytes, 0, Convert.ToInt32(fs.Length));
        //    }

        //    // 2) Create a Files object
        //    var img = new UserProfile_Images()
        //    {
        //        ProfileID = userProfileID,
        //        ImageData = fileBytes,
        //        ImageName = fileName,
        //        ImageSize = fileBytes.Length
        //    };


        //    // 3) Add and save it in database
        //    _context.UserProfile_Images.Add(img);
        //    _context.SaveChanges();

        //}


        //public Image Base64Image(string base64String)
        //{
        //    try
        //    {
        //        byte[] imageBytes = Convert.FromBase64String(base64String);
        //        using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //        {
        //            ms.Write(imageBytes, 0, imageBytes.Length);
        //            Image image = Image.FromStream(ms, true);
        //            return image;
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}

        //public void SaveImgURL(string img, long userProfile_ID)
        //{
        //    Image convertedImage = Base64Image(img);
        //    string picPath = "";

        //    if (convertedImage != null)
        //    {
        //        picPath = userProfile_ID + "H" + StringGenertaion() + ".jpg";

        //        try
        //        {
        //            convertedImage.Save(HttpContext.Current.Server.MapPath("/Images" + picPath), System.Drawing.Imaging.ImageFormat.Jpeg);

        //            try
        //            {
        //                //var myImage = new UserProfile_Images()
        //                //{
        //                //    ImagePath = picPath,
        //                //    ProfileID = userProfile_ID
        //                //};
        //                //_context.UserProfile_Images.Add(myImage);
        //                //_context.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                throw e;

        //            }

        //        }
        //        catch (Exception e)
        //        {

        //            throw e;
        //        }
        //    }

        //}


        ////String Generation Method
        //public static string StringGenertaion()
        //{
        //    String randomString = Path.GetRandomFileName();
        //    randomString = randomString.Replace(".", string.Empty);

        //    return randomString;
        //}







        //[Route("api/profiles/create")]
        //[HttpPost]
        //public IHttpActionResult Create(string username,string govern_Name,string city_Name,BDF_UserProfile profile)
        //{
        //    var Check = profile.FirstName == null || profile.BloodGroup == null || profile.MobileNo == null
        //               || govern_Name == null || city_Name == null;

        //    var IsUserNameTaken = _context.BDF_UserAccount.Where(e => e.Username == username).FirstOrDefault();

        //    var IsExisting = _context.BDF_UserProfile.Where(e => e.FirstName == profile.FirstName && e.MiddleName == profile.MiddleName && e.LastName == profile.LastName).FirstOrDefault();

        //    if (Check == true || IsUserNameTaken == null)
        //    {
        //        return Json( new ResponseMessage() { ErrorID = 1, ErrorMessage = "Error Occurs"}, new JsonSerializerSettings
        //                                                                                                                {
        //                                                                                                                    DefaultValueHandling = DefaultValueHandling.Ignore
        //                                                                                                                });
        //    }

        //    else
        //    {

        //        var govern_id = _context.Governorates.Where(e => e.governorate_name_en == govern_Name).Select(e => e.id).FirstOrDefault();
        //        var city_id = _context.Cities.Where(e => e.city_name_en == city_Name).Select(e => e.id).FirstOrDefault();

        //        //adding address of user in address table
        //        var myAddress = new BDF__Address();
        //        myAddress.GovernorateID = govern_id;
        //        myAddress.CityID = city_id;
        //        _context.BDF__Address.Add(myAddress);
        //        _context.SaveChanges();


        //        if (IsExisting != null)
        //            return Json(new ResponseMessage { ErrorID = 1, ErrorMessage = "this profile existing in database!!" }, new JsonSerializerSettings
        //                                                                                                                            {
        //                                                                                                                                DefaultValueHandling = DefaultValueHandling.Ignore
        //                                                                                                                            });

        //        var userAccount_id = _context.BDF_UserAccount.Where(e => e.Username == username).Select(e => e.AccountID).FirstOrDefault();

        //        var new_Profile = new BDF_UserProfile()
        //                        {
        //                            FirstName=profile.FirstName,
        //                            MiddleName = profile.MiddleName,
        //                            LastName = profile.LastName,
        //                            DOB = profile.DOB,
        //                            Weight = profile.Weight,
        //                            Gender = profile.Gender,
        //                            ImageURL = profile.Gender,
        //                            BloodGroup = profile.BloodGroup,
        //                            MobileNo = profile.MobileNo,
        //                            Address_Id = myAddress.AddressID,
        //                            User_Id = userAccount_id
        //                        };
        //        _context.BDF_UserProfile.Add(new_Profile);
        //        _context.SaveChanges();

        //    }
        //    return Json(new ResponseMessage { ErrorID = 2, ErrorMessage = "Added Successfully",UserID = profile.User_Id });

        //}


        //[Route("api/profiles/CreateProfile")]
        //[HttpPost]
        //public HttpResponseMessage CreateProfile(string username, string firstName, string middleName, string lastName,
        //                                        string DateOfBirth, double? weight, string gender,
        //                                        string imgUrl, string bloodGroup, string mobileNumber,
        //                                        string govern_Name, string city_Name)
        //{
        //    var Check = firstName == null || bloodGroup == null || mobileNumber == null
        //                || govern_Name == null || city_Name == null;

        //    var IsUserNameTaken = _context.BDF_UserAccount.Where(e => e.Username == username).FirstOrDefault();

        //    if (Check == true || IsUserNameTaken == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new ResponseMessage() { ErrorID = 0, ErrorMessage = "Error Occurs" });
        //    }

        //    else
        //    {

        //        var govern_id = _context.Governorates.Where(e => e.governorate_name_en == govern_Name).Select(e => e.id).FirstOrDefault();
        //        //var govern_ID = _context.BDF__Address.Where(e => e.GovernorateID == govern_id).Select(e => e.GovernorateID).FirstOrDefault();
        //        var city_id = _context.Cities.Where(e => e.city_name_en == city_Name).Select(e => e.id).FirstOrDefault();

        //        //adding address of user in address table
        //        var myAddress = new BDF__Address();
        //        myAddress.GovernorateID = govern_id;
        //        myAddress.CityID = city_id;
        //        _context.BDF__Address.Add(myAddress);
        //        _context.SaveChanges();

        //        var profile = new BDF_UserProfile();

        //        var userAccount_id = _context.BDF_UserAccount.Where(e => e.Username == username).Select(e => e.AccountID).FirstOrDefault();

        //        profile.FirstName = firstName;
        //        profile.MiddleName = middleName;
        //        profile.LastName = lastName;
        //        profile.DOB = DateOfBirth;
        //        profile.Weight = weight;
        //        profile.Gender = gender;
        //        profile.ImageURL = imgUrl;
        //        profile.BloodGroup = bloodGroup;
        //        profile.MobileNo = mobileNumber;
        //        profile.Address_Id = myAddress.AddressID;
        //        profile.User_Id = userAccount_id;


        //        _context.BDF_UserProfile.Add(profile);
        //        _context.SaveChanges();


        //    }

        //    return Request.CreateResponse(HttpStatusCode.Created, new ResponseMessage() { ErrorID = 1, ErrorMessage = "Added Successfully" });
        //}

    }
}
