using Core;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;


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
        [Route("post")]
        public async Task<ActionResult> Post(User user)
        {
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
                return BadRequest("Unexpected error");
            }
            
            return Ok();
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
    }
}
