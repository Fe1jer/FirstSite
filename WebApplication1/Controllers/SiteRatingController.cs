using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Models;
using WebApplication1.Data.Specifications;

namespace WebApplication1.Controllers
{
    public class SiteRatingController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISiteRatingRepository _siteRatingRepository;
        public SiteRatingController(IUserRepository IUserRepository, ISiteRatingRepository ISiteRatingRepository)
        {
            _userRepository = IUserRepository;
            _siteRatingRepository = ISiteRatingRepository;
        }

        public async Task<double> Index()
        {
            return await _siteRatingRepository.OverallSiteRating();
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(int rating)
        {
            User user = await _userRepository.GetUserAsync(User.Identity.Name);
            var siteRatings = await _siteRatingRepository.GetAllAsync(new SiteRatingSpecification().IncludeUser());
            SiteRating checkSiteRating = siteRatings.FirstOrDefault(p => p.User.Email == user.Email);
            if (checkSiteRating == null)
            {
                SiteRating siteRating = new SiteRating()
                {
                    Rating = rating,
                    User = user
                };
                await _siteRatingRepository.AddAsync(siteRating);
            }
            else
            {
                checkSiteRating.Rating = rating;
                await _siteRatingRepository.UpdateAsync(checkSiteRating);
            }
            return new EmptyResult();
        }
    }
}
