namespace IdentityService.Requests
{
    public class CreateUserRq
    {
        /// <summary>
        /// E-mail пользователя
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}
