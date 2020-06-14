using DotNetOpenAuth.GoogleOAuth2;
using Facebook;
using GivingActually_May.Models;
using GivingActually_May.Models.HelperModels;
using GivingActually_May.Service;
using Microsoft.AspNet.Membership.OpenAuth;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using static GivingActually_May.Models.HelperModels.Helper;

namespace GivingActually_May.Controllers
{
    [OutputCache(Duration = 15, VaryByParam = "None")]
    public class HomeController : Controller
    {
        CommonService IService = new CommonService();

        public ActionResult Index(string Category = "")
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                UserSession.UserName = "";
                UserSession.HasSession = false;
                UserSession.UserId = 0;
                UserSession.UserRole = null;
                var res = IService.GetCampaignsForIndex(CategoryId);
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);
                return View(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                LoginModel Model = new LoginModel();
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (model != null)
                {
                    var res = IService.ValidateUser(model);

                    var city = IService.GetCountryByIP();
                    if (!string.IsNullOrEmpty(res.UserName))
                    {
                        UserSession.UserName = res.UserName;
                        UserSession.HasSession = true;
                        UserSession.UserId = res.Id;
                        UserSession.UserCountry = city.Name;
                        UserSession.LastLoginDate = res.LastLoginTime != null ? res.LastLoginTime.Value : DateTime.Now;
                        if (res.IsAdmin)
                        {
                            UserSession.UserRole = RolesEnum.Admin;
                            UserSession.UserRoleId = RolesEnum.Admin.GetHashCode().ToString();
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            UserSession.UserRole = RolesEnum.User;
                            UserSession.UserRoleId = RolesEnum.User.GetHashCode().ToString();
                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        ViewBag.LoginMsg = "Please Check your credentials.";
                        return View(model);
                    }
                }
                else
                    return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        [HttpGet]
        public ActionResult HowItWorks()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                LoginModel Model = new LoginModel();

                UserSession.UserName = "";
                UserSession.HasSession = false;
                UserSession.UserId = 0;

                UserSession.UserRole = null;
                UserSession.UserRoleId = "";
                return View("Login", Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            try
            {
                RegisterModel Model = new RegisterModel();

                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Register(RegisterModel Model)
        {
            try
            {
                var res = IService.RegisterUser(Model);
                if (res == "Registered Successfully. Please try logging in!")
                {
                    return View("Login");
                }
                else
                {
                    ViewBag.RegisterMessage = res;
                    return View("Register");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

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

        [HttpGet]
        public ActionResult PasswordReset()
        {
            try
            {
                RegisterModel Model = new RegisterModel();

                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult PasswordReset(RegisterModel Model)
        {
            try
            {
                var res = IService.ResetPassword(Model);
                if (res == "Password Reset Successful!")
                {
                    ViewBag.RegisterMessage = res;
                    return View("Login");
                }
                else
                {
                    ViewBag.RegisterMessage = res;
                    return View("PasswordReset");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            try
            {
                RegisterModel Model = new RegisterModel();

                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult SignUp(RegisterModel Model)
        {
            try
            {
                var res = IService.RegisterUser(Model);
                if (res != "")
                { return View("Login"); }
                else
                {
                    ViewBag.RegisterMessage = "User Already Avilable. Please Login";
                    return View("SignUp", res);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetOTP(string UserName)
        {
            try
            {
                RegisterModel Model = new RegisterModel();
                var res = IService.GetOTPModel(UserName);
                if (res.IsSecurityTokenGenerated)
                {
                    ViewBag.GetOTPResult = "OTP is been sent to your resigtered mail. please input the same to proceed!";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult About()
        {
            try
            {
                ViewBag.Message = "GivingActually_May";

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Contact()
        {
            try
            {
                ViewBag.Message = "GivingActually_May";

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult FacebookLogin(FacebookLoginModel model)
        {
            Session["uid"] = model.uid;
            Session["accessToken"] = model.accessToken;

            return Json(new { success = true });
        }
        public ActionResult GetStoriesbyLocation(string Category = "")
        {
            try
            {

                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetCampaignsbyLoc(CategoryId);

                res.SelectedOptions = new int[] { CategoryId };
                res.SelectedOption = CategoryId;
                var listItem = (Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(
       e => new SelectListItem() { Text = e.ToString(), Value = e.GetHashCode().ToString() })).ToList();
                List<SelectListItem> ModelSelectedList = new List<SelectListItem>();
                foreach (var item in listItem)
                {
                    var selcList = new SelectListItem();
                    if (item.Value == CategoryId.ToString())
                    { selcList.Selected = true; }
                    selcList.Text = item.Text;
                    selcList.Value = item.Value;
                    ModelSelectedList.Add(selcList);


                }
                res.SelectedOptionsList = ModelSelectedList;
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                return View("AllCampaigns", res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult GoogleLogin(string email, string name, string gender, string lastname, string location)
        {
            RegisterModel mod = new RegisterModel();
            mod.UserName = email;
            mod.DPName = name;
            UserModel usermodel = IService.ToFBregisterModel(mod);
            Locationmodel city = IService.GetUserCountryByIp();
            UserSession.UserCountry = city.CountryCode;
            UserSession.UserName = usermodel.UserName;
            UserSession.HasSession = true;
            UserSession.UserId = usermodel.Id;
            UserSession.LastLoginDate = usermodel.LastLoginTime != null ? usermodel.LastLoginTime.Value : DateTime.Now;
            if (usermodel.IsAdmin)
            {
                UserSession.UserRole = RolesEnum.Admin;
                UserSession.UserRoleId = RolesEnum.Admin.GetHashCode().ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                UserSession.UserRole = RolesEnum.User;
                UserSession.UserRoleId = RolesEnum.User.GetHashCode().ToString();
                return RedirectToAction("Index", "User");
            }

        }

        public ActionResult AllCampaignsPartial(string Category = "", int page = 1)
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                var res = IService.GetCampaignsbyPage(CategoryId, page);
                var categoryList = Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                res.SelectedOptionsList = categoryList;
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
                //var list = res.CampaignViewModelList.Take(12*page);
                //res.CampaignViewModelList = list.ToList();

                res.SelectedOptions = new int[] { CategoryId };
                res.SelectedOption = CategoryId;
               var categoryList= Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                res.SelectedOptionsList = categoryList;
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);
                return View(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (ProviderName == null || ProviderName == "")
            {
                NameValueCollection nvs = Request.QueryString;
                if (nvs.Count > 0)
                {
                    if (nvs["state"] != null)
                    {
                        NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
                        if (provideritem["__provider__"] != null)
                        {
                            ProviderName = provideritem["__provider__"];
                        }
                    }
                }
            }
            GoogleOAuth2Client.RewriteRequest();
            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);
            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Home", "Login"));
            }

            string ProviderUserId = authResult.ProviderUserId;
            string ProviderUserName = authResult.UserName;

            string Email = null;
            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }

            RegisterModel mod = new RegisterModel();
            mod.UserName = Email;
            mod.DPName = ProviderUserName;
            UserModel usermodel = IService.ToFBregisterModel(mod);
            Locationmodel city = IService.GetUserCountryByIp();
            UserSession.UserCountry = city.CountryCode;
            UserSession.UserName = usermodel.UserName;
            UserSession.HasSession = true;
            UserSession.UserId = usermodel.Id;
            UserSession.LastLoginDate = usermodel.LastLoginTime != null ? usermodel.LastLoginTime.Value : DateTime.Now;
            if (usermodel.IsAdmin)
            {
                UserSession.UserRole = RolesEnum.Admin;
                UserSession.UserRoleId = RolesEnum.Admin.GetHashCode().ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                UserSession.UserRole = RolesEnum.User;
                UserSession.UserRoleId = RolesEnum.User.GetHashCode().ToString();
                return RedirectToAction("Index", "User");
            }



        }
        public ActionResult AllCampaignsWithSearch(string searchtext = "")
        {
            try
            {
                CampaignsListViewModel ModelList = new CampaignsListViewModel();
                var res = IService.GetCampaigns(-1);
                var result = res.CampaignViewModelList;
                result = result.Where(s => (s.NGOName != null ? s.NGOName.ToLower().Contains(searchtext) : false) ||
               (s.campaignDescription != null ? (s.campaignDescription.StoryDescription != null ? s.campaignDescription.StoryDescription.ToLower().Contains(searchtext) : false) : false) ||
                (s.BGroupName != null ? s.BGroupName.ToLower().Contains(searchtext) : false) ||
                (s.BName != null ? s.BName.ToLower().Contains(searchtext) : false) ||
                (s.CampaignTitle != null ? s.CampaignTitle.ToLower().Contains(searchtext) : false) ||
                (s.CategoryName != null ? s.CategoryName.ToLower().Contains(searchtext) : false)

                ).ToList();





                ModelList.CampaignViewModelList = new List<CampaignMainViewModel>();
                ModelList.CampaignViewModelList.AddRange(result);
                var categoryList = Enum.GetValues(typeof(StoryCategory)).Cast<StoryCategory>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                ModelList.SelectedOptionsList = categoryList;
                return View("AllCampaigns", ModelList);
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
                //Model.CategoryName = Category;
                return View(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            return PartialView("_Comments", Model);
        }
        private Uri RediredtUri
        {
            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }




        [AllowAnonymous]

        public ActionResult Facebook()

        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new

            {



                client_id = "894504637650396",

                client_secret = "c3896f7433ee29c50272e56374be7831",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"



            });

            return Redirect(loginUrl.AbsoluteUri);

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
                var categoryList = Enum.GetValues(typeof(MoneyType)).Cast<MoneyType>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

               Model.CampaignDonations.selectedListMoney = categoryList;
                if (Model.CountryCode == "IN")
                {
                    Model.CampaignDonations.DonationMoneyType = "1";
                }
                else if ((Model.CountryCode == "NL"))
                {
                    Model.CampaignDonations.DonationMoneyType = "0";
                }
                return View(Model);
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



        public ActionResult FacebookCallback(string code)

        {

            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "894504637650396",

                client_secret = "c3896f7433ee29c50272e56374be7831",

                redirect_uri = RediredtUri.AbsoluteUri,

                code = code




            });

            var accessToken = result.access_token;

            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            string email = me.email;

            TempData["email"] = me.email;

            TempData["first_name"] = me.first_name;

            TempData["lastname"] = me.last_name;

            TempData["picture"] = me.picture.data.url;

            FormsAuthentication.SetAuthCookie(email, false);
            RegisterModel mod = new RegisterModel();
            mod.UserName = me.email;
            mod.DPName = me.first_name + me.last_name;
            UserModel usermodel = IService.ToregisterModel(mod);
            Locationmodel city = IService.GetUserCountryByIp();
            UserSession.UserCountry = city.CountryCode;
            UserSession.UserName = usermodel.UserName;
            UserSession.HasSession = true;
            UserSession.UserId = usermodel.Id;
            UserSession.LastLoginDate = usermodel.LastLoginTime != null ? usermodel.LastLoginTime.Value : DateTime.Now;
            if (usermodel.IsAdmin)
            {
                UserSession.UserRole = RolesEnum.Admin;
                UserSession.UserRoleId = RolesEnum.Admin.GetHashCode().ToString();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                UserSession.UserRole = RolesEnum.User;
                UserSession.UserRoleId = RolesEnum.User.GetHashCode().ToString();
                return RedirectToAction("Index", "User");
            }


        }




    }

}