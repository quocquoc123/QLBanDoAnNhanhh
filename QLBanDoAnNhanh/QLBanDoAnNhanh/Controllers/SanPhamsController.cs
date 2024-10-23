using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBanDoAnNhanh.Models;
using System.IO; // Để sử dụng FileStream và Path
using Microsoft.AspNetCore.Http; // Để sử dụng IFormFile
    
namespace QLBanDoAnNhanh.Controllers
{
    public class SanPhamsController : Controller
    {
        private QlbanDoAnNhanhContext db = new QlbanDoAnNhanhContext();

        private readonly QlbanDoAnNhanhContext _context;

        public SanPhamsController(QlbanDoAnNhanhContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index()
        {
            var qlbanDoAnNhanhContext = _context.SanPhams.Include(s => s.MaDmNavigation).Include(s => s.MaGiamGiaNavigation);
            return View(await qlbanDoAnNhanhContext.ToListAsync());
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .Include(s => s.MaGiamGiaNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        public async Task<IActionResult> ChiTietSanPham(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            // Lấy sản phẩm và thông tin liên quan
            var sanPham = await _context.SanPhams
                .Include(sp => sp.HinhAnhs) // Nếu bạn cần thông tin hình ảnh liên quan
                .FirstOrDefaultAsync(sp => sp.MaSp == id);

            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["MaDm"] = new SelectList(_context.DanhMucs, "MaDm", "MaDm");
            ViewData["MaGiamGia"] = new SelectList(_context.GiamGia, "MaGiamGia", "MaGiamGia");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSp,TenSp,MaGiamGia,ThanhPhan,GiaTien,DonVi,ChitietSp,MaDm,SlbanTrongNgay")] SanPham sanPham, IFormFile HinhAnh1, IFormFile HinhAnh2)
        {
            // Kiểm tra tệp HinhAnh1 và HinhAnh2 có được chọn không
            if (HinhAnh1 != null && HinhAnh1.Length > 0)
            {
                // Lấy đường dẫn lưu tệp
                var fileName1 = Path.GetFileName(HinhAnh1.FileName);
                var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName1);

                // Lưu tệp vào thư mục 'wwwroot/images'
                using (var stream = new FileStream(filePath1, FileMode.Create))
                {
                    await HinhAnh1.CopyToAsync(stream);
                }

                // Lưu đường dẫn vào cơ sở dữ liệu
                sanPham.HinhAnh1 = "/images/" + fileName1;
            }

            if (HinhAnh2 != null && HinhAnh2.Length > 0)
            {
                var fileName2 = Path.GetFileName(HinhAnh2.FileName);
                var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName2);

                using (var stream = new FileStream(filePath2, FileMode.Create))
                {
                    await HinhAnh2.CopyToAsync(stream);
                }

                sanPham.HinhAnh2 = "/images/" + fileName2;
            }

            // Lưu sản phẩm vào database
            _context.Add(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["MaDm"] = new SelectList(_context.DanhMucs, "MaDm", "MaDm", sanPham.MaDm);
            ViewData["MaGiamGia"] = new SelectList(_context.GiamGia, "MaGiamGia", "MaGiamGia", sanPham.MaGiamGia);
            return View(sanPham);
        }

        // POST: Admin/SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSp,TenSp,MaGiamGia,ThanhPhan,GiaTien,DonVi,ChitietSp,MaDm,SlbanTrongNgay,HinhAnh1,HinhAnh2")]
        SanPham sanPham, IFormFile file1, IFormFile file2)
        {
            if (id != sanPham.MaSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem có tệp hình ảnh mới không
                    if ((file1 != null && file1.Length > 0) && (file2 != null && file2.Length > 0))
                    {
                        // Tạo tên file duy nhất để tránh xung đột
                        var fileName1 = Path.GetFileName(file1.FileName);
                        var fileName2 = Path.GetFileName(file2.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName1);
                        var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName2);

                        // Xóa hình ảnh cũ (nếu cần)

                        // Lưu file vào thư mục wwwroot/images
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file1.CopyToAsync(stream);

                        }
                        using (var stream = new FileStream(filePath1, FileMode.Create))
                        {

                            await file2.CopyToAsync(stream);
                        }

                        // Gán đường dẫn của ảnh mới cho thuộc tính AnhSp
                        sanPham.HinhAnh1 = "/images/" + fileName1; // Đảm bảo đường dẫn hợp lệ
                        sanPham.HinhAnh2 = "/images/" + fileName2; // Đảm bảo đường dẫn hợp lệ
                    }

                    // Cập nhật thông tin sản phẩm
                    _context.Update(sanPham);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.MaSp))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }


                }
            }
            ViewData["MaDm"] = new SelectList(_context.DanhMucs, "MaDm", "MaDm", sanPham.MaDm);
            ViewData["MaGiamGia"] = new SelectList(_context.GiamGia, "MaGiamGia", "MaGiamGia", sanPham.MaGiamGia);
            return View(sanPham);
        }
        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPhams
                .Include(s => s.MaDmNavigation)
                .Include(s => s.MaGiamGiaNavigation)
                .FirstOrDefaultAsync(m => m.MaSp == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }
        public ActionResult Search(string searchTerm)
        {

            if (string.IsNullOrWhiteSpace(searchTerm))
            {

                return RedirectToAction("TrangChu");
            }


            var searchTermLower = searchTerm.ToLower();

            var searchResults = db.SanPhams
                .Where(p => p.TenSp.ToLower().Contains(searchTermLower))
                .ToList();
            ViewBag.SearchTerm = searchTerm;
            return View("TrangChu", searchResults);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham != null)
            {
                _context.SanPhams.Remove(sanPham);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult TrangChu()
        {

            var products = _context.SanPhams.ToList(); // L?y danh sách s?n ph?m
            return View(products); // Truy?n danh sách s?n ph?m vào view
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSp == id);
        }
        public IActionResult SanPhamTheoTenDanhMuc(string TenHang)
        {
            var sanPhams = _context.SanPhams
                .Where(sp => sp.MaDmNavigation.TenDm == TenHang)
                .ToList();

            ViewBag.TenDanhMuc = TenHang; // Truyền tên danh mục cho View
            return View(sanPhams);
        }
    }
}
