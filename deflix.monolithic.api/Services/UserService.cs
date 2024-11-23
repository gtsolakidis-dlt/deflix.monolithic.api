using deflix.monolithic.api.DTOs;
using deflix.monolithic.api.Helpers;
using deflix.monolithic.api.Interfaces;
using deflix.monolithic.api.Models;

namespace deflix.monolithic.api.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseService _databaseService;

        #region SqlQueries
        private static string UserExistsQuery => @"SELECT COUNT(*) FROM [Users] WHERE [Username] = @Username OR [Email] = @Email";

        private static string GetUserQuery => @"SELECT * FROM [Users] WHERE [Username] = @Username AND [Password] = @Password";

        private static string GetUserByIdQuery => @"SELECT * FROM [Users] WHERE [UserId] = @UserId";

        private static string InsertQuery => @"INSERT INTO [Users]
                                                ([UserId],[Username],[Password],[Email])
                                                VALUES
                                                (@UserId,@Username,@Password,@Email)
                                                ";

        private static string UpdateQuery => @"UPDATE [Users]
                                                SET [Password] = @Password
                                                WHERE [UserId] = @UserId
                                                ";

        #endregion

        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public Guid Register(UserRegisterDto userDto)
        {
            var existingUserCount = _databaseService.QueryFirst<int>(UserExistsQuery, new { userDto.Username, userDto.Email });

            if (existingUserCount > 0)
            {
                return Guid.Empty;
            }

            var param = new UserModel
            {
                UserId = Guid.NewGuid(),
                Username = userDto.Username,
                Password = userDto.Password,
                Email = userDto.Email
            };

            _databaseService.Execute(InsertQuery, param);

            var subParam = new UserSubscriptionModel
            {
                UserId = param.UserId,
                SubscriptionCode = 1,
                ExpirationDate = DateTime.Now.Date.AddMonths(1),
                PaymentMethod = "None"
            };

            _databaseService.Execute(SubscriptionsService.InsertOrUpdateUserSubscriptionQuery, subParam);

            return param.UserId;
        }

        public UserDto Authenticate(string username, string password)
        {
            var user = _databaseService.QueryFirst<UserModel>(GetUserQuery, new { Username = username, Password = password });
            if (user != null)
            {
                return new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email
                };
            }
            return null;
        }

        public UserExtDto GetUserProfile(Guid userId)
        {
            var user = _databaseService.QueryFirst<UserModel>(GetUserByIdQuery, new { userId });
            var userSubscription = _databaseService.QueryFirst<UserSubscriptionModel>(SubscriptionsService.GetUserSubscriptionQuery, new { userId });
            return new UserExtDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                SubscriptionType = userSubscription.SubscriptionCode.ToSubscriptionType(),
                ExpirationDate = userSubscription.ExpirationDate,
                PaymentMethod = userSubscription.PaymentMethod
            };
        }

        public bool UpdateUserProfile(Guid userId, UserProfileUpdateDto profileUpdateDto)
        {
            var res = _databaseService.Execute(UpdateQuery, new { UserId = userId, profileUpdateDto.Password });
            return res > 0;
        }

    }

}
