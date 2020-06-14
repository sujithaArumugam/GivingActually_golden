using GivingActually_May.Models;
using GivingActually_May.Models.HelperModels;
using GivingActually_May.Service;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using static GivingActually_May.Models.HelperModels.Helper;
using Authorization = GivingActually_May.Models.HelperModels.Authorization;
using ViewType = GivingActually_May.Models.HelperModels.Helper.ViewType;

namespace GivingActually.Controllers
{
    [Authorization]
    [AdminAuthorization]
    [OutputCache(Duration = 15, VaryByParam = "None")]
    public class AdminController : Controller
    {

        CommonService IService = new CommonService();
        public ActionResult Index(string Category = "")
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                CampainListsforIndexViewModel StoryList = new CampainListsforIndexViewModel();


                var res = IService.GetCampaignsForIndex(CategoryId);

                StoryList.NewStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.UpdatedOn >= UserSession.LastLoginDate).ToList();
                var nextsetres = res.CampaignViewModelList.Except(StoryList.NewStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.PendingStoriesViewModel.CampaignViewModelList = nextsetres.Where(a => a.IsApprovedbyAdmin != true).ToList();
                var nexttsetres = nextsetres.Except(StoryList.PendingStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.FraudStoriesViewModel.CampaignViewModelList = nexttsetres.Where(a => a.IsApprovedbyAdmin != true).ToList(); //to work more
                StoryList.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                return View(StoryList);
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
        public ActionResult FraudCampaigns(string Category = "")
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                CampainListsforIndexViewModel StoryList = new CampainListsforIndexViewModel();


                var res = IService.GetCampaigns(CategoryId);

                StoryList.NewStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.UpdatedOn >= UserSession.LastLoginDate).ToList();
                var nextsetres = res.CampaignViewModelList.Except(StoryList.NewStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.PendingStoriesViewModel.CampaignViewModelList = nextsetres.Where(a => a.IsApprovedbyAdmin != true).ToList();
                var nexttsetres = nextsetres.Except(StoryList.PendingStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.FraudStoriesViewModel.CampaignViewModelList = nexttsetres.Where(a => a.IsApprovedbyAdmin != true).ToList(); //to work more
                StoryList.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                return View(StoryList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult PendingCampaigns(string Category = "")
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                CampainListsforIndexViewModel StoryList = new CampainListsforIndexViewModel();


                var res = IService.GetCampaigns(CategoryId);

                StoryList.NewStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.UpdatedOn >= UserSession.LastLoginDate).ToList();
                var nextsetres = res.CampaignViewModelList.Except(StoryList.NewStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.PendingStoriesViewModel.CampaignViewModelList = nextsetres.Where(a => a.IsApprovedbyAdmin != true).ToList();
                var nexttsetres = nextsetres.Except(StoryList.PendingStoriesViewModel.CampaignViewModelList).ToList();
                StoryList.FraudStoriesViewModel.CampaignViewModelList = nexttsetres.Where(a => a.IsApprovedbyAdmin != true).ToList(); //to work more
                StoryList.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                return View(StoryList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public ActionResult AllCampaigns(string Category = "", ViewType viewtype = 0)
        {
            try
            {
                int CategoryId = !string.IsNullOrEmpty(Category) ? Convert.ToInt16(Category) : -1;
                CampainListsforIndexViewModel StoryList = new CampainListsforIndexViewModel();


                var res = IService.GetCampaigns(CategoryId);
                res.CategoryType = (StoryCategory)Enum.Parse(typeof(StoryCategory), Enum.GetName(typeof(StoryCategory), Category != "" ? Convert.ToInt32(Category) : -1), true);

                if (viewtype == ViewType.New)
                {
                    StoryList.NewStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.CreatedOn >= UserSession.LastLoginDate).ToList();
                    return View(StoryList.NewStoriesViewModel);
                }
                if (viewtype == ViewType.Pending)
                {
                    StoryList.PendingStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.IsApprovedbyAdmin != true).ToList();
                    return View(StoryList.PendingStoriesViewModel);
                }

                if (viewtype == ViewType.Fraud)
                {
                    StoryList.FraudStoriesViewModel.CampaignViewModelList = res.CampaignViewModelList.Where(a => a.IsApprovedbyAdmin != true).ToList(); //to work more
                    return View(StoryList.FraudStoriesViewModel);
                }
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

        public ActionResult ApproveStory(int Id, bool Approve, string from = "")
        {
            try
            {
                if (Id > 0)
                {
                    var res = IService.ApproveCampaign(Id, Approve);
                }
                if (from == "campaign")
                { return RedirectToAction("AllCampaigns"); }

                else if (from == "detail") { return RedirectToAction("CampaignView"); }
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UserDetails()
        {

            try
            {
                UserDetails Model = new UserDetails();
                Model = IService.GetUserDetails();
                return View(Model);
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
    }
}