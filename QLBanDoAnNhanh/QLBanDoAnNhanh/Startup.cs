namespace QLBanDoAnNhanh
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Thêm dịch vụ session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout cho session
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddDistributedMemoryCache(); // Dùng để lưu trữ session trong bộ nhớ
        }

    }
}
