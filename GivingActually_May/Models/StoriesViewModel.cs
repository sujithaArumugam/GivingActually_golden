using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Web.Mvc;
using static GivingActually_May.Models.HelperModels.Helper;

namespace GivingActually_May.Models
{
    public class StoriesListViewModel
    {
        public StoriesListViewModel()
        {
            StoriesViewModel = new List<StoriesViewModel>();
            SelectedOptionsList = new List<SelectListItem>();
        }
        public List<StoriesViewModel> StoriesViewModel { get; set; }
        public int[] SelectedOptions { set; get; }
        public int SelectedOption { set; get; }
        public List<SelectListItem> SelectedOptionsList { get; set; }
        [Display(Name = "Camapign Category")]
        public StoryCategory CategoryType { get; set; }
    }
    public class CampaignsListViewModel
    {
        public CampaignsListViewModel()
        {
            CampaignViewModelList = new List<CampaignMainViewModel>();
            SelectedOptionsList = new List<SelectListItem>();
        }
        public List<CampaignMainViewModel> CampaignViewModelList { get; set; }
        public int[] SelectedOptions { set; get; }
        public int SelectedOption { set; get; }
        public List<SelectListItem> SelectedOptionsList { get; set; }
        [Display(Name = "Camapign Category")]
        public StoryCategory CategoryType { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
    }

    //public class StoriesViewModel
    //{
    //    public StoriesViewModel()
    //    {
    //        Files = new List<Files>();
    //        Beneficiary = new BeneficiaryViewModel();
    //    }

    //    public int Id { get; set; }
    //    public int UserId { get; set; }
    //    public bool IsApprovedbyAdmin { get; set; }
    //    [Display(Name = "Campaign Description")]
    //    [Required]
    //    public string Story { get; set; }
    //    [Display(Name = "Campaign Title")]
    //    [Required]
    //    public string StoryTitle { get; set; }
    //    [Display(Name = "What is your Campaign Category?")]
    //    [Required]
    //    public StoryCategory StoryCategory { get; set; }
    //    [Display(Name = "Campaign Story")]
    //    [Required]
    //    public string CampaignStory { get; set; }

    //    public string CategoryName { get; set; }

    //    public bool Status { get; set; }
    //    public string CreatedBy { get; set; }
    //    public Nullable<System.DateTime> CreatedOn { get; set; }
    //    public string UpdatedBy { get; set; }
    //    public Nullable<System.DateTime> UpdatedOn { get; set; }
    //    public List<Files> Files { get; set; }
    //    //[Display(Name = "Residing in")]
    //    //[Required]
    //    //public string Location { get; set; }
    //    [Required]
    //    public string Email { get; set; }
    //    //[Display(Name = "Beneficiary Name")]
    //    //[Required]
    //    //public string BeneficiaryDetails{ get; set; }

    //    public BeneficiaryViewModel Beneficiary { get; set; }

    //    public CampaignTargetModel CampaignTarget { get; set; }
    //    //[Display(Name = "BankAccount Number")]
    //    //[Required]
    //    //public string Bank { get; set; }


    //    //public List<byte[]> File { get; set; }
    //    //public List<string> FileName { get; set; }
    //    //public byte[] File2 { get; set; }
    //    //public string File2Name { get; set; }
    //    //public byte[] File3 { get; set; }
    //    //public string File3Name { get; set; }
    //    //public byte[] File4 { get; set; }
    //    //public string File4Name { get; set; }
    //    //public byte[] File5 { get; set; }
    //    //public string File5Name { get; set; }
    //}
    public class BeneficiaryViewModel
    {
        public BeneficiaryViewModel()
        {
            this.AvailableCountries = new List<SelectListItem>();
            this.States = new List<SelectListItem>();
            this.Cities = new List<SelectListItem>();
        }
        public int Id { get; set; }   // this is only used to retrieve record from Db
        [Display(Name = "Beneficiary Type")]
        [Required]
        public BeneficiaryType BeneficiaryType { get; set; }
        [Required]
        [Display(Name = "Beneficiary/Representative Name")]
        public string BName { get; set; }


