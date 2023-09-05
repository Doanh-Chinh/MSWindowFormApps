using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanVien
{
    public class NhanVien
    {
        // Properties
        public string MaNV { get; set; } // Mã nhân viên
        public string HoTen { get; set; } // Họ tên
        public string DienThoai { get; set; } // Điện thoại
        public string GioiTinh { get; set; } // Giới tính 
        public DateTime NgayVaoLam { get; set; } // Ngày vào làm
        public string LoaiNV { get; set; } // Loại nhân viên (Bán Hàng, Giao Nhận)
        public double DoanhSo { get; set; } // Doanh số (chỉ cho nhân viên Bán Hàng)
        public double TienPhuCap { get; set; } // Tiền phụ cấp nhiên liệu (chỉ cho nhân viên Giao Nhận)
        public bool ThamNienMore5Years { get; set; }
        // Constructor
        public NhanVien()
        {
            // Khởi tạo các giá trị mặc định
            MaNV = string.Empty;
            HoTen = string.Empty;
            DienThoai = string.Empty;
            GioiTinh = "Nam"; // Mặc định là Nam
            NgayVaoLam = DateTime.Now; // Mặc định là ngày hiện tại
            LoaiNV = "Bán Hàng"; // Mặc định là Bán Hàng
            DoanhSo = 0; // Mặc định doanh số là 0
            TienPhuCap = 0; // Mặc định tiền phụ cấp nhiên liệu là 0
            ThamNienMore5Years = false;
        }

    }
}
