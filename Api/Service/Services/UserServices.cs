using AutoMapper;
using Service.DTO;
using Service.DTO.User;
using Service.Interfaces;
using Data.Entities;
using Data.Interfaces;

namespace Service.Services
{
    /// <inheritdoc cref="IUser"/>
    public class UserServices : IUser
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;

        /// <summary>
        /// Constructor for <see cref="UserServices"/>
        /// </summary>
        /// <param name="userRepo"><see cref="IUserRepo"/></param>
        /// <param name="mapper"><see cref="IMapper"/></param>
        public UserServices(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<bool> IsUserExisted(string userName)
        {
            return await _userRepo.IsUserExistedAsync(userName);
        }

        public async Task<UserDetails> Login(LoginRegisterModel user)
        {
            return _mapper.Map<UserDetails>(await _userRepo.IsValidUserAsync(_mapper.Map<User>(user)));
        }

        public async Task<bool> Register(LoginRegisterModel newUser)
        {
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            return await _userRepo.CreateUserAsync(_mapper.Map<User>(newUser));
        }
    }
}