        [Display(Name = "Age")]
        [Required]
        public int BAge { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public int BGender { get; set; }

        [Display(Name = "Phone")]
        [Required]
        public string BPhone { get; set; }

        [Display(Name = "Residing In/Based out of ")]
        [Required]
        public string BResidence { get; set; }

        [Display(Name = " Campaign Display Picture")]
        [Required]
        public byte[] BDisplayPic { get; set; }

        [Display(Name = "DP Name")]
        public string BDisplayPicName { get; set; }

        [Display(Name = "Group Name")]
        public string BGroupName { get; set; }

        [Display(Name = "Related to Me as")]
        [Required]
        public string Brelationship { get; set; }

        [Display(Name = "Group Members")]
        [Required]
        public int BMembers { get; set; }

        [Display(Name = "IFSC Code")]
        [Required]
        public string IFSCcode { get; set; }
        [Display(Name = "Bank Name")]
        [Required]
        public string BankName { get; set; }
        [Display(Name = "Bank Location")]
        [Required]
        public string BankLocation { get; set; }
        [Display(Name = "Bank Account Number")]
        [Required]
        public string AccountNo { get; set; }
        [Display(Name = "Name as per bank account")]
        [Required]
        public string BankUserName { get; set; }

        public List<SelectListItem> States { get; set; }
        public List<SelectListItem> Cities { get; set; }

        [Display(Name = "Residing In/Based out of ")]
        [Required]
        public int CountryId { get; set; }
        public List<SelectListItem> AvailableCountries { get; set; }
        public int StateId { get; set; }
        public int CityId
        {
            get; set;
        }
        public int storyId { get; set; }

    }

    public class CampainOrganizerViewModel
    {
        public CampainOrganizerViewModel()
        {
            //this.AvailableCountries = new List<SelectListItem>();
        }
        public int Id { get; set; }   // this is only used to retrieve record from Db
        public int storyId { get; set; }

        public int CountryId { get; set; }
      //  public List<SelectListItem> AvailableCountries { get; set; }

        
        public string BPinCode { get; set; }
        [Display(Name = "Residing In/Based out of ")]
        [Required]
        public string placeNmae { get; set; }
        public string Latitude { get; set;}
        public string longitude { get; set; }
        public int Bresidence { get; set; }
        public int StateId { get; set; }
        public int CityId
        {
            get; set;
        }

        [Display(Name = " Campaign Display Picture")]
        [Required]
        public byte[] BDisplayPic { get; set; }

        [Display(Name = "DP Name")]
        public string BDisplayPicName { get; set; }
    }
    [Serializable()]
    public class StoryAccountsDetails
    {
        public StoryAccountsDetails()
        {
            Debits = new List<DebitDetails>();
        }

        public int StoryID { get; set; }
        public double AvailableCredit { get; set; }

        public List<DebitDetails> Debits { get; set; }

    }

    public class DebitDetails
    {
        public double Expense { get; set; }
        public string ExpenseDescription { get; set; }
    }

    public class Files
    {
        public int AttId { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int updtId { get; set; }
        public int Index { get; set; }
    }

    public class CampaignTargetModel
    {
        public int Id { get; set; }   // this is only used to retrieve record from Db

        public int storyId { get; set; }

        [Display(Name = "Campaign Target Amount")]
        public decimal Amount { get; set; }
        public MoneyType MoneyTypes { get; set; }
        public string MoneyType { get; set; }
        [Required]
        [Display(Name = "Campaign Target Date")]
        public DateTime TargetDate { get; set; }

        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }

        [Display(Name = "Hospital Location")]
        public string HospitalLocation { get; set; }

        [Display(Name = "Ailment")]
        public string Reason { get; set; }
    }
    public class StorieListsforViewModel
    {
        public StorieListsforViewModel()
        {
            NewStoriesViewModel = new StoriesListViewModel();
            PendingStoriesViewModel = new StoriesListViewModel();
            FraudStoriesViewModel = new StoriesListViewModel();
            SelectedOptionsList = new List<SelectListItem>();
        }

