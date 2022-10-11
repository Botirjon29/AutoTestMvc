using AutoTest.WebMvc.Models;
using AutoTest.WebMvc.Repositories;

namespace AutoTest.WebMvc.Services;

public class UsersServices
{
	private readonly UsersRepository _usersRepository;
	private readonly CookiesServices _cookiesServices;

	public UsersServices()
	{
		_usersRepository = new UsersRepository();
		_cookiesServices = new CookiesServices();
	}

	public User? GetUserFromCookie(HttpContext context)
	{
        var userPhone = _cookiesServices.GetUserFromPhoneCookie(context);
        if (userPhone != null)
        {
            var user = _usersRepository.GetUserByPhone(userPhone);
            if (user.Phone == userPhone)
            {
                return user;
            }
        }
        return null;
    }
}
