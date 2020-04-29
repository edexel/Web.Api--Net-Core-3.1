using System.ComponentModel.DataAnnotations;

namespace Web.Api.Object.Requests.User
{
    public class UserRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El {0} es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