        public StoriesListViewModel NewStoriesViewModel { get; set; }
        public StoriesListViewModel PendingStoriesViewModel { get; set; }
        public StoriesListViewModel FraudStoriesViewModel { get; set; }
        public int[] SelectedOptions { set; get; }
        [Display(Name = "Camapign Category")]
        public StoryCategory CategoryType { get; set; }
        public List<SelectListItem> SelectedOptionsList { get; set; }
    }
    public class CampainListsforIndexViewModel
    {
        public CampainListsforIndexViewModel()
        {
            NewStoriesViewModel = new CampaignsListViewModel();
            PendingStoriesViewModel = new CampaignsListViewModel();
            FraudStoriesViewModel = new CampaignsListViewModel();
            SelectedOptionsList = new List<SelectListItem>();
        }

        public CampaignsListViewModel NewStoriesViewModel { get; set; }
        public CampaignsListViewModel PendingStoriesViewModel { get; set; }
        public CampaignsListViewModel FraudStoriesViewModel { get; set; }
        public int[] SelectedOptions { set; get; }
        [Display(Name = "Camapign Category")]
        public StoryCategory CategoryType { get; set; }
        public List<SelectListItem> SelectedOptionsList { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class StoriesViewModel
    {
        public StoriesViewModel()
        {
            Beneficiary = new BeneficiaryViewModel();
            Files = new List<Files>();
            Comments = new List<CommentsVM>();
        }
        public int Id { get; set; }
        [Display(Name = "Category?")]
        [Required]
        public StoryCategory StoryCategory { get; set; }

        public bool IsApprovedbyAdmin { get; set; }

        [Display(Name = "Campaign purpose")]
        [Required]
        public string Story { get; set; }

        [Display(Name = "Display Title")]
        [Required]
        public string StoryTitle { get; set; }

        [Required]
        public string StoryDescription { get; set; }
        public int UserId { get; set; }

        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public List<Files> Files { get; set; }
        [Required]
        public string Email { get; set; }
        public string CommentText { get; set; }
        public BeneficiaryViewModel Beneficiary { get; set; }

        public CampaignTargetModel CampaignTarget { get; set; }

        public List<CommentsVM> Comments { get; set; }
        public PostCommentsVM postcomments { get; set; }
        public bool isLiked;
        public long CommentCount { get; set; }
        public long LikeCount { get; set; }
    }
    public class CampaignDescription
    {
        public int Id { get; set; }
        [AllowHtml]
        [Required]
        public string StoryDescription { get; set; }
        public string StripedDescription { get; set; }
        public List<Files> Files { get; set; }
        public int StoryId { get; set; }
    }
    public class CampaignMainViewModel
    {
        public CampaignMainViewModel()
        {
            Beneficiary = new BeneficiaryViewModel();
            campaignDescription = new CampaignDescription();
            campaignupdate = new CampaignDescription();
            Updates = new List<CampaignUpdates>();
            Files = new List<Files>();
            Comments = new List<CommentsVM>();
            CampaignDonations = new CampaignDonation();
            CampaignDonationList = new List<CampaignDonation>();
        }
        public int Id { get; set; }
        [Display(Name = "Category?")]
        [Required]
        public StoryCategory StoryCategory { get; set; }
        public CampainOrganizerViewModel CampainOrganizer { get; set; }
        public bool IsApprovedbyAdmin { get; set; }
        public CampaignDonation CampaignDonations { get; set; }
        public List<CampaignDonation> CampaignDonationList { get; set; }
        [Display(Name = "Campaign Title")]
        [Required]
        public string CampaignTitle { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CommentText { get; set; }
        public int UserId { get; set; }

        public string CategoryName { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public List<Files> Files { get; set; }

        [Display(Name = "Group Name")]
        public string BGroupName { get; set; }

        [Display(Name = "Beneficiary Name")]
        public string BName { get; set; }

        [Display(Name = "Name of NGO")]
        public string NGOName { get; set; }

        [Display(Name = "Campaign For")]
        [Required]
        public BeneficiaryType BeneficiaryType { get; set; }

        [Required]
        [Display(Name = "Campaign Goal Amount")]
        public decimal CampaignTargetMoney { get; set; }

        [Required]
        public string CampaignTargetMoneyType { get; set; }
        public BeneficiaryViewModel Beneficiary { get; set; }

        public CampaignTargetModel CampaignTarget { get; set; }
        public CampaignDescription campaignDescription { get; set; }
        public CampaignDescription campaignupdate { get; set; }
        public List<CampaignUpdates> Updates { get; set; }
        public List<CommentsVM> Comments { get; set; }
        public PostCommentsVM postcomments { get; set; }
        public bool isLiked;
        public long CommentCount { get; set; }
        public long LikeCount { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public decimal RaisedAmount { get; set; }
        public long RaisedBy { get; set; }
        public int RaisedPercentage { get; set; }
        public bool loggedinUser { get; set; }
    }
    public class CampaignUpdates
    {
        public CampaignUpdates()
        {           
            UpdateDescription =new CampaignDescription();
            Files = new List<Files>();
        }
        public CampaignDescription UpdateDescription = new CampaignDescription();
        public DateTime updatedOn { get; set; }
        public List<Files> Files { get; set; }
    }
    public class PostCommentsVM
    {
        public string CommentText;
        public int Id;

    }
    public class imagesviewmodel
    {
        public string Url { get; set; }
    }
    public class Locationmodel
    {
        public string IPAddress { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string TimeZone { get; set; }
    }
    public class MiniLocationmodel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string DISTANCE { get; set; }
    }
    public class MiniDonationModel
    {
        public int DonateId { get; set; }
        public decimal Amount { get; set; }
        public string MType { get; set; }
        public string orderId { get; set; }
        public string Uname { get; set; }
        public string Email { get; set; }
        public string PhNo {get;set;}
    }
    public class PostLikes
    {
        public bool isLiked;
        public int Id;

    }
    public class CommentsVM
    {
        public CommentsVM()
        {
            SubComments = new List<SubCommentsVM>();
        }
        public int ComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public int campaignId { get; set; }
        public UserModel Users { get; set; }
        public List<SubCommentsVM> SubComments { get; set; }
    }
    public class SubCommentsVM
    {
        public int SubComID { get; set; }
        public string CommentMsg { get; set; }
        public DateTime CommentedDate { get; set; }
        public CommentsVM Comment { get; set; }
        public UserModel User { get; set; }
    }

    public class LikesVM
    {
        public int LikesID { get; set; }
        public DateTime likedDate { get; set; }
        public int campaignId { get; set; }
        public UserModel Users { get; set; }
    }
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    public class State
    {
        public int StateID { get; set; }
        public string StateName { get; set; }
        public int CountryID { get; set; }
    }
    public class CampaignDonation
    {
        public CampaignDonation()
        {
            this.selectedListMoney = new List<SelectListItem>();
        }
        public int id { get; set; }
        public List<SelectListItem> selectedListMoney { get; set; }
        public int StoryId { get; set; }
        [Required]
        public decimal DonationAmnt { get; set; }
        [Display(Name = "Campaign Country")]
    
        public int countryId { get; set; }
      
        [Display(Name = "PinCode/Postal Code")]
        public string pincode { get; set; }
        [Required]
        public string EMail { get; set; }
        public string DonationMoneyType { get; set; }
        [Display(Name = "Name")]
        public string PlaceName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IdentityName { get; set; }
        [Display(Name = "Make your Donation as Ananymous")]
        public bool isAnanymous { get; set; }
        public string Status { get; set; }
        public string DonatedBy { get; set; }
        public DateTime DonatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        [Display(Name = "Phone")]
        [Required]
        public string PhNo { get; set; }
        
    }
    public class CampaignDonationList
    {
        public List<CampaignDonation> campaignDonations { get; set; }
        public decimal TotalAmount { get; set; }
        public long Donors { get; set; }
        public int StoryId { get; set; }
    }
}
public static class HtmlExtensions
{
    public static MvcHtmlString GetImage(this HtmlHelper html, byte[] image)
    {
        var img = String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(image));
        return new MvcHtmlString("<img src='" + img + "' />");
    }






}