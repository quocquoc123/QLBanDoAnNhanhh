using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBanDoAnNhanh.Models;

namespace QLBanDoAnNhanh.Controllers
{
    public class GioHangsController : Controller
    {
        private GioHang GetGioHangFromSession()
        {
            var gioHang = HttpContext.Session.GetObjectFromJson<GioHang>("GioHang");
            if (gioHang == null)
            {
                gioHang = new GioHang();
            }
            return gioHang;
        }
        // Tính tổng số sản phẩm trong giỏ hàng và cập nhật ViewBag
        private void UpdateCartItemCount(GioHang gioHang)
        {
            ViewBag.CartItemCount = gioHang.ChiTietGioHangs.Sum(ct => ct.SoLuongSp);
        }

        // Lưu giỏ hàng vào session
        private void SaveGioHangToSession(GioHang gioHang)
        {
            HttpContext.Session.SetObjectAsJson("GioHang", gioHang);
        }
        //private void UpdateCartItemCount(GioHang gioHang)
        //{
        //    ViewBag.CartItemCount = gioHang.ChiTietGioHangs.Sum(ct => ct.SoLuongSp);
        //}
        // Phương thức thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public IActionResult AddToCart(int MaSp, int quantity)
        {
            var gioHang = GetGioHangFromSession(); // Lấy giỏ hàng hiện tại từ session

            // Tìm sản phẩm theo mã sản phẩm
            using (var context = new QlbanDoAnNhanhContext())
            {
                var sanPham = context.SanPhams.FirstOrDefault(sp => sp.MaSp == MaSp);
                if (sanPham == null)
                {
                    return NotFound(); // Nếu không tìm thấy sản phẩm, trả về lỗi 404
                }

                // Kiểm tra nếu sản phẩm đã có trong giỏ hàng
                var chiTietGioHang = gioHang.ChiTietGioHangs.FirstOrDefault(ct => ct.MaSp == MaSp);
                if (chiTietGioHang == null)
                {
                    // Nếu sản phẩm chưa có trong giỏ, thêm sản phẩm mới vào giỏ
                    chiTietGioHang = new ChiTietGioHang
                    {
                        MaSp = MaSp,
                        MaSpNavigation = sanPham,
                        SoLuongSp = quantity,
                        TongTien = (int)(quantity * sanPham.GiaTien)
                    };
                    gioHang.ChiTietGioHangs.Add(chiTietGioHang);
                }
                else
                {
                    // Nếu sản phẩm đã có trong giỏ, cập nhật số lượng và tổng tiền
                    chiTietGioHang.SoLuongSp += quantity;
                    chiTietGioHang.TongTien = (int)(chiTietGioHang.SoLuongSp * sanPham.GiaTien);
                }

                // Lưu lại giỏ hàng vào session
                SaveGioHangToSession(gioHang);
                UpdateCartItemCount(gioHang);
            }

            // Điều hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }

        // Hiển thị giỏ hàng
        public IActionResult Index()
        {
            var gioHang = GetGioHangFromSession();
            UpdateCartItemCount(gioHang);
            return View(gioHang); // Trả về View để hiển thị giỏ hàng
        }

        // Xóa sản phẩm khỏi giỏ hàng
        public IActionResult RemoveFromCart(int MaSp)
        {
            var gioHang = GetGioHangFromSession(); // Lấy giỏ hàng từ session

            // Tìm sản phẩm trong giỏ hàng
            var chiTietGioHang = gioHang.ChiTietGioHangs.FirstOrDefault(ct => ct.MaSp == MaSp);
            if (chiTietGioHang != null)
            {
                gioHang.ChiTietGioHangs.Remove(chiTietGioHang); // Xóa sản phẩm khỏi giỏ hàng
            }

            // Cập nhật lại session
            SaveGioHangToSession(gioHang);
            UpdateCartItemCount(gioHang);

            // Điều hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateCart(int MaSp, int quantity)
        {
            var gioHang = GetGioHangFromSession(); // Lấy giỏ hàng từ session

            // Tìm sản phẩm trong giỏ hàng
            var chiTietGioHang = gioHang.ChiTietGioHangs.FirstOrDefault(ct => ct.MaSp == MaSp);
            if (chiTietGioHang != null)
            {
                chiTietGioHang.SoLuongSp = quantity;
                chiTietGioHang.TongTien = (int)(quantity * chiTietGioHang.MaSpNavigation.GiaTien);
            }

            // Cập nhật lại session
            SaveGioHangToSession(gioHang);
            UpdateCartItemCount(gioHang);

            // Điều hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }

        // Xóa toàn bộ giỏ hàng
        public IActionResult ClearCart()
        {
            var gioHang = new GioHang(); // Tạo mới giỏ hàng rỗng
            SaveGioHangToSession(gioHang); // Lưu lại giỏ hàng trống vào session

            // Điều hướng về trang giỏ hàng
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var gioHang = GetGioHangFromSession(); // Lấy giỏ hàng từ session

            if (gioHang.ChiTietGioHangs.Count == 0)
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Ở đây, bạn có thể thêm logic lưu thông tin đơn hàng vào cơ sở dữ liệu
            // hoặc thực hiện thanh toán thông qua các cổng thanh toán trực tuyến.

            // Xóa giỏ hàng sau khi thanh toán
            ClearCart();

            TempData["Message"] = "Thanh toán thành công! Cảm ơn bạn đã mua hàng.";
            return RedirectToAction("Index");
        }
    }
}
