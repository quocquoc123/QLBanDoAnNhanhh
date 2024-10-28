using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBanDoAnNhanh.Models;


public class DonHangsController : Controller
{
    private readonly QlbanDoAnNhanhContext _context; // Thay đổi theo DbContext của bạn

    public DonHangsController(QlbanDoAnNhanhContext context)
    {
        _context = context;
    }

    // GET: DonHangs
    public async Task<IActionResult> Index(string trangThai = null)
    {
        var query = _context.DonHangs.AsQueryable();

        // Lọc theo trạng thái nếu có
        if (!string.IsNullOrEmpty(trangThai))
        {
            query = query.Where(d => d.TrangThai == trangThai);
        }

        var donHangs = await query.ToListAsync();
        ViewBag.TrangThai = trangThai; // Truyền trạng thái hiện tại về view
        return View(donHangs);
    }


    // GET: DonHangs/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donHang = await _context.DonHangs
            .FirstOrDefaultAsync(m => m.MaDh == id);
        if (donHang == null)
        {
            return NotFound();
        }

        return View(donHang);
    }

    // GET: DonHangs/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donHang = await _context.DonHangs.FindAsync(id);
        if (donHang == null)
        {
            return NotFound();
        }
        return View(donHang);
    }

    // POST: DonHangs/Edit/5
    // POST: DonHangs/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("MaDh,Username,TrangThai,Diachi,MaKhuyenMai,TongTien,SoLuong,CreatedAt,UpdatedAt")] DonHang donHang)
    {
        if (id != donHang.MaDh)
        {
            return NotFound();
        }

        // Kiểm tra nếu Username bị bỏ trống
        if (string.IsNullOrEmpty(donHang.Username))
        {
            ModelState.AddModelError("Username", "Tên người dùng không được bỏ trống.");
            return View(donHang);
        }

        // Kiểm tra MaKhuyenMai có hợp lệ hay không
        var khuyenMai = await _context.KhuyenMais.FindAsync(donHang.MaKhuyenMai);
        if (khuyenMai == null)
        {
            ModelState.AddModelError("MaKhuyenMai", "Mã khuyến mãi không hợp lệ.");
            return View(donHang);
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Không thay đổi MaKhuyenMai nếu không cần
                _context.Entry(donHang).Property(d => d.MaKhuyenMai).IsModified = false;

                // Cập nhật các thuộc tính khác
                _context.Update(donHang);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonHangExists(donHang.MaDh))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(donHang);
    }



    // Cập nhật trạng thái đơn hàng
    public async Task<IActionResult> UpdateTrangThai_ChuaGiao(string id)
    {
        return await UpdateTrangThai(id, "Chưa Giao");
    }

    public async Task<IActionResult> UpdateTrangThai_DaGiao(string id)
    {
        return await UpdateTrangThai(id, "Đã Giao");
    }

    public async Task<IActionResult> UpdateTrangThai_DangGiao(string id)
    {
        return await UpdateTrangThai(id, "Đang Giao");
    }

    public async Task<IActionResult> UpdateTrangThai_DaHuy(string id)
    {
        return await UpdateTrangThai(id, "Đã Hủy");
    }

    private async Task<IActionResult> UpdateTrangThai(string id, string trangThai)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donHang = await _context.DonHangs.FindAsync(id);
        if (donHang == null)
        {
            return NotFound();
        }

        donHang.TrangThai = trangThai; // Cập nhật trạng thái
        donHang.UpdatedAt = DateTime.Now; // Cập nhật thời gian chỉnh sửa
        _context.Entry(donHang).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: DonHangs/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donHang = await _context.DonHangs
            .FirstOrDefaultAsync(m => m.MaDh == id);
        if (donHang == null)
        {
            return NotFound();
        }

        return View(donHang);
    }

    // POST: DonHangs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var donHang = await _context.DonHangs.FindAsync(id);
        _context.DonHangs.Remove(donHang);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // Kiểm tra tồn tại đơn hàng
    private bool DonHangExists(string id)
    {
        return _context.DonHangs.Any(e => e.MaDh == id);
    }
}
