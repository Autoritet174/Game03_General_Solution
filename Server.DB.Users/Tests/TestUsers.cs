using Server.DB.Users.Entities;
using Server.DB.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DB.Users.Tests;
public class TestUsers
{
    private readonly UserRepository _userRepository;

    /// <summary>
    /// Конструктор с внедрением зависимостей.
    /// </summary>
    public TestUsers(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }
}
