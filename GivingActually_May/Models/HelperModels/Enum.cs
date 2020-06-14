using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GivingActually_May.Models.HelperModels
{
    public static class Helper
    {

        public enum RolesEnum
        {
            Admin = 0,
            User = 1
        }

        public enum StoryCategory
        {
            All=-1,
            [Display(Name = "Medical & Illness")]
            Medical = 0,
            [Display(Name = "Agriculture")]
            Agriculture = 1,
            [Display(Name = "Animals")]
            Animals = 2,
            [Display(Name = "Annadhanam")]
            Annadhanam = 3,
            [Display(Name = "Charity")]
            Charity = 4,
            [Display(Name = "Education")]
            Education = 5,
            [Display(Name = "Elderly Care")]
            ElderlyCare = 6,
            [Display(Name = "Emergency")]
            Emergency = 7,
            [Display(Name = "Funeral")]
            Funeral = 8,
            [Display(Name = "Mental Health")]
            MentalHealth = 9,
            [Display(Name = "Nutrition")]
            Nutrition = 10,
            [Display(Name = "Spirituality")]
            Spirituality = 11,
            [Display(Name = "Sports")]
            Sports = 12,
            [Display(Name = "Volunteer")]
            Volunteer = 13,
            [Display(Name = "Wedding")]
            Wedding = 14,

        }
        public enum MoneyType
        {
            [Display(Name = "EUR")]
            EUR = 0,
           [Display(Name = "INR")]
            INR = 1
           
            
        }
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = System.Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }


        public enum BeneficiaryType
        {
            Select =-1,
            [Display(Name = "Myself")]
            Myself = 0,
            [Display(Name = "My Family - Individual")]
            FamilyIndividual = 1,
            [Display(Name = "My Family - Group")]
            FamilyGroup = 2,
            [Display(Name = "My Friends - Individual")]
            FriendIndividual = 3,
            [Display(Name = "My Friends - Group")]
            FriendGroup = 4,
            [Display(Name = "Others")]
            Others = 5,
         [Display(Name = "Registered NGO")]
            NGO = 6
        }

        public enum GenderType
        {
            Male = 1,
            Female = 2,
            Others=3
        }
        public enum ViewType
        {
            New = 1,
            Pending = 2,
            Fraud = 3
        }
    }
}

