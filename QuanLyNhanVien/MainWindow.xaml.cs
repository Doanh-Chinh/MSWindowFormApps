using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyNhanVien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetDefaultValues();
            
        }

        private void SetDefaultValues()
        {
            // Xóa dữ liệu từ các TextBox nhập liệu
            txtMa.Clear();
            txtTen.Clear();
            txtDienThoai.Clear();
            txtDoanhSoPhuCap.Clear();
            // Đặt giới tính "Nam" mặc định được chọn
            radNam.IsChecked = true;

            // Đặt ngày vào làm mặc định là ngày hiện tại
            dtpHiredDate.SelectedDate = DateTime.Now;

            // Đặt loại nhân viên "Bán Hàng" mặc định được chọn
            radBanHang.IsChecked = true;

            // Đưa con trỏ lên TextBox nhập mã nhân viên
            txtMa.Focus();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng chương trình?", "Xác nhận đóng", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void radBanHang_Checked(object sender, RoutedEventArgs e)
        {
            if (radBanHang.IsChecked == true)
            {
                lblDoanhSoPhuCap.Content = "Doanh Số:";
            }
               
        }

        private void radGiaoNhan_Checked(object sender, RoutedEventArgs e)
        {
            if (radGiaoNhan.IsChecked == true)
            {
                lblDoanhSoPhuCap.Content = "PC Nhiên Liệu:";
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultValues();
        }

        private bool IsDataValid(string MaNV, string HoTen, DateTime NgayVaoLam)
        {
            // Check if MaNV (Employee ID) is not empty
            if (string.IsNullOrEmpty(MaNV))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.");
                return false;
            }

            // Check if HoTen (Full Name) is not empty
            if (string.IsNullOrEmpty(HoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên.");
                return false;
            }

            // Check if NgayVaoLam (Joining Date) is not in the future
            if (NgayVaoLam > DateTime.Now)
            {
                MessageBox.Show("Ngày vào làm không thể trong tương lai.");
                return false;
            }


            return true; // Data is valid
        }

        private bool IsThamNienMore5Years(DateTime ngayVaoLam)
        {
            TimeSpan thoiGianLamViec = DateTime.Now - ngayVaoLam;
            int thamNienNam = (int)(thoiGianLamViec.TotalDays / 365);

            if (thamNienNam > 5)
            {
                return true; // Thâm niên lớn hơn 5 năm
            }
            else
            {
                return false; // Thâm niên không đạt 5 năm
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string MaNV = txtMa.Text;
            string HoTen = txtTen.Text;
            DateTime NgayVaoLam = dtpHiredDate.SelectedDate.Value;
            bool res = IsDataValid(MaNV, HoTen, NgayVaoLam);
            if (res)
            {
                // Tạo một đối tượng nhân viên từ thông tin nhập liệu
                NhanVien nhanVien = new NhanVien
                {
                    MaNV = MaNV,
                    HoTen = HoTen,
                    DienThoai = txtDienThoai.Text,
                    NgayVaoLam = NgayVaoLam,
                    // Các thông tin khác tương tự
                };

                // giới tính
                if (radNam.IsChecked == true)
                {
                    nhanVien.GioiTinh = "Nam";
                }
                else
                    nhanVien.GioiTinh = "Nữ";

                // doanh số/phụ cấp
                if (radBanHang.IsChecked == true)
                {
                    nhanVien.LoaiNV = "Bán Hàng";
                    if (txtDoanhSoPhuCap.Text != "")
                        nhanVien.DoanhSo = double.Parse(txtDoanhSoPhuCap.Text);
                }
                else
                {
                    nhanVien.LoaiNV = "Giao Nhận";
                    if (txtDoanhSoPhuCap.Text != "")
                        nhanVien.TienPhuCap = double.Parse(txtDoanhSoPhuCap.Text);
                }

                // tính thâm niên
                nhanVien.ThamNienMore5Years = IsThamNienMore5Years(nhanVien.NgayVaoLam);
                // Thêm nhân viên vào ListView
                lvNhanVien.Items.Add(nhanVien);

                // Chọn dòng dữ liệu vừa thêm vào
                lvNhanVien.SelectedItem = nhanVien;
            }
        }

        private void lvNhanVien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvNhanVien.SelectedItem != null)
            {
                // Get the selected employee from the ListView
                NhanVien selectedEmployee = (NhanVien)lvNhanVien.SelectedItem;

                StringBuilder stringBuilder = new StringBuilder();
                // Append to string builder with the data from the selected employee
                stringBuilder.AppendLine("Mã NV: " + selectedEmployee.MaNV);
                stringBuilder.AppendLine("Họ Tên: " + selectedEmployee.HoTen);
                stringBuilder.AppendLine("Giới Tính: " + selectedEmployee.GioiTinh);
                stringBuilder.AppendLine("Điện Thoại: " + selectedEmployee.DienThoai);
                stringBuilder.AppendLine("Ngày Vào Làm: " + selectedEmployee.NgayVaoLam.ToString());
                stringBuilder.AppendLine("Loại NV: " + selectedEmployee.LoaiNV);
                if (selectedEmployee.LoaiNV == "Bán Hàng")
                {
                    stringBuilder.AppendLine("Doanh Số: " + selectedEmployee.DoanhSo.ToString());

                }
                else
                {
                    stringBuilder.AppendLine("PC Nhiên Liệu: " + selectedEmployee.TienPhuCap.ToString());

                }

                // Show in MessageBox
                MessageBox.Show(stringBuilder.ToString());

                // Populate input fields with the data from the selected employee
                txtMa.Text = selectedEmployee.MaNV;
                txtTen.Text = selectedEmployee.HoTen;
                txtDienThoai.Text = selectedEmployee.DienThoai;
                // Set other input fields as needed

                // Update the RadioButton for LoaiNV based on the selected employee's type
                if (selectedEmployee.LoaiNV == "Bán Hàng")
                {
                    radBanHang.IsChecked = true;
                    txtDoanhSoPhuCap.Text = selectedEmployee.DoanhSo.ToString();

                }
                else 
                {
                    radGiaoNhan.IsChecked = true;
                    txtDoanhSoPhuCap.Text = selectedEmployee.TienPhuCap.ToString();

                }

                if (selectedEmployee.GioiTinh == "Nam")
                    radNam.IsChecked = true;
                else
                    radNu.IsChecked = true;

                // Update the DatePicker for NgayVaoLam
                dtpHiredDate.SelectedDate = selectedEmployee.NgayVaoLam;


            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (lvNhanVien.SelectedItem != null)
            {
                NhanVien selectedEmployee = (NhanVien)lvNhanVien.SelectedItem;
                int selectedIndex = lvNhanVien.SelectedIndex;
                // Ask for confirmation before deleting
                MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    lvNhanVien.Items.Remove(selectedEmployee);
                    int lvCount = lvNhanVien.Items.Count;
                    // Clear input fields or set them to default values if the list is empty
                    if (lvCount == 0)
                    {
                        SetDefaultValues();
                    }
                    else if (selectedIndex == lvCount)
                    {
                        lvNhanVien.SelectedItem = lvNhanVien.Items[selectedIndex - 1];
                    }
                    else if (selectedIndex < lvCount)
                        lvNhanVien.SelectedItem = lvNhanVien.Items[selectedIndex];
                    
                }
            }
        }

        private void mnuFileSave_Click(object sender, RoutedEventArgs e)
        {
            // Create a SaveFileDialog to choose the location and name of the text file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string filePath = saveFileDialog.FileName;

                // Create a StreamWriter to write the data to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (NhanVien nhanVien in lvNhanVien.Items)
                    {
                        // Write each employee's data to the file in the desired format
                        writer.WriteLine($"{nhanVien.MaNV},{nhanVien.HoTen}, {nhanVien.GioiTinh}, {nhanVien.DienThoai},{nhanVien.NgayVaoLam},{nhanVien.LoaiNV}, {nhanVien.DoanhSo}, {nhanVien.TienPhuCap}, {nhanVien.ThamNienMore5Years}");
                    }
                }

                MessageBox.Show("Dữ liệu đã được lưu vào tệp tin.");
            }
        }

        private void mnuFileLoad_Click(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog to choose the text file to load
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string filePath = openFileDialog.FileName;

                lvNhanVien.Items.Clear();

                // Read the data from the file and add it to the ObservableCollection
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 9)
                        {
                            NhanVien nhanVien = new NhanVien
                            {
                                MaNV = parts[0],
                                HoTen = parts[1],
                                GioiTinh = parts[2],
                                DienThoai = parts[3],
                                NgayVaoLam = DateTime.Parse(parts[4]),
                                LoaiNV = parts[5],
                                DoanhSo = double.Parse(parts[6]),
                                TienPhuCap = double.Parse(parts[7]),
                                ThamNienMore5Years = bool.Parse(parts[8]),
                            };
                            //lvNhanVien.Tag = nhanVien;
                            lvNhanVien.Items.Add(nhanVien);
                        }
                    }
                }

                MessageBox.Show("Dữ liệu đã được nạp từ tệp tin.");
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            int editedIndex = lvNhanVien.SelectedIndex;


            // Enable or disable controls based on LoaiNV
            NhanVien editedEmployee = (NhanVien)lvNhanVien.Items[editedIndex];
            // Update the selected item with edited data
            editedEmployee.MaNV = txtMa.Text;
            editedEmployee.HoTen = txtTen.Text;
            editedEmployee.DienThoai = txtDienThoai.Text;
            editedEmployee.NgayVaoLam = dtpHiredDate.SelectedDate.Value;
            // Update other properties as needed
            if (radNam.IsChecked == true)
            {
                editedEmployee.GioiTinh = "Nam";
            }
            else
                editedEmployee.GioiTinh = "Nữ";

            // Depending on the selected LoaiNV, update DoanhSo or TienPhuCapNhienLieu
            if (radBanHang.IsChecked == true)
            {
                editedEmployee.LoaiNV = "Bán Hàng";
                if (txtDoanhSoPhuCap.Text != "")
                    editedEmployee.DoanhSo = double.Parse(txtDoanhSoPhuCap.Text);
            }
            else if (radGiaoNhan.IsChecked == true)
            {
                editedEmployee.LoaiNV = "Giao Nhận";
                if (txtDoanhSoPhuCap.Text != "")
                    editedEmployee.TienPhuCap = double.Parse(txtDoanhSoPhuCap.Text);
            }

            // Refresh the ListView
            lvNhanVien.Items.Refresh();

        }
        private int CalculateThamNien(DateTime ngayVaoLam)
        {
            // Calculate seniority based on the ngày vào làm
            // You can implement your logic here
            // For example, calculate the difference in years between ngày vào làm and the current date
            // and return that value as thâm niên
            TimeSpan span = DateTime.Now - ngayVaoLam;
            int years = span.Days / 365;

            return years;
        }
        private void btnSapXep_Click(object sender, RoutedEventArgs e)
        {
            List<NhanVien> nhanVienList = new List<NhanVien>();
            foreach (NhanVien nv in lvNhanVien.Items)
            {
                nhanVienList.Add(nv);

            }

            var sortedList = nhanVienList
                            .OrderByDescending(nv => CalculateThamNien(nv.NgayVaoLam))
                            .ThenBy(nv => nv.HoTen)
                            .ToList();

            lvNhanVien.Items.Clear();

            // Add the sorted items back to the ObservableCollection
            foreach (var employee in sortedList)
            {
                lvNhanVien.Items.Add(employee);
            }
        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            List<NhanVien> nhanVienList = new List<NhanVien>();
            foreach (NhanVien nv in lvNhanVien.Items)
            {
                nhanVienList.Add(nv);

            }
            int countBanHang = nhanVienList.Count(nv => nv.LoaiNV == "Bán Hàng");
            int countGiaoNhan = nhanVienList.Count(nv => nv.LoaiNV == "Giao Nhận");

            double totalLuongBanHang = nhanVienList.Where(nv => nv.LoaiNV == "Bán Hàng").Sum(nv => nv.DoanhSo);
            double totalLuongGiaoNhan = nhanVienList.Where(nv => nv.LoaiNV == "Giao Nhận").Sum(nv => nv.TienPhuCap);

            MessageBox.Show($"Số nhân viên bán hàng: {countBanHang}\n" +
                            $"Số nhân viên giao nhận: {countGiaoNhan}\n\n" +
                            $"Tổng lương nhân viên bán hàng: {totalLuongBanHang:C}\n" +
                            $"Tổng lương nhân viên giao nhận: {totalLuongGiaoNhan:C}");
        }
    }
}
