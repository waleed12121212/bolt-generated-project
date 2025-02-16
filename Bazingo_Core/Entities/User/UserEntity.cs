using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Core.Entities.User
{
    public class UserEntity
    {
        public string Id { get; set; } // معرف المستخدم
        public string UserName { get; set; } // اسم المستخدم
        public string Email { get; set; } // البريد الإلكتروني
        public string PasswordHash { get; set; } // هاش كلمة المرور
        public DateTime CreatedAt { get; set; } // تاريخ إنشاء المستخدم
        public DateTime UpdatedAt { get; set; } // تاريخ آخر تحديث للمستخدم
    }
}
