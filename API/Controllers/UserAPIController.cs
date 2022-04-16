using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Core;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserAPIController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("get")]
        public ActionResult<User> Get(int id)
        {
            return Ok(_unitOfWork.UserRepository.FindByCondition(user => user.Id == id));
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Post(User user)
        {
            var hashPassword = HashPassword(user.Password);
            user.Password = hashPassword;
            try
            {
                _unitOfWork.CreateTransaction();
                await _unitOfWork.UserRepository.CreateAsync(user);
                _unitOfWork.Commit();
                _unitOfWork.Save();
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest("Something went wrong");
            }

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult<string> Login(string username, string password)
        {
            var user = _unitOfWork.UserRepository.FindByCondition(user => user.UserName == username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("User is not found");
            }

            if (!VerifyHashedPassword(user.Password, password))
            {
                return BadRequest("Wrong password");
            }

            var token = CreateToken(user);

            return Ok(token);
        }

        [HttpDelete]
        [Route("delete")]
        public ActionResult Delete(int id)
        {
            var user = _unitOfWork.UserRepository.FindByCondition(user => user.Id == id).FirstOrDefault();

            try
            {
                _unitOfWork.CreateTransaction();
                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.Commit();
                _unitOfWork.Save();
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest("User is not found");
            }

            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public ActionResult Update(int id)
        {
            var user = _unitOfWork.UserRepository.FindByCondition(user => user.Id == id).FirstOrDefault();
            
            try
            {
                _unitOfWork.CreateTransaction();
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Commit();
                _unitOfWork.Save();
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest("User is not found");
            }

            return Ok();
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }

            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        
        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;

            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }

            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }

            return ByteArraysEqual(buffer3, buffer4);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Secret").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims : claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        
        private bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2)
            {
                return true;
            }

            if (b1 == null || b2 == null)
            {
                return false;
            }

            if (b1.Length != b2.Length)
            {
                return false;
            }

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
