using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MallAPI.DTO.Requset
{
    public class UserParam
    {
        public string Name { get; set; }

        /// <summary>
        /// 电话，作为登录账号
        /// </summary>
        [Required]
        [RegularExpression("0?(13|14|15|17|18|19)[0-9]{9}",ErrorMessage ="请输入正确的手机号码")]
        public string Tel { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(16,MinimumLength =8,ErrorMessage ="请输入8~16位长的密码")]
        public string Password { get; set; }
    }
}
