using System.Collections.Generic;
namespace FootballApp
{
    public class Errors
    {
        static Dictionary<int, string> errors = new Dictionary<int, string>()
        {
            { 0, "Каждое поле должно быть заполнено"},
            { 1, "Пароли не совпадают. Повторите попытку"},
            { 2, "Данный пользователь не зарегистрирован. Повторите попытку"},
            { 3, "Неправильный пароль. Повторите попытку"},
            { 4, "Проверьте подключение к Интернету"}
        };

        public static string GetErrorById(int id)
        {
            return errors[id];
        }
    }
}
