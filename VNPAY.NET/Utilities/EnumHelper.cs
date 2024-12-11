using System.ComponentModel;
using System.Reflection;

namespace VNPAY.NET.Utilities
{
    /// <summary>
    /// Lớp hỗ trợ để lấy mô tả của các giá trị trong enum.  
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Lấy mô tả của giá trị enum thông qua thuộc tính Description nếu có.
        /// Nếu giá trị enum không có mô tả, phương thức này sẽ trả về tên của giá trị enum.
        /// </summary>
        /// <param name="value">Giá trị enum cần lấy mô tả.</param>
        /// <returns>Mô tả của giá trị enum nếu có, nếu không thì trả về tên giá trị enum.</returns>
        public static string GetDescription(Enum value)
        {
            // Lấy thông tin trường (field) của giá trị enum.
            FieldInfo field = value.GetType().GetField(value.ToString());

            // Tìm thuộc tính Description đã được gán cho trường enum.
            DescriptionAttribute attribute = (DescriptionAttribute)field.GetCustomAttribute(typeof(DescriptionAttribute));

            // Nếu có mô tả, trả về mô tả, nếu không trả về tên giá trị enum.
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }

}
