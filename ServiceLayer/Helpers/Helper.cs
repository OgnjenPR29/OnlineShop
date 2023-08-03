using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.DataBase.ArticleDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Helpers
{
    public class Helper : IHelper
    {
        private IWorkingRepository workingRepo;

		string _key;

		private readonly string ArticleImageRelativePath = "../ArticleImages";

		readonly int sslPort = 44301;

		public Helper(IWorkingRepository workingRepository, IConfiguration configuration) {
            workingRepo = workingRepository;
			_key = configuration.GetSection("SecretKey").Value;
		}

		public IUser FindUserByUsername(string username) {
            if (workingRepo.AdminRepository.FindFirst(x => x.Username == username) is Admin admin) {
                return admin;
            }
            else if (workingRepo.SalesmanRepository.FindFirst(x => x.Username == username) is Salesman salesman) {
                return salesman;
            }
            else if (workingRepo.ShopperRepository.FindFirst(x => x.Username == username) is Shopper shopper) {
                return shopper;
            }
            return null;
        }
		public IUser FindUserByEmail(string email) {
            if (workingRepo.AdminRepository.FindFirst(x => x.Email == email) is Admin admin) {
                return admin;
            }
            else if (workingRepo.SalesmanRepository.FindFirst(x => x.Email == email) is Salesman salesman) {
                return salesman;
            }
            else if (workingRepo.ShopperRepository.FindFirst(x => x.Email == email) is Shopper shopper) {
                return shopper;
            }
            return null;
        }

		public IUser FindUserByJwt(string token)
        {
			long id = int.Parse(GetClaimValueFromToken(token, "id"));
			string role = (GetClaimValueFromToken(token, "role"));
			IUser user = FindByIdAndRole(id, role);

			return user;
		}

		public IUser FindByIdAndRole(long id, string role)
		{
			if (role == UserType.Admin.ToString() && workingRepo.AdminRepository.FindFirst(x => x.Id == id) is Admin admin)
			{
				return admin;
			}
			else if (role == UserType.Shopper.ToString() && workingRepo.ShopperRepository.FindFirst(x => x.Id == id) is Shopper customer)
			{
				return customer;

			}
			else if (role == UserType.Salesman.ToString() && workingRepo.SalesmanRepository.FindFirst(x => x.Id == id) is Salesman seller)
			{
				return seller;
			}

			return null;
		}

		public string GetUsernameFromToken(string tokenString)
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.ReadJwtToken(tokenString);
			string username = token.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

			return username;
		}

		public string GetClaimValueFromToken(string tokenString, string claimType)
		{
			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = handler.ReadJwtToken(tokenString);
			string claimValue = token.Claims.Where(x => x.Type == claimType).FirstOrDefault().Value;

			return claimValue;
		}

		public bool IsPassOK(string pass, string passCheck) {
            return BCrypt.Net.BCrypt.Verify(pass, passCheck);
        }

		public bool IsPassWeak(string password)
		{
			password = password.Trim();

			if (password.Length < 7)
			{
				return true;
			}

			return false;
		}

		public string IssueJwt(IEnumerable<Claim> claims)
		{
			SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

			var tokenOptions = new JwtSecurityToken(
				issuer: $"http://localhost:{sslPort}",
				claims: claims,
				expires: DateTime.Now.AddMinutes(360),
				signingCredentials: signinCredentials
			);

			JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
			string token = handler.WriteToken(tokenOptions);

			return token;
		}

		private string IssueAdminJwt(IAdmin admin)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, UserType.Admin.ToString()),
				new Claim("role", UserType.Admin.ToString()),
				new Claim("id", admin.Id.ToString()),
				new Claim("username", admin.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		private string IssueCostumerJwt(IShopper customer)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, UserType.Shopper.ToString()),
				new Claim("role", UserType.Shopper.ToString()),
				new Claim("id", customer.Id.ToString()),
				new Claim("username", customer.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		private string IssueSellerJwt(ISalesman seller)
		{
			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Role, UserType.Salesman.ToString()),
				new Claim("role", UserType.Salesman.ToString()),
				new Claim("id", seller.Id.ToString()),
				new Claim("username", seller.Username)
			};

			string token = IssueJwt(claims);

			return token;
		}

		public string IssueUserJwt(IUser user)
		{
			string token = null;

			if (user.GetType().Equals(typeof(Admin)))
			{
				token = IssueAdminJwt((Admin)user);
			}
			else if (user.GetType().Equals(typeof(Shopper)))
			{
				token = IssueCostumerJwt((Shopper)user);
			}
			else if (user.GetType().Equals(typeof(Salesman)))
			{
				token = IssueSellerJwt((Salesman)user);
			}

			return token;
		}

		public List<ArticleDetailDto> ReturnArticlesDetail(List<IArticle> articles)
		{
			List<ArticleDetailDto> articleDtoList = new List<ArticleDetailDto>();

			foreach (IArticle article in articles)
			{
				byte[] productImage = GetArticleProductImage(article);
				articleDtoList.Add(new ArticleDetailDto(article.Id, article.Name, article.Description, article.Quantity, article.Price, productImage));
			}

			return articleDtoList;
		}

		public byte[] GetArticleProductImage(IArticle article)	
		{
			string productImageName = article.Image;
			if (productImageName == null)
			{
				return null;
			}

			string productImagePath = Path.Combine(Directory.GetCurrentDirectory(), ArticleImageRelativePath, productImageName);

			if (!File.Exists(productImagePath))
			{
				return null;
			}

			byte[] image = File.ReadAllBytes(productImagePath);

			return image;
		}

	}
}
