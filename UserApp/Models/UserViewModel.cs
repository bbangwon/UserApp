using System.ComponentModel.DataAnnotations;

namespace UserApp.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "아이디")]
        [Required(ErrorMessage = "아이디를 입력하세요")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "아이디는 3자 이상 25자 이하로 입력하세요.")]
        public string UserId { get; set; } = string.Empty;

        [Display(Name = "암호")]
        [Required(ErrorMessage = "암호를 입력하세요")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "비밀번호는 6자 이상 20자 이하로 입력하세요.")]
        public string Password { get; set; } = string.Empty;
    }
}
