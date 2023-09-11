using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoBaoCao.Models
{
    public class InPut
    {
    }
    public class Clients : PageCommon
    {
        // địa chỉ id
        public int ClientId { get; set; }
        // số hiệu tk
        public string ClientCode { get; set; }
        // Họ tên khách hàng
        public string ClientName { get; set; }
        // địa chỉ khách hàng.
        public string ClientAddress { get; set; }
        // số căn cước công dân 
        public string ClientIdNumber { get; set; }
        // nơi cấp
        public string CientIdLssuePlace { get; set; }
        // ngày cấp
        public DateTime ClientIDLssueDate { get; set; }
        // số điện thoại
        public string ClientSMSNumber { get; set; }
        // email
        public string ClientEmail { get; set; }
        // chi nhánh
        public string ClientBranch { get; set; }
    }

    public class Products
    {
        // Địa chỉ id
        public int ProductId { get; set;}
        // Tên sản phẩm
        public string ProductName { get; set; }
        // Ngày hiệu lực
        public DateTime ProductEffectiveDateFrom { get; set; }
        // ngày kết thúc
        public DateTime ProductEffectiveDateTo { get; set; }
        // ngày gửi tối thiểu
        public int ProductMinimumSendingDate { get; set;}
        // ngày gửi tối đa
        public int ProductMaximumSendingDate { get; set; }
        // hạn mức tối thiểu
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ProductPackageMinimumAmount { get; set; }
        // tổng hạn mức
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ProductPackageMaximumAmount { get; set; }
        // hạn mức còn lại
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ProductTotalRemainingLimit { get; set; }
        // lãi xuất không kì hạn
        public decimal ProductNonTermRate { get; set; }
    }


    public class Contracts
    {
        [Key]
        // Số thứ tự hợp đồng
        public int ContractId { get; set;}
        // Số hợp đồng
        public string ContractCode { get; set; }
        // Giá trị hợp đồng
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ContractValue { get; set; }
        // Số tiền đã rút 
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ContractAmountWithdrawn { get; set; }
        // Số tiền còn lại
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ContractRemainingAmount { get; set; }
        // ngày vay
        public DateTime ContractBorrowedDate { get; set; }
        // số ngày
        public int ContractNumberOfDays { get; set; }
        // ngày hiệu lực
        public DateTime ContractEffectiveDate { get; set; }
        // ngày tất toán
        public DateTime ContractPaymentDate { get; set; }
        // khóa liên kết với bảng Client
        public int ClientId { get; set; }
        // Khóa liên kết với bảng product
        public int ProductId { get; set; }



        // số hiệu tk
        public string ClientCode { get; set; }
        // Họ tên khách hàng
        public string ClientName { get; set; }
        // địa chỉ khách hàng.
        public string ClientAddress { get; set; }
        // số căn cước công dân 
        public string ClientIdNumber { get; set; }
        // số điện thoại
        public string ClientSMSNumber { get; set; }
        // email
        public string ClientEmail { get; set; }



        // Tên sản phẩm
        public string ProductName { get; set; }
        // tổng hạn mức
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ProductPackageMaximumAmount { get; set; }
        // hạn mức còn lại
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal ProductTotalRemainingLimit { get; set; }
        // lãi xuất không kì hạn 
        // kết hợp chuyển đổi giá trị về %
        [DisplayFormat(DataFormatString = "{0:N1}%", ApplyFormatInEditMode = true)]
        public decimal ProductNonTermRate { get; set; }
    }


    public class DemoLogin
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public bool KeepLoggedIn { get; set; }
        [Required(ErrorMessage = "Mã Pin không được để trống")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Mã Pin phải là 4 chữ số")]
        public string Pin { get; set; }
    }

    public abstract class PageCommon
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public int TotalRecords { get; set; }
    }
}
