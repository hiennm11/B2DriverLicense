using B2DriverLicense.Core.Dtos;
using System.Threading.Tasks;

namespace B2DriverLicense.Service
{
    public interface IUserService
    {
        Task<string> Authenticate(UserLoginDto user);
        Task<bool> Register(UserRegisterDto user);
    }
}