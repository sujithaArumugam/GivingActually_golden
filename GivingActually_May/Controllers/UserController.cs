using GivingActually_May.Models;
using GivingActually_May.Models.HelperModels;
using GivingActually_May.Service;
using Newtonsoft.Json;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GivingActually_May.Models.HelperModels.Helper;

namespace GivingActually_May.Controllers
{
    [Authorization]
    [UserAuthorization]
    [OutputCache(Duration = 15, VaryByParam = "None")]
    public class UserController : Controller
    {
        CommonService IService = new CommonService();
        public ActionResult Index(string Category = "", int page = 1)
        {
            try
            {

                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetUserCampaignsbypage(CategoryId);
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);
                var categoryList = Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                res.SelectedOptionsList = categoryList;
                return View(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void uploadnow(HttpPostedFileWrapper upload)
        {
            if (upload != null)
            {
                string ImageName = upload.FileName;
                string path = System.IO.Path.Combine(Server.MapPath("~/Images/uploads"), ImageName);
                upload.SaveAs(path);
            }
        }
        public ActionResult uploadPartial()
        {
            var appData = Server.MapPath("~/Images/uploads");
            var images = Directory.GetFiles(appData).Select(x => new imagesviewmodel
            {
                Url = Url.Content("/images/uploads/" + Path.GetFileName(x))
            });
            return View(images);
        }
        public ActionResult GetMyStories(string Category = "")
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetUserStories(CategoryId);
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                //res.SelectedOptionsList.Add(new SelectListItem
                //{
                //    Text = "Select",
                //    Value = ""
                //});
                //foreach (Helper.StoryCategory eVal in Enum.GetValues(typeof(Helper.StoryCategory)))
                //{
                //    res.SelectedOptionsList.Add(new SelectListItem { Text = Enum.GetName(typeof(Helper.StoryCategory), eVal), Value = eVal.ToString() });
                //}
                return View(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AllCampaignsPartial(string Category = "", int page = 1)
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetCampaignsbyPage(CategoryId, page);

                return PartialView("_CampaignListView", res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AllCampaigns(string Category = "", int page = 1)
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetCampaignsbyPage(CategoryId, page);
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);
                var categoryList = Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                res.SelectedOptionsList = categoryList;

                return View(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //StoriesViewModel
        public ActionResult AddStories(int Id = 0)
        {
            try
            {
                StoriesViewModel Model = new StoriesViewModel();
                if (Id > 0)
                {
                    Model = IService.GetStorie(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult CampaignView(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();
                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult AddBeneficiary(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();
                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                    //Model.CampainOrganizer.AvailableCountries.AddRange(IService.GetCountryNames());
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult AddBeneficiary(CampaignMainViewModel story)
        {
            try
            {
                int result = 0;
                if (Request != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file1 = Request.Files[0];
                        if (file1 != null && file1.ContentLength > 0)
                        {
                            story.CampainOrganizer.BDisplayPicName = System.IO.Path.GetFileName(file1.FileName);
                            story.CampainOrganizer.BDisplayPic = Bytes(file1);
                        }
                    }
                    story.CampainOrganizer.storyId = story.Id;
                    story.CampainOrganizer.Latitude = story.Latitude;
                    story.CampainOrganizer.longitude = story.Longitude;
                    result = IService.AddOrganizer(story.CampainOrganizer);
                }
                story = IService.GetCamapign(story.Id);
                return View("AddCampaignDescription", story);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static byte[] Bytes(HttpPostedFileBase result)
        {
            try
            {
                
                var length = result.InputStream.Length; //Length: 103050706
                MemoryStream target = new MemoryStream();
                result.InputStream.CopyTo(target); // generates problem in this line
                byte[] data = target.ToArray();
                if (data.Length > 200000 && result.ContentType!= "video/mp4")
                {
                    result.InputStream.Seek(0, SeekOrigin.Begin);
                    data = SaveJpeg(Image.FromStream(target), 60);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static byte[] DefaultCompressionPng(Image original)
        {
            MemoryStream ms = new MemoryStream();
            original.Save(ms, ImageFormat.Png);
            Bitmap compressed = new Bitmap(ms);
            ms.Close();

            MemoryStream ms1 = new MemoryStream();
            compressed.Save(ms1, ImageFormat.Png);
            byte[] data = ms1.ToArray();
            ms1.Close();
            ms1.Dispose();

            return data;
        }
        public static byte[] SaveJpeg(Image img, int quality)
        {
            

            // Encoder parameter for image quality
            EncoderParameter qualityParam =
                new EncoderParameter(Encoder.Quality, quality);
            // Jpeg image codec
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, jpegCodec, encoderParams);
            //var img1 = Image.FromStream(ms);
            byte[] data = ms.ToArray();
            ms.Close();
            ms.Dispose();
           
            return data;
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        //public static byte[] ImageResize(Stream imageStream, bool thumb)
        //{
        //    Dictionary<string, string> versions = new Dictionary<string, string>();
        //    //Define the versions to generate
        //    versions.Add("_thumb", "width=100&height=100&crop=auto&format=jpg"); //Crop to square thumbnail
        //    versions.Add("_medium", "maxwidth=200&maxheight=200&format=jpg"); //Fit inside 400x400 area, jpeg
        //    versions.Add("_large", "format=jpg&mode=max&quality=50"); //Fit inside 1900x1200 area

        //    if (thumb)
        //    {
        //        using (var thumbStream = new MemoryStream())
        //        {
        //            ImageBuilder.Current.Build(imageStream, thumbStream, new ResizeSettings(versions["_medium"]));
        //            return thumbStream.ToArray();
        //        }
        //    }
        //    else
        //    {
        //        using (var originalStream = new MemoryStream())
        //        {
        //            ImageBuilder.Current.Build(imageStream, originalStream, new ResizeSettings(versions["_large"]));
        //            return originalStream.ToArray();
        //        }
        //    }
        //}
        [HttpPost]
        //public ActionResult AddStories([Bind(Exclude = "File1,File2,File3,File4,File5")]StoriesViewModel ViewModel)//, HttpPostedFileBase File1)//, HttpPostedFileBase File2, HttpPostedFileBase File3, HttpPostedFileBase File4, HttpPostedFileBase File5)
        public ActionResult AddStories([Bind(Exclude = "File1,File2,File3,File4,File5")]StoriesViewModel ViewModel)
        {
            try
            {
                Files fileModel = new Files();
                ViewModel.Files = new List<Files>();
                if (Request != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file1 = Request.Files[0];

                        if (file1 != null && file1.ContentLength > 0)
                        {
                            fileModel = new Files();
                            fileModel.File = Bytes(file1); // file1 to store image in binary formate  
                            fileModel.FileName = System.IO.Path.GetFileName(file1.FileName);
                            ViewModel.Files.Add(fileModel);
                        }
                    }
                    if (Request.Files.Count > 1)
                    {
                        var file2 = Request.Files[1];

                        if (file2 != null && file2.ContentLength > 0)
                        {
                            fileModel = new Files();
                            fileModel.File = Bytes(file2); // file1 to store image in binary formate  
                            fileModel.FileName = System.IO.Path.GetFileName(file2.FileName);
                            ViewModel.Files.Add(fileModel);
                        }
                    }
                    if (Request.Files.Count > 2)
                    {
                        var file3 = Request.Files[2];

                        if (file3 != null && file3.ContentLength > 0)
                        {

                            fileModel = new Files();
                            fileModel.File = Bytes(file3); // file1 to store image in binary formate  
                            fileModel.FileName = System.IO.Path.GetFileName(file3.FileName);
                            ViewModel.Files.Add(fileModel);
                        }
                    }
                    if (Request.Files.Count > 3)
                    {
                        var file4 = Request.Files[3];

                        if (file4 != null && file4.ContentLength > 0)
                        {

                            fileModel = new Files();
                            fileModel.File = Bytes(file4); // file1 to store image in binary formate  
                            fileModel.FileName = System.IO.Path.GetFileName(file4.FileName);
                            ViewModel.Files.Add(fileModel);
                        }
                    }
                    if (Request.Files.Count > 4)
                    {
                        var file5 = Request.Files[4];

                        if (file5 != null && file5.ContentLength > 0)
                        {
                            fileModel = new Files();
                            fileModel.File = Bytes(file5); // file1 to store image in binary formate  
                            fileModel.FileName = System.IO.Path.GetFileName(file5.FileName);
                            ViewModel.Files.Add(fileModel);
                        }
                    }
                }
                var res = IService.CreateStory(ViewModel);
                if (res)
                    return RedirectToAction("Index");
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UploadFiles(StoriesViewModel ViewModel)
        {
            var profile = Request.Files;

            string imgname = string.Empty;
            string ImageName = string.Empty;

            //Following code will check that image is there 
            //If it will Upload or else it will use Default Image

            //if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            //{
            //    var logo = System.Web.HttpContext.Current.Request.Files["file"];
            //    if (logo.ContentLength > 0)
            //    {
            //        var profileName = Path.GetFileName(logo.FileName);
            //        var ext = Path.GetExtension(logo.FileName);

            //        ImageName = profileName;
            //        var comPath = Server.MapPath("/Images/") + ImageName;

            //        logo.SaveAs(comPath);
            //        userDetail.Profile = comPath;
            //    }

            //}
            //else
            //    userDetail.Profile = Server.MapPath("/Images/") + "profile.jpg";

            int response = 1;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteStories(int Id = 0)
        {
            try
            {

                if (Id > 0)
                {
                    var res = IService.DeleteStorie(Id);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddExpenseDetails(int Id = 0)
        {
            try
            {
                var Model = IService.GetStoryAccountsDetails(Id);
                if (Model.Debits.Count == 0)
                {
                    DebitDetails m = new DebitDetails();
                    m.Expense = 0.0;
                    m.ExpenseDescription = "";
                    Model.Debits.Add(m);
                }
                return View(Model);

            }
            catch (Exception ex) { throw ex; }
        }
        [HttpPost]
        public ActionResult AddExpenseDetails(StoryAccountsDetails Model)
        {
            try
            {
                var res = IService.AddStoryAccountsDetails(Model);
                return RedirectToAction("Index");
            }
            catch (Exception ex) { throw ex; }
        }

        public PartialViewResult Load(string Cnt)
        {
            try
            {

                return PartialView("_LoadExpense");
            }
            catch (Exception ex) { throw ex; }

        }

        public ActionResult AddCampaign(int Id = 0)
        {
            try
            {
                StoriesViewModel Model = new StoriesViewModel();
                if (Id > 0)
                {
                    Model = IService.GetStorie(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult AddCampaign(CampaignMainViewModel ViewModel)
        {
            try
            {
                var Id = IService.CreateCampaign(ViewModel);
                if (Id > 0)
                {
                    StoriesViewModel Model = new StoriesViewModel();
                    Model = IService.GetStorieDetails(Id);

                    return View("AddBeneficiary", Model);
                }
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddCampaignTarget(int Id = 0)
        {
            try
            {
                StoriesViewModel Model = new StoriesViewModel();
                if (Id > 0)
                {
                    Model = IService.GetStorie(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult AddCampaignTarget(StoriesViewModel ViewModel)
        {
            try
            {
                ViewModel.CampaignTarget.storyId = ViewModel.Id;
                var Id = IService.CreateTarget(ViewModel.CampaignTarget);
                if (Id > 0)
                {
                    StoriesViewModel Model = new StoriesViewModel();
                    Model = IService.GetStorieDetails(Id);

                    return View("AddCampaignDescription", Model);
                }
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddCampaignDescription(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();
                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddCampaignDescription([Bind(Exclude = "File1,File2,File3,File4,File5")]CampaignMainViewModel ViewModel)
        {
            try
            {
                Files fileModel = new Files();
                ViewModel.Files = new List<Files>();
                if (Request != null)
                {
                    List<Files> fileDetails = new List<Files>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            Files fileDetail = new Files()
                            {
                                File = Bytes(file),
                                FileName = fileName,
                                ContentType = file.ContentType
                            };
                            fileDetails.Add(fileDetail);
                        }
                    }
                    ViewModel.Files.AddRange(fileDetails);

                }
                var res = IService.CreateCampaignDescription(ViewModel);
                if (res)
                    return RedirectToAction("Index");
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult PostUpdate(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();
                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PostUpdate([Bind(Exclude = "File1,File2,File3,File4,File5")]CampaignMainViewModel ViewModel)
        {
            try
            {
                Files fileModel = new Files();
                ViewModel.Files = new List<Files>();
                if (Request != null)
                {
                    List<Files> fileDetails = new List<Files>();
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            Files fileDetail = new Files()
                            {
                                File = Bytes(file),
                                FileName = fileName,
                                ContentType = file.ContentType
                            };
                            fileDetails.Add(fileDetail);
                        }
                    }
                    ViewModel.Files.AddRange(fileDetails);
                    ViewModel.campaignupdate.StoryId = ViewModel.Id;
                }
                var res = IService.AddPostUpdate(ViewModel);
                if (res)
                {
                    var m = IService.GetCamapign(ViewModel.Id);
                    return RedirectToAction("CampaignView",m);
                }
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult AddNewCampaign(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();
                Model.CampaignTargetMoneyType = "1";
                if (UserSession.UserCountry.ToString() == "IN")
                { Model.CampaignTargetMoneyType = "0"; }
                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult AddNewCampaign(CampaignMainViewModel ViewModel)
        {
            try
            {
                ViewModel.CampaignTargetMoneyType = "1";
                if (UserSession.UserCountry.ToString() == "IN")
                { ViewModel.CampaignTargetMoneyType = "0"; }
                var Id = IService.CreateCampaign(ViewModel);
                if (Id > 0)
                {
                    var Model = IService.GetCamapign(Id);
                    Model.CampainOrganizer.storyId = Id;
                    Model.Id = Id;
                    //  Model.CampainOrganizer.AvailableCountries.AddRange(IService.GetCountryNames());
                    ModelState.Clear();
                    return View("AddBeneficiary", Model);
                }
                else
                    return View(ViewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DonateCampaign(int Id = 0)
        {
            try
            {
                CampaignMainViewModel Model = new CampaignMainViewModel();

                if (Id > 0)
                {
                    Model = IService.GetCamapign(Id);
                }
                Model.CampaignDonations.DonationMoneyType = "1";
                if (UserSession.UserCountry.ToString() == "IN")
                { Model.CampaignDonations.DonationMoneyType = "0"; }
                var user = IService.GetUserDetail();
                Model.CampaignDonations.IdentityName = user.DisplayName;
                Model.CampaignDonations.EMail = user.UserName;
                var categoryList = Enum.GetValues(typeof(MoneyType)).Cast<MoneyType>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                Model.CampaignDonations.selectedListMoney = categoryList; return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DonateCampaign(CampaignMainViewModel ViewModel)
        {
            try
            {

                ViewModel.CampaignDonations.StoryId = ViewModel.Id;
                ViewModel.CampaignDonations.Latitude = ViewModel.Latitude;
                ViewModel.CampaignDonations.Longitude = ViewModel.Longitude;
                var DId = IService.AddCampaignDonation(ViewModel.CampaignDonations);
                var m = IService.GetCamapign(ViewModel.Id);
                //if (UserSession.UserCountry.ToString() == "IN")
                //{ m.CampaignDonations.DonationMoneyType = "0"; }

                MiniDonationModel model = new MiniDonationModel();
                model.DonateId = DId;
                model.Amount = ViewModel.CampaignDonations.DonationAmnt;
                model.MType = ViewModel.CampaignDonations.DonationMoneyType;
                model.Uname = ViewModel.CampaignDonations.IdentityName;
                model.Email = ViewModel.CampaignDonations.EMail;
                model.PhNo = ViewModel.CampaignDonations.PhNo != "" ? ViewModel.CampaignDonations.PhNo : "9999999999";
                return RedirectToAction("PayYourDonations", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Charge(string razorpay_order_id, string razorpay_payment_id, string razorpay_signature)
        {
            var model = IService.UpdateCampaignDonationSuccess(razorpay_order_id, razorpay_payment_id, razorpay_signature);
            ViewBag.successMsg = "donation Successfully!. Thanks for your Kindness!!";
            return View("CampaignView", model);
        }

        [HttpPost]
        public ActionResult AddComments(int Id, string comments)
        {
            CampaignMainViewModel Model = new CampaignMainViewModel();
            if (Id > 0)
            {
                PostCommentsVM pstcmt = new PostCommentsVM();
                pstcmt.Id = Id;
                pstcmt.CommentText = comments;
                Model = IService.AddComments(pstcmt);
            }
            return PartialView("_miniComments", Model);
        }
        [HttpPost]
        public ActionResult AddCommentcount(int Id, string comments)
        {
            CampaignMainViewModel Model = new CampaignMainViewModel();
            if (Id > 0)
            {
                Model = IService.GetCamapign(Id);
            }
            return PartialView("_commentcount", Model);
        }

        [HttpPost]
        public ActionResult ToggleLike(int Id)
        {
            CampaignMainViewModel Model = new CampaignMainViewModel();
            if (Id > 0)
            {
                Model = IService.GetCamapign(Id);
                PostLikes pstcmt = new PostLikes();
                pstcmt.Id = Id;
                pstcmt.isLiked = !(Model.isLiked);
                Model = IService.ToggleLikes(pstcmt);
            }
            return PartialView("_Like", Model);
        }
        [HttpPost]
        public ActionResult RemoveAttachement(int AttId, int SId)
        {
            CampaignMainViewModel Model = new CampaignMainViewModel();
            if (SId > 0)
            {
                bool res = IService.DeleteAttachment(AttId, SId);

                Model = IService.GetCamapign(SId);
            }
            return PartialView("_fileAttachement", Model);
        }
        public ActionResult PayYourDonations(MiniDonationModel model)
        {

            var typeM = (MoneyType)(Convert.ToInt32(model.MType));
            var typeNmae = typeM.DisplayName();

            Dictionary<string, object> input = new Dictionary<string, object>();
            input.Add("amount", model.Amount * 100); // this amount should be same as transaction amount
            input.Add("currency", typeNmae);
            input.Add("receipt", model.DonateId.ToString());
            input.Add("payment_capture", 0);

            string key = ConfigurationManager.AppSettings["RPapikey"];
            string secret = ConfigurationManager.AppSettings["RPSecretkey"];
            //string key = "rzp_test_5iPslGRlz5M0ss";
            //string secret = "qEsCxVcyaRoSAatpBaVZMBdp";
            ////string key = "rzp_live_S9v0s4ePfuxE1p";
            ////string secret = "KJylHsBzeIlVeF336TPxjk6w";
            System.Net.ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;


            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            var orderId = order["id"].ToString();
            ViewBag.Order = orderId;
            model.orderId = orderId;
            ViewBag.Amount = model.Amount * 100;
            ViewBag.EMail = model.Email;
            ViewBag.RPkey = key;
            IService.UpdateCampaignDonationorder(model.DonateId, orderId, "", "");
            return View(model);
        }
        public List<SelectListItem> LoadCountries()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select", Value = "0" });
            li.Add(new SelectListItem { Text = "India", Value = "1" });
            li.Add(new SelectListItem { Text = "Srilanka", Value = "2" });
            li.Add(new SelectListItem { Text = "China", Value = "3" });
            li.Add(new SelectListItem { Text = "Austrila", Value = "4" });
            li.Add(new SelectListItem { Text = "USA", Value = "5" });
            li.Add(new SelectListItem { Text = "UK", Value = "6" });
            ViewData["country"] = li;
            return li;
        }

       

        public JsonResult GetStates(string id)
        {
            List<SelectListItem> states = new List<SelectListItem>();
            states = IService.GetStateNames(Convert.ToInt32(id));
            return Json(new SelectList(states, "Value", "Text"));
        }

        //public JsonResult GetCity(string id)
        //{
        //    List<SelectListItem> City = new List<SelectListItem>();
        //    City = IService.GetCityNames(Convert.ToInt32(id));

        //    return Json(new SelectList(City, "Value", "Text"));
        //}
    }
}