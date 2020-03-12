using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hexace.Data.Objects;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hexace.Models
{
    public class ProfileModel
    {
        public class GotenAchievements
        {
            public string AchievementName { get; set; }
            public string AchievementDescription { get; set; }
            public bool IsAchieved { get; set; }
            public DateTime AchievementDate { get; set; }

            public GotenAchievements(Achievement achievement, UserAchievement userAchievement)
            {
                AchievementName = achievement.AchievementName;
                AchievementDescription = achievement.AchievementDescription;
                if (userAchievement != null)
                {
                    IsAchieved = true;
                    AchievementDate = userAchievement.AchievementGotDatetime;
                }
                else
                {
                    IsAchieved = false;
                }
                
            }
        }

        [Required(ErrorMessage = "Nickname not specified")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Email not specified")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password not specified")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string PasswordConfirmation { get; set; }

        public User User { get; set; }
        public Profile Profile { get; set; }
        public List<GotenAchievements> UserAchievements { get; set; }

        public ProfileModel(User user, Profile profile, List<Achievement> allAchievements, List<UserAchievement> userAchievements)
        {
            this.UserAchievements = new List<GotenAchievements>();
            this.User = user;
            this.Profile = profile;
            foreach (var item in userAchievements)
            {
                UserAchievements.Add(new GotenAchievements(allAchievements.Where(x=>x.Id==item.AchievementId).FirstOrDefault(), item));
            }

            foreach (var item in allAchievements)
            {
                bool flag = true;
                foreach (var userAchievement in UserAchievements)
                {
                    if (item.AchievementName == userAchievement.AchievementName)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    UserAchievements.Add(new GotenAchievements(item, null));
                }
            }
        }
    }
}