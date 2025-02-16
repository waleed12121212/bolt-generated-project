using Bazingo_Core.Entities.Identity;
using Bazingo_Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazingo_Core.Entities.Product
{
    public class ProductReviewEntity : BaseEntity
    {
        public int Id { get; set; } // معرف المراجعة
        public int ProductId { get; set; } // معرف المنتج المرتبط
        public string UserId { get; set; } // معرف المستخدم الذي كتب المراجعة
        public string Title { get; set; } // عنوان المراجعة
        public string Comment { get; set; } // نص المراجعة
        public DateTime ReviewDate { get; set; } // تاريخ المراجعة
        public int Rating { get; set; } // تقييم المنتج (مثلاً من 1 إلى 5)
        public bool IsVerifiedPurchase { get; set; } 
        public int Helpful { get; set; }

        // Navigation properties
        public virtual ProductEntity Product { get; set; } // الكائن Product المرتبط
        public virtual ApplicationUser User { get; set; } // الكائن User المرتبط

        public ProductReviewEntity()
        {
            ReviewDate = DateTime.UtcNow;
            IsVerifiedPurchase = false;
            Helpful = 0;
        }
    }
}
